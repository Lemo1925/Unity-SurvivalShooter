using UnityEngine;
namespace Player
{
    public class PlayerAnimator : MonoBehaviour
    {
        private PlayerManager m_PlayerManager;
        private Animator m_Anim;
        private void Awake()
        {
            m_PlayerManager = GetComponent<PlayerManager>();
            m_Anim = GetComponent<Animator>();
        }
        private void FixedUpdate()
        {
            MoveAnimator();
        }
        private void OnDisable()
        {
            DeathAnimator();
        }
        void MoveAnimator()
        {
            if (m_PlayerManager.horizontal == 0 
                && m_PlayerManager.vertical == 0)
                m_Anim.SetBool($"Move",false);
            else m_Anim.SetBool($"Move",true);
        }
        void DeathAnimator()
        {
            m_Anim.SetTrigger($"Death");
        }
    }
}