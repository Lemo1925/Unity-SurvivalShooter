using UnityEngine;
public class PlayerMove : MonoBehaviour
{
    public PlayerManager playerManager;
    public Rigidbody rigid;
    public float speed = 10f;
    public Vector3 moveDir;
    // Start is called before the first frame update
    void Start()
    {
        playerManager = GetComponent<PlayerManager>();
        rigid = GetComponent<Rigidbody>();
    }
    // Update is called once per frame
    void FixedUpdate()
    {
       Move(); Turn();
    }
    void Move()
    {
        playerManager.horizontal = Input.GetAxis("Horizontal");
        playerManager.vertical = Input.GetAxis("Vertical");
        moveDir.Set(playerManager.horizontal,0,playerManager.vertical);
        rigid.MovePosition(transform.position + moveDir.normalized * speed * Time.fixedDeltaTime);
    }
    void Turn()
    {
        Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(mouseRay,out var hitInfo))
            if (hitInfo.collider.CompareTag("Floor")) 
                transform.LookAt(hitInfo.point);
    }
}
