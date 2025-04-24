using UnityEngine;
using System.Collections;
using System;
using Games.Bingo;
public class BingoInactivityManager : MonoBehaviour
{
    public static BingoInactivityManager instance;
    private float timeSinceLastClick = 0f;
    private bool isTimerRunning = false;
    private bool timeoutTriggered = false;
    public static event Action OnTimeoutEvent;
    public static event Action OnTimeResetEvent;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }
    private void Start()
    {
        StartInactivityTimer();
    }
    private IEnumerator TrackInactivity()
    {
        isTimerRunning = true;
        timeSinceLastClick = 0f;
        timeoutTriggered = false;
        while (timeSinceLastClick < 5f)
        {
            timeSinceLastClick += Time.deltaTime;
            yield return null;
        }
        if (!timeoutTriggered)
        {
            timeoutTriggered = true;
            OnTimeout();
        }
        isTimerRunning = false;
    }
    public void StartInactivityTimer()
    {
        if (!isTimerRunning)
            StartCoroutine(TrackInactivity());
    }
    public void ResetTimer()
    {
        OnTimeResetEvent?.Invoke();
        StopAllCoroutines();
        StartInactivityTimer();
    }
    void Update()
    {
         
            if(UIManager.instance.isTutorial)
                UIManager.instance.hideTutorial();
         
    }
    private void OnTimeout()
    {
        OnTimeoutEvent?.Invoke();
    }
}