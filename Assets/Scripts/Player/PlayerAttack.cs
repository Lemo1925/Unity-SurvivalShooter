using UnityEngine;
public class PlayerAttack : MonoBehaviour
{
    private PlayerManager m_PlayerManager;
    private Transform m_BulletPoint;
    private float timeBetweenShoot = 0.15f;
    private float m_Timer;
    private int bulletsNum = 10;
    private int angleBullet = 10;
    private float attackPower = 20f;
    void Start()
    {
        m_PlayerManager = GetComponent<PlayerManager>();
        m_BulletPoint = transform.Find($"GunBarrelEnd");
        m_Timer = timeBetweenShoot;
    }
    void Update()
    {
        Shooting();
    }
    void Shooting()
    {
        if (m_Timer < timeBetweenShoot) m_Timer += Time.deltaTime;
        if (Input.GetMouseButtonDown(0) && m_Timer >= timeBetweenShoot && !GameTimer.isPaused)
        {
            m_Timer = 0;
            m_PlayerManager.voice.Shoot();
            CameraController.Instance.TriggerShake();
            GenerateBullet();
        }
    }
    private void GenerateBullet()
    {
        if (bulletsNum%2 != 0) GenerateBulletByOdd();
        else GenerateBulletByEven();
    }
    private void GenerateBulletByOdd()
    {
        GameObject firstBullet = GenerateTheBullet(m_BulletPoint.position, m_BulletPoint.rotation);
        for (int i = 1, n = -1; i < 2; i++)
            for (int j = 1; j <= (bulletsNum - 1) / 2; j++)
            {
                Vector3 angleOffset = Vector3.up * angleBullet * n * j;
                Vector3 angleTheBullet = firstBullet.transform.rotation.eulerAngles + angleOffset;
                GenerateTheBullet(m_BulletPoint.position, Quaternion.Euler(angleTheBullet));
            }
    }
    private void GenerateBulletByEven()
    {
        for (int i = 1, n = -1; i < 2; i++, n *= -1)
            for (int j = 1; j <= bulletsNum / 2; j++)
            {
                Vector3 angleOffset = Vector3.up * angleBullet / 2 + Vector3.up * angleBullet * (j - 1) * n;
                Vector3 angleTheBullet = m_BulletPoint.rotation.eulerAngles + angleOffset;
                GenerateTheBullet(m_BulletPoint.position, Quaternion.Euler(angleTheBullet));
            }
    }
    private GameObject GenerateTheBullet(Vector3 bulletPointPosition, Quaternion bulletPointRotation)
    {
        GameObject bullet = GameObjectPool.Instance.GetObj("Bullet");
        bullet.transform.position = bulletPointPosition;
        bullet.transform.rotation = bulletPointRotation;
        Bullet myBullet = bullet.GetComponent<Bullet>();
        if (myBullet == null) myBullet = bullet.AddComponent<Bullet>();
        myBullet.enabled = true;
        myBullet.bulletPower = attackPower * Mathf.Min((1.0f + GameManager.Instance.playerKillNum / 100),1.5f);
        return bullet;
    }
}
