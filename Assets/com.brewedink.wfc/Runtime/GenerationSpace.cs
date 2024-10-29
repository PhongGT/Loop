using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;
using Debug = UnityEngine.Debug;
using Random = System.Random;

namespace BrewedInk.WFC
{
    /// <summary>
    /// An exception that is thrown when the WFC tries to remove the last remaining module from a slot's possibility space. A slot cannot have zero possibilities.
    /// </summary>
    public class SlotCannotHaveEmptyModuleSetException : Exception
    {
        public SlotCannotHaveEmptyModuleSetException(Slot slot, Module module) 
            : base($"Cannot remove module from slot. It would result in an empty slot. slot=[{slot.Coordinate}] module=[{module.Display}]")
        {
        }
    }

    /// <summary>
    /// An exception that is thrown if a slot tries to select a module that is no longer possible, due to constraints.
    /// </summary>
    public class SlotCannotSelectUnavailableModule : Exception
    {
        public SlotCannotSelectUnavailableModule(Slot slot, Module module)
        : base($"Cannot select option flor slot. It isn't availble. slot=[{slot.Coordinate}] module=[{module.Display}]")
        {
            
        }
    }

    /// <summary>
    /// A ModuleSet is a collection of modules. It is a HashSet, with a few helpful methods attached to it.
    /// The generic type in the hash set must be assignable from the Module type
    /// </summary>
    [Serializable]
    public class ModuleSet<TModule> : HashSet<TModule> where TModule : Module
    {
        public ModuleSet()
        {
            
        }

        public ModuleSet(IEnumerable<TModule> other) : base(other)
        {
            
        }

        public List<TModule> ModuleList => this.ToList();
        
        
        public TModule FindByDisplay(string display)
        {
            return this.FirstOrDefault(m => m.Display.Equals(display));
        }
        
        public TModule FindByDisplay(Module module)
        {
            return FindByDisplay(module.Display);
        }


        public ModuleSet ProduceConstraints(ConstraintGenerator<TModule> constraintGenerator)
        {
            return constraintGenerator.ProduceConstraints(this);
            // var originalSet = this.ToList();
            // var next = new ModuleSet();
            // foreach (var source in originalSet)
            // {
            //     var newConstraints = new List<ModuleConstraint>();
            //     if (constraintGenerator.CreateConstraints(source, out var singleConstraints))
            //     {
            //         newConstraints.AddRange(singleConstraints);
            //     }
            //     foreach (var target in originalSet)
            //     {
            //         if (constraintGenerator.CreateConstraints(source, target, out var constraints))
            //         {
            //             newConstraints.AddRange(constraints);
            //         }
            //     }
            //     var copy = constraintGenerator.Copy(source, newConstraints);
            //     next.Add(copy);
            // }
            // return next;
        }
    }
    
    /// <summary>
    /// A ModuleSet is a collection of modules. It is a HashSet, with a few helpful methods attached to it. 
    /// </summary>
    [Serializable]
    public class ModuleSet : ModuleSet<Module>
    {
        public ModuleSet()
        {
            
        }

        public ModuleSet(IEnumerable<Module> other) : base(other)
        {
            
        }
        
        public ModuleSet(params Module[] modules) : base(modules){}


        public ModuleSet ProduceConstraints<TModule>(ConstraintGenerator<TModule> constraintGenerator) where TModule : Module
        {
            var set = new ModuleSet<TModule>(ModuleList.Where(m => m is TModule).Cast<TModule>());
            return constraintGenerator.ProduceConstraints(set);
        }
    }

    public abstract class ConstraintGenerator<TModule> where TModule : Module
    {
        public abstract TModule Copy(TModule module, List<ModuleConstraint> constraints);

        public virtual List<ModuleConstraint> CreateConstraints(TModule source)
        {
            return null;
        }
        
        public abstract List<ModuleConstraint> CreateConstraints(TModule source, TModule target);

        public ModuleSet ProduceConstraints(ModuleSet<TModule> modules)
        {
            var originalSet = modules.ModuleList;
            var next = new ModuleSet();
            foreach (var source in originalSet)
            {
                var newConstraints = new List<ModuleConstraint>();

                var singleConstraints = CreateConstraints(source);
                if (singleConstraints != null)
                {
                    newConstraints.AddRange(singleConstraints);
                }
                foreach (var target in originalSet)
                {
                    var constraints = CreateConstraints(source, target);
                    if (constraints != null)
                    {
                        newConstraints.AddRange(constraints);
                    }
                }
                var copy = Copy(source, newConstraints);
                next.Add(copy);
            }
            return next;
        }
        // public ModuleSet ProduceConstraints<TModule>(ModuleSet modules) where TModule: Module
        // {
        //     var originalSet = this.ToList();
        //     var next = new ModuleSet();
        //     foreach (var source in originalSet)
        //     {
        //         var typedSource = source as TModule;
        //         var newConstraints = new List<ModuleConstraint>();
        //         if (constraintGenerator.CreateConstraints(typedSource, out var singleConstraints))
        //         {
        //             newConstraints.AddRange(singleConstraints);
        //         }
        //         foreach (var target in originalSet)
        //         {
        //             // TODO: The type checking here feels wrong...
        //             if (constraintGenerator.CreateConstraints(typedSource, target as TModule, out var constraints))
        //             {
        //                 newConstraints.AddRange(constraints);
        //             }
        //         }
        //         var copy = constraintGenerator.Copy(typedSource, newConstraints);
        //         next.Add(copy);
        //     }
        //     return next;
        // }
        
    }

    /// <summary>
    /// The <see cref="WFCProgress"/> class represents a unit of progress in the WFC algorithm. As the algorithm is run, a sequence of WCFProgress items will be returned.
    /// Different types of progress will be returned, and you can switch on the type to gain insights to the algorithm's decisions...
    ///
    /// </summary>
    public class WFCProgress
    {
        
    }

