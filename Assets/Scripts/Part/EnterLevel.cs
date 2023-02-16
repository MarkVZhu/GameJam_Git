using UnityEngine;
using UnityEngine.SceneManagement;

namespace Mark {
    public class EnterLevel : MonoBehaviour {
        public void Enter(int x) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}