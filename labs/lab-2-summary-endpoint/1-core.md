# Lab 2 — Core Self-Directed: Add Tool Grouping to the Summary Endpoint

**Goal:** Use Copilot inline completion to add `by_tool` grouping to the existing `/summary` endpoint, unlocking the Tool Comparison chart.

**Time:** 15 minutes

**You will need:** Lab 1 completed, Copilot instructions file in place, API running.

> Note: If you are not finished in time, copy the completed file from the `rescue/` folder for your language into your project folder (see the [rescue README](../../rescue/README.md) for details), commit, and push. You will continue with Labs 3 and 4 from there.

---

## Your Task

The `/summary` endpoint already returns `total` and `by_verdict`. The Dashboard shows a working Verdict Breakdown chart, but the Tool Comparison chart says *"Add `by_tool` to unlock this chart."*

Find the `TODO: Add by_tool grouping` comment in your API file. Use Copilot inline completion — write a descriptive comment and let Copilot suggest the `by_tool` implementation. The existing `by_verdict` loop is your reference for the pattern.

## Success Criteria

- `GET /summary` returns JSON with `total`, `by_verdict`, **and** `by_tool`
- The Tool Comparison chart appears on the Dashboard
- The placeholder message is gone
- Changes committed with a message referencing an issue

> **Tip:** Look at how `by_verdict` is built — `by_tool` follows the same pattern but groups counts per tool name.
