using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Fexla {
    public class EndControl : MonoBehaviour {

        private void OnTriggerEnter2D(Collider2D col) {
            if (col.gameObject.CompareTag("Player")) {
                Next();
            }
        }

        public void Update() {
            if (Input.GetKeyDown(KeyCode.Comma)) {
                Previous();
            }
            if (Input.GetKeyDown(KeyCode.Period)) {
                Next();
            }
        }

        public void Previous() {
            if(AudioManager.Instance)
                AudioManager.Instance.PlaySE(AudioManager.SoundEffect.Card);
            SceneManage.instance.LastScene();
        }

        public void Next() {
            if (AudioManager.Instance)
                AudioManager.Instance.PlaySE(AudioManager.SoundEffect.Card);
            SceneManage.instance.NextScene();
        }
    }
}