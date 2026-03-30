# Lab 0 — Challenge Guided: Customize Your Dev Container

**Goal:** Add a CLI tool, a VS Code extension, and a VS Code setting to your devcontainer configuration.

**Time:** 25 minutes

**You will need:** Lab 0 Core completed (your Codespace has your chosen language configured and the app runs).

---

## Steps

### 1. Open a new Codespace

1. Go to your repository on GitHub.com
2. Click the green **Code** button → **Codespaces** tab → **Create codespace on main**

   ![The Codespaces panel showing the Create codespace on main button](../../images/module1-image.png)

3. Wait for the Codespace to open — your language configuration from Core will be picked up automatically since you pushed it. Open `.devcontainer/devcontainer.json` to continue customizing it.

### 2. Add the Azure CLI feature

4. Open `.devcontainer/devcontainer.json` and add the Azure CLI entry to the `"features"` section (alongside your language feature from Core):

   ```json
   "ghcr.io/devcontainers/features/azure-cli:1": {}
   ```

   Dev container features are pre-built scripts that install tools into the container. You can find the full list at [containers.dev/features](https://containers.dev/features).

5. Save the file and click **Rebuild Now** when the notification appears

   Wait for the rebuild to complete, then open a new terminal and run `az --version` to verify the Azure CLI is installed.

### 3. Add a VS Code extension

6. Pick a VS Code extension you actually use (e.g., a theme, a linter, or a formatter). To find the identifier for any extension, open it in the Extensions sidebar or on the VS Code Marketplace and look for the **Identifier** field in the installation details panel:

   ![The extension page with the Identifier field highlighted](../../images/module2-image-1.png)

7. Locate the `customizations.vscode.extensions` array and add the extension identifier string to it

8. Save the file and click **Rebuild Now** when the notification appears

   Wait for the rebuild to complete, then verify the extension appears in the Extensions sidebar.

### 4. Add a VS Code setting to hide the minimap

9. In `customizations.vscode.settings`, add the minimap setting (you already have `editor.formatOnSave` there from the base config):

   ```json
   "settings": {
     "editor.formatOnSave": true,
     "editor.minimap.enabled": false
   }
   ```

10. Save the file and click **Rebuild Now** when the notification appears

    Wait for the rebuild to complete, then open any source file and confirm the minimap on the right side of the editor is gone.

**Expected result:** All three customizations are active after their respective rebuilds — `az --version` works in the terminal, your chosen extension is installed, and the minimap is hidden.

> **Tip:** VS Code settings placed in `devcontainer.json` apply to everyone who opens the repo in a Codespace. This is a great way to enforce consistent editor behaviour across a team.
