using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public float horizontalSpeed = 100.0f;
    public float verticalSpeed = 80.0f;
    public float cameraSpeed;
    public float speed;
    [SerializeField]private GameObject m_PlayerHandle;
    [SerializeField]private GameObject m_CameraHandle;
    [SerializeField]private float m_TempEulerX;
    [SerializeField]private GameObject m_Model;
    [SerializeField]private GameObject m_Camera;

    private Vector3 m_CameraDampVelocity;
    void Awake()
    {
        m_CameraHandle = transform.parent.gameObject;
        m_PlayerHandle = m_CameraHandle.transform.parent.gameObject;
        m_TempEulerX = 20;
        m_Model = m_PlayerHandle.GetComponent<ActorController>().model;
        // ReSharper disable once PossibleNullReferenceException
        m_Camera = Camera.main.gameObject;
        Cursor.visible = false;
    }

    private void Update()
    {
        cameraSpeed = Mathf.Lerp(cameraSpeed, Input.GetAxis("Mouse X") * speed, 0.2f);
        
    }

    void FixedUpdate()
    {
        var tempModelEuler = m_Model.transform.eulerAngles;
        
        m_PlayerHandle.transform.Rotate(Vector3.up,
            /*PlayerInput.InstancePlayerInput.cameraVertical* */  cameraSpeed * horizontalSpeed * Time.fixedDeltaTime);
        m_TempEulerX -= PlayerInput.InstancePlayerInput.cameraHorizontal * verticalSpeed * Time.fixedDeltaTime;
        m_TempEulerX = Mathf.Clamp(m_TempEulerX, -40, 30);
        m_CameraHandle.transform.localEulerAngles = new Vector3(m_TempEulerX, 0, 0);

        m_Model.transform.eulerAngles = tempModelEuler;
        
        m_Camera.transform.position = Vector3.SmoothDamp(m_Camera.transform.position,transform.position,ref m_CameraDampVelocity, 0.15f);
        m_Camera.transform.eulerAngles = transform.eulerAngles;
    }
    
}
