using UnityEngine;


public class MusicController : MonoBehaviour
{
    private static MusicController _instance;
    public static MusicController Instance
    {
        get
        {
            if (_instance == null)
                _instance = GameObject.Find("GameManager").GetComponent<MusicController>();
            return _instance;
        }
    }
    
    public AudioClip m_HellephantDeath, m_HellephantHurt;
    public AudioClip m_ZomBearDeath, m_ZomBearHurt;
    public AudioClip m_ZomBunnyDeath, m_ZomBunnyHurt;
    public AudioClip m_HurtClip, m_DeathClip, m_ShootClip;

    public float musicVol, effectVol;
    private AudioSource m_BgmSource;

    private void Start() => m_BgmSource = GameObject.Find("Canvas").GetComponent<AudioSource>();

    private void Update()
    {
        effectVol = GameConfig.config.EffectVolume;
        musicVol = GameConfig.config.MusicVolume;
        SetMusicVolume(m_BgmSource);
    }

    public void SetMusicVolume(AudioSource audioSource)
    {
        if (audioSource.volume != musicVol)
        {
           audioSource.volume = musicVol;
        }
    }

    public void SetEffectVolume(AudioSource audioSource)
    {
        if (audioSource.volume != effectVol)
        {
            audioSource.volume = effectVol;
        }
    }
}
