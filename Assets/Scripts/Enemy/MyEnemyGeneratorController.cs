using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public enum EnemyType
{
    Hellephant = 0,
    ZomBear = 1,
    Zombunny = 2,
}

public class MyEnemyGeneratorController : MonoBehaviour
{
   private Transform m_EnemyGenPlaces;
   private List<Transform> m_Places;
   private Dictionary<Transform, List<Transform>> m_PatrolsDic;
   public GameObject[] enemyPre;
   private List<GameObject> m_CurrentEnemy;
   public int MaxCount = 5; 
   public int enemyIndex;
   void Start()
    {   
        InitEnemyGenController();
        enemyPre = Resources.LoadAll<GameObject>($"Enemies");
    }
     
    private void Update()
    {
        m_CurrentEnemy = new List<GameObject>(GameObject.FindGameObjectsWithTag($"Enemy"));
        var stage = GameManager.Instance.GetStage();
        var enemyCount = MaxCount + GetExtraCount(stage);
        if (m_CurrentEnemy.Count < enemyCount)
        {
            enemyIndex =(enemyIndex + Random.Range(0, 2)) % 3;
            GenEnemy(enemyIndex);
        }
    }
    private void InitEnemyGenController()
    {
        m_EnemyGenPlaces = transform.Find($"EnemyGeneratePlaces");
        m_Places = new List<Transform>();
        m_PatrolsDic = new Dictionary<Transform, List<Transform>>();
        for (int i = 0; i < m_EnemyGenPlaces.childCount; i++)
        {
            Transform thePlace = m_EnemyGenPlaces.GetChild(i);
            m_Places.Add(thePlace);
            m_PatrolsDic.Add(thePlace, new List<Transform>());
            for (int j = 0; j < thePlace.childCount; j++)
                m_PatrolsDic[thePlace].Add(thePlace.GetChild(j));
        }
    }

    private void GenEnemy(int index)
    {
        int i = Random.Range(0, m_Places.Count);
        Transform theRandomPlace = m_Places[i];
        GameObject enemy = Instantiate(enemyPre[index], theRandomPlace.position, theRandomPlace.rotation,transform.Find("Enemy"));
        enemy.GetComponent<MyEnemyManager>().patrolPlaces = m_PatrolsDic[theRandomPlace];
        enemy.GetComponent<MyEnemyManager>().enemyType = (EnemyType)index;
    }

    private int GetExtraCount(int stage)
    {
        var extra = 0;
        switch (stage)
        {
            case 1:
                extra = 2;
                break;
            case 2:
                extra = 5;
                break;
            case 3:
                extra = 10;
                break;
            default:
                break;
        }
        return extra;
    }
}