    /// <summary>
    /// A WFCError instance could be returned by the WFC algorithm. If you receive one of these instances, it means something with the process has broken.
    /// </summary>
    public class WFCError : WFCProgress
    {
        
    }
    
    /// <summary>
    /// A progress instance that explains what module was removed from a slot's possibility space.
    /// </summary>
    [System.Serializable]
    public class SlotModuleRemoved : WFCProgress
    {
        /// <summary>
        /// The slot that has had a module removed.
        /// </summary>
        public Slot slot;
        
        /// <summary>
        /// The module that is no longer available for the slot
        /// </summary>
        public Module module;

        public SlotModuleRemoved(Slot slot, Module module)
        {
            this.slot = slot;
            this.module = module;
        }
    }

    /// <summary>
    /// A progress instance that explains what module was selected for a slot. When this progress type shows up, it means the superposition for the given slot has collapsed to the given module.
    /// </summary>
    public class SlotModuleSelected : WFCProgress
    {
        /// <summary>
        /// The slot whose superposition has collapsed. 
        /// </summary>
        public Slot slot;
        
        /// <summary>
        /// The module that has been selected for the given slot
        /// </summary>
        public Module module;

        public SlotModuleSelected(Slot slot, Module module)
        {
            this.slot = slot;
            this.module = module;
        }
    }

    /// <summary>
    /// A wrapper class around the invocation of a WFC collapse operation.
    /// You should use this in conjunction with <see cref="WFCProgressExtensions.RunAsCoroutine"/>
    /// </summary>
    public class WFCProgressObserver
    {
        /// <summary>
        /// The number of progress events received for this operation. There isn't a knowable upper bound, but you can use this to track that progress is happening.
        /// </summary>
        public int ProgressCount { get; private set; }

        /// <summary>
        /// Returns true when the operation is complete. This will be true the instant BEFORE the onComplete callbacks are invoked.
        /// </summary>
        public bool IsComplete { get; private set; }

        private List<Action<WFCProgress>> _progressListeners = new List<Action<WFCProgress>>();
        private List<Action<SlotModuleSelected>> _selectionListeners = new List<Action<SlotModuleSelected>>();
        private List<Action> _completionListeners = new List<Action>();

        /// <summary>
        /// A set of selections you can use inspect to see what has finished. If you want an event driven approach, use <see cref="WFCProgressObserver.OnSelectedModule"/>
        /// </summary>
        public List<SlotModuleSelected> SlotSelections { get; private set; } = new List<SlotModuleSelected>();
        
        public WFCProgressObserver(out Action<WFCProgress> progressHandler, out Action completionHandler)
        {
            progressHandler = (progress) =>
            {
                ProgressCount++;
                Invoke(_progressListeners, progress);

                switch (progress)
                {
                    case SlotModuleSelected selection:
                        SlotSelections.Add(selection);
                        Invoke(_selectionListeners, selection);
                        break;
                }
            };

            completionHandler = () =>
            {
                IsComplete = true;
                Invoke(_completionListeners);
            };
        }

        private void Invoke<T>(List<Action<T>> listeners, T arg)
        {
            foreach (var listener in listeners)
            {
                try
                {
                    listener?.Invoke(arg);
                }
                catch (Exception ex)
                {
                    Debug.LogException(ex);
                    // continue...
                }
            }
        }
        private void Invoke(List<Action> listeners)
        {
            foreach (var listener in listeners)
            {
                try
                {
                    listener?.Invoke();
                }
                catch (Exception ex)
                {
                    Debug.LogException(ex);
                    // continue...
                }
            }
        }
        
        /// <summary>
        /// A callback that executes anytime the WFC has new progress. Progress elements can represent a module being removed, or a module being selected, an error, or a general propagation of data.
        /// </summary>
        /// <param name="onProgress">A callback to run on progress</param>
        /// <returns>The same WFCProgressObserver</returns>
        public WFCProgressObserver OnProgress(Action<WFCProgress> onProgress)
        {
            _progressListeners.Add(onProgress);
            return this;
        }

        /// <summary>
        /// A callback that executes when the WFC operation has completed. This happens right after the <see cref="WFCProgressObserver.OnComplete"/> is set to true.
        /// </summary>
        /// <param name="onComplete">a callback to run on completion</param>
        /// <returns>The same WFCProgressObserver</returns>
        public WFCProgressObserver OnCompleted(Action onComplete)
        {
            _completionListeners.Add(onComplete);
            return this;
        }

        /// <summary>
        /// A callback that executes anytime a slot collapses to a single module possibility. This is a type of <see cref="WFCProgress"/>, and will execute right after the <see cref="WFCProgressObserver.OnProgress"/> callbacks
        /// </summary>
        /// <param name="selection">A callback that takes a general selection. The module isn't typed, so you'll need to type-check it.</param>
        /// <returns>The same WFCProgressObserver</returns>
        public WFCProgressObserver OnSelectedModule(Action<SlotModuleSelected> selection)
        {
            _selectionListeners.Add(selection);
            return this;
        }

        /// <summary>
        /// A callback that executes anytime a slot collapses to a single module possibility. This is a type of <see cref="WFCProgress"/>, and will execute right after the <see cref="WFCProgressObserver.OnProgress"/> callbacks
        /// </summary>
        /// <param name="selection">A callback that takes a slot and a general selection. The module isn't typed, so you'll need to type-check it.</param>
        /// <returns>The same WFCProgressObserver</returns>
        public WFCProgressObserver OnSelectedModule(Action<Slot, Module> selection)
        {
            _selectionListeners.Add(evt => selection?.Invoke(evt.slot, evt.module));
            return this;
        }
        
