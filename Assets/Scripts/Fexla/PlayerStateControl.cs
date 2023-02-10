using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Fexla {
    public class PlayerStateControl : MonoBehaviour {
        public Collider2D[] colliders;
        public Collider2D[] dieColliders;
        public Transform GroundColliders;
        public Transform sprite;
        public float speed = 1;
        public float Jump = 15;
        public AnimationCurve JumpCurve;
        public float jumpDuration = 1f;
        public Animator animator;
        public float power = 2;
        public Collider2D TriggerCollider;

        private int[][] code = {
            new int[] {1, 1, 1, 1, 0, 0, 1, 0, 0},
            new int[] {1, 1, 1, 0, 0, 1, 0, 0, 1},
            new int[] {1, 1, 1, 0, 0, 1, 0, 0, 1},
            new int[] {1, 1, 1, 1, 0, 0, 1, 0, 0},
        };

        public bool isRotating = false;
        public bool isRotate = false;
        private bool overlappedGround = false;
        private bool isRight = true;
        private float lastTimeOnGround;
        private Box overlapedBox;
        private bool overlapped = false;
        private float direction;


        public AudioSource footAudio;
        public AudioSource jumpAudio;

        public bool IsRotate {
            get {
                return isRotate;
            }
            set {
                if (isRotate == value) {
                    return;
                }
                if (!overlappedGround) {
                    isRotate = value;
                    if (value) {
                        animator.Play("PlayerRotate");
                    } else {
                        animator.Play("RotateBack");
                    }
                    isRotating = true;
                    RotateStart = Time.time;
                    IEnumerator enumerator = Rotate();
                    Coroutine coroutine = StartCoroutine(enumerator);
                    UpdateState();
                }
            }
        }

        public bool IsRight {
            get {
                return isRight;
            }
            set {
                if (isRight == value) {
                    return;
                }
                if (!overlappedGround) {
                    isRight = value;
                    UpdateState();
                }
            }
        }

        public float LastTimeOnGround {
            get {
                return lastTimeOnGround;
            }
            set {
                lastTimeOnGround = value;
            }
        }


        public float Direction {
            get {
                return direction;
            }
            set {
                direction = value;
            }
        }

        ContactFilter2D filter;

        private void Start() {
            filter.layerMask = LayerMask.GetMask("Ground");
            UpdateState();
        }

        private Vector2 GetDirection() {
            Vector2 ans;
            if (isRotate) {
                if (isRight) {
                    ans = new Vector2(-1, 0);
                } else {
                    ans = new Vector2(1, 0);
                }
            } else {
                ans = new Vector2(0, 1);
            }
            return ans;
        }

        private void Update() {
            CheckDie();
            overlapedBox = null;
            overlappedGround = false;
            CheckOverlap();
            CheckOnGround();
            MoveControl();
            if (!isRotating) {
                var triggerResult = new List<Collider2D>();
                Physics2D.OverlapCollider(TriggerCollider, filter, triggerResult);
                foreach (var c in triggerResult) {
                    if (c.gameObject.TryGetComponent(out Box box)) {
                        if (isRotate) {
                            box.Extend(GetDirection(), power);
                        } else {
                            box.Extend(GetDirection(), power);
                        }
                    }
                }
                // if (overlapedBox != null) {
                //     if (isRotate) {
                //         overlapedBox.Extend(GetDirection(), math.pow(overlapedBox.gameObject.transform.localScale.x, power));
                //     } else {
                //         overlapedBox.Extend(GetDirection(), math.pow(overlapedBox.gameObject.transform.localScale.y, power));
                //     }
                // }
            }
        }

        public float lastTimeJump = 0;

        public void MoveControl() {
            if (isRotating) {
                return;
            }
            direction = (Input.GetKey(KeyCode.A) ? -1 : 0) + (Input.GetKey(KeyCode.D) ? 1 : 0);
            if (direction < 0) {
                IsRight = false;
            } else if (direction > 0) {
                IsRight = true;
            }
            Vector3 v = GetComponent<Rigidbody2D>().velocity;
            v.x = speed * direction;
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space)) {
                // v.y += Jump * JumpCurve.Evaluate((Time.time - lastTimeOnGround) / jumpDuration) * Time.deltaTime;
                if (Time.time > lastTimeJump + 0.5f && Time.time < lastTimeOnGround + 0.01f) {
                    v.y = Jump;
                    lastTimeJump = Time.time;
                    // AudioManager.Instance.PlaySE(AudioManager.SoundEffect.Jump);
                    jumpAudio.Play();
                }
            }
            GetComponent<Rigidbody2D>().velocity = v;
            if (Input.GetKeyDown(KeyCode.R)) {
                IsRotate = !IsRotate;
            }
        }

        public void UpdateState() {
            int s = (IsRight ? 0 : 1) | (IsRotate ? 2 : 0);
            var mode = code[s];
            for (int i = 0; i < 9; i++) {
                colliders[i].isTrigger = mode[i] != 1;
                colliders[i].transform.localScale = mode[i] != 1 ? new Vector3(0.95f, 0.95f, 1f) : new Vector3(1, 1, 1);
            }
            colliders[4].transform.localScale = new Vector3(1.1f, 1.1f, 1);
            Vector3 flip = new Vector3(isRight ? 1 : -1, 1, 0);
            GroundColliders.localScale = flip;

            sprite.localScale = flip;

        }

        public void CheckOverlap() {
            for (int i = 0; i < 9; i++) {
                if (colliders[i].isTrigger) {
                    List<Collider2D> result = new List<Collider2D>();
                    Physics2D.OverlapCollider(colliders[i], filter, result);
                    foreach (var c in result) {
                        if (c.gameObject.layer == 6) {
                            overlappedGround = true;
                        }
                        if (c.gameObject.TryGetComponent(out Box box)) {
                            overlapedBox = box;
                            if (box.state == Box.BoxState.Negative) {
                                Die();
                            }
                        }
                    }
                }
            }
        }

        public void CheckDie() {
            if (transform.position.y < -17) {
                Die();
            }
            // foreach (var c in dieColliders) {
            //     List<Collider2D> result = new List<Collider2D>();
            //     Physics2D.OverlapCollider(c, filter, result);
            //     foreach (var resC in result) {
            //         if (resC.gameObject.layer == 6) {
            //             Die();
            //             return;
            //         }
            //     }
            // }
        }

        public void CheckOnGround() {
            ContactFilter2D contactFilter2D = new ContactFilter2D();
            contactFilter2D.layerMask = LayerMask.GetMask("Ground");
            var groundCollider = GroundColliders.GetComponents(typeof(Collider2D));
            foreach (var component in groundCollider) {
                var c = (Collider2D) component;
                List<Collider2D> result = new List<Collider2D>();
                Physics2D.OverlapCollider(c, contactFilter2D, result);
                foreach (var res in result) {
                    if (res.transform.IsChildOf(transform)) {
                        continue;
                    }
                    lastTimeOnGround = Time.time;
                    return;
                }
            }
        }

        private float RotateStart;
        public float RotateDuration = 1;

        IEnumerator Rotate() {
            GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
            while (Time.time < RotateStart + RotateDuration) {
                yield return null;
            }
            isRotating = false;
            GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        }

        public GameObject diecanvas;

        public void Die() {
            var Canvas = diecanvas;
            Canvas.gameObject.SetActive(true);
            Destroy(gameObject);
            // SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

    }
}