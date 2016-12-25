using UnityEngine;
using System.Collections;

public class CalculoEnergeticoManager : Kilt.Singleton<CalculoEnergeticoManager>
{
    public static event System.Action OnValueChanged;

    #region Private Variables

    //Informacoes necessarias para o calculo energetico diario
    [SerializeField]
    string m_Sex = "m";
    [SerializeField]
    string m_Objective = "manter";
    [SerializeField]
    float m_Peso = 40.0f;
    [SerializeField]
    float m_Altura = 140.0f;
    [SerializeField]
    float m_Idade = 15.0f;
    [SerializeField]
    float m_CalculoEnergeticoDiario = 0;
    [SerializeField]
    float m_GastoMetabolicoBasal;
    [SerializeField]
    int m_AceleraEmagrecimento;

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

    [SerializeField]
    float m_SomatorioKcalPercent = 0;

    #endregion

    #region Public Properties

    //Properties: get: executado quando a propriedade é lida, set: executado quando se deseja atribuir um novo valor para a propriedade
    //properties nao nao variaveis, portanto nao podem ser referenciadas.

    public string Sex
    {
        get { return m_Sex; }
        set
        {
            if (m_Sex == value)
                return;
            m_Sex = value;
            SetDirty();
        }
    }

    public string Objective
    {
        get { return m_Objective; }
        set
        {
            if (m_Objective == value)
                return;
            m_Objective = value;
            SetDirty();
        }
    }

    public float Peso
    {
        get { return m_Peso; }
        set {
            if (m_Peso == value)
                return;
            m_Peso = value;
            SetDirty();
        }
    }

    public float Idade
    {
        get { return m_Idade; }
        set
        {
            if (m_Idade == value)
                return;
            m_Idade = value;
            SetDirty();
        }
    }

    public float Altura
    {
        get { return m_Altura; }
        set {
            if (m_Altura == value)
                return;
            m_Altura = value;
            SetDirty();
        }
    }

    public float CalculoEnergeticoDiario
    {
        get { return m_CalculoEnergeticoDiario; }
        set {

            if (m_CalculoEnergeticoDiario == value)
                return;
            m_CalculoEnergeticoDiario = value;
            RefeicaoManager.Instance.KiloCaloriasTotais = value;
            SetDirty();
        }
    }

    public float GastoMetabolicoBasal
    {
        get { return m_GastoMetabolicoBasal; }
        set {
            if (m_GastoMetabolicoBasal == value)
                return;
            m_GastoMetabolicoBasal = value;
            SetDirty();
        }
    }

