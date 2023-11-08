using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;

/// <summary>
/// Attached to each scene trigger
/// </summary>
public class SceneInteraction : MonoBehaviour {
    
    // The name of the next scene to show.
    public string nextSceneName;

    // Description of the trigger.
    public string description = "Gaze to go to new scene.";

    // Use this for initialization
    void Start () {

        TimerManager.Instance.AttachTimerEvents(gameObject);
    }

    /// <summary>
    /// Register the change scene function to the timer. 
    /// </summary>
    public void RegisterChangeScene()
    {
        TimerManager.Instance.DoAction = ChangeScene;
        GlobalManager.Instance.ChangeDescription(description, gameObject);

    }

    /// <summary>
    /// Change scene to the next one.  
    /// </summary>
    public void ChangeScene()
    {
        GlobalManager.Instance.descriptionPanel.SetActive(false);

        GlobalManager.Instance.backgroundTransMask.GetComponent<Renderer>().enabled = true;
        Material backgoundMaskMaterial = GlobalManager.Instance.backgroundTransMask.GetComponent<Renderer>().material;
        backgoundMaskMaterial.SetTexture("_Tex", GlobalManager.Instance.currentSceneContents.GetComponent<SceneData>().backgroundTexture);
        backgoundMaskMaterial.SetFloat("_BlendCubemaps", 1f);
        backgoundMaskMaterial.SetFloat("_Transparency", 1f);

        GameObject nextScene = GameObject.Find(nextSceneName);

        Debug.Log(nextSceneName);
        
        // Set new scene
        GlobalManager.Instance.currentSceneContents = nextScene;

        backgoundMaskMaterial.SetTexture("_Tex2", nextScene.GetComponent<SceneData>().backgroundTexture);
        

        GlobalManager.Instance.imageBackground.GetComponent<Renderer>().material.SetTexture("_Tex", nextScene.GetComponent<SceneData>().backgroundTexture);

        // Disable video.
        GlobalManager.Instance.videoBackground.GetComponent<VideoPlayer>().Stop();
        GlobalManager.Instance.videoBackground.GetComponent<Renderer>().enabled = false;

        GlobalManager.Instance.BackgroundTransition();
        

        // Disable the old scene (this) finally. 
        GetComponentInParent<SceneData>().currentChildrenGroup.SetActive(false);
    }

}

/// <summary>
/// Class reserved for future use. It will help save and load scene trigger data from database.
/// </summary>
public class SceneTriggerHelper
{
    public string nextSceneName;

    public void InitiateSceneTriggerOjbect()
    {

    }
}
