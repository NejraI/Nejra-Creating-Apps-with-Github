# Lab 0 — Core Self-Directed: Work with a Codespace

**Goal:** Start a Codespace, add your chosen language stack to the dev container, run the app, and commit your changes.

**Time:** 20 minutes

**You will need:** A GitHub account with Codespaces access.

> if you are stuck, open the guided lab [here](../lab-0-codespaces/2-core-guided.md) for step-by-step instructions.
---

## Your Task

- Create a new repository out of the workshop repo ([github.com/Ba4bes/Creating-Apps-with-GitHub](https://github.com/Ba4bes/Creating-Apps-with-GitHub/)) by using the template and launch it in a Codespace.
- The Codespace starts with a minimal base image — no language runtimes are installed yet. Your first job is to add your chosen language.

### Add your language to the dev container

Pick **one** language (Node, Python, or .NET) and modify `.devcontainer/devcontainer.json` to add:

1. A **dev container feature** for your language runtime
2. The matching **VS Code extension**
3. A **`postCreateCommand`** that installs your language's dependencies
4. A **`forwardPorts`** entry for the port your app uses

Use the reference table below:

| | Node | Python | .NET |
|---|---|---|---|
| Feature | `"ghcr.io/devcontainers/features/node:1": {}` | `"ghcr.io/devcontainers/features/python:1": {}` | `"ghcr.io/devcontainers/features/dotnet:2": { "version": "10.0" }` |
| Extension | `dbaeumer.vscode-eslint` | `ms-python.python` | `ms-dotnettools.csdevkit` |
| postCreateCommand | `cd node && npm install` | `cd python && pip install -r requirements.txt` | `cd dotnet && dotnet restore && cd Api.Tests && dotnet restore` |
| Port | 3000 | 5000 | 5001 |

After editing the file, rebuild the container (Command Palette → **Dev Containers: Rebuild Container**).

### Run the app and verify

- Start the app for your language and confirm it is running in the browser.
- Look around the app.

### Commit and push

- Add your name to the top of `README.md`.
- Commit and push all changes (the devcontainer update and the README change) from within the Codespace.
- Verify the changes are visible in the web interface of the repo.

### Clean up

- Stop the Codespace from the web interface.

> Note: you don't need to stop the codespace before deleting it. This exercise is just to prove the point that you can stop and restart a codespace without losing your work.

- Delete the Codespace.


## Success Criteria

- The dev container was customized with your chosen language
- The container was rebuilt and the app was visible in the browser
- Changes were committed and pushed from within the Codespace
- The Codespace was stopped and deleted


> **Tip:** Uncommitted changes are lost when a Codespace is deleted. Committed and pushed changes are saved to the branch and will be there the next time you create a Codespace.
