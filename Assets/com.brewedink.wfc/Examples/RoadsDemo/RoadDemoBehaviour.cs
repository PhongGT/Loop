using System.Collections;
using System.Collections.Generic;
using BrewedInk.WFC;
using UnityEngine;

namespace BrewedInk.WFC.Examples.RoadDemo
{
    public class RoadDemoBehaviour : MonoBehaviour
    {
        public SpriteConfig wfcConfig;
        public GameObject roadPrefab;
        

        [Range(0.001f, .2f)] 
        public float frameBudgetTime = .1f;

        private WFCProgressObserver _handle;

        private Slot _slot;
        void Start()
        {

            // step 1. use the configuration to generate a blank Generation Space
            var space = wfcConfig.Create();
           space.CollapseLowestEntropySlot();
           
         
            // step 2. run the WFC algorithm as a coroutine, so it doesn't block the render thread.
            //         Attach some callbacks to do interesting things with the results of the WFC
            _handle = space.CollapseSlot(_slot)
                .RunAsCoroutine(this, frameBudgetTime)
                .OnSelectedModule<SpriteConfigModule>((slot, module) =>
                {
                    var road = Instantiate(roadPrefab, transform);
                    var meshRenderer = road.GetComponent<MeshRenderer>();
                    meshRenderer.material.mainTexture = module.sprite.texture;
                    
                    road.transform.localPosition = 10 * new Vector3(wfcConfig.Width - slot.Coordinate.x, 0,
                        wfcConfig.Height - slot.Coordinate.y);
                })
                .OnCompleted(() =>
                {
                    Debug.Log("Wave function collapse completed!");
                });
        }

        void Update()
        {
/*            if (_handle.IsComplete)
            {
                // the operation is complete
                var selections = _handle.SlotSelections.Count;
                var operations = _handle.ProgressCount;
               *//* Debug.Log("SS: " + selections + " Op: " +  operations);*//*
            }*/
        }
    }
}
