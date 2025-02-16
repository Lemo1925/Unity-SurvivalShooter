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

    public int playerKillNum;
    public GameTimer GameTimer;
    private void Start() => InitGame();
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ScorePanel.Instance.ShowPanel("Game Pause", true);
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
                ScorePanel.Instance.ShowPanel("Weak Up", false);
                break;
            case GameState.Fail:
                ScorePanel.Instance.ShowPanel("Fail", false);
                break;
        }
    }

    public int GetStage()
    {
        var timeLeft = GameTimer.GameMinutes();
        var stage = 0;
        if ((timeLeft <= 7 && timeLeft > 5) || playerKillNum >= 10)
        {
            stage = 1;
        }
        else if ((timeLeft <= 5 && timeLeft > 2) || playerKillNum >= 30)
        {
            stage = 2;        
        }
        else if ((timeLeft <= 2)|| playerKillNum > 50)
        {
            stage = 3;
        }
        return stage;
    }
}
