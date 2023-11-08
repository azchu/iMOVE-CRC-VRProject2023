using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GlobalManager : ManagerSingleton<GlobalManager> {

    // Background sphere of 360 image.
    public GameObject imageBackground;
    // Background sphere of 360 video.
    public GameObject videoBackground;
    // The background mask sphere for background transition.
    public GameObject backgroundTransMask;

    // The text panel for all text descriptions. 
    public GameObject descriptionPanel;
    // General menu for the app, providing tips or quit game function. 
    public GameObject menuCanvas;

    // Currently active scene's element (children) group.
    public GameObject currentSceneContents;

    // Store the trigger of the text description now.
    GameObject previousTextSender;
    // Coroutine to hide the text description panel.
    private Coroutine panelCoroutineNow;


    // Use this for initialization
    void Start () {

        descriptionPanel.SetActive(false);

    }
	
	// Update is called once per frame
	void Update () {

        // Check input to show menu. 
        if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger) || Input.GetKeyDown(KeyCode.Return))
        {
            ToggleMenu();
        }

        // Make floating animation for the text panel.
        if (descriptionPanel.activeSelf)
        {
            descriptionPanel.transform.position += new Vector3(0f, 3f * Mathf.Sin(Time.time)*Time.deltaTime, 0f);
        }
    }

    /// <summary>
    /// Toggle bottom menu to the floor or in front of viewer. 
    /// </summary>
    public void ToggleMenu()
    {
        RectTransform menuTransform = menuCanvas.GetComponentInChildren<RectTransform>();
        Transform menuParentTransform= menuCanvas.GetComponent<Transform>();

        if (menuTransform.rotation != Quaternion.identity)
        {
            menuParentTransform.rotation = Quaternion.identity;
            menuTransform.rotation = Quaternion.identity;
        }
        else
        {
            menuTransform.rotation = Quaternion.Euler(-80f, 0f, 0f);
            menuParentTransform.rotation = Camera.main.transform.rotation;

        }
    }

    /// <summary>
    /// Hide description panel.
    /// </summary>
    public void HideDescriptionPanel()
    {
        if (panelCoroutineNow == null)
        {
            panelCoroutineNow = StartCoroutine(HideDescriptionPanelWait());
        }
        else
        {
            StopCoroutine(panelCoroutineNow);
            panelCoroutineNow = StartCoroutine(HideDescriptionPanelWait());
        }
    }

    // Wait several seconds before hiding description panel.
    IEnumerator HideDescriptionPanelWait()
    {
        yield return new WaitForSeconds(10f);
        descriptionPanel.SetActive(false);
    }

    /// <summary>
    /// Change and show description of a new trigger.
    /// </summary>
    /// <param name="text"> Text to show </param>
    /// <param name="sender"> Trigger that sends this description. </param>
    public void ChangeDescription(string text, GameObject sender)
    {
        if (panelCoroutineNow != null) StopCoroutine(panelCoroutineNow);
        if (previousTextSender ==sender && descriptionPanel.activeSelf) return;
        previousTextSender = sender;
        descriptionPanel.SetActive(false);
        descriptionPanel.GetComponentInChildren<Text>().text = text;
        descriptionPanel.GetComponentInParent<RectTransform>().rotation = Camera.main.transform.rotation;
        descriptionPanel.SetActive(true);
    }

    /// <summary>
    /// Call the coroutine function to start visual effect of background change. 
    /// </summary>
    public void BackgroundTransition()
    {
        StartCoroutine(StartBackgroundTransition());
    }

    /// <summary>
    /// Make fade-in transition of background change by modifying background material's transparency value. 
    /// </summary>
    /// <returns></returns>
    public IEnumerator StartBackgroundTransition()
    {
        float leftTime = 1f;
        Renderer backgroundMaskRenderer = backgroundTransMask.GetComponent<Renderer>();

        while (true)
        {
            leftTime -= Time.deltaTime;

            if (leftTime <= 0f)
            {
                backgroundMaskRenderer.enabled = false;
                break;
            }
            backgroundMaskRenderer.material.SetFloat("_BlendCubemaps", leftTime);

            yield return null;
        }

        currentSceneContents.GetComponent<SceneData>().currentChildrenGroup.SetActive(true);
    }

    /// <summary>
    /// Register quit game function to timer.
    /// </summary>
    public void RegisterQuitGame()
    {
        TimerManager.Instance.DoAction = QuitGame;

    }

    /// <summary>
    /// Quit game function.
    /// </summary>
    public void QuitGame()
    {
        TimerManager.Instance.DoAction = Application.Quit;
        Debug.Log("Quit Game");
    }

    
}