        /// <summary>
        /// A callback that executes anytime a slot collapses to a single module possibility. This is a type of <see cref="WFCProgress"/>, and will execute right after the <see cref="WFCProgressObserver.OnProgress"/> callbacks
        /// </summary>
        /// <param name="selection">A callback that takes a slot and typed module. This callback only fires when a module of the given type is selected</param>
        /// <typeparam name="TModule">the type of module this callback applies for. </typeparam>
        /// <returns>The same WFCProgressObserver</returns>
        public WFCProgressObserver OnSelectedModule<TModule>(Action<Slot, TModule> selection)
        {
            _selectionListeners.Add(evt =>
            {
                if (evt.module is TModule typedModule)
                {
                    selection?.Invoke(evt.slot, typedModule);
                }
            });
            return this;
        }
    }

    /// <summary>
    /// A Utility class for IEnumerable sets of <see cref="WFCProgress"/>
    /// </summary>
    public static class WFCProgressExtensions
    {
        /// <summary>
        /// A method to run any WFC operation within a Unity Coroutine with frame time budgeting. This method will process as many elements in the WFC operation as possible in on frame, then yield a render frame, and continue the operation on the next frame.
        /// This is the recommended way to run WFC operations in a realtime game.
        /// This returns a <see cref="WFCProgressObserver"/> object which you can attach progress callbacks and inspect the running state.
        /// </summary>
        /// <param name="operation">Any WFC operation. This could be the return value from any Collapse related function in the <see cref="GenerationSpace"/></param>
        /// <param name="context">Some Monobehaviour to run the Coroutine in.</param>
        /// <param name="timeBudgetPerFrame">How much is available per frame to operate on the WFC sequence? This should be a low number, so that it doesn't cause hitches in your game.</param>
        /// <returns>a WFCProgressObserver</returns>
        public static WFCProgressObserver RunAsCoroutine(this IEnumerable<WFCProgress> operation, MonoBehaviour context, float timeBudgetPerFrame = .1f)
        {
            var observer = new WFCProgressObserver(out var notifier, out var completeNotifier);

            IEnumerator Routine()
            {
                yield return new WaitForEndOfFrame(); // give caller a chance to register methods...
                var t = Time.realtimeSinceStartup;
                foreach (var progress in operation)
                {
                    notifier?.Invoke(progress);
                    var d = Time.realtimeSinceStartup - t;
                    if (d > timeBudgetPerFrame)
                    {
                        yield return new WaitForEndOfFrame();
                        t = Time.realtimeSinceStartup;
                    }
                }  
                completeNotifier?.Invoke();
            }

            var routine = context.StartCoroutine(Routine());
            return observer;
        }

        /// <summary>
        /// Completely iterate through a WFC operation in one frame. Using this method will likely cause lag in your game, and it is only recommended for testing or advanced use.
        /// </summary>
        /// <param name="operation">Any WFC operation. This could be the return value from any Collapse related function in the <see cref="GenerationSpace"/></param>
        /// <param name="progressHandler">An optional callback that runs every time a WFCProgress element is processed.</param>
        public static void RunAsImmediate(this IEnumerable<WFCProgress> operation,
            Action<WFCProgress> progressHandler = null)
        {
            foreach (var progress in operation)
            {
                progressHandler?.Invoke(progress);
            }
        }
    }
    
    /// <summary>
    /// The Generation Space is the main access point for the WFC algorithm. The Generation Space holds a directed graph of Slots, and a set of all possible modules per slot.
    /// The Generation Space has methods for collapsing the superposition of the Slots, until all slots only have one module option left.
    /// </summary>
    public class GenerationSpace
    {
        public ModuleSet AllModules;
        public List<Slot> Slots;
        public List<SlotEdge> Edges;

        public Random Random;
        
        private Dictionary<Slot, List<SlotEdge>> _slotToEdges = new Dictionary<Slot, List<SlotEdge>>();

        private Dictionary<Slot, ModuleSet> _slotToPossibleModules = new Dictionary<Slot, ModuleSet>();
        
        private Dictionary<Vector3Int, Slot> _coordinateToSlot = new Dictionary<Vector3Int, Slot>();

        private float _totalWeightSum = 0;
        
        /// <summary>
        /// Create a new instance of the WFC algorithm. Each time you construct an instance of the Generation Space, you are setting up a new superposition with nothing collapsed.
        /// You can also use utility methods to create a new Generation space. <see cref="GenerationSpace.From2DGrid"/> 
        /// </summary>
        /// <param name="slots">A set of slots to perform the WFC on. Every slot will be assumed to have every module as a possibility</param>
        /// <param name="edges">A set of edges that connect the slots. </param>
        /// <param name="allModules">A set of all the modules, with the constraints already provided</param>
        /// <param name="seed">An optional random seed. If you provide the same seed, the WFC algorithm will produce the same output given the same inputs. </param>
        public GenerationSpace(List<Slot> slots, List<SlotEdge> edges, ModuleSet allModules, int? seed)
        {
            AllModules = allModules;
            Slots = slots;
            Edges = edges;

            Random = seed.HasValue ? new Random(seed.Value) : new Random();
            foreach (var edge in edges)
            {
                if (_slotToEdges.ContainsKey(edge.Source))
                {
                    _slotToEdges[edge.Source].Add(edge);
                }
                else
                {
                    _slotToEdges.Add(edge.Source, new List<SlotEdge>{edge});
                }
            }
            
            foreach (var slot in slots)
            {
                if (!_slotToEdges.ContainsKey(slot))
                {
                    _slotToEdges.Add(slot, new List<SlotEdge>());
                }
                _slotToPossibleModules.Add(slot, new ModuleSet(allModules));
            }

            foreach (var module in allModules)
            {
                _totalWeightSum += module.Weight;
            }

            _coordinateToSlot = slots.ToDictionary(s => s.Coordinate);
        }

