using System.Collections.Generic;
using System.IO;
using UnityEngine;
public class GameObjectPool : MonoBehaviour
{
    private Dictionary<string, Queue<GameObject>> m_ObjectDic = new Dictionary<string, Queue<GameObject>>();
    private static GameObjectPool _instance;
    private const int Capacity = 18;
    public static GameObjectPool Instance
    {
        get
        {
            if (_instance == null) 
                _instance = GameObject.Find($"GameManager").GetComponent<GameObjectPool>();
            return _instance;
        }
    }
    void CreateObjPool(string objPath)
    {
        string objName = Path.GetFileName(objPath);
        if (!m_ObjectDic.ContainsKey(objName))
        {
            m_ObjectDic.Add(objName,new Queue<GameObject>(Capacity));
            var objRoot = new GameObject(objName);
            objRoot.transform.SetParent(transform);
            GameObject objPre = Resources.Load<GameObject>(objPath);
            var obj = Instantiate(objPre);
            obj.SetActive(false);
            m_ObjectDic[objName].Enqueue(obj);
        }
    }
    public GameObject GetObj(string objPath, bool hide = false)
    {
        CreateObjPool(objPath);
        string objName = Path.GetFileName(objPath);
        if (m_ObjectDic[objName].Count == 1)
        {
            Transform objRoot = transform.Find(objName);
            for (int i = 0; i < Capacity; i++)
            {
                GameObject obj = Instantiate(m_ObjectDic[objName].Peek(), base.gameObject.transform);
                obj.name = objName;
                obj.SetActive(false);
                obj.transform.SetParent(objRoot);
                m_ObjectDic[objName].Enqueue(obj);
            }
        }
        GameObject gameObject = m_ObjectDic[objName].Dequeue();
        gameObject.SetActive(!hide);
        return gameObject;
    }
    public void RecycleObj(GameObject obj)
    {
        var key = obj.name.Replace("(Clone)", "");
        if (!m_ObjectDic.ContainsKey(key)) return;
        obj.SetActive(false);
        m_ObjectDic[key].Enqueue(obj);
    }
}
