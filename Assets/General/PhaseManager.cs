using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class PhaseManager : MonoBehaviour
{
    public float duration = 10f; // Duration of the timer in seconds
    private float timeRemaining;
    private bool isRunning = false;

    public string sceneName;

    void Start()
    {
        ResetTimer();
        StartTimer();
    }

    void Update()
    {
        if (isRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
            }
            else
            {
                timeRemaining = 0;
                isRunning = false;
                OnTimerEnd();
            }
        }
    }

    // Start the timer
    public void StartTimer()
    {
        isRunning = true;
    }

    // Stop the timer
    public void StopTimer()
    {
        isRunning = false;
    }

    // Reset the timer to its original duration
    public void ResetTimer()
    {
        timeRemaining = duration;
    }

    // Called when the timer ends
    private void OnTimerEnd()
    {
            LoadScene(sceneName);
 
        ResetTimer();
        StartTimer();
    }

    // Load a scene based on the provided name
    private void LoadScene(string sceneName)
    {
        if (!string.IsNullOrEmpty(sceneName))
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}