# TODO
List of things to do for AxisEngine development

<hr/>

## High Priority
These objects should be completed before work on anything else can continue. Either because other development relies on them, or they merit completion on their own.

### Start making a test game
The features of the engine have only been tested independently and in very controlled environments. We need to make sure that all features will work together before adding other features.
* Test all features that are currently implemented
    * physics
    * animation
        * transitioning animations
        * play-till-end animations
    * parent/child object relationships
    * debug features
* Once all of that works and is implemented
    * get world manager/loading/disposing up and working
    * brush up the physics to make it more customizable
    * add a UI framework

### World
* test the world manager in the test bed. add loading/disposing of worlds.
* ability to change between worlds
  * loading/disposing of worlds

<hr/>

## Medium Priority
These are things that need to get done at some point, but should not be focused on until there are no items in High Priority.

### CollisionManager and Physics Engine
* ability for collision between colliders (BoxCollider, CircleCollider)
* ability to detect trigger entry/exit and fire events accordingly
* classes for collisions to hold data about the collision
* allow for a dynamic gravity value (right now, it is hardcoded in)
    * remove friction all together so that top-down/sidescrolling physics can happen independently

<hr/>

## Low Priority
These are the kinds of things that should get done at some point or have just been written down as ideas that need more thought before completing.

### Create/Finish API Documentation
* A documentation/API for AxisEngine needs to be completed on the wiki.
* Not important when its done, as long as it is complete before using AxisEngine before making any actual games with it.

<hr/>

## New Features
These features have not been started yet, and are really just notes on ideas that should be remembered.

### UI Framework
* elements should be parentable with a "pane" object to exist as their base in the world
* elements to add
    * buttons
    * text fields
    * color picker ?
    * tab control
    * slider
* customizable images for the elements
* make all object clear and easy to use

### Templating
* it would be awesome if there were a way to make an xml template file for building worlds and layers.
* Visual Studio AxisEngine Project Template
* Predefined game class template
    * for the game
    * for worlds
    * for layers
    * for worldobjects

<hr/>
