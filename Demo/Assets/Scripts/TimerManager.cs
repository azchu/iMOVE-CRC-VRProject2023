using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

// Class for managing timer of gaze interaction. Inherit singleton.
public class TimerManager : ManagerSingleton<TimerManager> {

    // Wait for several seconds to invoke action.
    public int timeWait = 3;

    // Timer's panel which shows a circle to indicate counting state.
    public Transform timerPanel;

    // Time count now. Count from 0 everytime a counting process starts.
    public float timer;

    // For storing an existing counting process. If another interaction is invoked, it will be replaced by the new counting process.
    private Coroutine timeCoroutineNow;

    // The action to do when the time is reached.
    public Action DoAction;

    // Flag to check if the image or video is already in playing process.
    public bool isAlreadyPlaying;


    private void Start()
    {

    }

    /// <summary>
    /// Start timer.
    /// </summary>
    public void StartTimer()
    {
        timeCoroutineNow = StartCoroutine(IncreaseTimer());
    }

    /// <summary>
    /// Stop timer and clear the circle indicator.
    /// </summary>
    public void StopTimer()
    {
        GlobalManager.Instance.HideDescriptionPanel();

        DoAction = null;
        
        if (timeCoroutineNow != null)
        {
            StopCoroutine(timeCoroutineNow);
        }

        timer = 0.0f;
        timerPanel.GetComponent<Image>().fillAmount = 0f;
        isAlreadyPlaying = false;

    }

    // The time counting loop thread.
    IEnumerator IncreaseTimer()
    {
        timer = 0.0f;

        while (true)
        {
            // When this trigger is already activated, stop counting.
            if (isAlreadyPlaying) 
            {
                timerPanel.GetComponent<Image>().fillAmount = 0f;
                break;
            }

            // When this trigger is not active, increase timer and fill the circle indicator.
            timer += Time.deltaTime;
            timerPanel.GetComponent<Image>().fillAmount = timer / timeWait;

            if (timer >= timeWait)
            {
                if (DoAction != null)
                {
                    DoAction();
                }
                timerPanel.GetComponent<Image>().fillAmount = 0f;
                break;
            }

            // Wait for one frame. 
            yield return null;
        }
    }

    /// <summary>
    /// Attach timer event to the trigger script when a trigger is created in the scene.
    /// </summary>
    /// <param name="triggerSender"> The trigger itself. </param>
    public void AttachTimerEvents(GameObject triggerSender)
    {
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerEnter;
        entry.callback.AddListener((eventData) => { StartTimer(); });
        triggerSender.GetComponent<EventTrigger>().triggers.Add(entry);

        EventTrigger.Entry entry2 = new EventTrigger.Entry();
        entry2.eventID = EventTriggerType.PointerExit;
        entry2.callback.AddListener((eventData) => { StopTimer(); });
        triggerSender.GetComponent<EventTrigger>().triggers.Add(entry2);
    }
}
