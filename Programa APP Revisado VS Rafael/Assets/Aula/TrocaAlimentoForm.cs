using UnityEngine;
using System.Collections;

public class TrocaAlimentoForm : AlimentoForm
{

    TrocaAlimentoFormPopup m_owner = null;
    public TrocaAlimentoFormPopup Owner
    {
        get
        {
            return m_owner;
        }
        set
        {
            if (m_owner == value)
                return;
            m_owner = value;
        }
    }

    public void TrocaAlimento()
    {
        var v_quemChamou = Owner.Owner;
        v_quemChamou.SetAlimento(this._alimentoDefinition);
        Owner.Hide();
    }
}
