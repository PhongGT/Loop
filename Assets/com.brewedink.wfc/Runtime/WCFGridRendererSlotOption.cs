using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BrewedInk.WFC;
using Random = System.Random;

namespace BrewedInk.WFC
{


    public class WCFGridRendererSlotOption : MonoBehaviour
    {

        public SpriteRenderer Sprite;
        public Collider2D Collider;

        public float HoverScaleFactor = 1.1f;
        public float HoverScaleSpeed = 1;

        public float ScaleFactor = 1;
        public float ScaleVel;

        public float CenterSpeed = 1;

        public bool IsHover;

        public bool IsAvailable;
        private Vector3 _startScale, _startPos;
        private Vector3 _posVel;
        public float ColorSpeed = 1;
        public float SelectedScale = 3;
        public Module module;

        public Rigidbody2D body;

        private GenerationSpace _space;
        private Slot _slot;

        private BoxCollider2D _collider;

        // Start is called before the first frame update
        void Start()
        {
            _startScale = transform.localScale;
            _startPos = transform.localPosition;
        }

        private bool available = true, isOnly = false;
        public WCFGridRendererSlot renderSlot;

        // Update is called once per frame
        void Update()
        {
            // var options = _space.GetSlotOptions(_slot);
            //var available = options.Contains(module);

            //var isOnly = options.Count == 1 && available;
            var targetColor = new Vector4(1, 1, 1, 1);

            if (available)
            {

                transform.localPosition = Vector3.SmoothDamp(transform.localPosition, isOnly ? Vector3.zero : _startPos,
                    ref _posVel,
                    Time.deltaTime * CenterSpeed);
            }
            else
            {
                targetColor = Color.clear;

            }

            var targetScale = IsHover ? HoverScaleFactor : 1;
            //targetScale = isOnly ? (SelectedScale) : targetScale;
            ScaleFactor = Mathf.SmoothDamp(ScaleFactor, targetScale, ref ScaleVel,
                Time.deltaTime * HoverScaleSpeed);


            if (!isOnly)
            {

                transform.localScale = _startScale * ScaleFactor;
            }
            else
            {
                transform.localScale = Vector3.one * .778f;
                //  SetGlobalScale(transform, transform.parent.lossyScale);
            }


            if (!available && IsAvailable)
            {
                // disable!
                targetColor = Color.clear;
                Blowup();
            }

            if (!available && !isOnly)
            {
                targetColor = Color.clear;
            }

            var color = Vector4.MoveTowards(new Vector4(Sprite.color.r, Sprite.color.g, Sprite.color.b, Sprite.color.a),
                targetColor,
                Time.deltaTime * ColorSpeed);
            Sprite.color = new Color(color.x, color.y, color.z, color.w);

            IsAvailable = available;

            if (transform.position.y < -1000)
            {
                Destroy(gameObject);
            }
        }

        public static void SetGlobalScale(Transform transform, Vector3 globalScale)
        {
            transform.localScale = Vector3.one;
            transform.localScale = new Vector3(globalScale.x / transform.lossyScale.x,
                globalScale.y / transform.lossyScale.y, globalScale.z / transform.lossyScale.z);
        }

        private void OnMouseEnter()
        {
            IsHover = true;
        }

        private void OnMouseExit()
        {
            IsHover = false;
        }

        private void OnMouseOver()
        {
            if (!_collider.enabled) return;
            // remove the option
            if (Input.GetMouseButtonDown(1))
            {
                renderSlot.gridRenderer.RemoveModule(_slot, module);
            }
            else if (Input.GetMouseButtonDown(0))
            {
                renderSlot.gridRenderer.SelectionModule(_slot, module);

            }

        }

        public IEnumerator RunAction(IEnumerable<WFCProgress> progress)
        {
            foreach (var p in progress)
            {
                yield return new WaitForEndOfFrame();
            }
        }

        public void Blowup()
        {

            IsHover = false;

            if (!_collider.enabled) return;
            Sprite.color = Color.red;
            _collider.enabled = false;
            body = body != null ? body : gameObject.AddComponent<Rigidbody2D>();
            body.isKinematic = false;
            body.AddForce(
                new Vector2(UnityEngine.Random.Range(1, 3) * (UnityEngine.Random.value > .5f ? 1 : -1),
                    UnityEngine.Random.Range(4, 5)), ForceMode2D.Impulse);
            body.AddTorque(UnityEngine.Random.Range(1, 4) * (UnityEngine.Random.value > .5f ? 1 : -1),
                ForceMode2D.Impulse);
        }


        public void OnCreated(WCFConfigObject config, GenerationSpace space, Slot slot, Module module,
            Sprite previewSprite)
        {
            _space = space;
            _slot = slot;

            this.module = module;
            Sprite.sprite = previewSprite;
            _collider = gameObject.AddComponent<BoxCollider2D>();
            IsAvailable = true;
        }

        public void Remove()
        {
            available = false;
        }

        public void Select()
        {
            isOnly = true;
        }
    }
}