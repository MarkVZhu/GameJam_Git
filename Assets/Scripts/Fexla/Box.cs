using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace Fexla {
    public class Box : MonoBehaviour {
        public bool MergeAble = false;
        public MergeCheck mergeCheck;

        public SpriteRenderer NormalRender;
        public SpriteRenderer NegativeRender;
        public float extendDuration;
        public AnimationCurve curve;
        public int times = 1;
        public BoxState state = BoxState.Idle;
        private Vector2 extendDirection;
        private float ExtendStart;
        private float TargetScale;
        private float StartScale;
        private const float eps = 0.001f;


        public bool Control = false;

        public enum BoxState {
            Idle,
            Extend,
            Negative
        }

        private void Start() {
            if (state == BoxState.Negative) {
                state = BoxState.Idle;
                NegativeFlip();
            }
        }

        private void Update() {

            if (MergeAble) {
                if (mergeCheck.TryMerge()) return;
            }

            #region debug

            if (Control) {
                // if (Input.GetKey(KeyCode.W)) {
                //     Extend(Vector2.up,transform.localScale.y*transform.localScale.y);
                // }
                // if (Input.GetKey(KeyCode.S)) {
                //     Extend(Vector2.down,transform.localScale.y*transform.localScale.y);
                // }
                // if (Input.GetKey(KeyCode.A)) {
                //     Extend(Vector2.left,transform.localScale.x*transform.localScale.x);
                // }
                // if (Input.GetKey(KeyCode.D)) {
                //     Extend(Vector2.right,transform.localScale.x*transform.localScale.x);
                // }

                if (Input.GetKey(KeyCode.W)) {
                    Extend(Vector2.up, math.sqrt(transform.localScale.y));
                }
                if (Input.GetKey(KeyCode.S)) {
                    Extend(Vector2.down, math.sqrt(transform.localScale.y));
                }
                if (Input.GetKey(KeyCode.A)) {
                    Extend(Vector2.left, math.sqrt(transform.localScale.x));
                }
                if (Input.GetKey(KeyCode.D)) {
                    Extend(Vector2.right, math.sqrt(transform.localScale.x));
                }
                if (Input.GetKeyDown(KeyCode.P)) {
                    NegativeFlip();
                }
            }

            #endregion

            if (state == BoxState.Extend) {
                float k = curve.Evaluate((Time.time - ExtendStart) / extendDuration);
                float targetLength = StartScale * (1 - k) + TargetScale * k;
                float stepLength;
                if (extendDirection.x != 0) {
                    stepLength = targetLength - transform.localScale.x;
                } else {
                    stepLength = targetLength - transform.localScale.y;
                }
                float rest = TryExtend(extendDirection, stepLength);
                if (math.abs(rest) > eps) {
                    rest = TryExtend(-extendDirection, rest);
                    if (math.abs(rest) > eps) {
                        state = BoxState.Idle;
                        return;
                    }
                }
                if (Time.time > ExtendStart + extendDuration) {
                    state = BoxState.Idle;
                }
            }
        }

        public float TryExtend(Vector2 direction, float length) {
            Vector2 boxStart = (Vector2) transform.position;
            boxStart += new Vector2(direction.x * (transform.localScale.x), direction.y * (transform.localScale.y)) / 4;
            Vector2 boxSize = new Vector2(transform.localScale.x * (direction.x == 0 ? 1 : 0.5f), transform.localScale.y * (direction.y == 0 ? 1 : 0.5f));
            var all = Physics2D.BoxCastAll(boxStart, boxSize, 0, direction, 100, LayerMask.GetMask("Ground"));
            var res = GetFirst(all);
            float actualLength = length;
            if (res.transform != null) {
                actualLength = math.min(length, res.distance);
            }
            Vector3 posBias = direction * actualLength / 2;
            transform.position += posBias;
            Vector3 localScale = transform.localScale;
            if (direction.x != 0) {
                localScale.x += actualLength;
            } else {
                localScale.y += actualLength;
            }
            transform.localScale = localScale;
            return length - actualLength;
        }

        public List<float> historyx = new List<float>();
        public List<float> historyy = new List<float>();

        /**
        * 返回是否成功
        */
        // public bool Extend(Vector2 direction, float targetScale) {
        public bool Extend(Vector2 direction, float power) {
            if (direction.x != 0) {
                if (math.abs(transform.localScale.x - 1) < eps) {
                    return false;
                }
            } else {
                if (math.abs(transform.localScale.y - 1) < eps) {
                    return false;
                }
            }
            if (state != BoxState.Idle) {
                return false;
            }
            float targetScale;
            bool replace = false;
            if (direction.x != 0) {
                targetScale = math.pow(transform.localScale.x, power);
                for (int i = 0; i < historyx.Count; i++) {
                    if (math.abs(historyx[i] * power - 1) < eps) {
                        replace = true;
                        historyx.RemoveAt(i);
                        break;
                    }
                }
                if (!replace) {
                    if (historyx.Count + historyy.Count >= times) return false;
                    historyx.Add(power);
                }
            } else {
                targetScale = math.pow(transform.localScale.y, power);
                for (int i = 0; i < historyy.Count; i++) {
                    if (math.abs(historyy[i] * power - 1) < eps) {
                        replace = true;
                        historyy.RemoveAt(i);
                        break;
                    }
                }
                if (!replace) {
                    if (historyx.Count + historyy.Count >= times) return false;
                    historyy.Add(power);
                }
            }
            TargetScale = targetScale;
            ExtendStart = Time.time;
            state = BoxState.Extend;
            extendDirection = direction;
            if (direction.x != 0) {
                StartScale = transform.localScale.x;
            } else {
                StartScale = transform.localScale.y;
            }
            return true;
        }

        private RaycastHit2D GetFirst(RaycastHit2D[] result) {
            float distance = 100;
            RaycastHit2D ans = default;
            foreach (var hit in result) {
                if (hit.transform.gameObject == gameObject || hit.transform.IsChildOf(transform)) {
                    continue;
                }
                if (hit.distance < distance) {
                    distance = hit.distance;
                    ans = hit;
                }
            }
            return ans;
        }

        public void NegativeFlip() {
            if (state == BoxState.Idle) {
                gameObject.layer = 9;
                NormalRender.enabled = false;
                NegativeRender.enabled = true;
                state = BoxState.Negative;
            } else if (state == BoxState.Negative) {
                var filter = new ContactFilter2D();
                filter.layerMask = LayerMask.GetMask("Ground");
                List<Collider2D> list = new List<Collider2D>();
                Physics2D.OverlapCollider(GetComponent<BoxCollider2D>(), filter, list);
                if (list.Count != 0) {
                    return;
                }
                gameObject.layer = 6;
                NormalRender.enabled = true;
                NegativeRender.enabled = false;
                state = BoxState.Idle;
            }
        }
    }

}