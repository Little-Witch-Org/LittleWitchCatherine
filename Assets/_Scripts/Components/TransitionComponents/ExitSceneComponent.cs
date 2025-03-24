using _Scripts;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitSceneComponent : MonoBehaviour
{
    [SerializeField] private SceneNames SceneName;
    
    public void Exit()
    {
        SceneManager.LoadScene(SceneName.ToString());
    }
}
