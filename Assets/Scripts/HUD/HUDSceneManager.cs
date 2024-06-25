using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HUDSceneManager : MonoBehaviour
{
    public void OnStartGame()
    {
/*        GameEventSystem.Instance.ClearAll();
        GameStateEvent.Instance.ClearAll();*/
        SceneManager.LoadScene(GameParametres.SceneName.SCENE_GAME);
    }

    public void OnMainMenu()
    {
        SceneManager.LoadScene(GameParametres.SceneName.SCENE_MENU);
    }

    public void OnQuitGame()
    {
        Application.Quit();
    }
}