        /// <summary>
        /// Find a slot given a position. If no slot exists at the given position, this method will return null.
        /// </summary>
        /// <param name="coordinate"></param>
        /// <returns>The slot at the given position, or null if none exists</returns>
        public Slot GetSlot(Vector3Int coordinate)
        { 
            _coordinateToSlot.TryGetValue(coordinate, out var slot);
            return slot;
        }

        /// <summary>
        /// At any given moment, a slot may have many possible modules available to it. This method will give you the set of possible modules available on a slot.
        /// </summary>
        /// <param name="slot">The slot whose available modules will be returned</param>
        /// <returns>A set of modules representing all possible modules for the given slot</returns>
        public ModuleSet GetSlotOptions(Slot slot)
        {
            return _slotToPossibleModules[slot];
        }

        /// <summary>
        /// At any given moment, a slot may have many possible modules available to it. However, eventually, a slot will only have one possible module remaining.
        /// This method will help you identify when there is only one module left per slot.
        /// If there is only one module left for the given slot, this method will return true and store the assigned module in the out parameter. If there is more than one module possible, the method will return false, and the out variable will be null.
        ///
        /// You can also retrieve all of the available slot modules with the <c> GenerationSpace.GetSlotOptions </c> method.
        /// <see cref="GenerationSpace.GetSlotOptions"/>
        /// </summary>
        /// <param name="slot">The slot whose only remaining available module will be checked. </param>
        /// <param name="module">An out variable for the only remaining available module. After the method is invoked, the value of the module will be the last module for the slot, or null if there are more than one modules remaining.</param>
        /// <returns>true if there was only one module available, or false otherwise. </returns>
        public bool TryGetOnlyOption(Slot slot, out Module module)
        {
            var options = GetSlotOptions(slot);
            module = null;
            if (options.Count == 1)
            {
                module = options.First();
                return true;
            }

            return false;
        }
        
        /// <summary>
        /// Given a slot with many module possibilities, collapse the possibility space so that only one module is possible for the given slot.
        /// This will cause many propagation events, and may cause other slots to collapse to a single module possibility as well.
        /// If the given slot has many module possibilities, one module will be selected at random.
        /// </summary>
        /// <param name="slot">The slot to collapse to a single module possibility</param>
        /// <returns>A sequence of <see cref="WFCProgress"/> representing the completion of the collapse operation. The resulting sequence must be enumerated, or the collapse won't complete. </returns>
        public IEnumerable<WFCProgress> CollapseSlot(Slot slot)
        {
            // remove random slot options, until there is only one.
         
            
            var options = GetSlotOptions(slot).ToList();
            if (options.Count == 1) yield break;

            var weightedSum = 0f;
            var randomNumber = Random.NextDouble();
            for (var i = 0; i < options.Count; i++)
            {
                weightedSum += options[i].Weight;
            }

            if (weightedSum < .001f)
            {
                throw new Exception("The sum of all module weights is less than .001f, and will likely cause rounding errors.");
            }
            options.Sort(((a, b) => a.Weight > b.Weight ? -1 : 1)); // sort highest weights forward.

            var forceAccept = false;
            var attempts = 0;
            var accumulatedStart = 0f;
            var index = 0;
            var errorFree = false;
            while (attempts < options.Count)
            //for (var i = 0; i < options.Count; i++)
            {
                
                var start = accumulatedStart;
                var range = (options[index].Weight / weightedSum);
                var end = start + range;

                var isInRange = randomNumber > start && randomNumber <= end;
                if (forceAccept || isInRange)
                {
                    var chosen = options[index];
                    forceAccept = true;

                    var removed = new Stack<SlotModuleRemoved>();
                    var storedProgressElements = new List<WFCProgress>();
                    var foundError = false;
                    foreach (var progress in CollapseSlot(slot, chosen))
                    {
                        switch (progress)
                        {
                            case SlotModuleRemoved removal:
                                removed.Push(removal);
                                storedProgressElements.Add(removal);
                                break;
                            case WFCError error:
                                foundError = true;
                                break;
                            case SlotModuleSelected selection:
                                storedProgressElements.Add(selection);
                                break;
                            default:
                                yield return progress;
                                break;
                        }

                        if (foundError) break;

                    }

                    if (foundError)
                    {
                        // roll back...
                        while (removed.Count > 0)
                        {
                            var removal = removed.Pop();
                            _slotToPossibleModules[removal.slot].Add(removal.module);
                        }

                        attempts++;

                        // need to try again : (
                        // just let i continue to the next...
                        // foreach (var retryProgress in CollapseSlot(slot, index + 1))
                        // {
                        //     yield return retryProgress;
                        // }
                    }
                    else
                    {
                        errorFree = true;
                        // emit all the queued up selections...
                        foreach (var p in storedProgressElements)
                        {
                            yield return p;
                        }

                        break;
                    }
                }
                else
                {
                    index = (index + 1) % options.Count;
                }
                
                accumulatedStart = end;
                if (accumulatedStart > 1)
                {
                    accumulatedStart = accumulatedStart - 1;
                }
            }

            if (!errorFree)
            {
                yield return new WFCError();
            }
        }

