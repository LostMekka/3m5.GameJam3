using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour {

    public void LoacScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void ExitGameButton()
    {
        Application.Quit();
    }
    
}
