# GDD traceability matrix — Lesson 02

| Requirement / Reference | Implementation / Artifact | Verification / Evidence | Status | Notes |
| --- | --- | --- | --- | --- |
| AGB-FR-008 | `Room.Items`, `PlaceItem`, `ShowRoom` | Room-collection test; Gallery LOOK | VERIFIED | Several collectible items in one room. |
| AGB-FR-009 | `Player.TryTake`, TAKE branch | Transfer and rejection tests; manual TAKE | VERIFIED | Current-room item selection. |
| AGB-FR-010 | `Player.Inventory`, `ShowInventory` | Multi-item test; empty and populated INVENTORY | VERIFIED | Multi-item possession and display. |
| AGB-FR-011 | `Player.TryDrop`, `Room.PlaceItem`, DROP branch | Drop/retake test; manual route | VERIFIED | Transfer to the current room. |
| AGB-RULE-001 | Initial placement and transfer operations | Source/destination membership checks | PARTIAL | Sample items occupy intended locations; cross-room setup duplicates are not prevented. |
| AGB-RULE-002 | `TryTake`, `TryDrop` | Transfer and rejection tests | VERIFIED | Successful operations move the original reference. |
| AGB-RULE-003 | Persistent room and player collections | Travel/drop/retake tests; manual route | PARTIAL | Item changes persist in this lesson's world state. |
| AGB-FR-002 | Extended `ShowRoom` | LOOK in the three visible sample rooms | PARTIAL | Dark-room visibility is not implemented. |
| AGB-FR-001 | Inherited `Room.Connect` | Room-connection regression tests | VERIFIED | Three interconnected rooms remain available. |
| AGB-FR-003 | Inherited `TryMove`, GO | Movement regression tests and manual route | PARTIAL | Ordinary exits work; closed-door blocking is not implemented. |
| AGB-FR-004 | Inherited `TryMove`, GO | Invalid-direction and absent-exit tests | PARTIAL | Ordinary invalid moves reject; doors and other blockers are not modeled. |
| AGB-NFR-001 | Console command loop | Manual command scenarios | VERIFIED | Current features use a text interface. |
| AGB-NFR-002 | `TryTake`, `TryDrop`, console messages | Success and rejection scenarios | PARTIAL | Supported commands provide explanatory feedback. |
| AGB-NFR-003 | Input and target checks, EOF handling | Inherited input tests; malformed-command scenarios | PARTIAL | Supported invalid commands and targets are handled. |

Evidence (2026-11-16):

- Lesson 01: build succeeded with 0 warnings and 0 errors; 22 tests passed; movement, invalid commands, and room displays checked.
- Lesson 02: dotnet build -m:1 -p:NuGetAudit=false succeeded with 0 warnings and 0 errors; dotnet test --no-build -m:1 passed 33 tests, 0 failed.
- All five manual scenarios in Lesson 02 were executed successfully, including empty/populated inventory, transfer, drop/travel/retake, rejected requests, and case-insensitive multiword names. EOF exits cleanly.
- NuGet audit was disabled for the build command because the sandbox could not reach api.nuget.org. Tests ran with local process communication allowed.
