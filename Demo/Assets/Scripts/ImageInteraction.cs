using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// Script attached to image trigger or text trigger.
/// </summary>
// F
public class ImageInteraction : MonoBehaviour {

    // The renderer of the trigger, which is flashing to indicate the trigger area.
    private Renderer selfRenderer;

    // Renderer of the image. If this is served as a text trigger, this field is left null.
    public Image imageRenderer;

    // Canvas of the image panel. If this is served as a text trigger, this field is left null.
    public Canvas imagePanel;

    // Coroutine of hiding image animation.
    public Coroutine hideCouroutineNow;

    // Description for this trigger to display as tips.
    public string description = "Gaze to see details.";


    // Use this for initialization
    void Start () {
        
        // Hide image panel in the beginning
        if (imagePanel != null) imagePanel.enabled = false;
        selfRenderer = GetComponent<Renderer>();

        TimerManager.Instance.AttachTimerEvents(gameObject);

    }
	

    /// <summary>
    /// Register show image function to timer. 
    /// </summary>
    public void RegisterShowImage()
    {
        // If the image panel is not null (this is not served as a text trigger), link the show image function to the timer action.
        if (imagePanel != null) TimerManager.Instance.DoAction = ShowImage;
        if (imagePanel == null || imagePanel.enabled)
        {
            TimerManager.Instance.isAlreadyPlaying = true;
        }

        // Show text description
        GlobalManager.Instance.ChangeDescription(description, gameObject);
    }

    /// <summary>
    /// Show image action.
    /// </summary>
    public void ShowImage()
    {
        imagePanel.enabled = true;
        selfRenderer.enabled = false;

        if (hideCouroutineNow != null)
        {
            StopCoroutine(hideCouroutineNow);
        }

        StartCoroutine(ShowImageCoroutine());
        
        GlobalManager.Instance.descriptionPanel.SetActive(true);
    }

    // Animate the fade-in effect for showing image.
    IEnumerator ShowImageCoroutine()
    {
        float leftTime = 0f;

        while (true)
        {
            leftTime += Time.deltaTime;

            if (leftTime >= 1f)
            {
                imageRenderer.fillAmount = 1;
                break;
            }

            imageRenderer.fillAmount = leftTime;
            yield return null;
        }

    }

    /// <summary>
    /// Hide image when no gazing at the trigger.
    /// </summary>
    public void HideImage()
    {
        if (hideCouroutineNow != null)
        {
            StopCoroutine(hideCouroutineNow);
        }

        if (imagePanel.enabled)
        {
            hideCouroutineNow = StartCoroutine(HideImageCoroutine());
        }
    }

    /// <summary>
    /// Hide the image after several seconds, and animate the dissappearing effect for hiding.
    /// </summary>
    /// <returns></returns>
    public IEnumerator HideImageCoroutine()
    {
        yield return new WaitForSeconds(10f);

        float leftTime = 0f;

        while (true)
        {
            leftTime += Time.deltaTime;

            if (leftTime >= 1f)
            {
                imageRenderer.fillAmount = 0f;
                imagePanel.enabled = false;

                break;
            }

            imageRenderer.fillAmount = 1f - leftTime;

            yield return null;
        }

        selfRenderer.enabled = true;
    }

    /// <summary>
    /// Hide the image and show the trigger again, if the scene is deactivated.
    /// </summary>
    private void OnDisable()
    {
        if (imagePanel != null) imagePanel.enabled = false;
        selfRenderer.enabled = true;
    }
    
}

/// <summary>
/// Class reserved for future use. It will help save and load image trigger data from database.
/// </summary>
public class ImageTriggerHelper
{
    public string parentSceneName;

    public string textContent;
    public int textSize;

    public float textShowTime;

    public Vector3 triggerPosition;
    public Vector3 triggerRotation;

    public Vector3 panelPosition;
    public Vector3 panelRotation;
    public Vector3 panelScale;

    public Vector4 panelColor;



    public void InstantiateImageTrigger()
    {

    }
}