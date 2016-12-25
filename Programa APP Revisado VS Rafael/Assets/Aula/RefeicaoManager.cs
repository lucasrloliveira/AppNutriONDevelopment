using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Aula;

public class RefeicaoManager : Kilt.Singleton<RefeicaoManager>
{
    #region Private Variables

    [SerializeField]
    List<Alimento> m_definicaoDeAlimentos;
    [SerializeField]
    List<RefeicaoPredefinida> m_refeicoesDefinitions = new List<RefeicaoPredefinida>();
    [SerializeField]
    string[] m_prefixosPorPercentual = new string[] { "CafeManha", "Colacao", "Almoco" ,"LancheTarde", "Jantar", "Ceia"};
    float[] m_percentuaisRefeicoes = new float[] { CalculoEnergeticoManager.Instance.CafeDaManhaKcal, CalculoEnergeticoManager.Instance.ColacaoKcal, CalculoEnergeticoManager.Instance.AlmocoKcal, CalculoEnergeticoManager.Instance.LancheTardeKcal, CalculoEnergeticoManager.Instance.JantarKcal, CalculoEnergeticoManager.Instance.CeiaKcal };

    [SerializeField]
    float m_kiloCaloriasTotais = 0;

    //SerializeField para as variáveis que irão aparecer no Inspector
    //Variaveis com m_: variáveis do manager.

    #endregion

    #region Public Properties

    //Properties: get: executado quando a propriedade é lida, set: executado quando se deseja atribuir um novo valor para a propriedade
    //properties nao nao variaveis, portanto nao podem ser referenciadas.
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

