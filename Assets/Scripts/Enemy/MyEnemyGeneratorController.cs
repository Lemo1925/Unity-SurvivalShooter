using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
public class MyEnemyGeneratorController : MonoBehaviour
{
   private Transform m_EnemyGenPlaces;
   private List<Transform> m_Places;
   private Dictionary<Transform, List<Transform>> m_PatrolsDic;
   public GameObject[] enemyPre;
   private List<GameObject> m_CurrentEnemy;
   public static int MaxCount = 3;
   public int enemyIndex;
   void Start()
    {   
        InitEnemyGenController();
        enemyPre = Resources.LoadAll<GameObject>($"Enemies");
    }

    private void Update()
    {
        m_CurrentEnemy = new List<GameObject>(GameObject.FindGameObjectsWithTag($"Enemy"));
        if (m_CurrentEnemy.Count < (MaxCount > 15 ? 15: MaxCount ))
        {
            enemyIndex =(enemyIndex + Random.Range(0, 2)) % 3;
            GenEnemy(enemyPre[enemyIndex]);
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
    public static void AddMaxEnemy()
    {
        MaxCount++;
    }
    private void GenEnemy(GameObject enemyPre)
    {
        int i = Random.Range(0, m_Places.Count);
        Transform theRandomPlace = m_Places[i];
        GameObject enemy = Instantiate(enemyPre, theRandomPlace.position, theRandomPlace.rotation,transform.Find("Enemy"));
        enemy.GetComponent<MyEnemyManager>().patrolPlaces = m_PatrolsDic[theRandomPlace];
    }
}
