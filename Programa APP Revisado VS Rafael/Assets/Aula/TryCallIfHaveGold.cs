using UnityEngine;
using System.Collections;
using UnityEngine.Events;


public class TryCallIfHaveGold : MonoBehaviour {

    [SerializeField]
    int m_valueToCheck = 0;

    public int ValueToCheck
    {
        get
        {
            return m_valueToCheck;
        }
        set
        {
            if (m_valueToCheck == value)
                return;
            m_valueToCheck = value;
        }
    }

    public UnityEvent OnCallSucess;
    public UnityEvent OnCallFailed;

    public void TryCall()
    {
        if (XpGoldManager.Instance.Gold >= m_valueToCheck)
            OnCallSucess.Invoke();
        else
            OnCallFailed.Invoke();
    }
}
