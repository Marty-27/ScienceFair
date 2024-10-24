using UnityEngine;

public class Timer : MonoBehaviour
{
    public TextMesh timerTextMesh;
    private float elapsedTime;

    void Start()
    {
        elapsedTime = 0f;
    }

    void Update()
    {
        elapsedTime += Time.deltaTime;

        string timeString = FormatTime(elapsedTime);

        timerTextMesh.text = timeString;
    }

    string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
