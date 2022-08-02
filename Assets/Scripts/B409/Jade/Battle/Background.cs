using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Sirenix.OdinInspector;

namespace B409.Jade.Battle {
    public class Background : MonoBehaviour {
        [SerializeField] private float parallaxSpeed = 0.0f;

        private float _parallaxSize = 0.0f;
        private Transform _cameraTransform;
        private Transform[] _images;
        private float _lastCameraX;
        private int _leftIndex;
        private int _rightIndex;
        private const string DefaultSpritePath = "UI/Textures/Common/8x8_rect_transparent";

        private void Awake() {
            _cameraTransform = Camera.main.transform;
            var initPosition = transform.localPosition;
            var cameraPosition = _cameraTransform.localPosition;
            initPosition.x += cameraPosition.x;
            transform.localPosition = Vector3.zero;
            // create objects
            InstantiateChildren();

            InitParallax(initPosition);
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
            // create two gameObject
            for(int i = 0; i < 2; ++i) {
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

        private void InitParallax(Vector3 initPosition) {
            var mainCamera = Camera.main;
            var cameraHeight = 2 * mainCamera.orthographicSize;
            var cameraWidth = cameraHeight * mainCamera.aspect;
            _images = new Transform[transform.childCount];
            for(var i = 0; i < transform.childCount; ++i) {
                _images[i] = transform.GetChild(i);
                if(i == 0) {
                    var sr = _images[i].GetComponent<SpriteRenderer>();
                    if(sr != null) {
                        var minWidth = cameraWidth + 2f;

                        if(sr.drawMode == SpriteDrawMode.Tiled) {
                            _parallaxSize = Mathf.Max(sr.size.x, minWidth);
                                //sr.size.x > minWidth ? sr.size.x : minWidth;

                        } else {
                            var sprite = sr.sprite;

                            if(!(sprite is null)) {
                                var width = sprite.rect.width / sprite.pixelsPerUnit;
                                _parallaxSize = Mathf.Max(width, minWidth);
                                    //width > minWidth ? width : minWidth;
                            }
                        }

                        Debug.Log(string.Format("name: {0}, minWidth: {1}, parallaxSize: {2}", this.name, minWidth, _parallaxSize));
                    }
                }
                _images[i].localPosition = new Vector3(_parallaxSize * i + initPosition.x, initPosition.y, initPosition.z);
            }

            _lastCameraX = _cameraTransform.position.x;
            _leftIndex = 0;
            _rightIndex = _images.Length - 1;
        }

        private void LateUpdate() {
            var camPosX = _cameraTransform.position.x;

            if(!Mathf.Approximately(parallaxSpeed, 0.0f)) {
                var deltaX = camPosX - _lastCameraX;
                transform.position += Vector3.right * (deltaX * parallaxSpeed * 3f);
                _lastCameraX = camPosX;
            }

            if(_images.Length <= 1) {
                return;
            }

            if(camPosX < _images[_leftIndex].transform.position.x) {
                MoveLeft();
            }

            if(camPosX > _images[_rightIndex].transform.position.x) {
                MoveRight();
            }
        }

        private void MoveLeft() {
            var position = _images[_leftIndex].position;
            position.x -= _parallaxSize;
            _images[_rightIndex].position = position;

            _leftIndex = _rightIndex;
            _rightIndex--;
            if(_rightIndex < 0) {
                _rightIndex = _images.Length - 1;
            }
        }

        private void MoveRight() {
            var position = _images[_rightIndex].position;
            position.x += _parallaxSize;
            _images[_leftIndex].position = position;

            _rightIndex = _leftIndex;
            _leftIndex++;
            if(_leftIndex == _images.Length) {
                _leftIndex = 0;
            }
        }

        [Button]
        private void Test() {
            var rend = GetComponent<SpriteRenderer>();
            Debug.Log(string.Format("rect: {0}, ppu: {1}, bounds: {2}", rend.sprite.rect, rend.sprite.pixelsPerUnit, rend.sprite.bounds));
        }
    }
}