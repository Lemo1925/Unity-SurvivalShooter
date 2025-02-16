using System.Collections.Generic;
using System.IO;
using UnityEngine;
public class GameObjectPool : MonoBehaviour
{
    private Dictionary<string, List<GameObject>> m_ObjectDic = new Dictionary<string, List<GameObject>>();
    private static GameObjectPool _instance;
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
            m_ObjectDic.Add(objName,new List<GameObject>());
            var objRoot = new GameObject(objName);
            objRoot.transform.SetParent(transform);
            GameObject objPre = Resources.Load<GameObject>(objPath);
            m_ObjectDic[objName].Add(objPre);
        }
    }
    public GameObject GetObj(string objPath)
    {
        CreateObjPool(objPath);
        string objName = Path.GetFileName(objPath);
        if (m_ObjectDic[objName].Count == 1)
        {
            Transform objRoot = transform.Find(objName);
            for (int i = 0; i < 30; i++)
            {
                GameObject obj = Instantiate(m_ObjectDic[objName][0],gameObject.transform);
                obj.name = objName;
                obj.SetActive(false);
                obj.transform.SetParent(objRoot);
                m_ObjectDic[objName].Add(obj);
            }
        }
        GameObject secondObj = m_ObjectDic[objName][1];
        secondObj.SetActive(true);
        m_ObjectDic[objName].RemoveAt(1);
        return secondObj;
    }
    public void RecycleObj(GameObject obj)
    {
        if (!m_ObjectDic.ContainsKey(obj.name)) return;
        obj.SetActive(false);
        m_ObjectDic[obj.name].Add(obj);
    }
}
