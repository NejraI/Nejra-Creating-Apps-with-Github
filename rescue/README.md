# Rescue files — Lab 2 completed

These files contain the completed `/summary` endpoint with the `by_tool` grouping already implemented. Use them if you run out of time during the Lab 2 core sprint and need to move on to Lab 3.

## How to use

Copy the file for your language into the matching project folder, replacing the original:

| Language | Copy from | Copy to |
|----------|-----------|---------|
| Python   | `rescue/python/app.py` | `python/app.py` |
| Node.js  | `rescue/node/server.js` | `node/server.js` |
| .NET     | `rescue/dotnet/Program.cs` | `dotnet/Program.cs` |

Then commit and push so Lab 3 CI picks up the change:

```bash
git add .
git commit -m "Add by_tool grouping to summary endpoint"
git push
```