        /// <summary>
        /// Given a slot with many module possibilities, collapse the possibility space so that only one module is possible for the given slot.
        /// This will cause many propagation events, and may cause other slots to collapse to a single module possibility as well.
        /// If the given slot has many module possibilities, one module will be selected at random.
        /// </summary>
        /// <param name="slot">The slot to collapse to a single module possibility</param>
        /// <returns>A sequence of <see cref="WFCProgress"/> representing the completion of the collapse operation. The resulting sequence must be enumerated, or the collapse won't complete. </returns>
        // public IEnumerable<WFCProgress> CollapseSlot(Slot slot, int? offset = null)
        // {
        //     // remove random slot options, until there is only one.
        //
        //
        //     var options = GetSlotOptions(slot).ToList();
        //     if (options.Count == 1) yield break;
        //
        //     var index = offset ?? Random.Next(0, options.Count);
        //     index %= options.Count;
        //     var chosen = options[index];
        //
        //     var removed = new Stack<SlotModuleRemoved>();
        //     var storedProgressElements = new List<WFCProgress>();
        //     var foundError = false;
        //     foreach (var progress in CollapseSlot(slot, chosen))
        //     {
        //         switch (progress)
        //         {
        //             case SlotModuleRemoved removal:
        //                 removed.Push(removal);
        //                 storedProgressElements.Add(removal);
        //                 break;
        //             case WFCError error:
        //                 foundError = true;
        //                 break;
        //             case SlotModuleSelected selection:
        //                 storedProgressElements.Add(selection);
        //                 break;
        //             default:
        //                 yield return progress;
        //                 break;
        //         }
        //
        //         if (foundError) break;
        //
        //     }
        //
        //     if (foundError)
        //     {
        //         // roll back...
        //         Debug.LogWarning($"Found error... rolling back... slot={slot.Coordinate} module={chosen.Display} i=[{index}]");
        //         while (removed.Count > 0)
        //         {
        //             var removal = removed.Pop();
        //             _slotToPossibleModules[removal.slot].Add(removal.module);
        //         }
        //
        //         // need to try again : (
        //         // just let i continue to the next...
        //         foreach (var retryProgress in CollapseSlot(slot, index + 1))
        //         {
        //             yield return retryProgress;
        //         }
        //     }
        //     else
        //     {
        //         // emit all the queued up selections...
        //         foreach (var p in storedProgressElements)
        //         {
        //             yield return p;
        //         }
        //     }
        //
        // }

        /// <summary>
        /// Collapse the superposition of some slot. The slot that is selected is the slot in the Generation Space with the lowest entropy. A Slot's entropy is found by taking the count of available modules remaining at the slot.
        /// This will cause many propagation events, and may cause other slots to collapse to a single module possibility as well.
        /// By default, WFC always prefer to collapse the slot with the lowest entropy, because it is less likely to produce an invalid outcome.
        ///
        /// If there are multiple slots with the same entropy, the first one detected in the Generation Space's internal data structure will be used. It is not random. It depends on how the slots were created in the constructor of the Generation Space.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<WFCProgress> CollapseLowestEntropySlot()
        {
            if (TryGetLowestEntropySlotWithOptions(out var slot))
            {
                foreach (var progress in CollapseSlot(slot))
                {
                    
                    yield return progress;
                }
            }
        }

        /// <summary>
        /// Calculates the current entropy of a slot.
        /// Entropy is roughly equal to the number of modules left in the slot's wave function. The contribution of each module is weighted by the module's weight. <see cref="Module.Weight"/>
        /// The exact entropy calculation is <code> -sum( p log(p) ) </code> over all modules where each p is the probability of the module. 
        /// </summary>
        /// <param name="slot">The slot to find the entropy of</param>
        /// <returns>The current entropy of the slot</returns>
        public float GetEntropy(Slot slot) => GetEntropy(GetSlotOptions(slot));
        
        /// <summary>
        /// Calculates the entropy of a set of modules.
        /// Entropy is <code> -sum(p log(p) )</code> over all modules where each p is the probability of the module
        /// </summary>
        /// <param name="modules">A set of modules</param>
        /// <returns>The entropy of the given set of modules</returns>
        public float GetEntropy(ModuleSet modules)
        {
            var liklihood = 0f;
            var sum = 0f;
            foreach (var m in modules)
            {
                sum += m.Weight;
            }

            foreach (var m in modules)
            {
                var w = m.Weight < .001f ? .001f : m.Weight;
                var p = w / sum;
                liklihood += (p * Mathf.Log(p));
            }

            return -liklihood;
        }

        /// <summary>
        /// Reset all slot superpositions so that all modules are likely again. 
        /// </summary>
        public void Reset()
        {
            foreach (var slot in Slots)
            {
                _slotToPossibleModules[slot] = new ModuleSet(AllModules);
            }
        }

        /// <summary>
        /// Evaluate all constraints in the Generation Space and return the number if invalid constraints.
        /// Ideally, this method should always return zero. However, you can use this to check that the constraints are indeed valid if
        /// you suspect that the collapse has errored. 
        /// </summary>
        /// <returns>The number of broken constraints in the Generation Space.</returns>
        public int Validate()
        {
            var removes = 0;
            foreach (var slot in Slots)
            {
                foreach (var edge in GetEdges(slot))
                {
                    var toRemove = Eval(edge);
                    removes += toRemove.Count;
                }
            }

            return removes;
        }

        
        /// <summary>
        /// Run the WFC algorithm to completion on the Generation Space.
        /// This method will keep on collapsing the slot with the lowest entropy until all slots have been collapsed to a single module possibility.
        ///
        /// This method may take a long time to complete if the Generation Space is large. You should interweave the resulting progress set with a Coroutine with render frames. 
        /// </summary>
        /// <returns>A sequence of <see cref="WFCProgress"/> representing the completion of the collapse operation. The entire sequence must be enumerated or the collapse won't finish.</returns>
        public IEnumerable<WFCProgress> Collapse()
        {
            // var c = 1;
            // while (c > 0)
            // {
            //     c = 0;
            //     foreach (var progress in CollapseLowestEntropySlot())
            //     {
            //         c++;
            //         yield return progress;
            //     }
            // }
            while (TryGetLowestEntropySlotWithOptions(out var slot))
            {
                foreach (var progress in CollapseSlot(slot))
                {
                    yield return progress;
                }
            }
        }

