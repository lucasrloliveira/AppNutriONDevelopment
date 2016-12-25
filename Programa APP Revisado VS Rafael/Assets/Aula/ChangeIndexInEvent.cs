using UnityEngine;
using System.Collections;

public class ChangeIndexInEvent : MonoBehaviour
{
    [SerializeField]
    int m_visibleIndex = 0;
    public IntUnityEvent OnEventCalled;

    public int VisibleIndex
    {
        get
        {
            return m_visibleIndex;
        }
        set
        {
            if (m_visibleIndex == value)
                return;
            m_visibleIndex = value;
            TryCallEvent();
        }
    }

	protected virtual void OnEnable ()
    {
        TryCallEvent();
    }

    protected virtual void Start()
    {
        TryCallEvent();
    }

    public void TryCallEvent()
    {
        if (PlayerPrefs.HasKey("Peso") &&
            PlayerPrefs.HasKey("Altura") &&
            PlayerPrefs.HasKey("Idade") &&
            PlayerPrefs.HasKey("Objective") &&
            PlayerPrefs.HasKey("Sex"))
        {
            OnEventCalled.Invoke(m_visibleIndex);
        }
    }

    [System.Serializable]
    public class IntUnityEvent : UnityEngine.Events.UnityEvent<int> { }
}
