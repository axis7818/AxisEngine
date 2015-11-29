# TODO
List of things to do for AxisEngine

## Start making a test game
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

## World
* test the world manager in the test bed. add loading/disposing of worlds.
* ability to change between worlds
  * loading/disposing of worlds

## CollisionManager and Physics Engine
* ability for collision between colliders (BoxCollider, CircleCollider)
* ability to detect trigger entry/exit and fire events accordingly
* classes for collisions to hold data about the collision
* allow for a dynamic gravity value (right now, it is hardcoded in)
    * remove friction all together so that top-down/sidescrolling physics can happen independently

## UI Framework
* elements should be parentable with a "pane" object to exist as their base in the world
* elements to add
    * buttons
    * text fields
    * color picker ?
    * tab control
    * slider
* customizable images for the elements
* make all object clear and easy to use

## XML Templating?
* it would be awesome if there were a way to make an xml template file for building worlds and layers.

## Predefined game class template
* for the game
* for worlds
* for layers
* for worldobjects
