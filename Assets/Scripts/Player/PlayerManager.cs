using Player;
using UnityEngine;
using UnityEngine.UI;
public enum PlayerState { Alive, Death }
public class PlayerManager : MonoBehaviour
{
    [HideInInspector] public PlayerState state = PlayerState.Alive;
    [HideInInspector] public PlayerMove movement;
    [HideInInspector] public PlayerAnimator animator;
    [HideInInspector] public PlayerAttack attack;
    [HideInInspector] public PlayerHealth health;
    [HideInInspector] public PlayerVoice voice;
    public float horizontal, vertical;

    // Start is called before the first frame update
    void Start() { InitPlayer(); }
    //初始化
    void InitPlayer() { AddAllPlayerScript(); }
    public void SetPlayerState(PlayerState state)
    {
        this.state = state;
        if (state == PlayerState.Death)
        {
            voice.GetDeath();
            movement.enabled = false;
            animator.enabled = false;
            attack.enabled = false;
            health.enabled = false;
            voice.enabled = false;
            GameManager.Instance.GameOver(GameState.Fail);
        }
    }
    void AddAllPlayerScript()
    {
        movement = AddScript<PlayerMove>();
        animator = AddScript<PlayerAnimator>();
        attack = AddScript<PlayerAttack>();
        health = AddScript<PlayerHealth>();
        voice = AddScript<PlayerVoice>();
    }
    T AddScript<T>() where T : MonoBehaviour
    {
        T script = GetComponent<T>();
        if (script == null) script = this.gameObject.AddComponent(typeof(T)) as T;
        if (script!.enabled == false) script.enabled = true;
        return script;
    }
}