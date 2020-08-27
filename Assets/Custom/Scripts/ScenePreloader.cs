using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class ScenePreloader : MonoBehaviour
{
    public event Action SceneReady;

    [SerializeField]
    private int sceneBuildIndex;

    [SerializeField]
    private TMP_Text loadingText;

    private AsyncOperation asyncLoad;

    public float LoadProgress { get; private set; } = 0f;

    private IEnumerator Start()
    {
        asyncLoad = SceneManager.LoadSceneAsync(sceneBuildIndex);

        asyncLoad.allowSceneActivation = false;

        while (!asyncLoad.isDone)
        {
            LoadProgress = asyncLoad.progress;
            if (LoadProgress >= 0.9f)
            {
                SceneReady?.Invoke();
                if (loadingText != null)
                {
                    loadingText.gameObject.SetActive(false);
                }
                break;
            }
            yield return null;
        }
    }

    public void ActivateScene()
    {
        if (asyncLoad.progress >= 0.9f)
        {
            asyncLoad.allowSceneActivation = true;
        }
    }

}
