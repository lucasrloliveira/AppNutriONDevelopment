using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SelecionaRefeicao : MonoBehaviour {

    [SerializeField]
    string m_mainPrefix = "";
    [SerializeField]
    string m_title = "Ceia";
    [SerializeField]
    Text m_titleField = null;
    [SerializeField]
    string m_prefix = "Ceia";
    [SerializeField]
    MostraRefeicaoForm m_form = null;


    public string Title
    {
        get
        {
            return m_title;
        }
        set
        {
            if (m_title == value)
                return;
            m_title = value;
            SetDirty();
        }
    }

    public string Prefix
    {
        get
        {
            return m_prefix;
        }
        set
        {
            if (m_prefix == value)
                return;
            m_prefix = value;
            SetDirty();
            SetCorrectPrefix();
        }
    }


    public string MainPrefix
    {
        get
        {
            return m_mainPrefix;
        }
        set
        {
            if (m_mainPrefix == value)
                return;
            m_mainPrefix = value;
            SetDirty();
        }
    }

    bool _isDirty = false;

    public void SetDirty()
    {
        _isDirty = true;
    }

    protected virtual void OnEnable()
    {
        TryApplyInformations();
    }

    protected virtual void OnStart()
    {
        TryApplyInformations();
    }

    protected virtual void Update()
    {
        TryApplyInformations();
    }

    public void TryApplyInformations(bool force = false)
    {
        if (_isDirty || force)
        {
            _isDirty = false;
            ApplyInformations();
        }
    }

    protected virtual void ApplyInformations()
    {
        if (m_titleField != null)
            m_titleField.text = m_title;
        if (m_form != null)
            m_form.PrefixoPossivel = m_mainPrefix+m_prefix;
    }

    public void SetCorrectPrefix()
    {
        RefeicaoManager.Instance.CurrentPrefix = m_prefix;
    }
}
