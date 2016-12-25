using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Aula;

public class MostraRefeicaoForm : MonoBehaviour
{
    [SerializeField]
    Transform m_content = null;
    [SerializeField]
    AlimentoForm m_prefab = null;
    [SerializeField]
    TrocaAlimentoFormPopup m_popupDeTroca = null;
    [SerializeField]
    string m_prefixoPossivel = "Ceia";
    //[SerializeField]
    //float m_porcentagemDeKilocaloriasTotais = 0.1f;

    [SerializeField]
    Text m_nota = null;
    [SerializeField]
    Text m_quantidadeCal = null;
    [SerializeField]
    Text m_quantidadeCarb = null;
    [SerializeField]
    Text m_quantidadeProt = null;
    [SerializeField]
    Text m_quantidadeGord = null;
    [SerializeField]
    Text m_kcalCorretaPrato = null;
    [SerializeField]
    Text m_key = null;

    [SerializeField]
    string m_currentPrefix;

    RefeicaoPredefinida _currentRefeicao = null;

    public float PorcentagemDeKilocaloriasTotais
    {
        get
        {
            foreach (var v_prefixoPorPorcentagem in RefeicaoManager.Instance.PrefixosPorPercentual)
            {
                if (v_prefixoPorPorcentagem != null && m_prefixoPossivel != null)
                {
                    if (v_prefixoPorPorcentagem.Contains("CafeManha"))
                        return RefeicaoManager.Instance.PercentuaisRefeicoes[0];
                    else if (v_prefixoPorPorcentagem.Contains("Colacao"))
                        return RefeicaoManager.Instance.PercentuaisRefeicoes[1];
                    else if (v_prefixoPorPorcentagem.Contains("Almoco"))
                        return RefeicaoManager.Instance.PercentuaisRefeicoes[2];
                    else if (v_prefixoPorPorcentagem.Contains("LancheTarde"))
                        return RefeicaoManager.Instance.PercentuaisRefeicoes[3];
                    else if (v_prefixoPorPorcentagem.Contains("Jantar"))
                        return RefeicaoManager.Instance.PercentuaisRefeicoes[4];
                    else if (v_prefixoPorPorcentagem.Contains("Ceia"))
                        return RefeicaoManager.Instance.PercentuaisRefeicoes[5];
                    else
                    {
                        Debug.Log("o prefixo é:" + v_prefixoPorPorcentagem);
                        return 0.1f;
                    }
                    
                }
            }
            return 0.2f;
        }
    }

    public RefeicaoPredefinida CurrentRefeicao
    {
        get
        {
            return _currentRefeicao;
        }
    }



    public TrocaAlimentoFormPopup PopupDeTroca
    {
        get
        {
            return m_popupDeTroca;
        }
    }
    public Transform Content
    {
        get
        {
            if (m_content == null)
                m_content = this.transform;
            return m_content;
        }
    }

    public string PrefixoPossivel
    {
        get
        {
            return m_prefixoPossivel;
        }
        set
        {
            if (m_prefixoPossivel == value)
                return;
            m_prefixoPossivel = value;
            SetDirty();
        }
    }

    #region Unity Functions

    protected virtual void OnEnable()
    {
        if (_stated)
            TryCreateFood();
    }

    bool _stated = false;
    protected virtual void Start()
    {
        _stated = true;
        TryCreateFood();
    }

    bool _isDirty = false;
    protected virtual void Update()
    {
        TryCreateFood();
    }

    #endregion

    #region Helper Functions

    public void SetDirty()
    {
        _isDirty = true;
    }

    public void TryCreateFood(bool p_force = false)
    {
        if (_isDirty || p_force)
        {
            _isDirty = false;
            CriarAlimentos();
        }
    }

    public List<RefeicaoPredefinida> GetRefeicoesComPrefixo()
    {

        List<RefeicaoPredefinida> v_retorno = new List<RefeicaoPredefinida>();
        foreach (var v_refeicao in RefeicaoManager.Instance.RefeicoesDefinitions)
        {
            if(v_refeicao != null && v_refeicao.Key != null && v_refeicao.Key.StartsWith(m_prefixoPossivel))
            {
                v_retorno.Add(v_refeicao);
            }
        }
        return v_retorno;
    }

