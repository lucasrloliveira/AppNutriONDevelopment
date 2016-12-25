using UnityEngine;
using System.Collections;

public class ListaSupermercadoForm : MonoBehaviour {

    [SerializeField]
    ItemSupermecadoForm m_ItemSupermercadoPrefab;
    [SerializeField]
    Transform m_Content;

    protected virtual void Start()
    {
        CriaItensIniciais();
    }
    public void CriaItensIniciais()
    {
        DeletaTodosOsItems();
        // Aparecer apenas
        // 4 cereais, 2 frutas, 1 queijo, 2 lacteos, 1 proteina, 1 oleaginosa, 1 leguminosa, 
        // 2 legumes tipo A e 2 legumes tipo B, entre os alimentos mais consumidos.
        int v_cereais = 4;
        int v_frutas = 2;
        int v_queijo = 1;
        int v_lacteos = 2;
        int v_proteina = 1;
        int v_oleaginosa = 1;
        int v_leguminosa = 1;
        int v_batata = 1;
        int v_legumesA = 2;
        int v_legumesB = 2;
        foreach (string v_key in RefeicaoManager.AlimentosMaisConsumidos.Keys)
        {
            //int v_quantidadeQueApareceu = RefeicaoManager.AlimentosMaisConsumidos[v_key];
            Aula.Alimento v_alimento = RefeicaoManager.Instance.GetAlimentoByKey(v_key);
            if (v_alimento != null)
            {
                if (v_alimento.Tipo == Aula.AlimentoTipoEnum.Batatas)
                {
                    if (v_batata <= 0)
                        continue;
                    v_batata--;
                }
                else if (v_alimento.Tipo == Aula.AlimentoTipoEnum.Cereais)
                {
                    if (v_cereais <= 0)
                        continue;
                    v_cereais--;
                }
                else if (v_alimento.Tipo == Aula.AlimentoTipoEnum.Frutas)
                {
                    if (v_frutas <= 0)
                        continue;
                    v_frutas--;
                }
                else if (v_alimento.Tipo == Aula.AlimentoTipoEnum.Lacteos)
                {
                    if (v_lacteos <= 0)
                        continue;
                    v_lacteos--;
                }
                else if (v_alimento.Tipo == Aula.AlimentoTipoEnum.LegumesA)
                {
                    if (v_legumesA <= 0)
                        continue;
                    v_legumesA--;
                }
                else if (v_alimento.Tipo == Aula.AlimentoTipoEnum.LegumesB)
                {
                    if (v_legumesB <= 0)
                        continue;
                    v_legumesB--;
                }
                else if (v_alimento.Tipo == Aula.AlimentoTipoEnum.Leguminosas)
                {
                    if (v_leguminosa <= 0)
                        continue;
                    v_leguminosa--;
                }
                else if (v_alimento.Tipo == Aula.AlimentoTipoEnum.Oleaginosas)
                {
                    if (v_oleaginosa <= 0)
                        continue;
                    v_oleaginosa--;
                }
                else if (v_alimento.Tipo == Aula.AlimentoTipoEnum.Proteinas)
                {
                    if (v_proteina <= 0)
                        continue;
                    v_proteina--;
                }
                else if (v_alimento.Tipo == Aula.AlimentoTipoEnum.Queijos)
                {
                    if (v_queijo <= 0)
                        continue;
                    v_queijo--;
                }
                else
                {
                    continue;
                }
                ItemSupermecadoForm v_Item = GameObject.Instantiate(m_ItemSupermercadoPrefab);
                if (v_Item.NomeField != null)
                {
                    v_Item.NomeField.text = v_alimento.Nome;
                    v_Item.NomeField.interactable = false;
                }
                if (m_Content == null)
                    m_Content = this.transform;
                v_Item.transform.SetParent(this.m_Content);
                v_Item.transform.localScale = Vector3.one;
            }
        }
    }

    public void CriaItem()
    {
        ItemSupermecadoForm v_Item = GameObject.Instantiate(m_ItemSupermercadoPrefab);
        if (m_Content == null)
            m_Content = this.transform;
        v_Item.transform.SetParent(this.m_Content);
        v_Item.transform.localScale = Vector3.one;
    }

    public void DeletaTodosOsItems()
    {
        var v_forms = GetComponentsInChildren<ItemSupermecadoForm>();
        foreach(var v_form in v_forms)
        {
            if(v_form != null)
                Object.Destroy(v_form.gameObject);
        }
    }
}
