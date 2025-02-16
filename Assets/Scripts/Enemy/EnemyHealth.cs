using System.Collections;
using UnityEngine;
using UnityEngine.UI;
public class EnemyHealth : MonoBehaviour
{
    private GameObject m_Blood;
    private Image m_ImgBlood;
    private GameObject m_Canvas;
    private readonly Vector3 m_Offset = new Vector3(0, 60, 0);
    private bool m_IsUpdateBlood = false;
    private float bloodUpdateSpeed = 1f;
    private readonly float m_MaxHealth = 100f;
    private float m_Health;
    private Material m_Material;
    private Color m_MaterialColor;
    private MyEnemyManager m_EnemyManager;
    private Behavior m_MyBehavior;
    void Start()
    {
        InitEnemyBlood();
        InitEnemyHealth();
        InitEnemyEffect();
    }
    void Update()
    {
        BloodFollowEnemy();
        UpdateEnemyBlood();
    }
    private void OnDisable()
    {
        m_MyBehavior = gameObject.AddComponent<Behavior>();
        m_MyBehavior.Action = BehaviorAfterDeath;
    }
    private void InitEnemyBlood()
    {
        m_Blood = GameObjectPool.Instance.GetObj("EnemyBlood");
        m_ImgBlood = m_Blood.transform.Find("Blood").GetComponent<Image>();
        m_Canvas = GameObject.Find("Canvas");
        m_Blood.transform.SetParent(m_Canvas.transform);
        m_Blood.transform.localScale = Vector3.one;
    }
    private void BloodFollowEnemy()
    {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);
        m_Blood.transform.position = screenPos + m_Offset;
    }
    private void InitEnemyHealth()
    {
        m_IsUpdateBlood = true;
        m_Health = m_MaxHealth * GetExtraProp();
        m_EnemyManager = GetComponent<MyEnemyManager>();
    }
    public void Damage(float value)
    {
        if (m_EnemyManager.enemyState == EnemyState.Death) return;
        m_Health -= value;
        m_EnemyManager.enemyVoice.HurtVoice();
        m_EnemyManager.SetEnemyState(EnemyState.Wake);
        m_IsUpdateBlood = true;
        OpenDamageEffect();
        if (m_Health <= 0) m_EnemyManager.SetEnemyState(EnemyState.Death);
    }
    private void UpdateEnemyBlood()
    {
        if (m_IsUpdateBlood == false) return;
        float value = Mathf.Lerp(m_ImgBlood.fillAmount, m_Health / m_MaxHealth, bloodUpdateSpeed / 10);
        m_ImgBlood.fillAmount = value;
        if (!(m_ImgBlood.fillAmount - m_Health / m_MaxHealth < 1f)) return;
        m_ImgBlood.fillAmount = m_Health / m_MaxHealth;
        m_IsUpdateBlood = false;
    }
    private void InitEnemyEffect()
    {
        m_Material = transform.GetChild(0).GetComponent<SkinnedMeshRenderer>().materials[0];
        m_MaterialColor = m_Material.GetColor($"_RimColor");
    }
    IEnumerator DamageEffect()
    {
        m_Material.SetColor($"_RimColor",Color.red);
        yield return new WaitForSeconds(0.3f);
        m_Material.SetColor($"_RimColor",m_MaterialColor);
    }
    void OpenDamageEffect() => StartCoroutine(nameof(DamageEffect));
    void BehaviorAfterDeath()
    {
        UpdateEnemyBlood();
        GameObjectPool.Instance.RecycleObj(m_Blood);
        if (m_IsUpdateBlood) return;
        m_MyBehavior.Finish();
    }

    private float GetExtraProp()
    {
        var stage = GameManager.Instance.GetStage();
        var extraProp = 1.0f;
        switch (stage)
        {
            case 1:
                extraProp = 1.2f;
                break;
            case 2:
                extraProp = 1.5f;
                break;
            case 3:
                extraProp = 2.0f;
                break;
            default:
                break;
        }
        return extraProp;
    }
}
