# GitHub Copilot Instructions for the RimWorld Mod: Brrr and Phew (Continued)

## Mod Overview and Purpose

The "Brrr and Phew (Continued)" mod is an update from the original by Pelador. Its primary purpose is to enhance the game's AI to help colonists avoid the negative effects of environmental conditions such as hypothermia, heatstroke, toxic buildup, and cabin fever. The mod introduces a proactive approach by allowing early responses to these conditions, thus improving the overall survival experience and quality of life for colonists within RimWorld.

## Key Features and Systems

### Environmental Management
- **Configurable Early Response:** React to initial buildup of conditions (hypothermia, heatstroke, toxic buildup) at a customizable percentage (10%-30%) instead of the default 35% from vanilla.
- **Rest and Recovery:** Pawns will seek out their owned bed, an unoccupied bed, or even the floor to recover from environmental conditions, ensuring that recovery does not require a full sleep cycle.
- **Toxic Buildup Management:** The "Yuk" feature allows for recovery from toxic buildup with adjustable safety levels and potentially longer recovery times than temperature-related conditions.

### Unique Pawn Behaviors
- **Cabin Fever Relief ("Ooh")**: Provides pawns with cabin fever the opportunity to take outdoor breaks, either through wandering or sky watching, which contributes to their "outdoors" need without fully converting the activities into standard joy activities.

### Animal Behavior
- **Animal Mirroring:** Colony animals also adopt reactive behaviors to environmental conditions ("Brrr", "Phew", and "Yuk"), providing they have accessible areas to retreat to.

### Mod Options
- **Toggleable Conditions:** Enable or disable AI behavior for each condition and set the recovery initiation threshold.
- **Joy Activity Configuration:** Decide the allowance and level of joy activities during recovery periods.
- **Animal Behavior Settings:** Enable or disable mirrored animal behavior for environmental reactions.

### Compatibility and Support
- **Safe to Add/Remove:** This mod can be added or removed from save games as it modifies only thought and job behavior.
- **Multiplayer Support:** Offered in beta form.

## Coding Patterns and Conventions

- The project follows typical C# conventions, such as PascalCase for class names and methods.
- Class names are descriptive of their purpose, e.g., `JobDriver_BrrrRecovery` indicates a driver handling the Brrr recovery process.

### XML Integration

- XML files should be used to configure mod options accessible in-game and to define any necessary game data related to job definitions and AI behavior.

### Harmony Patching

- If required, use Harmony for non-invasive patches to methods in the base game, ensuring the mod operates seamlessly alongside existing game functionality.

## Suggestions for Copilot

1. **Pattern Recognition:** Use Copilot for generating repetitive methods, especially when creating new condition checks or job drivers.
   
2. **Harmony Setup:** Utilize Copilot to scaffold Harmony patches, including methods like `Prefix` and `Postfix` to augment base game methods where necessary.

3. **XML Data Management:** Auto-generate XML templates for new condition settings and mod options using Copilot.

4. **ThinkNode and JobGiver Logic:** Employ Copilot to assist in writing the logic-heavy conditional checks within ThinkNodes and job assignments.

By following these instructions, contributors can effectively navigate the codebase and utilize GitHub Copilot to aid development and maintenance of the "Brrr and Phew (Continued)" RimWorld mod.