        private bool TryGetLowestEntropySlotWithOptions(out Slot slot)
        {


            var lowest = _totalWeightSum;
            slot = null;
            foreach (var kvp in _slotToPossibleModules)
            {

                if (kvp.Value.Count > 1)
                {
                    var e = GetEntropy(kvp.Value);
                    if (e < lowest)
                    {
                        slot = kvp.Key;
                        lowest = e;
                    }                    
                }
                //
                // if (kvp.Value.Count < lowest && kvp.Value.Count > 1)
                // {
                //     slot = kvp.Key;
                //     lowest = kvp.Value.Count;
                // }
            }

            return slot != null;
        }

        /// <summary>
        /// Collapse a slot to a given module outcome.
        /// If a slot had N possible modules, after this method runs, the slot would only have 1 possible module.
        /// This will cause many propagation events, and may cause other slots to collapse to a single module possibility as well.
        /// </summary>
        /// <param name="slot">A slot to collapse to one module possibility</param>
        /// <param name="module">the module that will be only remaining possibility for the given slot</param>
        /// <returns>A sequence of <see cref="WFCProgress"/> representing the progress of the collapse operation. The sequence must be enumerated, or the collapse won't finish.</returns>
        /// <exception cref="SlotCannotSelectUnavailableModule">You cannot select a module for a slot if the slot doesn't already contain the module in its superposition. </exception>
        public IEnumerable<WFCProgress> CollapseSlot(Slot slot, Module module)
        {
            var existing = GetSlotOptions(slot);
            if (!existing.Contains(module))
            {
                throw new SlotCannotSelectUnavailableModule(slot, module);
            }

            if (existing.Count == 1)
            {
                yield break; // already done....
            }
            
            // remove all options but that one...
            var copy = new ModuleSet(existing);
            foreach (var toKill in copy)
            {
                // if (toKill.Equals(module)) continue; // TODO: MOre correct...
                // foreach (var progress in RemoveSlotOption(slot, toKill))
                // {
                //     yield return progress;
                // }
                
                if (toKill.Equals(module)) continue;
                existing.Remove(toKill);
                yield return new SlotModuleRemoved(slot, toKill);
            }

            yield return new SlotModuleSelected(slot, module);
            foreach (var progress in Propagate(slot))
            {
                yield return progress;
            }
        }
        
    
        ModuleSet Eval(SlotEdge edge)
        {
            var sourceSlot = edge.Source;
            var existing = GetSlotOptions(sourceSlot);

            // for all the remaining modules, we need to test if their constraints are met. If any aren't, we remove that 
            var toRemove = new ModuleSet();
            
            // if (existing.Count == 1) // TODO: Speed optimization to put this is, but it stops validate() from working...
            // {
            //     return toRemove;
            // }
            
            foreach (var testModule in existing)
            {
                foreach (var constraint in testModule.Constraints)
                {
                    var shouldRemove = constraint.ShouldRemoveModule(edge, this, testModule, toRemove);
                    if (shouldRemove)
                    {
                        toRemove.Add(testModule);
                    
                    }
                }
                    
            }

           
            return toRemove;
        }

        /// <summary>
        /// A small way to collapse the wave function. At some given slot, this method removes some given module from the set of possible modules at the slot.
        /// Before this method is run, the given slot may have N possible modules. After the method is run, that slot will have N-1 possible modules.
        /// This method collapses the superposition at the given slot, which also means that neighboring slots may need to remove modules given the new position.
        /// This method triggers a propagation event in the Generation Space. As such, it returns a consumable sequence of WFCProgress elements.
        /// The faster you consume the sequence, the faster the entire propagation phase happens. You may wish to sow render frames between calls. The sequence must be iterated through, or the propagation won't finish.
        /// 
        ///
        /// </summary>
        /// <param name="startingSlot">A slot to remove a module option from</param>
        /// <param name="removeModule">The module to be removed from the slot</param>
        /// <returns>A sequence of <see cref="WFCProgress"/> representing the progress of the propagation event. The sequence must be iterated through, or the propagation won't occur. </returns>
        /// <exception cref="SlotCannotHaveEmptyModuleSetException">An exception will be thrown if removing a module from a slot would cause the slot to have zero possible modules. A slot must always have at least one possibility.</exception>
        public IEnumerable<WFCProgress> RemoveSlotOption(Slot startingSlot, Module removeModule)
        {
            var startingSlotModules = _slotToPossibleModules[startingSlot];

            if (startingSlotModules.Contains(removeModule))
            {
                if (startingSlotModules.Count == 1)
                {
                    throw new SlotCannotHaveEmptyModuleSetException(startingSlot, removeModule);
                }

                startingSlotModules.Remove(removeModule);
                yield return new SlotModuleRemoved(startingSlot, removeModule);

                foreach (var progress in Propagate(startingSlot))
                {
                    yield return progress;
                }
            }
        }

