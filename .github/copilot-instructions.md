# Copilot Instructions for "Brrr and Phew (Continued)"

## Mod Overview and Purpose
The "Brrr and Phew (Continued)" mod enhances the AI behavior in RimWorld to help colonists and animals avoid the adverse effects of environmental conditions such as hypothermia, heatstroke, toxic buildup, cabin fever, vacuum exposure (Odyssey DLC), and blood rain exposure (Anomaly DLC). The primary aim is to introduce a "common sense" approach, allowing for preventive actions before conditions become severe.

## Key Features and Systems
- **Early Response Mechanism:** Configurable threshold for colonists to respond to environmental hazards like hypothermia and heatstroke between 10% and 30% buildup, avoiding severe outcomes.
- **Recovery Behaviors:** Pawns will seek shelter or rest in a temperature-appropriate location until conditions improve.
- **Toxic Buildup Management:** Implements proactive measures for toxic exposure with longer recovery times.
- **Cabin Fever Mitigation:** Offers outdoor recreation opportunities, like wandering or sky watching, to relieve cabin fever while controlling fulfillment rates.
- **Animal Behavior Mirroring:** Animals replicate colonist responses, factoring in obedience and area restrictions.
- **Mod Options:** Toggle AI behaviors per condition, set recovery initiation levels, manage joy interruptions, and configure animal response.

## Coding Patterns and Conventions
- Follow PascalCase for class and method names.
- Use meaningful names for classes such as `JobDriver`, `JobGiver`, and `ThinkNode`.
- Implement clear class hierarchies using base classes like `JobDriver` and `ThinkNode_Conditional` for consistency.
- Ensure methods are concise and have single responsibilities.
- Prefer composition over inheritance where suitable.

## XML Integration
- Use XML files for defining game data such as `JobDefs` to ensure flexibility and easy modifications.
- Structure XML files with clear hierarchies and include comments for clarity and documentation.
- Ensure all XML definitions correspond to C# classes where logic is implemented.

## Harmony Patching
- Employ Harmony patches to extend or modify existing game logic without altering the core game mechanics.
- Target methods that trigger environmental effects, applying Prefix or Postfix patches as needed.
- Use descriptive method names for patches and include comments to explain the purpose and behavior of each patch.

## Suggestions for Copilot
- Use Copilot to suggest boilerplate code for job drivers and think nodes, ensuring consistency in the implementation.
- Automate repetitive code generation tasks, such as XML file structure and patch templates.
- Provide context-aware suggestions for method logic within job drivers, especially when managing environmental condition checks.
- Utilize Copilot to quickly prototype new features or tweak existing functionalities, especially when expanding mod options or adding new DLC support.

With this guide, you will have a comprehensive understanding of the mod's framework, coding standards, and integration techniques. Utilize GitHub Copilot to streamline the development process, ensuring consistency and efficiency in managing RimWorld AI behaviors.

## Project Solution Guidelines
- Relevant mod XML files are included as Solution Items under the solution folder named XML, these can be read and modified from within the solution.
- Use these in-solution XML files as the primary files for reference and modification.
- The `.github/copilot-instructions.md` file is included in the solution under the `.github` solution folder, so it should be read/modified from within the solution instead of using paths outside the solution. Update this file once only, as it and the parent-path solution reference point to the same file in this workspace.
- When making functional changes in this mod, ensure the documented features stay in sync with implementation; use the in-solution `.github` copy as the primary file.
- In the solution is also a project called Assembly-CSharp, containing a read-only version of the decompiled game source, for reference and debugging purposes.
- For any new documentation, update this copilot-instructions.md file rather than creating separate documentation files.
