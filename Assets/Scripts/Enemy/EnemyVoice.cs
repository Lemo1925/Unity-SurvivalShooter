using UnityEngine;

public class EnemyVoice : MonoBehaviour
{
    MyEnemyManager m_EnemyManager;
    AudioClip m_DeathClip, m_HurtClip;
    AudioSource m_EnemyAudioSource;

    private void OnEnable()
    {
        switch (GetComponent<MyEnemyManager>().enemyType)
        {
            case EnemyType.Hellephant:
                m_DeathClip = MusicController.Instance.m_HellephantDeath;
                m_HurtClip = MusicController.Instance.m_HellephantHurt;
                break;
            case EnemyType.ZomBear:
                m_DeathClip = MusicController.Instance.m_ZomBearDeath;
                m_HurtClip = MusicController.Instance.m_ZomBearHurt;
                break;
            case EnemyType.Zombunny:
                m_DeathClip = MusicController.Instance.m_ZomBunnyDeath;
                m_HurtClip = MusicController.Instance.m_ZomBunnyHurt;
                break;
        }
    }

    void Start()
    {
        m_EnemyAudioSource = GetComponent<AudioSource>();
    }

    public void HurtVoice() => PlayVoice(m_HurtClip);

    public void DeathVoice() => PlayVoice(m_DeathClip);

    public void PlayVoice(AudioClip clip)
    {
        if (clip != null && m_EnemyAudioSource != null)
        {
            MusicController.Instance.SetEffectVolume(m_EnemyAudioSource);
            m_EnemyAudioSource.Stop();
            m_EnemyAudioSource.clip = clip;
            m_EnemyAudioSource.Play();
        }
    }
}
