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
    string[] m_prefixosPorPercentual = new string[] { "CafeManha", "Colacao", "Almoco" ,"LancheTarde", "Jantar", "Ceia"};
    [SerializeField]
    float[] m_percentuaisRefeicoes = new float[] {0.15f , 0.1f , 0.3f , 0.15f , 0.2f , 0.1f };
    [SerializeField]
    float m_kiloCaloriasTotais = 0;
    [SerializeField]
    string m_currentPrefix;

    //variaveis teste lucas
    //Ajustando a % de kcal por refeições de acordo com a funcionalidade Gerenciador de Dietas

    [SerializeField]
    float m_CafeDaManhaKcal;
    [SerializeField]
    float m_ColacaoKcal;
    [SerializeField]
    float m_AlmocoKcal;
    [SerializeField]
    float m_LancheTardeKcal;
    [SerializeField]
    float m_JantarKcal;
    [SerializeField]
    float m_CeiaKcal;

    public float CafeDaManhaKcal
    {
        get { return m_CafeDaManhaKcal; }
        set
        {
            if (m_CafeDaManhaKcal == value)
                return;
            m_CafeDaManhaKcal = value;
            SetDirty();
            PlayerPrefs.SetFloat("CafeManhaValor", m_CafeDaManhaKcal);
            PlayerPrefs.Save();
        }
    }

    public float ColacaoKcal
    {
        get { return m_ColacaoKcal; }
        set
        {
            if (m_ColacaoKcal == value)
                return;
            m_ColacaoKcal = value;
            SetDirty();
            PlayerPrefs.SetFloat("ColacaoValor", m_ColacaoKcal);
            PlayerPrefs.Save();
        }
    }

    public float AlmocoKcal
    {
        get { return m_AlmocoKcal; }
        set
        {
            if (m_AlmocoKcal == value)
                return;
            m_AlmocoKcal = value;
            SetDirty();
            PlayerPrefs.SetFloat("AlmocoValor", m_AlmocoKcal);
            PlayerPrefs.Save();
        }
    }

    public float LancheTardeKcal
    {
        get { return m_LancheTardeKcal; }
        set
        {
            if (m_LancheTardeKcal == value)
                return;
            m_LancheTardeKcal = value;
            SetDirty();
            PlayerPrefs.SetFloat("LancheTardeValor", m_LancheTardeKcal);
            PlayerPrefs.Save();
        }
    }

    public float JantarKcal
    {
        get { return m_JantarKcal; }
        set
        {
            if (m_JantarKcal == value)
                return;
            m_JantarKcal = value;
            SetDirty();
            PlayerPrefs.SetFloat("JantarValor", m_JantarKcal);
            PlayerPrefs.Save();
        }
    }

    public float CeiaKcal
    {
        get { return m_CeiaKcal; }
        set
        {
            if (m_CeiaKcal == value)
                return;
            m_CeiaKcal = value;
            SetDirty();
            PlayerPrefs.SetFloat("CeiaValor", m_CeiaKcal);
            PlayerPrefs.Save();
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

    //SerializeField para as variáveis que irão aparecer no Inspector
    //Variaveis com m_: variáveis do manager.

    #endregion

    #region Public Properties

    //Properties: get: executado quando a propriedade é lida, set: executado quando se deseja atribuir um novo valor para a propriedade
    //properties nao sao variaveis, portanto nao podem ser referenciadas.

    //properties teste

    public float KiloCaloriasTotais
    {
        get { return m_kiloCaloriasTotais; }
        set { m_kiloCaloriasTotais = value; }
    }

    public float []PercentuaisRefeicoes
    {
        get { return m_percentuaisRefeicoes; }
        set { m_percentuaisRefeicoes = value; }
    }

    public string[] PrefixosPorPercentual
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


    //teste lucas

    protected virtual void Update()
    {
        if (_isDirty)
        {
            SetPercentTodasAsRefeicoes();
            SalvaInfoGerenciadorDietas();
            _isDirty = false;
            if (OnValueChanged != null)
                OnValueChanged();
        }
    }



    //FUNCAO PARA SALVAR TODAS AS INFORMACOES DO GERENCIADOR DE DIETAS, DEVE SER ATUALIZADA NO START OU AWAKE
    public void SalvaInfoGerenciadorDietas()
    {
        if (PlayerPrefs.HasKey("CafeManhaValor"))
            m_CafeDaManhaKcal = PlayerPrefs.GetFloat("CafeManhaValor");
        else
            m_CafeDaManhaKcal = 0.15f;

        if (PlayerPrefs.HasKey("ColacaoValor"))
            m_ColacaoKcal = PlayerPrefs.GetFloat("ColacaoValor");
        else
            m_ColacaoKcal = 0.10f;

        if (PlayerPrefs.HasKey("AlmocoValor"))
            m_AlmocoKcal = PlayerPrefs.GetFloat("AlmocoValor");
        else
            m_AlmocoKcal = 0.3f;

        if (PlayerPrefs.HasKey("LancheTardeValor"))
            m_LancheTardeKcal = PlayerPrefs.GetFloat("LancheTardeValor");
        else
            m_LancheTardeKcal = 0.15f;

        if (PlayerPrefs.HasKey("JantarValor"))
            m_JantarKcal = PlayerPrefs.GetFloat("JantarValor");
        else
            m_JantarKcal = 0.2f;

        if (PlayerPrefs.HasKey("CeiaValor"))
            m_CeiaKcal = PlayerPrefs.GetFloat("CeiaValor");
        else
            m_CeiaKcal = 0.1f;
        PlayerPrefs.Save();
        SetDirty();
    }

    //seta o percentual de todas as refeicoes conforme a formula definida.
    public void SetPercentTodasAsRefeicoes()
    {
        float m_SomatorioKcalPercent;
        m_SomatorioKcalPercent = m_CafeDaManhaKcal + m_ColacaoKcal + m_AlmocoKcal + m_LancheTardeKcal + m_JantarKcal + m_CeiaKcal;


        m_CafeDaManhaKcal = m_CafeDaManhaKcal * (1 + ((0.98333f - m_SomatorioKcalPercent) / m_SomatorioKcalPercent));
        foreach (string v_prefixoPorPorcentagemCafeManha in PrefixosPorPercentual)
        {
            if (v_prefixoPorPorcentagemCafeManha.Contains("CafeManha"))
            {
                RefeicaoManager.Instance.PercentuaisRefeicoes[2] = m_CafeDaManhaKcal;
            }
        }

        m_ColacaoKcal = m_ColacaoKcal * (1 + ((0.98333f - m_SomatorioKcalPercent) / m_SomatorioKcalPercent));
        foreach (string v_prefixoPorPorcentagemColacao in RefeicaoManager.Instance.PrefixosPorPercentual)
        {
            if (v_prefixoPorPorcentagemColacao.Contains("Colacao"))
            {
                RefeicaoManager.Instance.PercentuaisRefeicoes[2] = m_ColacaoKcal;
            }
        }

        m_AlmocoKcal = m_AlmocoKcal * (1 + ((0.98333f - m_SomatorioKcalPercent) / m_SomatorioKcalPercent));
        foreach (string v_prefixoPorPorcentagemAlmoco in RefeicaoManager.Instance.PrefixosPorPercentual)
        {
            if (v_prefixoPorPorcentagemAlmoco.Contains("Almoco"))
            {
                RefeicaoManager.Instance.PercentuaisRefeicoes[3] = m_AlmocoKcal;
            }
        }

        m_LancheTardeKcal = m_LancheTardeKcal * (1 + ((0.98333f - m_SomatorioKcalPercent) / m_SomatorioKcalPercent));
        foreach (string v_prefixoPorPorcentagemLancheTarde in RefeicaoManager.Instance.PrefixosPorPercentual)
        {
            if (v_prefixoPorPorcentagemLancheTarde.Contains("LancheTarde"))
            {
                RefeicaoManager.Instance.PercentuaisRefeicoes[4] = m_LancheTardeKcal;
            }
        }

        m_JantarKcal = m_JantarKcal * (1 + ((0.98333f - m_SomatorioKcalPercent) / m_SomatorioKcalPercent));
        foreach (var v_prefixoPorPorcentagemJantar in RefeicaoManager.Instance.PrefixosPorPercentual)
        {
            if (v_prefixoPorPorcentagemJantar.Contains("Jantar"))
            {
                RefeicaoManager.Instance.PercentuaisRefeicoes[4] = m_JantarKcal;
            }
        }

        m_CeiaKcal = m_CeiaKcal * (1 + ((0.98333f - m_SomatorioKcalPercent) / m_SomatorioKcalPercent));
        foreach (var v_prefixoPorPorcentagemCeia in RefeicaoManager.Instance.PrefixosPorPercentual)
        {
            if (v_prefixoPorPorcentagemCeia.Contains("Ceia"))
            {
                RefeicaoManager.Instance.PercentuaisRefeicoes[5] = m_CeiaKcal;
            }
        }
        SetDirty();
    }

    void Start()
    {
        SetPercentTodasAsRefeicoes();
        SalvaInfoGerenciadorDietas();
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

