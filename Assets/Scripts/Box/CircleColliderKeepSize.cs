using Unity.Mathematics;
using UnityEngine;

namespace Part {
    public class CircleColliderKeepSize : MonoBehaviour {
        public float radius = 0.1f;

        public CircleCollider2D circleCollider2D;

        // Update is called once per frame
        void Update() {
            circleCollider2D.radius = radius / math.max(transform.lossyScale.x, transform.lossyScale.y);
        }
    }
}