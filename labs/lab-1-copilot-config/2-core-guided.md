# Lab 1 — Core Guided: Create Copilot Instructions & a Skill

**Goal:** Create Copilot configuration files that accurately describe your working style and this repo's conventions, then verify they shape output.

**Time:** 35 minutes

**You will need:** Lab 0 completed, Codespace open, your language folder chosen.

---

## Part 1 — Create `copilot-instructions.md`

1. In the root of your repo, create the folder `.github/` if it doesn't already exist
2. Inside `.github/`, create a new file called `copilot-instructions.md`
3. Add a **Language** section that tells Copilot which language folder you are working in. For example:

   ```markdown
   ## Language

   I am working in the [node/python/dotnet] folder of this repo.
   ```

4. Add a **Code style** section with your preferred style rules. For example:

   ```markdown
   ## Code style

   - Use async/await instead of callbacks or raw promises
   - Add a brief comment above every function explaining what it does
   - Use descriptive variable names, not abbreviations
   ```

5. Add a **Testing** section that tells Copilot about your testing approach. For example:

   ```markdown
   ## Testing

   - Always suggest a test alongside any new function
   - Use [Jest/pytest/xUnit] as the test framework
   ```

6. Add a line that tells Copilot to **end every answer with an encouraging message**. This is how you will verify your instructions are being picked up. For example:

   ```markdown
   End every message with an encouraging message.
   ```

7. Save the file. Copilot picks this up immediately — no restart required.

## Part 2 — Create a Security Review Skill

Skills live in their own subdirectory inside `.github/skills/`. Each skill folder contains a `SKILL.md` file that defines the skill's metadata and instructions.

8. Create the folder `.github/skills/security-review/` in the repo root (create parent folders as needed)
9. Inside that folder, create a file called `SKILL.md`
10. Add YAML frontmatter at the top of the file. The `name` must match the folder name, and the `description` is what Copilot uses to decide when to invoke this skill automatically. For example:

    ```markdown
    ---
    name: security-review
    description: "Perform a security review of the codebase, checking for common vulnerabilities and best practices"
    ---
    ```

11. Below the frontmatter, add the skill body with instructions for the security review. For example:

    ```markdown
    # Security Review

    Perform a thorough security review of the code. Check for:

    1. Input validation — are all user inputs sanitized?
    2. Authentication and authorization — are endpoints properly protected?
    3. Data exposure — is sensitive data logged or returned in responses?
    4. Dependency vulnerabilities — are there known issues with dependencies?
    5. Error handling — do error messages leak implementation details?

    Provide a summary of findings with severity levels (high, medium, low) and suggested fixes.
    ```

12. Save the file. Your skill folder structure should now look like this:

    ```
    .github/
    └── skills/
        └── security-review/
            └── SKILL.md
    ```

## Part 3 — Test Your Configuration

13. Open the Copilot Chat panel
14. Ask Copilot to perform a security review of your API file:
    - **Node:** *"Do a security review of server.js"*
    - **Python:** *"Do a security review of app.py"*
    - **.NET:** *"Do a security review of Program.cs"*
15. Verify that:
    - Copilot follows the style from your instructions file
    - The response ends with an encouraging message (proving `copilot-instructions.md` is active)
    - The security review is thorough (proving the skill is being used)

**Expected output:**
- `.github/copilot-instructions.md` exists with Language, Code style, and Testing sections
- `.github/skills/security-review/SKILL.md` exists with a `name`, `description`, and review guidance
- You have tested the setup by asking for a security review and confirmed Copilot follows your instructions
- Zero premium requests consumed

> **Important:** What you write in these files now is what Copilot uses in Lab 2. Take the time to make them accurate.
