using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Mark {
    public class SceneReload : MonoBehaviour {
        private void Update() {
            if (Input.GetKeyDown(KeyCode.Escape)) {
                Application.Quit();
            }
            if (Input.GetKeyDown(KeyCode.Backspace)) {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
    }
}