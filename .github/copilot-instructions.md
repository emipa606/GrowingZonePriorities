# GitHub Copilot Instructions for Growing Zone Priorities (Continued)

## Mod Overview and Purpose

**Growing Zone Priorities (Continued)** is a mod for RimWorld that enhances the management of growing zones by allowing players to set priorities for sowing and harvesting. This mod covers various planting areas including plant pots and hydroponic basins. The goal is to provide a more nuanced control over agricultural production processes, ensuring the most critical zones are attended to first.

### Key Features and Systems
- **Zone Priority Management**: Define the priority levels for planting and harvesting in different zones.
- **Multi-zone Configuration**: Ability to apply priority settings across multiple zones and plant containers simultaneously.
- **User Interface Enhancements**: Integrated gizmos for intuitive priority adjustments.
- **Compatibility & Performance**: Simplified codebase reduces performance impact and minimizes incompatibility with other mods.
- **Bug Fixes**: Resolves issues such as incorrect crop planting.

## Coding Patterns and Conventions

- **Static Classes**: Utilized throughout the codebase for classes like `Zone_Deregister`, `Zone_Growing_ExposeData`, and `Zone_Growing_GetGizmos` that do not maintain instance data but operate on zone data collectively.
- **Internal Access Levels**: Used selectively for classes that should not be exposed outside the mod's context, visible in classes like `Command_GrowingPriority` and `PriorityIntHolder`.
- **Comprehensive Method Naming**: Methods are clearly named to reflect their purpose, enhancing readability and maintainability, e.g., `GetGizmos` in various classes.
- **Code Simplification**: Recent updates focus on streamlining logic to improve both performance stability and compatibility.

## XML Integration

The mod utilizes XML for data-driven aspects primarily concerning game content extensions such as defining new gizmos or adjusting existing game behaviors. Ensure XML files are validated and properly integrated with the in-game defs to reflect the new priority functionalities.

## Harmony Patching

Harmony patches play a critical role in modifying the base game functionality without altering the original game code. In this mod:
- Harmony is used to inject methods or replace them within the game’s logic, specifically related to zone priority management.
- The `HarmonyPatches.cs` file centralizes all patches, making maintenance and updates more manageable.

## Suggestions for Copilot

- **Code Suggestions**: If adding new features, Copilot can help generate utility methods such as priority checking or UI enhancements based on existing patterns.
- **Error Handling**: Suggest improvements in error handling around data exposure and synchronization as shown in classes like `Zone_Growing_ExposeData`.
- **Performance Enhancements**: Assist with optimizing logic, especially when modifying large datasets or iterating over zones.
- **Integration Tips**: Ensure that XML definitions align with C# code features, and provide prompts for synchronization between XML and C#.
- **Refactoring**: Recommend refactoring opportunities that align with the simplified and efficient approach outlined in the changelog (v1.1.0.0).

By following these guidelines and suggestions, developers can effectively extend and maintain the Growing Zone Priorities mod. Copilot can greatly assist in streamlining development workflows, promoting consistency, and identifying optimization opportunities.
