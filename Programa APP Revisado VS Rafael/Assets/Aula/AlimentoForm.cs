using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Aula;

public class AlimentoForm : MonoBehaviour {

    [SerializeField]
    Text m_nome;
    [SerializeField]
    Text m_unidade;
    [SerializeField]
    string m_nomePorcaoFormatted;
    [SerializeField]
    Text m_peso;
    
    protected Aula.Alimento _alimentoDefinition = null;

    protected virtual void OnEnable()
    {
        if (_stated)
            PreencheCampos();
    }

    bool _stated = false;
    protected virtual void Start()
    {
        _stated = true;
        PreencheCampos();
    }


    public void SetAlimento(Aula.Alimento p_alimentoDefinition)
    {
        _alimentoDefinition = p_alimentoDefinition;
        PreencheCampos();
    }

    public Aula.Alimento GetAlimento()
    {
        return _alimentoDefinition;
}

    public void PreencheCampos()
    {
        if (_alimentoDefinition != null)
        {
            if (m_nome != null)
            {
                m_nome.text = _alimentoDefinition.Nome;
            }
            if (m_unidade != null)
            {   
                m_nomePorcaoFormatted = _alimentoDefinition.Porcao.GetTipoPorcaoFormatted();
                if (_alimentoDefinition.Porcao.QuantidadeDaPorcao == 0.5f)
                {
                    m_unidade.text = "1/2  " + m_nomePorcaoFormatted;
                }
                else
                {
                    m_unidade.text = _alimentoDefinition.Porcao.QuantidadeDaPorcao.ToString() + "  " + m_nomePorcaoFormatted;
                }
            }
            if (m_peso != null)
            {
                if (_alimentoDefinition.Tipo == AlimentoTipoEnum.Lacteos || _alimentoDefinition.Tipo == AlimentoTipoEnum.Sucos)
                {
                    m_peso.text = _alimentoDefinition.Porcao.PesoEmGramas.ToString() + "ml";
                }
                else
                {
                    m_peso.text = _alimentoDefinition.Porcao.PesoEmGramas.ToString() + "g";
                }    
            }
           
        }
    }

    public void ChamarPopup()
    {
        var v_owner = GetComponentInParent<MostraRefeicaoForm>();
        if (v_owner != null && v_owner.PopupDeTroca != null)
        {
            v_owner.PopupDeTroca.Owner = this;
            v_owner.PopupDeTroca.Show();
        }
    }
}
