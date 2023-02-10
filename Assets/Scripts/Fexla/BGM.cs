using System;
using UnityEngine;

namespace Fexla {
    public class BGM :MonoBehaviour{
        private void Awake() {
            DontDestroyOnLoad(gameObject);
        }
    }
}