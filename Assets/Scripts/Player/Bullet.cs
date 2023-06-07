using UnityEngine;
public class Bullet : MonoBehaviour
{
    [Header("子弹相关变量")] 
    public float bulletSpeed = 20f;
    public ParticleSystem bulletNormalPs;
    public Color bulletColor = Color.yellow;
    public float bulletPower;
    private Vector3 m_OldPos, m_NewPos;
    private ParticleSystem m_BulletDestroyPS;
    private bool m_IsExecute = true;
    [Header("时间相关变量")]
    public float timeStartDestroy = 0.5f;
    public float timeDynamicDestroy;
    public float timer = 0;
    void OnEnable()
    {
        InitBullet();
    }
    void Update()
    {
        if (m_IsExecute)
        {
            BulletMove(); BulletCollisionByRay();
        }
        BulletDestroy();
    }
    private void InitBullet()
    {
        if (bulletNormalPs == null) bulletNormalPs = transform.Find($"NormalTrail").GetComponent<ParticleSystem>();
        bulletNormalPs.gameObject.SetActive(true);
        ParticleSystem.MainModule bulletNormalPsMain = bulletNormalPs.main;
        bulletNormalPsMain.startColor = bulletColor;
        bulletNormalPs.Play();
        timeDynamicDestroy = timeStartDestroy;
        m_NewPos = transform.position;
        m_OldPos = m_NewPos;
        if (m_BulletDestroyPS == null)
            m_BulletDestroyPS = transform.Find("Impact").GetComponent<ParticleSystem>();
        m_IsExecute = true;
    }
    private void BulletMove()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * bulletSpeed);
    }
    private void BulletDestroy()
    {
        timer += Time.deltaTime;
        if (timer >= timeDynamicDestroy)
        {
            timer = 0;
            bulletNormalPs.Stop();
            m_BulletDestroyPS.Stop();
            enabled = false;
            GameObjectPool.Instance.RecycleObj(gameObject);
        }
    }
    private void BulletCollisionByRay()
    {
        m_NewPos = transform.position;
        Ray ray = new Ray(m_OldPos, m_NewPos - m_OldPos);
        if (Physics.Raycast(ray, out var hitInfo, Vector3.Distance(m_OldPos,m_NewPos))) 
            ActionAfterCollision(hitInfo);
        m_OldPos = m_NewPos;
    }
    private void ActionAfterCollision(RaycastHit hitInfo)
    {
        if (hitInfo.collider.CompareTag($"Player")) return;
        if (hitInfo.collider.CompareTag($"Enemy"))
        {
            EnemyHealth enemyHealth = hitInfo.collider.GetComponent<EnemyHealth>();
            if (enemyHealth != null) enemyHealth.Damage(bulletPower);
        }
        bulletNormalPs.gameObject.SetActive(false);
        transform.position = hitInfo.point;
        m_BulletDestroyPS.Play();
        timeDynamicDestroy += 0.5f;
        m_IsExecute = false;
    }
}
