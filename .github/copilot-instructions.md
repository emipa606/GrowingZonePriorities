# GitHub Copilot Instructions for Growing Zone Priorities (Continued) Mod

## Mod Overview and Purpose

Growing Zone Priorities (Continued) is an updated version of the original Ilyakis mod. This mod allows players to set priorities for which growing zone to sow and harvest first, making plant management more efficient. It supports various types of plant-growing structures, including plant pots and hydroponics. The mod is save-friendly and includes performance improvements to reduce potential compatibility issues.

## Key Features and Systems

- **Priority Setting**: Players can set priorities for multiple zones or pods simultaneously.
- **Plant Management**: The mod enhances the management of plant-growing zones, ensuring that crops are harvested and sown in an orderly manner.
- **Compatibility**: The mod is compatible with various other mods, but performance improvements have been made to reduce potential conflicts.
- **Bug Fixes**: Version 1.1.0.0 includes fixes for issues such as colonists planting the wrong crops.

## Coding Patterns and Conventions

- **Harmony Patching**: The mod heavily relies on Harmony, a popular modding framework for RimWorld. Harmony patches are used to modify existing code behavior without altering the original game files.
- **C# Structure**: The C# code is organized into classes that represent different aspects of the mod, such as zone deregistration, plant grower gizmos, and exposure data handling.
- **Postfix Patches**: Postfix patches are commonly used to add functionality after the original method execution. This is particularly useful for modifying behavior without completely replacing existing code.

## XML Integration

Growing Zone Priorities (Continued) does not include any custom XML definitions. However, the mod interacts with existing XML definitions provided by RimWorld. Understanding how these XML files work is essential for maintaining compatibility with the mod.

## Harmony Patching

Harmony is a critical component of the mod. The following Harmony patch classes are used in the mod:

- **HarmonyPatches**: This class contains the main harmony patches for the mod.
- **Zone_Deregister**: This class handles the deregistration of zones.
- **Building_PlantGrower_GetGizmos**: This class modifies the gizmos available for plant growers.
- **Building_PlantGrower_DeSpawn**: This class handles the despawning of plant growers.
- **Building_PlantGrower_ExposeData**: This class manages the serialization and deserialization of plant grower data.

To effectively use GitHub Copilot for this mod, consider the following suggestions:

- **Harmony Patching**: When generating patches, ensure that they are correctly integrated with Harmony to avoid conflicts with the original game code.
- **C# Conventions**: Follow the existing coding patterns and conventions used in the mod, such as using postfix patches where appropriate.
- **Testing**: After generating code, thoroughly test the mod to ensure that it works as intended and does not introduce new bugs.
- **Performance**: Pay attention to the performance impact of the generated code, especially if it involves complex logic or interactions with other mods.

## Suggestions for Copilot

- **Harmony Integration**: When generating Harmony patches, ensure that they are correctly integrated with the existing Harmony framework used in the mod.
- **C# Conventions**: Follow the existing coding patterns and conventions used in the mod, such as using postfix patches where appropriate.
- **Testing**: After generating code, thoroughly test the mod to ensure that it works as intended and does not introduce new bugs.
- **Performance**: Pay attention to the performance impact of the generated code, especially if it involves complex logic or interactions with other mods.

## Project Solution Guidelines
- Relevant mod XML files are included as Solution Items under the solution folder named XML, these can be read and modified from within the solution.
- Use these in-solution XML files as the primary files for reference and modification.
- The `.github/copilot-instructions.md` file is included in the solution under the `.github` solution folder, so it should be read/modified from within the solution instead of using paths outside the solution. Update this file once only, as it and the parent-path solution reference point to the same file in this workspace.
- When making functional changes in this mod, ensure the documented features stay in sync with implementation; use the in-solution `.github` copy as the primary file.
- In the solution is also a project called Assembly-CSharp, containing a read-only version of the decompiled game source, for reference and debugging purposes.
- For any new documentation, update this copilot-instructions.md file rather than creating separate documentation files.


## Hard rules (must follow)
- Do NOT run commands that modify the repo (no git commit, git apply, dotnet format) unless explicitly asked.
- Prefer minimal reads: read only the smallest code region needed (around the suspicious lines).
- When mentioning SonarQube issues, automatically use the SonarQube MCP service to fetch and address issues instead of making inferred fixes without querying SonarQube first.
- When mentioning the rimworld log, automatically use the Rimworld MCP service to fetch the log.

