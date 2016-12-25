using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TrocaAlimentoFormPopup : MonoBehaviour
{
    [SerializeField]
    Transform m_content = null;
    [SerializeField]
    TrocaAlimentoForm m_prefab = null;

    AlimentoForm m_owner = null;

    public Transform Content
    {
        get
        {
            if (m_content == null)
                m_content = this.transform;
            return m_content;
        }
    }

    public AlimentoForm Owner
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

    public void Show()
    {
        CriarAlimentos();
        gameObject.SetActive(true);
    }

    public void CriarAlimentos()
    {
        MostraRefeicaoForm v_mostraRefeicao = Owner.GetComponentInParent<MostraRefeicaoForm>();
        Aula.Alimento v_alimento = Owner.GetAlimento();
        //pega todos os alimentos do tipo do cara que chamou o popup
        List<Aula.Alimento> v_alimentos = v_mostraRefeicao.CurrentRefeicao.GetAlimentosWithNotaByTipo(v_alimento.Tipo, 0);
        //remove o proprio alimento que chamou o popup de substituicao
        for (int i = 0; i < v_alimentos.Count; i++)
        {
            if (v_alimentos[i].Key == v_alimento.Key)
            {
                v_alimentos.RemoveAt(i);
                break;
            }
        }
        List<TrocaAlimentoForm> v_components = new List<TrocaAlimentoForm>(GetComponentsInChildren<TrocaAlimentoForm>());

        while (v_alimentos.Count != v_components.Count)
        {
            if (v_alimentos.Count > v_components.Count)
            {
                var v_prefabInstance = GameObject.Instantiate(m_prefab);
                v_prefabInstance.transform.SetParent(Content, true);
                v_prefabInstance.transform.localScale = Vector3.one;
                v_components.Add(v_prefabInstance); //Cria Alimento Form
            }
            else
            {
                Object.Destroy(v_components[v_components.Count - 1].gameObject); //Destroi alimentoform extra
                v_components.RemoveAt(v_components.Count - 1);
            }
        }

        //Seta todos os alimentos retornados pela refeicao manager nos Forms
        var v_ownerAlimentoModificado = Owner.GetAlimento();
        var v_ownerAlimentoOriginal = RefeicaoManager.Instance.GetAlimentoByKey(v_ownerAlimentoModificado.Key);
        float v_fatorMultiplicador = ((float)v_ownerAlimentoModificado.QuantidadeCal) / v_ownerAlimentoOriginal.QuantidadeCal;

        for (int i = 0; i < v_alimentos.Count; i++)
        {
            v_alimentos[i].ModificaAlimentoPorFatorMultiplicator(v_fatorMultiplicador);
            v_components[i].SetAlimento(v_alimentos[i]);
            v_components[i].Owner = this;
        }
    }

    public void Hide()
    {
        MostraRefeicaoForm v_form = Owner.GetComponentInParent<MostraRefeicaoForm>();
        AlimentoForm[] v_alimentosForm = v_form.GetComponentsInChildren<AlimentoForm>();
        List<Aula.Alimento> v_alimentos = new List<Aula.Alimento>();
        foreach (var v_alimentoForm in v_alimentosForm)
        {
            if (v_alimentoForm != null)
                v_alimentos.Add(v_alimentoForm.GetAlimento());
        }
        v_form.DebugLogQuantidades(v_alimentos);
        gameObject.SetActive(false);
        
    }


}
