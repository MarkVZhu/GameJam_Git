using System;
using UnityEngine;

namespace Mark {
    public class ShaderControl : MonoBehaviour {
        private static readonly int Lrud = Shader.PropertyToID("_lrud");
        public SpriteRenderer spriteRenderer;

        private void Update() {
            Vector4 v = new Vector4(
                transform.position.x - transform.localScale.x * 0.5f,
                transform.position.x + transform.localScale.x * 0.5f,
                transform.position.y - transform.localScale.y * 0.5f,
                transform.position.y + transform.localScale.y * 0.5f
            );

            MaterialPropertyBlock materialPropertyBlock = new MaterialPropertyBlock();
            materialPropertyBlock.SetVector(Lrud, v);
            spriteRenderer.SetPropertyBlock(materialPropertyBlock);
        }
    }
}