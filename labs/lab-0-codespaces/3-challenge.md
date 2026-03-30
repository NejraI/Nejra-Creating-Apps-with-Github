# Lab 0 — Challenge Self-Directed: Customize Your Dev Container

**Goal:** Extend your dev container configuration with a CLI tool, a VS Code extension, and a VS Code setting.

**Time:** 25 minutes

**You will need:** Lab 0 Core completed (your Codespace has your chosen language configured and the app runs).

> if you are stuck, open the guided lab [here](../lab-0-codespaces/4-challenge-guided.md) for step-by-step instructions.   

---

## Your Task

Open a new Codespace (your language configuration from Core will be picked up automatically since you pushed it).

Modify `.devcontainer/devcontainer.json` to add:

1. The Azure CLI as a dev container feature
2. A VS Code extension of your choice (find one you actually use)
3. A VS Code setting that hides the minimap

Rebuild the container after each step and verify all three changes are applied.

## Success Criteria

- Running `az --version` in the terminal succeeds
- Your chosen extension is installed and visible in the Extensions sidebar
- The minimap is no longer visible on the right side of the editor
