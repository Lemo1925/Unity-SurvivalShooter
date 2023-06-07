using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using Debug = UnityEngine.Debug;
public class EnemyAI : MonoBehaviour
{
    #region 敌人不同状态时共用的变量
    private MyEnemyManager m_EnemyManager;
    private float navDifference = 0.2f;
    #endregion
    #region 巡逻状态时的变量
    private int m_CurrentPlaceIndex;
    private Transform m_CurrentPlace;
    private NavMeshAgent m_Nav;
    private float patrolSpeed = 1.5f;
    private float patrol_Acceleration = 8f;
    #endregion
    #region 停留状态时的变量
    private float timeStay = 2f;
    private float m_Timer;
    #endregion
    #region 警戒时的变量
    private float alertAngle = 100;
    private float alertAccuracy = 10;
    private float alertDistance = 10;
    #endregion
    #region 追击状态时的变量
    public float chaseDistance = 15f;
    private float attackDistance = 1.5f;
    private Transform m_ChaseTarget;
    private Vector3 m_PatrolCenterPoint = Vector3.zero;
    private float chaseSpeed = 10f;  //5f->10f
    private float chaseAcceleration = 24f; //16f->24f
    #endregion
    #region 返回状态时的变量
    private Vector3 m_ChaseBeforePoint;
    #endregion
    #region 攻击状态时的变量
    private float attackBufferTime = 0.5f;
    private float m_BufferTime;
    private float attackBetweenTime = 2f;
    private float m_AttackTimer;
    public float attackPower = MyEnemyGeneratorController.MaxCount > 90 ? 90 : MyEnemyGeneratorController.MaxCount;
    #endregion
    #region 惊觉状态时的变量
    private float startledTime = 0.5f;
    private float m_StartledTimer;
    private GameObject m_StartledEffect;
    private Transform m_Canvas;
    private Vector3 startledOffset = new Vector3(0, 70, 0);
    #endregion
    void Start()
    {
        InitEnemyAI();
    }
    void Update()
    {
        EnemyBehaviorByState();
    }
    private void OnDisable()
    {
        Stopping();
    }
    private void InitEnemyAI()
    {
        m_EnemyManager = GetComponent<MyEnemyManager>();
        m_Nav = GetComponent<NavMeshAgent>();
        m_CurrentPlace = m_EnemyManager.patrolPlaces[m_CurrentPlaceIndex];
        for (int i = 0; i < m_EnemyManager.patrolPlaces.Count; i++)
        {
            m_PatrolCenterPoint.x += m_EnemyManager.patrolPlaces[i].position.x;
            m_PatrolCenterPoint.y += m_EnemyManager.patrolPlaces[i].position.y;
            m_PatrolCenterPoint.z += m_EnemyManager.patrolPlaces[i].position.z;
        }
        m_PatrolCenterPoint /= m_EnemyManager.patrolPlaces.Count;
        m_AttackTimer = attackBetweenTime;
        if (m_EnemyManager.enemyState == EnemyState.Trace)
            SetNavSpeed(chaseSpeed,chaseAcceleration);
        else if (m_EnemyManager.enemyState == EnemyState.Patrol ||
                 m_EnemyManager.enemyState == EnemyState.Back )
            SetNavSpeed(patrolSpeed,patrol_Acceleration);
        m_Canvas = GameObject.Find("Canvas").transform;
    }
    private void EnemyBehaviorByState()
    {
        switch (m_EnemyManager.enemyState)
        {
            case EnemyState.Patrol:
                Patrol();
                break;
            case EnemyState.Stay:
                Stay();
                break;
            case EnemyState.Trace:
                Chase();
                break;
            case EnemyState.Back:
                BackingOut();
                break;
            case EnemyState.Attack:
                Attack();
                break;
            case EnemyState.Death:  
                Stopping();
                enabled = false;
                break;
            case EnemyState.Wake:
                Startled();
                break;
        }
    }
    private void Patrol()
    {
        if (Alert())
        {
            m_EnemyManager.SetEnemyState(EnemyState.Wake);
            return;
        }
        Nav(m_CurrentPlace.position);
        if (Vector3.Distance(transform.position,m_CurrentPlace.position) < navDifference)
        {
            transform.position = m_CurrentPlace.position;
            m_EnemyManager.SetEnemyState(EnemyState.Stay);
        }
    }
    private void Stay()
    {
        if (Alert())
        {
            m_EnemyManager.SetEnemyState(EnemyState.Wake);
            return;
        }
        if (m_Timer < timeStay) m_Timer += Time.deltaTime;
        if (m_Timer >= timeStay)
        {
            m_Timer = 0;
            m_CurrentPlaceIndex = (m_CurrentPlaceIndex + 1) % m_EnemyManager.patrolPlaces.Count;
            m_CurrentPlace = m_EnemyManager.patrolPlaces[m_CurrentPlaceIndex];
            m_EnemyManager.SetEnemyState(EnemyState.Patrol);
        }
    }
    bool Alert()
    {
        if (CreateRay(transform.forward)) return true;
        float subAngle = (alertAngle / 2) / alertAccuracy;
        for (int i = 1; i <= alertAccuracy; i++)
            for (int j = 0, n = -1; j < 2; j++, n *= -1)
            {
                Vector3 angle = new Vector3(0, transform.rotation.eulerAngles.y + subAngle * n * i, 0);
                Vector3 direction = Quaternion.Euler(angle) * Vector3.forward;
                if (CreateRay(direction)) return true;
            }
        return false;
    }
    bool CreateRay(Vector3 direction)
    {
        Ray ray = new Ray(transform.position, direction);
        if (Physics.Raycast(ray,out var hitInfo, alertDistance))
        {
            Debug.DrawLine(transform.position,hitInfo.point,Color.red);
            if (!hitInfo.collider.CompareTag($"Player")) return false;
            m_ChaseTarget = hitInfo.collider.transform;
            m_ChaseBeforePoint = transform.position;
            SetNavSpeed(chaseSpeed,chaseAcceleration);
            StartCoroutine(GenStartledEffect());
            return true;
        }
        Debug.DrawRay(transform.position, direction*alertDistance,Color.red);
        return false;
    }
   
