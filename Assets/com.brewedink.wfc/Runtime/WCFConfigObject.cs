using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using BrewedInk.WFC;

namespace BrewedInk.WFC
{

    public abstract class WCFConfigObject<TModuleObject, TModule> : WCFConfigObject
        where TModule : Module
        where TModuleObject : ModuleObject<TModule>
    {
        /// <summary>
        /// Should the <see cref="GenerationSpace"/> instances that this instance creates use a seed?/>
        /// </summary>
        [Tooltip("Should the GenerationSpaces instances that this instance creates use a seed?")]
        public bool useSeed;

        /// <summary>
        /// If the useSeed boolean is true, what seed should be used? This property should be ignored if useSeed is false.
        /// </summary>
        [Tooltip(
            "If the useSeed boolean is true, what seed should be used? This property should be ignored if useSeed is false.")]
        public int seed;

        public List<TModuleObject> moduleObjects;

        public virtual ModuleSet<TModule> GetModules() => new ModuleSet<TModule>(moduleObjects.Select(m => m.Module));

        private Dictionary<TModule, TModuleObject> _moduleToObject;

        /// <summary>
        /// Given the typeless Module, get the wrapper object for it. The wrapper object may have additional metadata about the module that isn't used in the WFC algorithm, but may be useful for other parts of the game.
        /// </summary>
        /// <param name="module">The module you want to get the wrapper object for</param>
        /// <param name="moduleObject">The wrapper object for the Module. Or null if there is no wrapper object for the given Module</param>
        /// <returns>True if there was an associated wrapper object with the module, or false.</returns>
        public bool TryGetObject(Module module, out TModuleObject moduleObject)
        {
            moduleObject = null;
            var tmod = module as TModule;
            if (tmod == null) return false;
            return _moduleToObject.TryGetValue(tmod, out moduleObject);
        }


        /// <summary>
        /// You need to create a Generation Space given a set of user defined modules and configuration. 
        /// </summary>
        /// <returns>You should return a new GenerationSpace instance that respects the seed properties. </returns>
        protected abstract GenerationSpace CreateSpace();

        public override GenerationSpace Create()
        {
            var space = CreateSpace();

            _moduleToObject = moduleObjects.ToDictionary(k =>
            {

                var actual = space.AllModules.FirstOrDefault(m => m.Display.Equals(k.Module.Display));
                var actualMod = actual as TModule;
                return actualMod;
            });

            return space;
        }

        public override bool TryGetSprite(Module module, out Sprite sprite)
        {
            Debug.LogWarning($"Default implementation of {nameof(TryGetSprite)} is being called on {this}. If you expect modules to have sprites, you need to implement the {nameof(TryGetSprite)} method on {GetType().Name}");
            sprite = null;
            return false;
        }
    }

    /// <summary>
    /// A ScriptableObject wrapper that streamlines creating <see cref="GenerationSpace"/> instances from MonoBehaviours. 
    /// </summary>
    [HelpURL("https://github.com/cdhanna/WaveFunctionCollapserDocs/blob/main/CodeDocs/WCFConfigObject.md")]
    public abstract class WCFConfigObject : ScriptableObject
    {
        /// <summary>
        /// Create a blank <see cref="GenerationSpace"/> from the configuration.
        /// This is a useful method for creating generation spaces pre-configured with edges, slots, and modules.
        /// </summary>
        /// <returns>A new Generation Space</returns>
        public abstract GenerationSpace Create();

        /// <summary>
        /// If a module has a Sprite associated with it, you can use this method to try and get it. Depending on the type of Module, there may not be any Sprite available. 
        /// </summary>
        /// <param name="module">The module you'd like to get a sprite for.</param>
        /// <param name="sprite">The Sprite associated with the given module. The sprite will be null if there is no associated Sprite for the module.</param>
        /// <returns>True if there is any sprite associated with the module, false otherwise. If the method returns false, you can expect the sprite to be null.</returns>
        public abstract bool TryGetSprite(Module module, out Sprite sprite);
    }

    [System.Serializable]
    public abstract class ModuleObject<TModule> where TModule : Module
    {
        public TModule Module;
    }
}