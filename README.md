# Knight Adventures

## Ownership
This project is developed by Bogdan-Alexandru Caplan for CSC3232 module within Newcastle University.

## To properly start the game, open "Scenes" folder and select "MainMenu".

### Coursework

● For Newtonian Physics, I have created multiple player movement attributes, which includes moving, adjusted jumping, coyote jumping, crouching, attacking, firing projectiles, double jump and blocking. For advanced usage, player can dash when using a certain key and if the player is interacting with a wall will allow player to wall jump, both will change gravity accordingly. All collectables have gravity.

● Collision Detection was achieved by using multiple objects, projectiles and enemies colliders interacting with the player. I have used simple traps, traps that fire projectiles, a special trap that will follow the player when it sees them within chosen range, props that will help player reach certain destinations (trampoline and falling platforms) as well as breakable walls and boxes. For specfic hit areas, I have created normal platforms that can be accessed from below and activate collider when on top, as well as when double tapping S or Down Arrow, will allow the player to go through the platform. More, I have created a moving platform using two separate box colliders, one for trigger and the other for collision, that will stick the player to it if accessed from above only. Player sword attack and block use a box collider for the range the player can attack or block.

● There are multiple ways Collision Response and Feedback is demonstrated. The above-mentioned moving platform which will let the player enter only from above and stick to it. Player can be damaged by enemies or traps, player can damage enemies. Projectiles will stop when they hit a wall and make them explode and dissapear. Breakable walls and boxes detect collision from both melee and ranged attack. For advanced usage, I have created physics material 2D properties for when a collectable hits the ground to bounce as well as for specific terrain such as ice or mud.

● For AI section, I have created a Menu System where the player can adjust settings, view controls and many more. I have created enemies that have different hierarchical state behaviour, patrol a chosen area, stop for a duration and will attack player upon detecting them. For making use of appropiate pathfinding, I have used A* algorithm to scan an area where the enemy will know if the player enters it and follow and attack them. For advanced usaged for pathfinding, by destroying breakable walls, A* refreshes the area scan and allow the AI to recognise the new path and allow the enemy to access it along with the player. For flocking, I have created a basic flocking which allow small birds to follow the player and keep distance between them.

● For probability, adversarial AI, and game design section, I have created a way where the enemies have a chance upon slaying to drop a collectable. I have created a lobby and four levels to give the player the feel of completing a level and returning to the safe place, along with implementing a score and highscore that will be tracked upon exitting the session. Each level contains checkpoints, that allows the player to return to a safe point upon dying, leading to less frustration rather than respawning from the start. The melon collectable rewards the player allowing the player to double jump for a certain duration, giving the player a positive feedback along with completing levels. Dust trail effect was added for more player satisfaction when changing directions and jumping.

## References and packages used

● Unity Assets used in this game project:
● Player: https://assetstore.unity.com/packages/2d/characters/knight-sprite-sheet-free-93897
● Enemy character: https://assetstore.unity.com/packages/2d/characters/dragon-warrior-free-93896
● Map and Objects: https://assetstore.unity.com/packages/2d/characters/pixel-adventure-1-155360
● In-Game Audio: https://gamesounds.xyz/?dir=No%20soap%20radio
● Sound Effects Audio: https://assetstore.unity.com/packages/audio/sound-fx/free-casual-game-sfx-pack-54116
● A* imported package: https://arongranberg.com/astar/docs_dev/index.php