    List<string> _alreadyChecked = new List<string>();
    public void CriarAlimentos()
    {
        var v_refeicoesPossiveis = GetRefeicoesComPrefixo();
        var v_index = Random.Range(0, v_refeicoesPossiveis.Count);
        if (!_alreadyChecked.Contains(m_prefixoPossivel))
        {
            v_index = 0;
            foreach (var v_ref in v_refeicoesPossiveis)
            {
                if (v_ref.Key.StartsWith(m_prefixoPossivel + "1"))
                    break;
                v_index++;
            }
            if (v_index > v_refeicoesPossiveis.Count)
                v_index = 0;
            _alreadyChecked.Add(m_prefixoPossivel);
        }
        _currentRefeicao = v_refeicoesPossiveis.Count > v_index? v_refeicoesPossiveis[v_index] : null;
        if (_currentRefeicao != null)
        {
            List<Aula.Alimento> v_alimentos = _currentRefeicao.GetAlimentos(RefeicaoManager.Instance.KiloCaloriasTotais*PorcentagemDeKilocaloriasTotais);
            List<AlimentoForm> v_components = new List<AlimentoForm>(GetComponentsInChildren<AlimentoForm>());

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
            for (int i = 0; i < v_alimentos.Count; i++)
            {
                v_components[i].SetAlimento(v_alimentos[i]);
            }
            DebugLogQuantidades(v_alimentos);
        }
        else
            Debug.LogError("Prefixo nao existe na refeicao!! corrija!!");
    }

    public void DebugLogQuantidades(List<Aula.Alimento> p_alimentos)
    {
        float v_quantidadeCal = 0;
        float v_quantidadeCarb = 0;
        float v_quantidadeProt = 0;
        float v_quantidadeGord = 0;
        float v_notaMedia = 0;
        foreach (var v_alimento in p_alimentos)
        {
            if(v_alimento != null)
            {
                v_quantidadeCal += v_alimento.QuantidadeCal;
                v_quantidadeCarb += v_alimento.QuantidadeCarb;
                v_quantidadeProt += v_alimento.QuantidadeProt;
                v_quantidadeGord += v_alimento.QuantidadeGord;
                v_notaMedia += v_alimento.Nota;
            }
        }
        v_notaMedia = p_alimentos.Count > 0 ? v_notaMedia / p_alimentos.Count : 0;

        Debug.Log("Cal: " + v_quantidadeCal);
        Debug.Log("Carb: " + v_quantidadeCarb);
        Debug.Log("Prot: " + v_quantidadeProt);
        Debug.Log("Gord: " + v_quantidadeGord);
        Debug.Log("Nota: " + v_notaMedia);
        Debug.Log("Kcal Ideal: " + Mathf.RoundToInt(RefeicaoManager.Instance.KiloCaloriasTotais));

        if (m_nota != null)
            m_nota.text = "Nota: " + v_notaMedia.ToString("0.0");
        if (m_quantidadeCal != null)
            m_quantidadeCal.text = "Calorias: " + v_quantidadeCal.ToString() + " kcal";
        if (m_quantidadeCarb != null)
            m_quantidadeCarb.text = "Carb: " + v_quantidadeCarb.ToString() + " g  " + (Mathf.RoundToInt(400*v_quantidadeCarb/v_quantidadeCal)).ToString() + "%";
        if (m_quantidadeProt != null)
            m_quantidadeProt.text = "Prot: " + v_quantidadeProt.ToString() + " g  " + (Mathf.RoundToInt(400* v_quantidadeProt / v_quantidadeCal)).ToString() + "%";
        if (m_quantidadeGord != null)
            m_quantidadeGord.text = "Gord: " + v_quantidadeGord.ToString() + " g  " + (Mathf.RoundToInt(900* v_quantidadeGord / v_quantidadeCal)).ToString() + "%";
        if (m_kcalCorretaPrato != null)
            m_kcalCorretaPrato.text = "KcalIDEAL: " + (Mathf.RoundToInt(RefeicaoManager.Instance.KiloCaloriasTotais * PorcentagemDeKilocaloriasTotais)).ToString() + "/"+ (Mathf.RoundToInt(RefeicaoManager.Instance.KiloCaloriasTotais)).ToString() + "kcal";
        if (m_key != null)
            m_key.text = "KcalIDEAL: " + _currentRefeicao.Key;
    }

    public void RegistraRefeicaoNasPreferencias()
    {
        Debug.Log("Entrou!");
        AlimentoForm[] v_alimentosForm = GetComponentsInChildren<AlimentoForm>();
        List<string> v_alimentos = new List<string>();
        foreach (var v_alimentoForm in v_alimentosForm)
        {
            if (v_alimentoForm != null)
            {
                var v_alimento = v_alimentoForm.GetAlimento();
                if(v_alimento != null)
                    v_alimentos.Add(v_alimento.Key);
            }
        }
        foreach (string v_alimento in v_alimentos)
        {
            RefeicaoManager.AdicionaAlimento(v_alimento, false);
        }
        PlayerPrefs.Save();

        foreach (var v_alimentosKey in RefeicaoManager.AlimentosMaisConsumidos.Keys)
        {
            Debug.Log("Key: " + v_alimentosKey + " Quantidade Consumida " + RefeicaoManager.AlimentosMaisConsumidos[v_alimentosKey]);
        }
    }

    #endregion
}
