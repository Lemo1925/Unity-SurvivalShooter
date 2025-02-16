using UnityEngine;
using UnityEngine.UI;
public class PlayerHealth : MonoBehaviour
{
    private GameObject m_BloodParent;
    private Image m_ImgBlood;
    private Image m_ImgTransition;
    private float maxHealth = 100f;
    private float m_Health;
    private bool m_IsDamage = false;
    private bool m_IsUpdateTransition = false;
    private float deductBloodSpeed = 1f;
    private PlayerManager m_PlayerManager;
    private GameObject m_DamageUI;
    private Behavior m_Behavior;
    private float timeDamageEnd = 1f;
    private float m_Timer = 0;
    void Start()
    {
        InitPlayerHealth();
    }
    void Update()
    {
        UpdatePh();
        UpdateTph();
    }
    private void OnDisable()
    {
        Death();
    }
    void InitPlayerHealth()
    {
        m_BloodParent = GameObject.Find("Canvas/Blood");
        m_ImgBlood = m_BloodParent.transform.Find("Blood").GetComponent<Image>();
        m_ImgTransition = m_BloodParent.transform.Find("Blood_Transition").GetComponent<Image>();
        m_Health = maxHealth;
        m_PlayerManager = GetComponent<PlayerManager>();
        m_DamageUI = m_BloodParent.transform.Find("DamageUI").gameObject;
    }
    public void Damage(float damage)
    {
        if (m_PlayerManager.state == PlayerState.Death) return;
        m_PlayerManager.voice.GetHurt();
        m_Health -= damage;
        ShowDamageUI();
        Invoke(nameof(HideDamageUI),0.1f);
        if (m_Health <= 0)
        {
            m_PlayerManager.SetPlayerState(PlayerState.Death); 
            return;
        }
        m_IsDamage = true;
        m_IsUpdateTransition = true;
        m_Timer = 0;
    }
    void UpdatePh()
    {
        if (m_IsDamage == false) return;
        float value = Mathf.Lerp(m_ImgBlood.fillAmount, m_Health / maxHealth, deductBloodSpeed / 10);
        m_ImgBlood.fillAmount = value;
    }
    void UpdateTph()
    {
        if (m_IsUpdateTransition == false) return;
        m_Timer += Time.deltaTime;
        if (m_Timer < timeDamageEnd) return;
        var value = Mathf.Lerp(m_ImgTransition.fillAmount, m_ImgBlood.fillAmount, deductBloodSpeed / 10);
        m_ImgTransition.fillAmount = value;
        if (m_ImgTransition.fillAmount - m_ImgBlood.fillAmount < 0.01f)
        {
            m_ImgTransition.fillAmount = m_ImgBlood.fillAmount;
            m_IsUpdateTransition = false;
        }
    }
    void ShowDamageUI()
    {
        m_DamageUI.SetActive(true);
    }
    void HideDamageUI()
    {
        m_DamageUI.SetActive(false);
    }
    void ImedUpdatePhAndTph()
    {
        float value = Mathf.Lerp(m_ImgBlood.fillAmount, 0, deductBloodSpeed / 10);
        m_ImgBlood.fillAmount = value;
        if (m_ImgBlood.fillAmount < 0.01f) m_ImgBlood.fillAmount = 0f;
        value = Mathf.Lerp(m_ImgTransition.fillAmount, 0, deductBloodSpeed / 10);
        m_ImgTransition.fillAmount = value;
        if (m_ImgTransition.fillAmount < 0.01f) 
            m_ImgTransition.fillAmount = 0f;
        if (m_ImgTransition.fillAmount == 0) 
            m_Behavior.Finish();
    }
    private void Death()
    {
        m_Behavior = gameObject.AddComponent<Behavior>();
        m_Behavior = GetComponent<Behavior>();
        m_Behavior.Action = ImedUpdatePhAndTph;
    }
}
