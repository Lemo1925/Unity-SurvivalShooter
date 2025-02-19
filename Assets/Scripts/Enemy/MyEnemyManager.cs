using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum EnemyState { Patrol, Stay, Death, Trace, Back, Attack, Wake }

public class MyEnemyManager : MonoBehaviour
{
    public EnemyState enemyState = EnemyState.Patrol;
    public List<Transform> patrolPlaces;
    public EnemyAI enemyAI;
    public EnemyAnimator enemyAnimator;
    public EnemyHealth enemyHealth;
    public EnemyType enemyType;
    public EnemyVoice enemyVoice;
    void Start()
    {
        InitEnemyManager();
    }
    private void InitEnemyManager()
    {
        AddAllPlayerScript();
    }
    public void SetEnemyState(EnemyState state)
    {
        enemyState = state;
        OpenAnimatorToggle();
        if (state == EnemyState.Death)
        {
            enemyVoice.DeathVoice();
            GameManager.Instance.AddPlayerKillNum();
            enemyAI.enabled = false;
            enemyHealth.enabled = false;
            enemyVoice.enabled = false;
            StartCoroutine(Destroy());
        }
    }
    void AddAllPlayerScript()
    {
        enemyAI = AddScript<EnemyAI>();
        enemyAnimator = AddScript<EnemyAnimator>();
        enemyHealth = AddScript<EnemyHealth>();
        enemyVoice = AddScript<EnemyVoice>();
    }

    T AddScript<T>() where T : MonoBehaviour
    {
        T script = GetComponent<T>();
        if (script == null) script = this.gameObject.AddComponent(typeof(T)) as T;
        if (script!.enabled == false) script.enabled = true;
        return script;
    }

    private void OpenAnimatorToggle()
    {
        enemyAnimator.enabled = true;
    }

    IEnumerator Destroy()
    {
        yield return new WaitForSeconds(2.0f);
        Destroy(gameObject);
    }
}
