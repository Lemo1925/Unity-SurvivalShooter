using UnityEngine;

public class PlayerVoice : MonoBehaviour
{
    [SerializeField]
    AudioClip m_HurtClip, m_DeathClip, m_ShootClip;
    AudioSource m_PlayerAudioSource;

    void Start()
    {
        m_HurtClip = MusicController.Instance.m_HurtClip;
        m_DeathClip = MusicController.Instance.m_DeathClip;
        m_ShootClip = MusicController.Instance.m_ShootClip;
        m_PlayerAudioSource = GetComponent<AudioSource>();
    }

    public void GetHurt() => PlayVoice(m_HurtClip);

    public void GetDeath() => PlayVoice(m_DeathClip);

    public void Shoot() => PlayVoice(m_ShootClip);

    public void PlayVoice(AudioClip clip)
    {
        if (m_PlayerAudioSource != null && clip != null)
        {
            MusicController.Instance.SetEffectVolume(m_PlayerAudioSource);
            m_PlayerAudioSource.Stop();
            m_PlayerAudioSource.clip = clip;
            m_PlayerAudioSource.Play();
        }
    }
}
