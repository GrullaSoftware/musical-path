using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.Mathematics;

public class LoadScene : MonoBehaviour
{
    public GameObject loadingScreen;
    public Slider loadingBar;
    public int IndexScene = 1;
    public TMP_Text progressText;
    private void Start()
    {
        LoadSceneByIndex(IndexScene);
    }
    private void OnEnable()
    {
        LoadSceneByIndex(IndexScene);
    }
    public void LoadSceneByIndex(int sceneIndex)
    {
        StartCoroutine(LoadSceneAsynchronusly(sceneIndex));
    }
    private IEnumerator LoadSceneAsynchronusly(int sceneIndex)
    {
        yield return new WaitForSecondsRealtime(1f);
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);
        while (!operation.isDone)
        {
            Debug.Log(operation.progress);
            loadingBar.value = operation.progress;
            progressText.text =  math.round(operation.progress * 100 / 0.9f) + "%";
            yield return new WaitForSecondsRealtime(10f);
        }
    }
}
