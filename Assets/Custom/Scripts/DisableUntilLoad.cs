using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableUntilLoad : MonoBehaviour
{
    [SerializeField]
    private ScenePreloader preloader = default;

    // Start is called before the first frame update
    void Start()
    {
        if (preloader == null)
        {
            Destroy(gameObject);
            return;
        }

        gameObject.SetActive(false);
        preloader.SceneReady += OnSceneReady;
    }

    private void OnSceneReady()
    {
        gameObject.SetActive(true);
    }
}
