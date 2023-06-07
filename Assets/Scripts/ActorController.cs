using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorController : MonoBehaviour
{
    [Header("Character Parameter")]
    public GameObject model;
    public float walkSpeed;
    public float runMultiplier;
    public float jumpForce;
    [Header("Check Parameter")]
    // [SerializeField] private Animator anim;
    [SerializeField] private PlayerInput pi;
    [SerializeField] private Rigidbody rigid;
    [SerializeField] private Vector3 planarVec;
    [SerializeField] private Vector3 thrustVec;
    // private Vector3 moveVec;
    
    
    void Awake()
    {
        pi = GetComponent<PlayerInput>();
        rigid = GetComponent<Rigidbody>();
    }
    
    void Update()
    {
        //控制角色行进方向，让角色的正前方对准输入向量Horizontal+Vertical在Transform上面的投影
        if (pi.mag > 0.1f) model.transform.forward = Vector3.Slerp(model.transform.forward, pi.vec, 0.1f);
        //获取玩家的移动意图(移动信息 = 移动的权重大小 * 移动的方向向量)
        planarVec = pi.mag * model.transform.forward * walkSpeed * (pi.run ? runMultiplier : 1.0f);

        if (pi.jump) Jump();
        
    }

    private void Jump()
    {
        thrustVec.Set(0,jumpForce,0);
    }

    void FixedUpdate()
    {
        /*moveVec += new Vector3(planarVec.x, rigid.velocity.y, planarVec.z) * walkSpeed * Time.fixedDeltaTime;
        rigid.MovePosition(moveVec);*/
        // rigid.position += movingVec * Time.fixedDeltaTime;
        rigid.velocity = new Vector3(planarVec.x, rigid.velocity.y, planarVec.z) + thrustVec;
        thrustVec = Vector3.zero;
    }

   
}
