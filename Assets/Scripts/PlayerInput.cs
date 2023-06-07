using System;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public static PlayerInput InstancePlayerInput;
    //Variable
    [Header("Key Settings")]
    public string keyUp;
    public string keyDown;
    public string keyLeft;
    public string keyRight;
    
    public string keyA;
    public string keyB;
    public string keyX;
    public string keyY;
    
    public string keyLa;
    public string keyRa;
    public string keyLb;
    public string keyRb;

    public string keyCUp;
    public string keyCDown;
    public string keyCRight;
    public string keyCLeft;
    
    [Header("Output Signals")]
    public float horizontal;
    public float vertical;
    public float mag;
    public Vector3 vec;

    public float cameraHorizontal;
    public float cameraVertical;
    
    //1.pressing signal
    public bool run;
    //2. trigger once signal
    public bool jump;
    private bool m_LastJump; 
    public bool attack;
    private bool m_LastAttack;
    //3. double trigger signal
    
    [Header("Others")]
    public bool inputEnable = true;
    
    private float m_TargetHorizontal;
    private float m_TargetVertical;
    private float m_VelocityHorizontal;
    private float m_VelocityVertical;

    private void Awake()
    {
        InstancePlayerInput = this;
    }

    void Update()
    {
        cameraHorizontal = (Input.GetKey(keyCUp) ? 1.0f : 0) - (Input.GetKey(keyCDown) ? 1.0f : 0);
        cameraVertical = (Input.GetKey(keyCRight) ? 1.0f : 0) - (Input.GetKey(keyCLeft) ? 1.0f : 0);
        
        m_TargetHorizontal = (Input.GetKey(keyUp) ? 1.0f : 0) - (Input.GetKey(keyDown) ? 1.0f : 0);
        m_TargetVertical = (Input.GetKey(keyRight) ? 1.0f : 0) - (Input.GetKey(keyLeft) ? 1.0f : 0);

        if (inputEnable == false)
        {
            m_TargetHorizontal = 0;
            m_TargetVertical = 0;
        }

        horizontal = Mathf.SmoothDamp(horizontal, m_TargetHorizontal, ref m_VelocityHorizontal, 0.1f);
        vertical = Mathf.SmoothDamp(vertical, m_TargetVertical, ref m_VelocityVertical, 0.1f);

        var dAxis = SquareToCircle(new Vector2(vertical, horizontal));
        
        mag = Mathf.Sqrt(dAxis.y * dAxis.y + dAxis.x * dAxis.x);
        // ReSharper disable once Unity.InefficientPropertyAccess
        vec = dAxis.x * transform.right + dAxis.y * transform.forward;

        run = Input.GetKey(keyRb);

        var newJump = Input.GetKey(keyA);
        if (newJump != m_LastJump && newJump ) jump = true;
            else jump = false;
        m_LastJump = newJump;
        
        var newAttack = Input.GetKey(keyB);
        if (newAttack != m_LastAttack && newAttack) attack = true;
            else attack = false;
        m_LastAttack = newAttack;
        
    }

    private Vector2 SquareToCircle(Vector2 input)
    {
        var output = Vector2.zero;

        output.x = input.x * Mathf.Sqrt(1 - input.y * input.y / 2.0f);
        output.y = input.y * Mathf.Sqrt(1 - input.x * input.x / 2.0f);

        return output;
    }
}
