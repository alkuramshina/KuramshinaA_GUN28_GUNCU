using UnityEngine.SceneManagement;

namespace Controllers
{
    public class UnitySceneController
    {
        public void OpenMainScene() => SceneManager.LoadScene(0);
        public void OpenGameScene() => SceneManager.LoadScene(1, LoadSceneMode.Additive);
    }
}