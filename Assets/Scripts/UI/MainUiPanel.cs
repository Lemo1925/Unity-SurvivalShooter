using UnityEngine;
using UnityEngine.UI;

public class MainUiPanel : MonoBehaviour
{
    [SerializeField]
    AudioSource m_AudioSource;
    [SerializeField]
    GameObject StartBtn, SettingBtn, ExitBtn;

    float curVolume;

    void Start()
    {
        StartBtn.GetComponent<Button>().onClick.AddListener(() => SceneConfig.LoadGameScene());
        SettingBtn.GetComponent<Button>().onClick.AddListener(OnSettingBtnClick);
        ExitBtn.GetComponent<Button>().onClick.AddListener(()=>Application.Quit());

        m_AudioSource.volume = GameConfig.config.MusicVolume;
    }

    void Update()
    {
        curVolume = GameConfig.config.MusicVolume;
        if ( m_AudioSource.volume != curVolume)
        {
            m_AudioSource.volume = curVolume;
        }
    }

    public void OnSettingBtnClick() => ScorePanel.Instance.ShowMenuPanel();
}
