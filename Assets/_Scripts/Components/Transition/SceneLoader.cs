using System.Collections;
using _Scripts;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader
{
    public IEnumerator LoadSceneAsync(SceneNames sceneName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName.ToString());
        
        yield return new WaitUntil(() => asyncLoad != null && asyncLoad.isDone);
    }
}
