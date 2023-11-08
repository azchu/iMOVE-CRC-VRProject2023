using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.EventSystems;

/// <summary>
/// Attached to a video player
/// </summary>
public class VideoInteraction : MonoBehaviour {

    // Renderer of this trigger
    private Renderer selfRenderer;

    // Renderer of the video to play.
    public Renderer videoRenderer;

    // Video player this trigger links to. For Background Video, you should manually assign global VideoSphere to it.
    public VideoPlayer videoPlayer;

    // The video clip to play.
    public VideoClip videoClip;

    // Path of the video of this trigger. This is reserved for future development.
    private string videoClipPath;
    

    // Hide a trigger, if it needs to show only after this trigger has been actived once.
    public GameObject nextTriggerHide;

    // Description for this trigger to display as tips.
    public string description = "Gaze to watch video.";


    // Use this for initialization
    void Start () {

        if (nextTriggerHide != null) nextTriggerHide.SetActive(false);
        selfRenderer = GetComponent<Renderer>();
        videoRenderer.enabled = false;

        TimerManager.Instance.AttachTimerEvents(gameObject);
    }

    /// <summary>
    /// Register the play video function to the timer. 
    /// </summary>
    public void RegisterPlayVideo()
    {
        TimerManager.Instance.DoAction = PlayVideo;
        videoPlayer.clip = videoClip;

        if (videoPlayer.isPlaying)
        {
            TimerManager.Instance.isAlreadyPlaying = true;
            GlobalManager.Instance.descriptionPanel.SetActive(false);
        }
        else
        {
            GlobalManager.Instance.ChangeDescription(description, gameObject);
        }
    }

    /// <summary>
    /// Play video and enable the renderer for playing.
    /// </summary>
    public void PlayVideo()
    {
        videoPlayer.loopPointReached += CheckOver;

        videoPlayer.Play();
        videoRenderer.enabled = true;
        StartCoroutine(VideoPlayRender());
        GlobalManager.Instance.descriptionPanel.SetActive(false);
    }

    // Check if the video is loaded and playing. If yes, activate the fade-in effect and enable the renderer.
    IEnumerator VideoPlayRender()
    {
        while (true)
        {
            if (videoPlayer.isPlaying)
            {
                StartCoroutine(BackgroundMaskTransition());

                videoRenderer.enabled = true;
                selfRenderer.enabled = false;

                break;
            }
            else
            {
                videoRenderer.enabled = false;
            }
            yield return null;
        }

    }
    
    // Make fade-in effect of the starting playing video by modifying material transparency.
    IEnumerator BackgroundMaskTransition()
    {
        GlobalManager.Instance.backgroundTransMask.GetComponent<Renderer>().enabled = true;
        Material backgoundMaskMaterial = GlobalManager.Instance.backgroundTransMask.GetComponent<Renderer>().material;
        Renderer backgroundMaskRenderer = GlobalManager.Instance.backgroundTransMask.GetComponent<Renderer>();


        float leftTime = 1f;

        while (true)
        {
            leftTime -= Time.deltaTime;

            if (leftTime <= 0f)
            {
                backgroundMaskRenderer.enabled = false;
                videoRenderer.material.SetFloat("_Transparency", 1f);
                break;
            }

            backgoundMaskMaterial.SetFloat("_Transparency", leftTime);
            videoRenderer.material.SetFloat("_Transparency", 1f - leftTime);

            yield return null;
        }
    }

    /// <summary>
    /// Method for checking the end of the video. If the video ends, it will trigger Fade-in effect and re-enable trigger.
    /// </summary>
    /// <param name="vp"></param>
    public void CheckOver(VideoPlayer vp)
    {
        StartCoroutine(BackgroundMaskCloseTransition());

        if (nextTriggerHide != null) nextTriggerHide.SetActive(true);
        selfRenderer.enabled = true;

        Debug.Log("Video over");
        videoPlayer.loopPointReached -= CheckOver;
    }


    // When the video ends, also make fade-in effect of dissappearing renderer by modifying material transparency.
    IEnumerator BackgroundMaskCloseTransition()
    {
        GlobalManager.Instance.backgroundTransMask.GetComponent<Renderer>().enabled = true;
        Material backgoundMaskMaterial = GlobalManager.Instance.backgroundTransMask.GetComponent<Renderer>().material;
        Renderer backgroundMaskRenderer = GlobalManager.Instance.backgroundTransMask.GetComponent<Renderer>();


        float leftTime = 0f;

        while (true)
        {
            leftTime += Time.deltaTime;

            if (leftTime >= 1f)
            {
                videoRenderer.enabled = false;

                backgroundMaskRenderer.enabled = false;

                break;
            }

            backgoundMaskMaterial.SetFloat("_Transparency", leftTime);
            videoRenderer.material.SetFloat("_Transparency", 1f - leftTime);

            yield return null;
        }
    }

    /// <summary>
    /// Stop playing the video and hide the video renderer, if the scene is changed.
    /// </summary>
    private void OnDisable()
    {
        videoRenderer.enabled = false;
        selfRenderer.enabled = true;
        videoPlayer.loopPointReached -= CheckOver;
    }
}

/// <summary>
/// Class reserved for future use. It will help save and load video trigger data from database.
/// </summary>
public class VideoTriggerHelper
{
    public string parentSceneName;

    public string videoClipPath;

    public Vector3 videoPanelRotation;


    public void InitiateVideoTrigger()
    {

    }
}