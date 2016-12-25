using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Aula;

public class RefeicaoManager : Kilt.Singleton<RefeicaoManager>
{
    public static event System.Action OnValueChanged;
    #region Private Variables

    [SerializeField]
    List<Alimento> m_definicaoDeAlimentos;
    [SerializeField]
    List<RefeicaoPredefinida> m_refeicoesDefinitions = new List<RefeicaoPredefinida>();
    [SerializeField]
    float m_kiloCaloriasTotais = 0;
    [SerializeField]
    string m_currentPrefix;
    [SerializeField]
    List<PrefixoPercentual> m_prefixosPorPercentual = new List<PrefixoPercentual>() {
        new PrefixoPercentual("CafeManha", 0.15f),
        new PrefixoPercentual("Colacao", 0.1f),
        new PrefixoPercentual("Almoco", 0.3f),
        new PrefixoPercentual("LancheTarde", 0.1f),
        new PrefixoPercentual("Jantar", 0.25f),
        new PrefixoPercentual("Ceia", 0.1f)
    };


    public string CurrentPrefix
    {
        get { return m_currentPrefix; }
        set {
            if (m_currentPrefix == value)
                return;
            m_currentPrefix = value;
            SetDirty();
        }
    }



    public bool IsDirty
    {
        get { return _isDirty; }
        set
        {
            if (_isDirty == value)
                return;
            _isDirty = value;
            SetDirty();
        }
    }

    bool _isDirty = false;
    public void SetDirty()
    {
        _isDirty = true;
    }

    #endregion

    #region Public Properties

    public float KiloCaloriasTotais
    {
        get { return m_kiloCaloriasTotais; }
        set { m_kiloCaloriasTotais = value; }
    }

    public List<PrefixoPercentual> PrefixosPorPercentual
    {
        get { return m_prefixosPorPercentual; }
        set { m_prefixosPorPercentual = value; }
    }

    public List<Alimento> DefinicaoDeAlimentos
    {
        get { return m_definicaoDeAlimentos; }
        set { m_definicaoDeAlimentos = value; }
    }

    public List<RefeicaoPredefinida> RefeicoesDefinitions
    {
        get { return m_refeicoesDefinitions; }
        set { m_refeicoesDefinitions = value; }
    }

    #endregion

    //Funcao para pegar o alimento pela key, pegando todas suas propriedades.
    //Parametros, sempre com p_
    public Alimento GetAlimentoByKey(string p_key)
    {
        foreach(var v_alimento in m_definicaoDeAlimentos)
        {
            if(v_alimento != null && string.Equals(p_key, v_alimento.Key))
            {
                return new Alimento(v_alimento);
            }
        }
        return null;
    }


    //declarando o dicionario de alimentos mais consumidos do tipo estatico
    static Dictionary<string, int> s_alimentosMaisConsumidos = null;
    
    //property do dicionario, apenas para leitura
    public static Dictionary<string, int> AlimentosMaisConsumidos
    {
        get
        {
            LoadAlimentosConsumidos();
            return s_alimentosMaisConsumidos;
        }
    }

    const string ALIMENTOS_MAIS_CONSUMIDOS_KEY = "AlimentosMaisConsumidos";

    //função que adiciona alimentos no json, o alimento tem key e quantidade de vezes que foi consumido
    //O que sao as variaveis com s_?
    public static void AdicionaAlimento(string p_key, bool p_forceSave = false)
    {
        LoadAlimentosConsumidos();
        if (s_alimentosMaisConsumidos.ContainsKey(p_key))
        {
            s_alimentosMaisConsumidos[p_key]+=1;
        }
        else
        {
            s_alimentosMaisConsumidos.Add(p_key, 1);
        }
        string v_json = Kilt.ExternLibs.KSerializer.NewtonsoftSerializationAPI.Serialize(typeof(Dictionary<string, int>), s_alimentosMaisConsumidos);
        PlayerPrefs.SetString(ALIMENTOS_MAIS_CONSUMIDOS_KEY, v_json);
        if(p_forceSave)
            PlayerPrefs.Save();
    }

