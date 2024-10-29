
## Welcome
Wave Function Collapser is a Unity package for solving the Wave Function Collapse algorithm (WFC) with a code first solution. If you are looking for information on how the WFC algorithm works in general, check out these amazing videos by [Oskar Stalberg](https://youtu.be/0bcZb-SsnrA) and [Martin Donald](https://www.youtube.com/watch?v=2SuvO4Gi7uY). If you'd rather do some reading, checkout this [overview](./Wave-Function-Collapse). 

## Installing 
The code is available on the Unity Asset Store. After you import the package into your project, you should be all set. There is a folder named `com.brewedink.wfc`. Do not rename the folder.

## Quick start
If you want to verify things are working, you should check out the example scenes included in the package. You can open the help page by going to `Help/Wave Function Collapser`. See the documentation examples here...
* [1 - Color based Constraints](./Example-01----Color-Constraints)
* [2 - Socket based Constraints](./Example-02---Socket-Constraints)

## Code First
The hardest thing you'll have to do as you integrate the Wave Function Collasper into your project, is create your own modules and constraints. The asset is meant to solve the WFC algorithm in a general sense, which means you need to provide your own specific implementations of constraints and modules. The Wave Function Collapser comes with a few standard constraints and modules, but you should be ready to create your own.

The entry point to the WFC is the [Generation Space](../blob/main/CodeDocs/GenerationSpace.md) class. It is responsible for maintaining all of the [slots](./Wave-Function-Collapse#slot) and [modules](./Wave-Function-Collapse#module) possibilities as the algorithm runs. When you create a [Generation Space](../blob/main/CodeDocs/GenerationSpace.md), it starts out every slot with all module possibilities. There are methods on the class itself to run the WFC algorithm step by step, or to solve the entire space. 

#### Custom Modules and Constraints

Before you can create a [Generation Space](../blob/main/CodeDocs/GenerationSpace.md), you need to have a set of [modules](./Wave-Function-Collapse#module) with [constraints](./Wave-Function-Collapse#module-constraints). You can use whatever you want for modules. You should feel free to subclass the [Module class](../blob/main/CodeDocs/Module.md) and use those. The only requirement of a subclassed Module, is that you **Must implement the HashCode and Equals method**

#### The Generation Space
You can create a Generation Space with the utility method, [From2DGrid](../blob/main/CodeDocs/GenerationSpace_From2DGrid(int_int_ModuleSet_Nullable_int__Action_List_Slot__List_SlotEdge__).md)
```csharp
var space = GenerationSpace.From2DGrid(
 3,       // the width of the grid
 3,       // the height of the grid
 modules, // the modules that every slot will start with
 seed:24  // an optional random seed that controls how random collapses happen
);
```

Or you can create a [Generation Space](../blob/main/CodeDocs/GenerationSpace.md) by hand using the [constructor](../blob/main/CodeDocs/GenerationSpace_GenerationSpace(List_Slot__List_SlotEdge__ModuleSet_Nullable_int_).md).
```csharp
var space = new GenerationSpace(
 slots,   // all of the slots in the generation space. Similar to nodes in a directed graph.
 edges,   // all of the edges that connect slots. Similar to edges in a directed graph.
 modules, // the modules that every slot will start with
 seed:24  // an optional random seed that controls how random collapses happen
);
```

#### Running the Algorithm
Once you have a Generation Space set up, you can run the WFC algorithm on it. There are a few ways to run the algorithm but they all generally work the same way. You can either collapse a single Slot at a time, which may be helpful to author constraints onto the outcome, or you can collapse the entire space at once. The simplest use case is to collapse the entire space all at once.

All of the methods that collapse the wave function return enumerable sets of [WFCProgress](../blob/main/CodeDocs/WFCProgress.md). The methods use the [C# generator pattern](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/yield). Each instance of WFCProgress will tell you about the most recent action taken in the WFC algorithm. You can inspect the instance to see if a Module was removed from a Slot's superposition, or if an error happened, or if the algorithm is just continuing to run. When you call any `Collapse` method on the Generation Space, the operation won't be completed until you completely iterate through the resulting `IEnumerable<WFCProgress>`. An easy way to do that is with a `foreach` loop. This also provides you an easy way to perform the algorithm at a rate that fits your desired framerate. In the code sample below, the entire Generation Space is collapsed all at once, with no render frames or pausing. Depending on the size of the Generation Space, this could cause a visible lagtime in the game.

```csharp
var collapseOperation = space.Collapse();
foreach (var progress in collapseOperation) 
{ 
   // you can inspect the progress variable, or yield a render frame, or immediately proceed to the next progress operation.
   switch (progress) {
       case SlotModuleRemoved removed:
            break; // a module was removed from a Slot's superposition
       case WFCError error:
            break; // something went wrong
       default:
            break; // a non-interesting progress event. The algorithm is working hard...
   }  
}
```

A better way to run the WFC algorithm would be to run it inside a Unity coroutine. In the sample code below, every progress element is interleaved with a render frame. This means that the game won't lag or pause, but that the algorithm will take much longer in realtime to finish.
```csharp
public class ExampleBehaviour : MonoBehaviour {
 
   public GenerationSpace space;

   void Start() {
       StartCoroutine(RunWFC());
   }
   
   IEnumerator RunWFC() {
       foreach (var _ in space.Collapse()){
            yield return new WaitForEndOfFrame();
       }
   }
}
```

However, spinning up and managing your own `Coroutine` every time time you want to invoke a collapse operation can be tedious. There is a utility method that will let you run a collapse operation in a `Coroutine`, control how much time per frame is allocated to the algorithm, attach callbacks to various events, and inspect the state of the operation asynchronously. In the example below, the [RunAsCoroutine](../blob/main/CodeDocs/WFCProgressExtensions_RunAsCoroutine(IEnumerable_WFCProgress__MonoBehaviour_float).md) method converts a set of `IEnumerable<WFCProgress>` into a [WFCProgressObserver](../blob/main/CodeDocs/WFCProgressObserver.md)

```csharp
public class Example : MonoBehaviour {
 
   public GenerationSpace space;
   private WFCProgressObserver _handle;

   void Start() {
        _handle = space.Collapse()
            .RunAsCoroutine(this, frameBudgetTime)
            .OnSelectedModule<SpriteConfigModule>((slot, module) =>
            {
                // a module of `SpriteConfigModule` has been selected
            })
            .OnCompleted(() =>
            {
                // the operation is complete.
            });
   }

    void Update()
    {
        if (_handle.IsComplete)
        {
            // the operation is complete
            var selections = _handle.SlotSelections.Count;
            var operations = _handle.ProgressCount;
        }
    }
}


```