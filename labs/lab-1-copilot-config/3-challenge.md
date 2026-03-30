# Lab 1 — Challenge Self-Directed: Skills with Resources & Community Plugins

**Goal:** Create a skill that includes supporting resources alongside its instructions, and install a community plugin from the Awesome Copilot marketplace.

**Time:** 25 minutes

**You will need:** Lab 1 Core completed (instructions file and security review skill exist).

---

## Your Task

1. **Create a testing skill with supporting resources.** In `.github/skills/`, create a new skill called `api-testing` that teaches Copilot how to write tests for this repo's API. Go beyond just a `SKILL.md` — add a supporting resource file to the skill folder (for example, a test template or an example test). Reference the resource from the `SKILL.md` using a relative path.

2. **Install a community plugin.** Browse the available plugins in the Awesome Copilot marketplace (search for `@agentPlugins` in the Extensions view, or install from source using `Chat: Install Plugin From Source` in the Command Palette). Pick a plugin that is relevant or interesting. Review its contents before installing.

3. **Test your setup.** Invoke your new testing skill via the `/` slash command in chat and verify that Copilot uses both the instructions and the supporting resource. Also confirm the plugin's skills or agents appear in the Configure Skills / Configure Tools menus.

## Success Criteria

- `.github/skills/api-testing/SKILL.md` exists with `name`, `description`, and testing instructions
- The skill folder contains at least one supporting resource file referenced from the `SKILL.md`
- A community plugin is installed and its capabilities are visible in chat
- You can invoke `/api-testing` as a slash command and get a relevant response
