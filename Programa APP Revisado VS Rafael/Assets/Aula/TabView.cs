using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class TabView : MonoBehaviour
{
    [SerializeField]
    List<TabPanelAndButton> m_tabs = new List<TabPanelAndButton>();
    [SerializeField]
    int m_visibleIndex = 0;

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
            SetDirty();
        }
    }

    #region Unity Functions

    protected virtual void OnEnable()
    {
        RegisterButtonEvents();
        ApplyTabs();
    }

    protected virtual void OnDisable()
    {
        UnregisterButtonEvents();
    }

    protected virtual void Update()
    {
        if (_isDirty)
        {
            _isDirty = false;
            ApplyTabs();
        }
    }

    #endregion

    #region Helper Functions

    public void RegisterButtonEvents()
    {
        var v_counter = 0;
        foreach (TabPanelAndButton v_tab in m_tabs)
        {
            var v_internalCounter = v_counter;
            if (v_tab != null && v_tab.m_Button != null)
            {
                if (v_tab.m_indexMethod == null)
                    v_tab.m_indexMethod = delegate { VisibleIndex = v_internalCounter; };

                if (v_tab.m_indexMethodToggle == null)
                    v_tab.m_indexMethodToggle = delegate(bool p_bool) 
                    {
                        if (p_bool)
                            VisibleIndex = v_internalCounter;
                    };

                Toggle v_toggle = v_tab.m_Button as Toggle;
                Button v_button = v_tab.m_Button as Button;
                if (v_toggle != null)
                    v_toggle.onValueChanged.AddListener(v_tab.m_indexMethodToggle);
                else if (v_button != null)
                    v_button.onClick.AddListener(v_tab.m_indexMethod);
            }
            v_counter++;
        }
    }

    public void UnregisterButtonEvents()
    {
        foreach (TabPanelAndButton v_tab in m_tabs)
        {
            if (v_tab != null && v_tab.m_Button != null)
            {
                Toggle v_toggle = v_tab.m_Button as Toggle;
                Button v_button = v_tab.m_Button as Button;
                if(v_toggle != null)
                    v_toggle.onValueChanged.RemoveListener(v_tab.m_indexMethodToggle);
                else if (v_button != null)
                    v_button.onClick.RemoveListener(v_tab.m_indexMethod);
            }
        }
    }

    bool _isDirty = false;
    public void SetDirty()
    {
        _isDirty = true;
    }

    public virtual void ApplyTabs()
    {
        int v_indexCounter = 0;

        UnregisterButtonEvents();
        foreach (TabPanelAndButton v_tab in m_tabs)
        {
            if (v_tab != null && v_tab.m_Panel != null)
            {
                Toggle v_toggle = v_tab.m_Button as Toggle;
                if (v_toggle != null)
                    v_toggle.isOn = v_indexCounter == m_visibleIndex;
                v_tab.m_Panel.SetActive(v_indexCounter == m_visibleIndex);
            }
            v_indexCounter++;
        }
        RegisterButtonEvents();
    }

	#endregion
}

[System.Serializable]
public class TabPanelAndButton
{
    public GameObject m_Panel;
    public UIBehaviour m_Button;
    [HideInInspector]
    public UnityEngine.Events.UnityAction m_indexMethod = null;
    [HideInInspector]
    public UnityEngine.Events.UnityAction<bool> m_indexMethodToggle = null;
}
