using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingSceneManager : MonoBehaviour
{
    public static string nextScene;

    [SerializeField]
    Image progressBar;
    [SerializeField]
    Image character;
    [SerializeField]
    Image monitorInside;

    private void Start()
    {
        character.color = new Color(1, 1, 1, 0);
        StartCoroutine(LoadScene());
    }

    string nextSceneName;
    public static void LoadScene(string sceneName)
    {
        nextScene = sceneName;
        SceneManager.LoadScene("LoadingScene");
    }

    IEnumerator LoadScene()
    {
        yield return null;

        AsyncOperation op = SceneManager.LoadSceneAsync(nextScene);
        op.allowSceneActivation = false;
        bool isReady = false;
        float timer = 0.0f;
        while (!op.isDone && !isReady)
        {
            yield return new WaitForSeconds(0.03f);
            monitorInside.color = new Color(timer * 5, timer * 5, timer * 5);
            timer += Time.deltaTime;
            Debug.Log(timer);
            if(timer > 0.5f)
            {
                character.color = new Color(1, 1, 1, 1);
            }
            if (timer > 1.0f && op.progress >= 0.9f)
            {
                Debug.Log("Load");
                isReady = true;
                sceneActivated(op);
            }
        }
    }
    public void sceneActivated(AsyncOperation op)
    {
        op.allowSceneActivation = true;
    }
}