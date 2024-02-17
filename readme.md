# Geoforce
Welcome to this open source Unity game! Hopefully it will provide a nice, clean look at a "full game" made with Unity. I'll be updating it over time to make it more of a complete project, but for now this is the jam version playable on the [Wayfarer Games Itch Account](https://wayfarergames.itch.io/geoforce). Newgrounds link coming soon (ish).

Download it, break it, add stuff to it - the main branch is locked, but you're welcome to branch and make pull requests. Pull requests will be reviewed and commented on to ensure they meet the standards of the project.
___
## Getting Started
The project was made with Unity 2023.2.3f1, and will upgrade to newer versions as they come out. [You can download Unity here](https://unity.com/).

You'll need to install LeanPool from the [Unity Asset Store](https://assetstore.unity.com/packages/tools/utilities/lean-pool-35666) - this is used for object pooling, it's a free assets and is used to spawn and despawn objects in the game.

When you're doing anything to the game, create a branch and make a pull request. I'll then do a code review and make comments. **Make lots of pull requests!** It doesn't matter if you think they're bad, it doesn't matter if you've just changed one tiny little thing - the more pull requests the better. It's a great way to learn, and it's a great way to get feedback on your code.  
___
## Project Structure
The project is structured so assets are grouped by the thing they are used for. Some examples:
- `Entity` contains assets that are shared between the game's entities, e.g. a script to track health, and take damage when hit by a particle bullet.
- `Enemies` contains all assets related to enemies, e.g. scripts such as `PopOutAndShoot` which controls the behaviour for the turtle enemy.
- `Player` contains assets related to the player, e.g. `MoveOnShoot` which moves the player when a shot is fired.
___
## Best Practices
The project is built with best practices in mind, and is designed to be a good example of how to structure a Unity project. Some examples:
- Use standard C# naming schemes - e.g. `PascalCase` for classes, `camelCase` for variables, etc.
  - More info at [Microsoft's C# naming conventions](https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/coding-style/identifier-names)
- Use object pooling with [LeanPool](https://assetstore.unity.com/packages/tools/utilities/lean-pool-35666) to reduce the number of objects created and destroyed, which can cause performance issues.
- Use small, self contained scripts - e.g. `LookAtPosition` which will make an object look at a position, and nothing else. This can be used on multiple different enemy types, for example.
- Communicate via events - e.g. when the player shoots it sends an event, which can then be used to:
  - Play a sound effect
  - Play visual effects
  - Trigger an ability
  - Grab the attention of an enemy
  - etc.
  - Because this uses an event, all of these can be done without player scripts needing to be modified.
- Because this is a web game, performance is important. This is why we use the particle system to shoot bullets, and why we use object pooling.
- To get the player's position, you can make a reference to a SerialPosition, e.g. `[SerializeField] private SerialPosition playerPosition;` and assign the object at `Assets/Player/Assets/PlayerPosition`. This will give you the player's position, it will update when the player moves - use `playerPosition.Position` to get the position.
- Prioritise **interesting, unique** enemies and abilities. We don't want a bunch of enemies or abilities that serve the same purpose.
  - For me, it helps to design enemies based on "priority" - i.e. when seeing the group of enemies in a wave, the player should be able to immediately work out which ones to focus. Some examples:
    - a high priority enemy might be one that deals a lot of damage from a distance, and needs to be taken out quickly.
    - a low priority enemy might have low health and damage, that moves quickly and causes a bit of a nuisance without being overly dangerous.
    - a medium priority enemy might be one that shields other enemies - it's not dangerous itself, but it makes other enemies more dangerous.
    - an "aggressively low priority" enemy would be one that starts out inert, but once damaged it "hatches" into a much more dangerous enemy. The player will want to avoid this until they've cleared most of the other enemies.
___
## Community
Join the [Discord](https://discord.gg/Tt7TxTp)! It's a great place to ask questions, get help, and share your own projects.

There's a dedicated place to share your own projects and get feedback, with a custom bot that's based around helping you get and provide the best feedback.
___
## Credits
The free version of DOTween has been used, read more about it [here](http://dotween.demigiant.com/).

Pistol sound effect from [Happy Soul Music Library](https://happysoulmusic.com/fire-weapons-sound-effects/).

Background music by [Christian Royle](https://soundcloud.com/christian-royle).