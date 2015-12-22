# AxisEngine
A 2D wrapper engine to monogame that adds Physics, Animation, an easy to use Object Management System, and much more.

<hr/>

## Goal

AxisEngine is being built to add features such as animation, physics, and an object management system to Monogame. The goal is to speed up the game creation process and remove the restrictive bounds of a single Game Loop from Monogame. Inspired by Unity, the WorldObject class allows objects to be created within the game world and linked to one another to form composite objects.

<hr/>

### Object Management
AxisEngine's biggest strength is its object management system. this system consists of 4 that structure a game in a hierarchy much like a folder structure. To understand and use AxisEngine, is is necessary to have an understanding of this structure.

#### WorldManager
The WorldManager is the link between AxisEngine and Monogame. To implement AxisEngine, a WorldManager only needs to be instantiated and placed within the base Game class to function. WorldManager then serves as the root of AxisEngine. <br/><br/>
The user will then create Worlds and add them to the WorldManager.

#### World
The World object holds everything that a player sees at any time in the game. This could be a splash screen, Level 1-1 of your game, a settings screen, or the end credits. Your game can have however many worlds it needs, but only one will be displayed on the screen at a time by the WorldManager. When a world comes to an end, it will be Unloaded, and the next world will be Loaded up by the WorldManager.

Within the World, there are Managers. These managers include
* CollisionManager
* TimeManager
* DrawManager

Each of these managers takes care of interactions between the objects in the world and are assigned to Layers.

#### Layer
Each World has a set of Layers. Layers provide a way to separate objects into groups within the world. For example, your bullet objects flying around the scene do not need to know about the UI elements. Depending on how the Managers are implemented in the World though, Layers can interact with each other. It might make sense to have a Backdrop layer, and Character layer, and a UI layer in some games.

The Layers themselves are then composed of WorldObjects.

#### WorldObject
The WorldObject is a single thing within the game. A health meter, a lightning strike, a goblin, etc. These objects are able to be nested within one another too, which means that there can be more layers of complexity beyond single WorldObjects if needed.

For example: To make a character, you might create its Sprite or Animator to display that character in the world. Both of these are WorldObjects and would be connected to the Layer. You might then add an InputManager WorldObject to be able to send user input to it, you would also want a Body object attached to it to allow it to have a physical presence within the world. Maybe you have made a machine gun object earlier that you want to attach to the character. Now he is complete!

This object management system will make it easier for programmers to think of individual parts of their game before worrying about how they will interact with the rest of the world.

<hr/>