    public float CafeDaManhaKcal
    {
        get { return m_CafeDaManhaKcal; }
        set {
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
        set {
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
        set {
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
        set {
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
        set {
            if (m_CeiaKcal == value)
                return;
            m_CeiaKcal = value;
            SetDirty();
            PlayerPrefs.SetFloat("CeiaValor", m_CeiaKcal);
            PlayerPrefs.Save();
        }
    }

    public int AceleraEmagrecimento
    {
        get { return m_AceleraEmagrecimento; }
        set
        {
            if (m_AceleraEmagrecimento == value)
                return;
            m_AceleraEmagrecimento = value;
            SetDirty();
        }
    }

    public float SomatorioKcalPercent
    {
        get { return m_SomatorioKcalPercent; }
        set
        {
            if (m_SomatorioKcalPercent == value)
                return;
            m_SomatorioKcalPercent = value;
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

    #endregion

    #region Unity Functions

    protected virtual void Update()
    {
        if (_isDirty)
        {
            SetValorEnergeticoTotal();
            EmagrecimentoAcelerado();
            SalvaInfoIniciais();
            SalvaInfoGerenciadorDietas();
            _isDirty = false;
            if (OnValueChanged != null)
                OnValueChanged();
        }
    }

    #endregion

    #region Helper Functions

    #region FUNCIONALIDADES DO GERENCIADOR DE DIETAS


    //seta o percentual de todas as refeicoes conforme a formula definida.
    public void SetPercentTodasAsRefeicoes()
    {

        m_SomatorioKcalPercent = m_CafeDaManhaKcal + m_ColacaoKcal + m_AlmocoKcal + m_LancheTardeKcal + m_JantarKcal + m_CeiaKcal;


        m_CafeDaManhaKcal = m_CafeDaManhaKcal * (1 + ((0.98333f - m_SomatorioKcalPercent) / m_SomatorioKcalPercent));
        foreach (string v_prefixoPorPorcentagemCafeManha in RefeicaoManager.Instance.PrefixosPorPercentual)
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

    #endregion

    //FUNCAO PARA SALVAR TODAS AS INFORMACOES DO GERENCIADOR DE DIETAS, DEVE SER ATUALIZADA NO START OU AWAKE
    public void SalvaInfoGerenciadorDietas()
    {
        if(PlayerPrefs.HasKey("CafeManhaValor"))       
            m_CafeDaManhaKcal = PlayerPrefs.GetFloat("CafeManhaValor");
        else            
            m_CafeDaManhaKcal = 0.15f;

        if (PlayerPrefs.HasKey("ColacaoValor"))            
            m_ColacaoKcal = PlayerPrefs.GetFloat("ColacaoValor");
        else
            m_ColacaoKcal = 0.10f;

        if(PlayerPrefs.HasKey("AlmocoValor"))
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

    //FUNCAO PARA SALVAR TODAS AS INFORMACOES NECESSARIAS PARA O CALCULO ENERGETICO, DEVE SER CHAMADA NO METODO START OU AWAKE

    public void SalvaInfoIniciais()
    {
        if (PlayerPrefs.HasKey("Sex") || PlayerPrefs.HasKey("Objective") || PlayerPrefs.HasKey("Peso") || PlayerPrefs.HasKey("Altura") || PlayerPrefs.HasKey("Idade"))
        {
            m_Sex = PlayerPrefs.GetString("Sex");
            m_Objective = PlayerPrefs.GetString("Objective");
            m_Peso = PlayerPrefs.GetFloat("Peso");
            m_Idade = PlayerPrefs.GetFloat("Idade");
            m_Altura = PlayerPrefs.GetFloat("Altura");
        }

        else
        {
            m_Sex = "m";
            PlayerPrefs.SetString("Sex", m_Sex);
            m_Objective = "manter";
            PlayerPrefs.SetString("Objective", m_Objective);
            PlayerPrefs.SetFloat("Peso", m_Peso);
            PlayerPrefs.SetFloat("Idade", m_Idade);
            PlayerPrefs.SetFloat("Altura", m_Altura);
        }
        PlayerPrefs.Save();
    }

    bool _isDirty = false;
    public void SetDirty()
    {
        _isDirty = true;
    }

    public void SetValorEnergeticoTotal()
    {
        if (Sex == "m")
        {
            CalculoEnergeticoDiario = 1.3f * (66.4730f + 13.7516f * Peso + 5.0033f * Altura - 6.7550f * Idade);
            GastoMetabolicoBasal = (66.4730f + 13.7516f * Peso + 5.0033f * Altura - 6.7550f * Idade);
        }
        else if (Sex == "f")
        {
            CalculoEnergeticoDiario = 1.3f * (655.0955f + 9.5634f * Peso + 1.8496f * Altura - 4.6756f * Idade);
            GastoMetabolicoBasal = (655.0955f + 9.5634f * Peso + 1.8496f * Altura - 4.6756f * Idade);
        }
        if (Objective == "perder")
        {
            CalculoEnergeticoDiario = CalculoEnergeticoDiario - 250;
        }
        else if (Objective == "ganhar")
        {
            CalculoEnergeticoDiario = CalculoEnergeticoDiario + 300;
        }
        if (CalculoEnergeticoDiario < GastoMetabolicoBasal)
        {
            CalculoEnergeticoDiario = GastoMetabolicoBasal;
        }
        EmagrecimentoAcelerado();
        PlayerPrefs.SetFloat("SetValorEnergeticoTotal", m_CalculoEnergeticoDiario);
        PlayerPrefs.SetFloat("SetGastoMetabolicoBasal", m_GastoMetabolicoBasal);
        PlayerPrefs.Save();
    }

    protected void EmagrecimentoAcelerado()
    {
        switch (AceleraEmagrecimento)
        {
            case 0:
                PlayerPrefs.SetInt("AceleraEmagrecimento", AceleraEmagrecimento);
                break;
            case 1:
                if (m_Objective == "manter")
                    CalculoEnergeticoDiario = CalculoEnergeticoDiario - 250;
                else if (m_Objective == "perder") { }
                else if (m_Objective == "ganhar")
                    CalculoEnergeticoDiario = CalculoEnergeticoDiario - 550;
                else
                    Debug.Log("Corrigir Key Objetivo");
                if (CalculoEnergeticoDiario < GastoMetabolicoBasal)
                {
                    CalculoEnergeticoDiario = GastoMetabolicoBasal;
                }
                PlayerPrefs.SetInt("AceleraEmagrecimento", AceleraEmagrecimento);
                break;
            case 2:
                if (m_Objective == "manter")
                    CalculoEnergeticoDiario = CalculoEnergeticoDiario - 500;
                else if (m_Objective == "perder")
                    CalculoEnergeticoDiario = CalculoEnergeticoDiario - 250;
                else if (m_Objective == "Ganhar")
                    CalculoEnergeticoDiario = CalculoEnergeticoDiario - 800;
                else
                    Debug.Log("Corrigir Key Objetivo");
                CalculoEnergeticoDiario = CalculoEnergeticoDiario - 800;
                if (CalculoEnergeticoDiario < GastoMetabolicoBasal)
                {
                    CalculoEnergeticoDiario = GastoMetabolicoBasal;
                }
                break;
            case 3:
                if (m_Objective == "perder")
                    CalculoEnergeticoDiario = CalculoEnergeticoDiario - 500;
                else if (m_Objective == "manter")
                    CalculoEnergeticoDiario = CalculoEnergeticoDiario - 750;
                else if (m_Objective == "ganhar")
                    CalculoEnergeticoDiario = CalculoEnergeticoDiario - 1050;
                else
                    Debug.Log("Corrigir Key Objetivo");
                if (CalculoEnergeticoDiario < GastoMetabolicoBasal)
                {
                    CalculoEnergeticoDiario = GastoMetabolicoBasal;
                }
                PlayerPrefs.SetInt("AceleraEmagrecimento", AceleraEmagrecimento);
                break;
            case 4:
                if (m_Objective == "perder")
                    CalculoEnergeticoDiario = CalculoEnergeticoDiario - 750;
                else if (m_Objective == "manter")
                    CalculoEnergeticoDiario = CalculoEnergeticoDiario - 1000;
                else if (m_Objective == "ganhar")
                    CalculoEnergeticoDiario = CalculoEnergeticoDiario - 1350;
                if (CalculoEnergeticoDiario < GastoMetabolicoBasal)
                {
                    CalculoEnergeticoDiario = GastoMetabolicoBasal;
                }
                PlayerPrefs.SetInt("AceleraEmagrecimento", AceleraEmagrecimento);
                break;
        }
    }

    public void GetValorCaloricoInicial()
    {
        m_CalculoEnergeticoDiario = PlayerPrefs.GetFloat("SetValorEnergeticoTotal");
        m_GastoMetabolicoBasal = PlayerPrefs.GetFloat("SetGastoMetabolicoBasal");
        SetDirty();
        PlayerPrefs.Save();
    }

    void Start()
    {
        SetValorEnergeticoTotal();
        GetValorCaloricoInicial();
        SetPercentTodasAsRefeicoes();
        EmagrecimentoAcelerado();
        SalvaInfoGerenciadorDietas();
        SalvaInfoIniciais();
    }

    #endregion
}
