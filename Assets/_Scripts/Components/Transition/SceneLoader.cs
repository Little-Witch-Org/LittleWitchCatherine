using System.Collections;
using _Scripts.Enums;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Scripts.Components.Transition
{
    public class SceneLoader
    {
        public IEnumerator LoadSceneAsync(SceneNamesEnum sceneNameEnum)
        {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneNameEnum.ToString());
        
            yield return new WaitUntil(() => asyncLoad != null && asyncLoad.isDone);
        }
    }
}
