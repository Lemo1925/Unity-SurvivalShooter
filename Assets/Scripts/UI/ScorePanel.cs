using UnityEngine;
using UnityEngine.UI;

public class ScorePanel : MonoBehaviour
{
    private static ScorePanel _instance;
    public static ScorePanel Instance
    {
        get
        {
            if (_instance == null)
            {
                Transform Canvas = GameObject.Find("Canvas").transform;
                _instance = Canvas.Find("ScorePanel").GetComponent<ScorePanel>();
            }
            return _instance;
        }
    }
    public Text m_TextComp;
    public Slider m_MusicSlider,m_EffectSlider;
    public Text m_MusicTextComp, m_EffectTextComp;
    public GameObject continueBtn, restarBtn, closeBtn, exitBtn;
    public GameObject GameButtonPanel, MenuButtonPanel;
    void Start()
    {
        continueBtn.GetComponent<Button>().onClick.AddListener(HidePanel);
        restarBtn.GetComponent<Button>().onClick.AddListener(OnRestartBtnClick);
        exitBtn.GetComponent<Button>().onClick.AddListener(OnExitBtnClick);
        closeBtn.GetComponent<Button>().onClick.AddListener(HidePanel);
        m_MusicSlider.value = GameConfig.config.MusicVolume;
        m_EffectSlider.value = GameConfig.config.EffectVolume;
    }

    private void Update() => UpdateUI();

    private void UpdateUI()
    {
        m_MusicTextComp.text = $"{(int)(m_MusicSlider.value * 100)}";
        m_EffectTextComp.text = $"{(int)(m_EffectSlider.value * 100)}";
        GameConfig.UpdateConfig(m_MusicSlider.value, m_EffectSlider.value);
    }

    public void ShowPanel(string text, bool showCountinue)
    {
        GameTimer.Pause();
        GameButtonPanel.SetActive(true);
        MenuButtonPanel.SetActive(false);
        continueBtn.SetActive(showCountinue);
        restarBtn.SetActive(!showCountinue);
        gameObject.SetActive(true);
        m_TextComp.text = text;
    }

    public void ShowMenuPanel()
    {
        GameTimer.Pause();
        MenuButtonPanel.SetActive(true);
        GameButtonPanel.SetActive(false);
        gameObject.SetActive(true);
        m_TextComp.text = "Game Setting";
    }

    public void HidePanel()
    {
        GameTimer.Resume();
        gameObject.SetActive(false);
    }

    public void OnExitBtnClick()
    {
        GameTimer.Resume();
        SceneConfig.LoadMenuScene();
    }

    public void OnRestartBtnClick()
    {
        GameTimer.Resume();
        SceneConfig.LoadGameScene();
    }

    private void OnDisable() => GameConfig.SaveConfig();

}
