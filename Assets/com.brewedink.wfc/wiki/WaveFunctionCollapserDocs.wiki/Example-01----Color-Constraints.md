# A simple world with colors
[Watch the Youtube Video](https://www.youtube.com/watch?v=os_-XyYz34E)
![](./images/example_colors_banner.gif)

## Finding the Example
The Color example can be opened by opening the `Help/Wave Function Collapser` menu item in Unity, and selecting the first available example.

## Basic Overview
It can be helpful to identify the WFC primitives in the example. The [Slots](./Wave-Function-Collapse#slot) are each grid cell. The [Modules](./Wave-Function-Collapse#module) are all the possible sprites that could go into each grid cell. The [Constraints](./Wave-Function-Collapse#module-constraints) are created automatically from the [WFC Config](../blob/main/CodeDocs/WCFConfigObject.md) Scriptable Object. The Sprite Sheet configuration that is being used in this example uses the colors along the edges of a module's sprite to determine what valid module neighbors can be. If a sprite had a red right edge, it couldn't be placed next to a module with a green left edge. 

## Configuration Options
The Sprite Sheet configuration has several configuration options. You can see what each option does by referring to the table below...
| Option | Type | Description |
| ----------- | ----------- | - |
| Use Seed | bool | `true` if every generated [Generation Space](../blob/main/CodeDocs/GenerationSpace.md) should use the same seed. |
| Seed     | int | if `UseSeed` is `true`, what number should be used to seed the random number generator |
| Width | int | the x-dimension of the generated [Generation Spaces](../blob/main/CodeDocs/GenerationSpace.md) |
| Height | int | the y-dimension of the generated [Generation Spaces](../blob/main/CodeDocs/GenerationSpace.md) |
| Sample Count | int | how many texture samples to take along each edge of each module's sprite |
| Sample Spacing Ratio | float | how compressed are the texture samples? |
| Sample Spacing Offset | float | are the texture samples offset? |
| Sample Tolerance | float | how different can texture color samples be and still be considered equal? |
| Sample Left Padding | int | for samples taken for the left edge, how many pixels in from the left edge should the samples be taken? |
| Sample Right Padding | int | for samples taken for the right edge, how many pixels in from the right edge should the samples be taken? |
| Sample Top Padding | int | for samples taken for the top edge, how many pixels in from the top edge should the samples be taken? |
| Sample Low Padding | int | for samples taken for the bottom edge, how many pixels in from the bottom edge should the samples be taken? |

## Setting up the GameObject
The main Gameobject in the scene is a `WFCBoard` object. It draws all of the module possibilities per slot. The object is also responsible for handling user interactions, and running the [collase](../blob/main/CodeDocs/GenerationSpace_Collapse().md) operations on the [Generation Space](../blob/main/CodeDocs/GenerationSpace.md)
You can make your own `WFCBoard` and assign it whatever configuration you'd like by creating one from the Hierarchy window. Right click in the Hierarchy window and select `BrewedInk WFC/2D Grid Renderer`