using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class TestHelpers
{
    public static IEnumerator LoadScene(string sceneName)
    {
        var loadSceneTask = SceneManager.LoadSceneAsync(sceneName);
        while (!loadSceneTask.isDone) yield return null;
    }

    public static PlayerShip GetPlayerShip()
    {
        var ship = GameObject.FindObjectOfType<PlayerShip>(true);
        ship.gameObject.SetActive(true);
        return ship;
    }

    public static GameManager GetGameManager()
    {
        return GameObject.FindObjectOfType<GameManager>();
    }

    public static Human GetHuman()
    {
        return GameObject.FindObjectOfType<Human>();
    }
}