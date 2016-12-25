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
            _isDirty = false;
            if (OnValueChanged != null)
                OnValueChanged();
        }
    }

    #endregion

    #region Helper Functions

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
        EmagrecimentoAcelerado();
        SalvaInfoIniciais();
    }

    #endregion
}
