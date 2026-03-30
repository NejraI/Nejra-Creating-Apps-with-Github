// API base URL — uses the same origin since all backends serve the frontend as static files.
// If the frontend is opened as a standalone file, override this to the correct backend URL.
const API_BASE = "";

// Path to the static JSON file used as a fallback when no API is running (e.g. GitHub Pages)
const STATIC_DATA_URL = "experiments.json";

// ── Navigation ──────────────────────────────────────────────

document.querySelectorAll(".nav-btn").forEach((btn) => {
  btn.addEventListener("click", () => {
    document.querySelectorAll(".nav-btn").forEach((b) => b.classList.remove("active"));
    document.querySelectorAll(".view").forEach((v) => v.classList.remove("active"));
    btn.classList.add("active");
    document.getElementById(`${btn.dataset.view}-view`).classList.add("active");

    if (btn.dataset.view === "log") loadEntries();
    if (btn.dataset.view === "dashboard") loadDashboard();
  });
});

// ── Log View ────────────────────────────────────────────────

// Fetches entries from the API, falling back to the static JSON file on failure
async function loadEntries() {
  const container = document.getElementById("entries-list");
  try {
    let entries;
    const apiRes = await fetch(`${API_BASE}/entries`).catch(() => null);
    if (apiRes && apiRes.ok) {
      entries = await apiRes.json();
    } else {
      // API unavailable — load from static data file (GitHub Pages fallback)
      const staticRes = await fetch(STATIC_DATA_URL);
      if (!staticRes.ok) throw new Error("Static data not available");
      entries = await staticRes.json();
    }

    if (entries.length === 0) {
      container.innerHTML = '<p class="empty">No experiments logged yet.</p>';
      return;
    }

    container.innerHTML = entries
      .map(
        (entry) => `
      <details class="entry-card">
        <summary>
          <span class="verdict-badge verdict-${entry.verdict.toLowerCase()}">${entry.verdict}</span>
          <strong>${escapeHtml(entry.task)}</strong>
          <span class="tool-tag">${escapeHtml(entry.tool)}</span>
        </summary>
        <div class="entry-detail">
          <p><strong>Expected:</strong> ${escapeHtml(entry.expected)}</p>
          <p><strong>Actual:</strong> ${escapeHtml(entry.actual)}</p>
          <p class="timestamp">${new Date(entry.timestamp).toLocaleString()}</p>
        </div>
      </details>`
      )
      .join("");
  } catch (err) {
    container.innerHTML = '<p class="error">Could not load entries. Is the API running?</p>';
  }
}

// ── Dashboard View ──────────────────────────────────────────

let verdictChart = null;
let toolChart = null;

// Builds a summary object from a raw entries array (client-side fallback)
function buildSummary(entries) {
  const byVerdict = {};
  const byTool = {};
  for (const entry of entries) {
    byVerdict[entry.verdict] = (byVerdict[entry.verdict] || 0) + 1;
    if (!byTool[entry.tool]) byTool[entry.tool] = {};
    byTool[entry.tool][entry.verdict] = (byTool[entry.tool][entry.verdict] || 0) + 1;
  }
  return { total: entries.length, by_verdict: byVerdict, by_tool: byTool };
}

// Loads dashboard data from the API, falling back to computing it from static data
async function loadDashboard() {
  const messageEl = document.getElementById("dashboard-message");
  const chartsEl = document.getElementById("charts-container");

  try {
    let data;
    const apiRes = await fetch(`${API_BASE}/summary`).catch(() => null);
    if (apiRes && apiRes.status === 501) {
      messageEl.innerHTML =
        '<p class="not-implemented">Summary not yet available — implement the <code>/summary</code> endpoint in Lab 2 to unlock the dashboard charts.</p>';
      chartsEl.style.display = "none";
      return;
    }
    if (apiRes && apiRes.ok) {
      data = await apiRes.json();
    } else {
      // API unavailable — compute summary from static data (GitHub Pages fallback)
      const staticRes = await fetch(STATIC_DATA_URL);
      if (!staticRes.ok) throw new Error("Static data not available");
      const entries = await staticRes.json();
      data = buildSummary(entries);
    }

    messageEl.innerHTML = `<p class="total">Total experiments: <strong>${data.total}</strong></p>`;
    chartsEl.style.display = "grid";

    renderVerdictChart(data.by_verdict);

    if (data.by_tool) {
      document.getElementById("tool-chart-placeholder").style.display = "none";
      document.getElementById("tool-chart").style.display = "block";
      renderToolChart(data.by_tool);
    } else {
      document.getElementById("tool-chart").style.display = "none";
      document.getElementById("tool-chart-placeholder").style.display = "flex";
    }
  } catch (err) {
    messageEl.innerHTML = '<p class="error">Could not load summary. Is the API running?</p>';
    chartsEl.style.display = "none";
  }
}

function renderVerdictChart(byVerdict) {
  const ctx = document.getElementById("verdict-chart").getContext("2d");
  if (verdictChart) verdictChart.destroy();

  const labels = Object.keys(byVerdict);
  const values = Object.values(byVerdict);
  const colors = {
    Faster: "#22c55e",
    Same: "#3b82f6",
    Slower: "#ef4444",
    Surprising: "#f59e0b",
  };

  verdictChart = new Chart(ctx, {
    type: "doughnut",
    data: {
      labels,
      datasets: [
        {
          data: values,
          backgroundColor: labels.map((l) => colors[l] || "#6b7280"),
        },
      ],
    },
    options: {
      responsive: true,
      plugins: { legend: { position: "bottom" } },
    },
  });
}

function renderToolChart(byTool) {
  const ctx = document.getElementById("tool-chart").getContext("2d");
  if (toolChart) toolChart.destroy();

  const tools = Object.keys(byTool);
  const verdicts = ["Faster", "Same", "Slower", "Surprising"];
  const colors = {
    Faster: "#22c55e",
    Same: "#3b82f6",
    Slower: "#ef4444",
    Surprising: "#f59e0b",
  };

  toolChart = new Chart(ctx, {
    type: "bar",
    data: {
      labels: tools,
      datasets: verdicts.map((verdict) => ({
        label: verdict,
        data: tools.map((t) => byTool[t][verdict] || 0),
        backgroundColor: colors[verdict],
      })),
    },
    options: {
      responsive: true,
      scales: { x: { stacked: true }, y: { stacked: true, beginAtZero: true } },
      plugins: { legend: { position: "bottom" } },
    },
  });
}

// ── Add Entry Form ──────────────────────────────────────────

document.getElementById("add-form").addEventListener("submit", async (e) => {
  e.preventDefault();
  const messageEl = document.getElementById("form-message");

  const entry = {
    tool: document.getElementById("tool").value,
    task: document.getElementById("task").value,
    expected: document.getElementById("expected").value,
    actual: document.getElementById("actual").value,
    verdict: document.getElementById("verdict").value,
  };

  try {
    const res = await fetch(`${API_BASE}/entries`, {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify(entry),
    });

    if (res.ok) {
      messageEl.innerHTML = '<p class="success">Entry added!</p>';
      document.getElementById("add-form").reset();
    } else {
      const err = await res.json();
      messageEl.innerHTML = `<p class="error">${escapeHtml(err.error)}</p>`;
    }
  } catch (err) {
    messageEl.innerHTML = '<p class="error">Could not add entry. Is the API running?</p>';
  }
});

// ── Helpers ─────────────────────────────────────────────────

function escapeHtml(text) {
  const div = document.createElement("div");
  div.textContent = text;
  return div.innerHTML;
}

// ── Initial Load ────────────────────────────────────────────

loadEntries();
