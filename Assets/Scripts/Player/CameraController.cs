using UnityEngine;
public class CameraController : MonoBehaviour
{
    private static CameraController _instance;
    public static CameraController Instance
    {
        get
        {
            if (_instance == null)
                _instance = GameObject.Find("CameraHandle").GetComponent<CameraController>(); 
            return _instance;
        }
    }

    private GameObject m_Target;
    private Vector3 m_Distance;
    private Vector3 m_Velocity;

    public float shakeStrength = 0.1f;
    public float shakeDuration = 0.2f;
    private Vector3 originalPos;
    private float curShakeDur;
    private Camera mainCamera;
    void Start()
    {
        m_Target = GameObject.FindWithTag("Player");
        m_Distance = transform.position - m_Target.transform.position;
        mainCamera = Camera.main;
        originalPos = mainCamera.transform.localPosition;
    }

    private void Update()
    {
        if (curShakeDur > 0) 
        {
            mainCamera.transform.localPosition = originalPos + Random.insideUnitSphere * shakeStrength;
            curShakeDur -= Time.deltaTime;
        }
        else
        {
            curShakeDur = 0;
            mainCamera.transform.localPosition = originalPos;
        }
    }
    public void TriggerShake() => curShakeDur = shakeDuration;
    void LateUpdate() => CameraFollow();
    void CameraFollow()
    {
        Vector3 targetPos = m_Target.transform.position + m_Distance;
        transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref m_Velocity, 0f);
    }
}
