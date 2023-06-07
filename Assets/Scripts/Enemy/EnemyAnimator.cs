using UnityEngine;
public class EnemyAnimator : MonoBehaviour
{
    private Animator m_Anim;
    private MyEnemyManager m_EnemyManager;
    private float animChaseSpeed = 3f;
    void Start()
    {
        InitAnimator();
    }
    void Update()
    {
        PlayAnimation();
    }
    private void InitAnimator()
    {
        m_Anim = GetComponent<Animator>();
        m_EnemyManager = GetComponent<MyEnemyManager>();
    }
    private void PlayAnimation()
    {
        SetAnimatorSpeed(1);
        if (m_EnemyManager.enemyState == EnemyState.Death) 
            m_Anim.SetTrigger($"Death");
        else if (m_EnemyManager.enemyState == EnemyState.Stay || 
                 m_EnemyManager.enemyState == EnemyState.Attack) 
            m_Anim.SetBool($"Move",false);
        else if (m_EnemyManager.enemyState == EnemyState.Trace)
        {
            m_Anim.SetBool($"Move",true);
            SetAnimatorSpeed(animChaseSpeed);
        }
        else m_Anim.SetBool($"Move",true);
    }
    void SetAnimatorSpeed(float speed)
    {
        m_Anim.speed = speed;
    }
}
