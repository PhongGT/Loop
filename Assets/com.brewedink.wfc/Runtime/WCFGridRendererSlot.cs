using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BrewedInk.WFC;

namespace BrewedInk.WFC
{


    public class WCFGridRendererSlot : MonoBehaviour
    {
        public SpriteRenderer Background;
        private GenerationSpace _space;
        public Slot slot;
        public WCFConfigObject config;

        [Range(0, 1)] public float Gutter = 0;
        public WCFGridRendererSlotOption OptionPrefab;

        private Dictionary<Module, WCFGridRendererSlotOption> moduleToRenderer =
            new Dictionary<Module, WCFGridRendererSlotOption>();

        public WFCGridRenderer gridRenderer;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public List<WCFGridRendererSlotOption> OnCreated(WCFConfigObject config, GenerationSpace space, Slot slot)
        {
            var createdOptions = new List<WCFGridRendererSlotOption>();
            this.slot = slot;
            _space = space;
            this.config = config;

            var options = space.GetSlotOptions(slot);

            // need to render all options in the space available, or if there is only one option, render the preview sprite...
            var size = Mathf.CeilToInt(Mathf.Sqrt(options.Count));
            var elemSizeX = Mathf.Min(Background.bounds.size.x, Background.bounds.size.y) / size;

            var realBounds = Background.bounds;
            var bounds = new BoundsInt(0, 0, 0, size, size, 0);

            var widthPerSlot = realBounds.size.x / bounds.size.x;
            var heightPerSlot = realBounds.size.y / bounds.size.y;

            var widthAsRatio = (1f / (size + 1)) - (Gutter * (1f / size));

            var i = 0;
            var coordX = 0;
            var coordY = 0;
            moduleToRenderer.Clear();
            foreach (var option in options)
            {
                if (!config.TryGetSprite(option, out var sprite)) continue;


                var optionGob = Instantiate(OptionPrefab, Background.transform);
                moduleToRenderer[option] = optionGob;
                optionGob.name = $"option {slot.Coordinate} _ {option.Display}";
                optionGob.renderSlot = this;

                var xRatio = (coordX + .5f) / (float) size;
                var yRatio = (coordY + .5f) / (float) size;
                var x = ((xRatio - .5f) * 1); //- realBounds.extents.x;// * bounds.size.x;
                var y = ((yRatio - .5f) * 1); // - realBounds.extents.y;
                optionGob.transform.localPosition = new Vector3(x, y, 0);
                optionGob.transform.localScale = new Vector3(widthAsRatio, widthAsRatio, 1);

                optionGob.OnCreated(config, space, slot, option, sprite);
                i++;
                coordX++;
                if (coordX >= size)
                {
                    coordY++;
                    coordX = 0;
                }

                createdOptions.Add(optionGob);
            }

            return createdOptions;
        }

        public void RemoveOption(Module removalModule)
        {
            moduleToRenderer[removalModule].Remove();
        }

        public void SelectOption(Module selectModule)
        {
            moduleToRenderer[selectModule].Select();
        }
    }
}