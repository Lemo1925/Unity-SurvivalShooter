using System;
using UnityEngine;
public class Behavior : MonoBehaviour
{ 
    public Action Action;
    private bool m_IsFinish = false;
    void Update()
    {
        if (Action != null) Action();
        if (m_IsFinish) Destroy(this);
    }
    public void Finish()
    {
        m_IsFinish = true;
    }
}
