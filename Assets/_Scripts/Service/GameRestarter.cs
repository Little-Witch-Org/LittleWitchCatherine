using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameRestarter : MonoBehaviour
{
    public void FullRestart()
    {
        StartCoroutine(RestartCoroutine());
    }

    private IEnumerator RestartCoroutine()
    {
        // 1. Уничтожаем все DontDestroyOnLoad-объекты (кроме этого скрипта)
        DestroyAllDontDestroyOnLoadObjects();

        // 2. Загружаем временную пустую сцену (синхронно)
        SceneManager.LoadScene("RestartScene", LoadSceneMode.Single);

        // 3. Ждём 1 кадр, чтобы сцена точно загрузилась
        yield return null;

        // 4. Загружаем основную сцену
        SceneManager.LoadScene("CatherineHouseMap", LoadSceneMode.Single);
    }

    private void DestroyAllDontDestroyOnLoadObjects()
    {
        // Создаём временный объект, чтобы получить доступ к DontDestroyOnLoad
        GameObject tempObj = new GameObject("TempForDestroy");
        DontDestroyOnLoad(tempObj);

        // Удаляем все объекты в DontDestroyOnLoad, кроме временного
        foreach (GameObject obj in tempObj.scene.GetRootGameObjects())
        {
            if (obj != tempObj && obj != this.gameObject) // Не удаляем себя и временный объект
                Destroy(obj);
        }

        // Удаляем временный объект
        Destroy(tempObj);
    }

    private void Awake()
    {
        // Сохраняем этот объект и скрипт при перезагрузке
        DontDestroyOnLoad(gameObject);
    }
}