    private void Chase()
    {
        if (m_ChaseTarget == null)
        {
            Stopping();
            m_EnemyManager.SetEnemyState(EnemyState.Back);
            SetNavSpeed(patrolSpeed,patrol_Acceleration);
            return;
        }
        if (Vector3.Distance(transform.position,m_PatrolCenterPoint)> chaseDistance)
        {
            Stopping();
            m_EnemyManager.SetEnemyState(EnemyState.Back);
            SetNavSpeed(patrolSpeed,patrol_Acceleration);
            return;
        }
        if (Vector3.Distance(transform.position, m_ChaseTarget.position)<= attackDistance)
        {
            Stopping();
            m_EnemyManager.SetEnemyState(EnemyState.Attack);
            return;
        }
        Nav(m_ChaseTarget.position);
    }
    private void Stopping()
    {
        m_Nav.enabled = false;
    }
    void Nav(Vector3 pos)
    {
        m_Nav.enabled = true;
        m_Nav.SetDestination(pos);
    }
    void BackingOut()
    {
        Nav(m_ChaseBeforePoint);
        if (Vector3.Distance(transform.position,m_ChaseBeforePoint)<navDifference) 
            m_EnemyManager.SetEnemyState(EnemyState.Patrol);
    }
    void Attack()
    {
        if (Vector3.Distance(transform.position, m_ChaseTarget.position)>attackDistance)
        {
            m_EnemyManager.SetEnemyState(EnemyState.Trace);
            m_BufferTime = 0;
            return;
        }
        m_BufferTime += Time.deltaTime;
        m_AttackTimer += Time.deltaTime;
        if (m_BufferTime < attackBufferTime) return;
        if (m_AttackTimer >= attackBetweenTime)
        {
            m_ChaseTarget.GetComponent<PlayerHealth>().Damage(attackPower);
            m_AttackTimer = 0;
            m_BufferTime = 0;
        }
    }
    void SetNavSpeed(float speed, float acceleration)
    {
        m_Nav.speed = speed;
        m_Nav.acceleration = acceleration;
    }
    void Startled()
    {
        // ReSharper disable once PossibleNullReferenceException
        Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);
        m_StartledEffect.transform.position = screenPos + startledOffset;
        m_StartledTimer += Time.deltaTime;
        if (m_StartledTimer >= startledTime)
        {
            m_EnemyManager.SetEnemyState(EnemyState.Trace);
            m_StartledTimer = 0;
            StartCoroutine(RecycleStartled());
        }
    }
    IEnumerator GenStartledEffect()
    {
        Stopping();
        m_StartledEffect = GameObjectPool.Instance.GetObj($"StartledEffect");
        yield return new WaitForSeconds(0.03f);
        m_StartledEffect.transform.SetParent(m_Canvas);
        m_StartledEffect.transform.localScale = Vector3.one;
    }
    IEnumerator RecycleStartled()
    {
        yield return new WaitForSeconds(0.05f);
        GameObjectPool.Instance.RecycleObj(m_StartledEffect);
    }
}
