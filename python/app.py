import json
import uuid
from datetime import datetime, timezone
from pathlib import Path

from flask import Flask, jsonify, request
from flask_cors import CORS

app = Flask(__name__, static_folder="../frontend", static_url_path="/")
CORS(app)

DATA_FILE = Path(__file__).resolve().parent.parent / "data" / "experiments.json"


def read_experiments():
    with open(DATA_FILE, "r", encoding="utf-8") as f:
        return json.load(f)


def write_experiments(data):
    with open(DATA_FILE, "w", encoding="utf-8") as f:
        json.dump(data, f, indent=2)


@app.route("/")
def index():
    return app.send_static_file("index.html")


@app.route("/entries", methods=["GET"])
def get_entries():
    """Return all log entries."""
    return jsonify(read_experiments())


@app.route("/entries/<entry_id>", methods=["GET"])
def get_entry(entry_id):
    """Return one entry by id."""
    experiments = read_experiments()
    entry = next((e for e in experiments if e["id"] == entry_id), None)
    if entry is None:
        return jsonify({"error": "Entry not found"}), 404
    return jsonify(entry)


@app.route("/entries", methods=["POST"])
def add_entry():
    """Add a new entry."""
    data = request.get_json()
    required = ["tool", "task", "expected", "actual", "verdict"]
    if not data or not all(field in data for field in required):
        return jsonify({"error": "All fields are required: tool, task, expected, actual, verdict"}), 400

    experiments = read_experiments()
    new_entry = {
        "id": f"exp-{uuid.uuid4().hex[:8]}",
        "tool": data["tool"],
        "task": data["task"],
        "expected": data["expected"],
        "actual": data["actual"],
        "verdict": data["verdict"],
        "timestamp": datetime.now(timezone.utc).isoformat(),
    }
    experiments.append(new_entry)
    write_experiments(experiments)
    return jsonify(new_entry), 201


@app.route("/summary", methods=["GET"])
def get_summary():
    """Aggregated experiment data.

    Returns a JSON object with this shape:
    {
        "total": <number of entries>,
        "by_verdict": { "Faster": <count>, "Same": <count>, ... },
        "by_tool": { "<tool name>": { "Faster": <count>, ... } }
    }
    """
    # Read all experiments and count verdicts
    experiments = read_experiments()

    by_verdict = {"Faster": 0, "Same": 0, "Slower": 0, "Surprising": 0}

    for exp in experiments:
        verdict = exp.get("verdict", "")
        if verdict in by_verdict:
            by_verdict[verdict] += 1

    # TODO: Add by_tool grouping — Lab 2
    # Add a by_tool dict that groups verdict counts per tool.
    # The frontend expects: { "<tool name>": { "Faster": 0, "Same": 0, ... } }

    return jsonify({
        "total": len(experiments),
        "by_verdict": by_verdict,
    })


if __name__ == "__main__":
    app.run(port=5000)