    public static void LoadAlimentosConsumidos(bool p_force = false)
    {
        //Load!
        if (s_alimentosMaisConsumidos == null || p_force)
        {
            string v_prefs = PlayerPrefs.HasKey(ALIMENTOS_MAIS_CONSUMIDOS_KEY) ? PlayerPrefs.GetString(ALIMENTOS_MAIS_CONSUMIDOS_KEY) : null;
            if (v_prefs == null)
                s_alimentosMaisConsumidos = new Dictionary<string, int>();
            else
                s_alimentosMaisConsumidos = Kilt.ExternLibs.KSerializer.NewtonsoftSerializationAPI.Deserialize(typeof(Dictionary<string, int>), v_prefs) as Dictionary<string, int>;
        }
    }


    protected virtual void Update()
    {
        if (_isDirty)
        {
            SetPercentTodasAsRefeicoes();
            _isDirty = false;
            if (OnValueChanged != null)
                OnValueChanged();
        }
    }



    //seta o percentual de todas as refeicoes conforme a formula definida.
    public void SetPercentTodasAsRefeicoes()
    {
        float m_SomatorioKcalPercent=0;

        foreach (var v_prefixoPorPorcentagem in m_prefixosPorPercentual)
        {
            m_SomatorioKcalPercent += v_prefixoPorPorcentagem.Percentual;
        }

        foreach (var v_prefixoPorPorcentagem in m_prefixosPorPercentual)
        {
            if (v_prefixoPorPorcentagem.Prefixo.Contains("CafeManha"))
            {
                v_prefixoPorPorcentagem.Percentual = v_prefixoPorPorcentagem.Percentual*(1 + ((0.98333f - m_SomatorioKcalPercent) / m_SomatorioKcalPercent));
                PlayerPrefs.SetFloat("CafeManhaKcal", v_prefixoPorPorcentagem.Percentual);
            }
        }

        foreach (var v_prefixoPorPorcentagem in m_prefixosPorPercentual)
        {
            if (v_prefixoPorPorcentagem.Prefixo.Contains("Colacao"))
            {
                v_prefixoPorPorcentagem.Percentual = v_prefixoPorPorcentagem.Percentual * (1 + ((0.98333f - m_SomatorioKcalPercent) / m_SomatorioKcalPercent));
                PlayerPrefs.SetFloat("ColacaoKcal", v_prefixoPorPorcentagem.Percentual);
            }
        }

        foreach (var v_prefixoPorPorcentagem in m_prefixosPorPercentual)
        {
            if (v_prefixoPorPorcentagem.Prefixo.Contains("Almoco"))
            {
                v_prefixoPorPorcentagem.Percentual = v_prefixoPorPorcentagem.Percentual * (1 + ((0.98333f - m_SomatorioKcalPercent) / m_SomatorioKcalPercent));
                PlayerPrefs.SetFloat("AlmocoKcal", v_prefixoPorPorcentagem.Percentual);
            }
        }

        foreach (var v_prefixoPorPorcentagem in m_prefixosPorPercentual)
        {
            if (v_prefixoPorPorcentagem.Prefixo.Contains("LancheTarde"))
            {
                v_prefixoPorPorcentagem.Percentual = v_prefixoPorPorcentagem.Percentual * (1 + ((0.98333f - m_SomatorioKcalPercent) / m_SomatorioKcalPercent));
                PlayerPrefs.SetFloat("LancheTardeKcal", v_prefixoPorPorcentagem.Percentual);
            }
        }

        foreach (var v_prefixoPorPorcentagem in m_prefixosPorPercentual)
        {
            if (v_prefixoPorPorcentagem.Prefixo.Contains("Jantar"))
            {
                v_prefixoPorPorcentagem.Percentual = v_prefixoPorPorcentagem.Percentual * (1 + ((0.98333f - m_SomatorioKcalPercent) / m_SomatorioKcalPercent));
                PlayerPrefs.SetFloat("JantarKcal", v_prefixoPorPorcentagem.Percentual);
            }
        }

        foreach (var v_prefixoPorPorcentagem in m_prefixosPorPercentual)
        {
            if (v_prefixoPorPorcentagem.Prefixo.Contains("Ceia"))
            {
                v_prefixoPorPorcentagem.Percentual = v_prefixoPorPorcentagem.Percentual * (1 + ((0.98333f - m_SomatorioKcalPercent) / m_SomatorioKcalPercent));
                PlayerPrefs.SetFloat("CeiaKcal", v_prefixoPorPorcentagem.Percentual);
            }
        }
        SetDirty();
    }

