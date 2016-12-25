using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ItemSupermecadoForm : MonoBehaviour {

    [SerializeField]
    InputField m_nomeField;

    public InputField NomeField
    {
        get
        {
            return m_nomeField;
        }
        set
        {
            if (m_nomeField == value)
                return;
            m_nomeField = value;
        }
    }

    public void DestroyForm()
    {
        GameObject.Destroy(gameObject);
    }
    public void ItemComprado(bool p_ItemComprado)
    {
        if (NomeField != null && NomeField.textComponent != null)
        {
            if (p_ItemComprado)
                NomeField.textComponent.color = Color.gray;
            else
                NomeField.textComponent.color = Color.black;
        }
    }
}
