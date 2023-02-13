using System;
using UnityEngine;

namespace Mark {
    public class BGM :MonoBehaviour{
        private void Awake() {
            DontDestroyOnLoad(gameObject);
        }
    }
}