    void Start()
    {
        getInfoGerenciadorDietas();
        SetPercentTodasAsRefeicoes();
    }

    void getInfoGerenciadorDietas()
    {
        if (PlayerPrefs.HasKey("CafeManhaKcal")) {
            foreach (var v_prefixoPorPorcentagem in m_prefixosPorPercentual)
            {
                if (v_prefixoPorPorcentagem.Prefixo.Contains("CafeManha"))
                {
                    v_prefixoPorPorcentagem.Percentual = PlayerPrefs.GetFloat("CafeManhaKcal");
                }
            }
        }
        if (PlayerPrefs.HasKey("ColacaoKcal"))
        {
            foreach (var v_prefixoPorPorcentagem in m_prefixosPorPercentual)
            {
                if (v_prefixoPorPorcentagem.Prefixo.Contains("Colacao"))
                {
                    v_prefixoPorPorcentagem.Percentual = PlayerPrefs.GetFloat("ColacaoKcal");
                }
            }
        }
        if (PlayerPrefs.HasKey("AlmocoKcal"))
        {
            foreach (var v_prefixoPorPorcentagem in m_prefixosPorPercentual)
            {
                if (v_prefixoPorPorcentagem.Prefixo.Contains("Almoco"))
                {
                    v_prefixoPorPorcentagem.Percentual = PlayerPrefs.GetFloat("AlmocoKcal");
                }
            }
        }
        if (PlayerPrefs.HasKey("LancheTardeKcal"))
        {
            foreach (var v_prefixoPorPorcentagem in m_prefixosPorPercentual)
            {
                if (v_prefixoPorPorcentagem.Prefixo.Contains("LancheTarde"))
                {
                    v_prefixoPorPorcentagem.Percentual = PlayerPrefs.GetFloat("LancheTardeKcal");
                }
            }
        }
        if (PlayerPrefs.HasKey("JantarKcal"))
        {
            foreach (var v_prefixoPorPorcentagem in m_prefixosPorPercentual)
            {
                if (v_prefixoPorPorcentagem.Prefixo.Contains("Jantar"))
                {
                    v_prefixoPorPorcentagem.Percentual = PlayerPrefs.GetFloat("JantarKcal");
                }
            }
        }
        if (PlayerPrefs.HasKey("CeiaKcal"))
        {
            foreach (var v_prefixoPorPorcentagem in m_prefixosPorPercentual)
            {
                if (v_prefixoPorPorcentagem.Prefixo.Contains("Ceia"))
                {
                    v_prefixoPorPorcentagem.Percentual = PlayerPrefs.GetFloat("CeiaKcal");
                }
            }
        }
    }
}

[System.Serializable]
public class PrefixoPercentual
{
    [SerializeField]
    string m_prefixo = "";
    [SerializeField, Range(0,1)]
    float m_percentual = 0;

    public string Prefixo
    {
        get
        {
            return m_prefixo;
        }
        set
        {
            if (m_prefixo == value)
                return;
            m_prefixo = value;
        }
    }

    public float Percentual
    {
        get
        {
            return m_percentual;
        }
        set
        {
            if (m_percentual == value)
                return;
            m_percentual = value;
        }
    }


    public PrefixoPercentual(string p_prefixo, float p_percentual)
    {
        m_prefixo = p_prefixo;
        m_percentual = p_percentual;
    }
}

