using UnityEngine;
using UnityEngine.UI;

public class GameTimer : MonoBehaviour
{
    private const float m_TotalTime = 10 * 60f;
    public static bool isPaused = false;
    private float m_ElapsedTime;
    private Text m_TimeTextComp;
 
    public void Start()
    {
        m_TimeTextComp = GameObject.Find("Canvas/Time").GetComponent<Text>();
        m_ElapsedTime = m_TotalTime;
        InvokeRepeating(nameof(CountDownTimer), 0, 1.0f);
    }

    private void Update() => m_TimeTextComp.text = $"Weak Up: {GetTimer()}";

    private void CountDownTimer()
    {
        if (!isPaused && m_ElapsedTime > 0)
        {
            m_ElapsedTime--;
        }
        else if (m_ElapsedTime < 0)
        {
            CancelInvoke(nameof(CountDownTimer));
            GameManager.Instance.GameOver(GameState.Win);
        }
    }

    public static void Pause()
    {
        Time.timeScale = 0;
        isPaused = true;
    }

    public static void Resume()
    {
        Time.timeScale = 1.0f;
        isPaused = false;
    }

    public int GameMinutes() => (int)((m_ElapsedTime % 3600) / 60);
    public string GetTimer()
    {
        int minutes = GameMinutes();
        int seconds = (int)(m_ElapsedTime % 60);

        return string.Format("{0:D2}:{1:D2}", minutes, seconds);
    }
}
