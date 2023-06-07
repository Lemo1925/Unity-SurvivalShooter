using UnityEngine;
public class CameraController : MonoBehaviour
{
    private GameObject m_Target;
    private Vector3 m_Distance;
    private Vector3 m_Velocity;
    void Start()
    {
        m_Target = GameObject.FindWithTag($"Player");
        m_Distance = transform.position - m_Target.transform.position;
    }
    void LateUpdate()
    {
        CameraFollow();
    }
    void CameraFollow()
    {
        Vector3 targetPos = m_Target.transform.position + m_Distance;
        transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref m_Velocity, 0f);
    }
}
