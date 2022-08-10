using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Sirenix.OdinInspector;

namespace B409.Jade.Battle {
    public class Background : MonoBehaviour {
        [SerializeField]
        private float parallaxSpeed = 0.0f;
        [SerializeField]
        private float parallaxSize = 0.0f;

        private Transform cameraTransform;
        private Transform[] images;
        private float lastCameraX;
        private int leftIndex;
        private int rightIndex;
        private const string DefaultSpritePath = "UI/Textures/Common/8x8_rect_transparent";

        private float cameraWidth;
        private int count;
        private Vector3 offset;

        private void Awake() {
            var cam = Camera.main;

            cameraTransform = cam.transform;

            offset = transform.localPosition;
            transform.localPosition = Vector3.zero;

            cameraWidth = cam.orthographicSize * 2 * cam.aspect;
            if(parallaxSize == 0f) {
                parallaxSize = cameraWidth;
            }
            count = Mathf.CeilToInt(cameraWidth / parallaxSize) + 1;

            // create objects
            InstantiateChildren();

            InitParallax();
        }

        private void InstantiateChildren() {
            // create child objects
            var spriteRenderer = transform.GetComponent<SpriteRenderer>();

            if(spriteRenderer == null) {
                var resource = Resources.Load<Sprite>(DefaultSpritePath);
                spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
                spriteRenderer.sprite = resource;
                spriteRenderer.sortingLayerName = "InGameBackground";
            }

            var animator = transform.GetComponent<Animator>();
            // get child gameObjects
            var children = new List<GameObject>();
            for(var i = 0; i < transform.childCount; ++i) {
                children.Add(transform.GetChild(i).gameObject);
            }
            // disable origin sprite renderer
            spriteRenderer.enabled = false;

            for(int i = 0; i < count; ++i) {
                var go = new GameObject($"background_{i}");
                var sr = go.AddComponent<SpriteRenderer>();
                go.transform.SetParent(transform);
                sr.sprite = spriteRenderer.sprite;
                sr.sortingLayerName = spriteRenderer.sortingLayerName;
                sr.sortingOrder = spriteRenderer.sortingOrder;
                sr.transform.localPosition = Vector3.zero;
                sr.drawMode = spriteRenderer.drawMode;
                sr.tileMode = spriteRenderer.tileMode;
                sr.size = spriteRenderer.size;
                if(animator != null) {
                    var ani = go.AddComponent<Animator>();
                    ani.runtimeAnimatorController = animator.runtimeAnimatorController;
                    animator.enabled = false;
                }

                foreach(var g in children.Select(Instantiate)) {
                    g.transform.SetParent(go.transform);
                }
            }

            // 아까 받아온거 새로 만들어서 하위에 뒀으니 얘네들은 없애고
            foreach(var child in children) {
                DestroyImmediate(child);
            }
        }

        private void InitParallax() {
            images = new Transform[transform.childCount];
            for(var i = 0; i < transform.childCount; ++i) {
                images[i] = transform.GetChild(i);
                images[i].localPosition = Vector3.right * parallaxSize * i + offset;
            }

            lastCameraX = cameraTransform.position.x;
            leftIndex = 0;
            rightIndex = images.Length - 1;
        }

        private void LateUpdate() {
            var camPosX = cameraTransform.position.x;

            if(!Mathf.Approximately(parallaxSpeed, 0.0f)) {
                var deltaX = camPosX - lastCameraX;
                transform.position += Vector3.right * (deltaX * parallaxSpeed * 3f);
                lastCameraX = camPosX;
            }

            if(images.Length <= 1) {
                return;
            }

            var left = images[leftIndex];
            if(camPosX - cameraWidth / 2 > (left.position - offset).x + parallaxSize / 2) {
                MoveRight();
            }
            var right = images[rightIndex];
            if(camPosX + cameraWidth / 2 < (right.position - offset).x - parallaxSize / 2) {
                MoveLeft();
            }
        }

        private void MoveLeft() {
            var position = images[leftIndex].position;
            position.x -= parallaxSize;
            images[rightIndex].position = position;

            leftIndex = rightIndex;
            rightIndex--;
            if(rightIndex < 0) {
                rightIndex = images.Length - 1;
            }
        }

        private void MoveRight() {
            var position = images[rightIndex].position;
            position.x += parallaxSize;
            images[leftIndex].position = position;

            rightIndex = leftIndex;
            leftIndex++;
            if(leftIndex == images.Length) {
                leftIndex = 0;
            }
        }

        [Button]
        private void SetParallaxSize() {
            var sr = GetComponent<SpriteRenderer>();
            if(sr == null) return;

            if(sr.drawMode == SpriteDrawMode.Tiled) {
                parallaxSize = sr.size.x;
            } else {
                var sprite = sr.sprite;
                if(sprite == null) return;
                var width = sprite.rect.width / sprite.pixelsPerUnit;
                parallaxSize = width;
            }
        }
    }
}