        IEnumerable<WFCProgress> Propagate(Slot source)
        {
            yield return new WFCProgress();
            var toPropagate = new Queue<Slot>();
            var visited = new HashSet<Slot>();
            
            var toDoubleCheck = new HashSet<Slot>();
            toDoubleCheck.Add(source);
            var forceCheck = true;
          //  var totalTime = 0f;
            //Debug.Log("Starting propagate...");
            while (forceCheck || toDoubleCheck.Count > 0)
            {
                visited.Clear();
                toPropagate.Clear();

                var added = new HashSet<Slot>();

                foreach (var check in toDoubleCheck)
                {
                    if (!added.Contains(check))
                    {
                        
                        added.Add(check);
                        toPropagate.Enqueue(check);
                    }
                    toPropagate.Enqueue(check);
                    foreach (var edge in GetEdges(check))
                    {
                        if (!added.Contains(edge.Target))
                        {
                            
                            added.Add(edge.Target);
                            toPropagate.Enqueue(edge.Target);
                        }
                    }
                }
                forceCheck = false;
                var toRemove = new Dictionary<Slot, ModuleSet>();

              //  var sw = new Stopwatch();
               // sw.Start();
                while (toPropagate.Count > 0)
                {

                    var curr = toPropagate.Dequeue();
                    if (visited.Contains(curr))
                    {
                        continue;
                    }

                    visited.Add(curr);
                    
                    var options = GetSlotOptions(curr);
                    if (options.Count == 1)
                    {
                        continue; // SKIP IT! This is a huge time saver...
                    }
          
                    var edges = GetEdges(curr);
                    foreach (var edge in edges)
                    {
                                  
                      
                        if (!toDoubleCheck.Contains(edge.Target))// && !toDoubleCheck.Contains(edge.Target))
                        {
                            continue; // don't bother, nothing on this edge changed...
                        }
                        
                        var target = edge.Target;

                      

                        
                        var impacts = Eval(edge);
                        if (impacts.Count > 0)
                        {
                            if (!added.Contains(target))
                            {
                                toPropagate.Enqueue(target);
                                added.Add(target);
                            }

                            if (!toRemove.ContainsKey(curr))
                            {
                                toRemove.Add(curr, new ModuleSet());
                            }

                            foreach (var impact in impacts)
                            {
                                toRemove[curr].Add(impact);
                            }
                        }
                    }
                    
                } // end propagate

               // sw.Stop();
                
                //totalTime += sw.ElapsedMilliseconds;
               // Debug.Log("Prop finished. " + toRemove.Count + "   " + sw.ElapsedMilliseconds + "       " + totalTime + " " + PropagateLoops);
               // sw.Reset();
                // actually perform the remove.
                var actuallyRemoved = new HashSet<Slot>();


                foreach (var kvp in toRemove)
                {
                    var s = kvp.Key;
                    var kills = kvp.Value;

                    var current = _slotToPossibleModules[s];
                   
                    foreach (var toKill in kills)
                    {
                        if (current.Contains(toKill))
                        {
                            if (current.Count == 1)
                            {
                                yield return new WFCError();

                                throw new SlotCannotHaveEmptyModuleSetException(s, toKill);
                                yield break;
                            }

                            current.Remove(toKill);
                            yield return new SlotModuleRemoved(s, toKill); // SUPER SLOW: TODO: Consider just removing this feature...

                            if (current.Count == 1)
                            {
                                yield return new SlotModuleSelected(s, current.First());
                            }
                            
                            actuallyRemoved.Add(s);
                        }
                    }
                }

                toDoubleCheck.Clear();

                foreach (var kvp in actuallyRemoved)
                {
                    toDoubleCheck.Add(kvp);
                }


                
            }
        }
        
        /// <summary>
        /// Get a Slot's edges.
        /// <see cref="SlotEdge"/>
        /// </summary>
        /// <param name="slot">The slot to get the edges for.</param>
        /// <returns>A list of edges for the given slot</returns>
        public List<SlotEdge> GetEdges(Slot slot)
        {
            return _slotToEdges[slot];
        }
        
        /// <summary>
        /// A utility method for building a Generation Space in the form of a 2D grid.
        /// All Generation Spaces are directed graphs, and the resulting Generation Space from this method will be a graph representing a 2D grid. Each cell in the grid will have neighbors to the west, east, south, and north. There are no diagonal neighbors.
        /// </summary>
        /// <param name="width">The width of the resulting 2D grid Generation Space</param>
        /// <param name="height">The height of the resulting 2D grid Generation Space</param>
        /// <param name="modules">The set of all possible modules, with constraints prepopulated. Every Slot that is created in this Generation Space will have all modules available at the start of the WFC.</param>
        /// <param name="seed">An optional seed that controls how the WFC decides random operations. </param>
        /// <param name="prepareSpace">An optional callback to control how edges are created in the 2D grid. If this parameter is left null, every slot will have edges to the west, east, south, and north. If you need to change how the edges are created, remove certain edges, or add additional edges, you can provide your own implementation of this callback. If you provide a callback value, you are responsible for populating the entire List of SlotEdges per given Slot.</param>
        /// <returns>A Generation Space representing a 2D grid of slots where every slot has every module available to it at the start of the WFC. </returns>
        public static GenerationSpace From2DGrid(int width, int height, ModuleSet modules, int? seed=null, Action<List<Slot>, List<SlotEdge>> prepareSpace=null)
        {
            var slots = new Slot[width * height];
            var edges = new List<SlotEdge>();
            for (var y = 0; y < height; y++)
            {
                for (var x = 0; x < width; x++)
                {
                    var slot = new Slot();
                    slot.Coordinate = new Vector3Int(x, y, 0);
                    var index = x + y * width;
                    slots[index] = slot;
                    
                    // above edge?
                    if (prepareSpace != null) continue;
                    
                    var lowIndex = (x + (y - 1) * width);
                    var leftIndex = ((x - 1) + (y * width));
                    if (leftIndex >= 0 && x > 0)
                    {
                        var toLeftEdge = new SlotEdge
                        {
                            Source = slots[index],
                            Target = slots[leftIndex]
                        };
                        var toSelfEdge = new SlotEdge
                        {
                            Source = slots[leftIndex],
                            Target = slots[index]
                        };
                        edges.Add(toLeftEdge);
                        edges.Add(toSelfEdge);
                    }
                    if (lowIndex >= 0)
                    {
                        // connect edges
                        var toLowEdge = new SlotEdge
                        {
                            Source = slots[index],
                            Target = slots[lowIndex]
                        };
                        var toSelfEdge = new SlotEdge
                        {
                            Source = slots[lowIndex],
                            Target = slots[index]
                        };
                        edges.Add(toLowEdge);
                        edges.Add(toSelfEdge);
                    }
                }
            }

            var slotList = slots.ToList();
            prepareSpace?.Invoke(slotList, edges);

            return new GenerationSpace(slotList, edges, modules, seed);
        }

    }

