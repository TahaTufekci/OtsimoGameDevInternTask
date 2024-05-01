using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers
{
    public class MainSceneManager : MonoBehaviour
    {
        public void StartGame()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            AudioManager.Instance.PlaySound("PlayButton");
        }
    }
}
