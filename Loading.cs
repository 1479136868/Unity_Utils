using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneInfoManager
{
    public static string _NextSceneName;

    public static void LoadingNextScene(string nextName) { _NextSceneName = nextName; SceneManager.LoadScene("Loading"); }
}


public class Loading : MonoBehaviour
{
    Slider slider;
    Text txt;
    void Start()
    {
        StartCoroutine(LoadScene());
    }
    AsyncOperation async;
    IEnumerator LoadScene()
    {
        async = SceneManager.LoadSceneAsync(SceneInfoManager._NextSceneName);
        async.allowSceneActivation = false;
       yield return null;
    }

    private void Awake()
    {
        txt = transform.Find("Text").GetComponent<Text>();
        slider = transform.Find("Slider").GetComponent<Slider>();
        slider.value = 0;
    }

    float value = 0;
    void Update()
    {
        txt.text = (int)(slider.value*100) + "%";
        if (async.progress < 0.9f)
        {
            value = async.progress;
        }
        else
        {
            value = 1f;
        }
        slider.value = Mathf.MoveTowards(slider.value, value, Time.deltaTime * 0.3f);
        if (slider.value >= 1f)
        {
            async.allowSceneActivation = true;
        }
    }
}