    /// <summary>
    /// In the WFC algorithm, a module is a possibility for some <see cref="Slot"/>
    /// </summary>
    public class Module 
    {
        /// <summary>
        /// Every module must have a unique display name. This name is used for hashing and equality checks.
        /// </summary>
        public string Display;

        /// <summary>
        /// A module's weight controls how likely it is to be chosen randomly during a slot collapse. A module with a with a weight of zero will be never be randomly picked.
        /// </summary>
        public float Weight = 1;
        
        /// <summary>
        /// Every module has a set of <see cref="ModuleConstraint"/>. At the start of the WFC algorithm, every module is possible in all slots.
        /// The constraints for the module in each slot are all still plausible. As the Wave Function collapses, module possibilities are removed, and various constraints become
        /// impossible to fulfill. When that happens, this module becomes invalid itself. 
        /// </summary>
        public List<ModuleConstraint> Constraints = new List<ModuleConstraint>();

        
        public override string ToString()
        {
            return Display;
        }
        
        public override int GetHashCode()
        {
            return Display.GetHashCode();
        }
        
        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (GetType().IsInstanceOfType(obj))
            {
                return ((Module)obj).Display.Equals(Display);
            }
        
            return false;
        }

    }

    /// <summary>
    /// In the WFC algorithm, a Slot is a place that holds one <see cref="Module"/>. As the Wave Function is being collapsed, the slot may have many potential modules available to it.
    /// As the collapse continues, modules are removed as possibilities from the slot, until there is only one module possible for the slot.
    ///
    /// </summary>
    public class Slot
    {
        /// <summary>
        /// The slot's position in the larger generation space. 
        /// </summary>
        public Vector3Int Coordinate;

        public override bool Equals(object obj)
        {
            if (obj is Slot other)
            {
                return other.Coordinate == Coordinate;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return Coordinate.GetHashCode();
        }
    }

    /// <summary>
    /// A WFC constraint that enforces neighboring modules per slot.
    /// For example, 
    /// This constraint lets a module express that its western neighbor must be of a certain module. 
    /// </summary>
    [Serializable]
    public class AdjacencyConstraint : ModuleConstraint
    {
        /// <summary>
        /// The positional difference this constraint applies for. If the module is being tested between two slots (A, and B), the constraint only applies if the positional difference between A and B equals the Delta.  
        /// </summary>
        public Vector3Int Delta;
        
        /// <summary>
        /// A set of possible module values that can exist at a slot with the given Delta. 
        /// </summary>
        public ModuleSet NeedsOneOf;
        
        public override bool ShouldRemoveModule(SlotEdge edge, GenerationSpace space, Module module, ModuleSet modulesToRemove)
        {
            var target = edge.Target;
            var source = edge.Source;
            var diff = target.Coordinate - source.Coordinate;
            if (diff != Delta) return false;

            var targetOptions = space.GetSlotOptions(target);
            var targetHasOptions = targetOptions.Overlaps(NeedsOneOf);
            
            if (!targetHasOptions)
            {
                return true;
            }

            return false;

        }
    }

    /// <summary>
    /// The base type for all WFC constraints. Feel free to subclass this type and make your own constraints. 
    /// </summary>
    public abstract class ModuleConstraint
    {
        /// <summary>
        /// The only required method of a WFC constraint.
        /// Given some edge and some module-in-question, should the module-in-question be removed from the edge's source's possibility space?
        /// 
        /// When the WFC algorithm propagates information, every possible module per slot is checked against every neighboring slot. If ANY neighboring slot has a constraint that disqualifies the module-in-question, the module will be removed from the source's superposition. This in turn, causes a new propagation wave.
        /// </summary>
        /// <param name="edge">The edge whose source slot's superposition is being checked. If the method returns true, the given module will be removed from the edge's source's slot's superposition.</param>
        /// <param name="space">The generation space that this edge and module are apart of. The space is made available so that checks can be made for other available modules at the edge source and destination slots. </param>
        /// <param name="module">The module-in-question. Should this module be removed from the edge source, because of the edge destination?</param>
        /// <param name="modulesToRemove">An advanced capability. Anything in this set will be removed from the edge source's superposition. You can manually add to the set, but only if you are extremely confident. </param>
        /// <returns>Return true if the module should be removed from the edge's source's super position, because the constraint is invalid. Return false if the module doesn't break the constraint.</returns>
        public abstract bool ShouldRemoveModule(SlotEdge edge, GenerationSpace space, Module module,
            ModuleSet modulesToRemove);
    }

    /// <summary>
    /// A SlotEdge is the connection between two slots. In a simple 2D 1X2 grid case, there are two slots, and one slot edge connecting them.
    /// The term "edge" comes from Graph Theory. These edges are directional.
    /// </summary>
    public class SlotEdge
    {
        /// <summary>
        /// The origin slot of the edge. 
        /// </summary>
        public Slot Source;
        
        /// <summary>
        /// The destination slot of the edge. 
        /// </summary>
        public Slot Target;
    }
}