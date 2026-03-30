# Lab 3 — Challenge Self-Directed: More Jobs, Linting, Dependencies, and Breaking CI

**Goal:** Add CI jobs for the other languages, enable linting, add a summary job that depends on all language jobs, and demonstrate that CI catches deliberate errors.

**Time:** 15 minutes

**You will need:** Lab 3 Core completed (CI green on your PR).

---

## Your Task

1. **Enable linting**. Uncomment or add the lint step for your language in `ci.yml`. Fix any lint errors that come up.
2. **Break CI on purpose**. Introduce a deliberate syntax error in your API file (e.g., remove a closing bracket), push it to a new branch, open a PR, and verify CI catches the error. Fix it and get back to green.

## Success Criteria

- CI jobs exist for all three languages, each following the same structure
- A lint step runs as part of CI for your chosen language
- A `ci-passed` summary job depends on the three language jobs using `needs`
- You have a PR where CI correctly failed on a deliberate bug
- After fixing, CI is green again
