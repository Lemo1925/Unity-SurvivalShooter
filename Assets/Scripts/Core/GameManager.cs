using UnityEngine;

public enum GameState { Win, Fail};
public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
                _instance = GameObject.Find("GameManager").GetComponent<GameManager>();
            return _instance;
        }
    }

    private static string GameTutorials = "\r\nMove: W、A、S、D\r\nShoot: Left Mouse Button\r\n\r\nLive Until Wake Up";

    private int playerKillNum;
    public GameTimer GameTimer;
    private void Start() => InitGame();
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ScorePanel.Instance.ShowPanel(GameTutorials, true);
        }
    }
    void InitGame()
    {
        playerKillNum = 0;
        AddAllManagerScript();
    }
    private void AddAllManagerScript()
    {
        GameTimer = AddScript<GameTimer>();
    }

    T AddScript<T>() where T : MonoBehaviour
    {
        T script = GetComponent<T>();
        if (script == null) script = this.gameObject.AddComponent(typeof(T)) as T;
        if (script!.enabled == false) script.enabled = true;
        return script;
    }

    public void GameOver(GameState state)
    {
        switch (state)
        {
            case GameState.Win:
                ScorePanel.Instance.ShowPanel("Wake Up", false);
                break;
            case GameState.Fail:
                ScorePanel.Instance.ShowPanel("Fail", false);
                break;
        }
    }

    public int GetPlayerKillNum() => playerKillNum;

    public void AddPlayerKillNum() => playerKillNum++;
    public int GetStage()
    {
        var timeLeft = GameTimer.GameMinutes();
        var stage = 0;
        if (timeLeft < 4 && timeLeft >= 3)
        {
            stage = 1;
        }
        else if (timeLeft < 3 && timeLeft >= 1)
        {
            stage = 2;        
        }
        else if (timeLeft < 1)
        {
            stage = 3;
        }
        return stage;
    }
}
