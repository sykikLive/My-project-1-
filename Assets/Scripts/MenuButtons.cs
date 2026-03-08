using UnityEngine;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;

public class MenuButtons : MonoBehaviour

{
    public float delay = 0.5f;
    public string LevelToLoad = "lvl1";
   public async void PlayGame() {
        await Task.Delay(250);
        SceneManager.LoadScene(LevelToLoad);
    }

    public void QuitGame() {
        Application.Quit();
    }

}
