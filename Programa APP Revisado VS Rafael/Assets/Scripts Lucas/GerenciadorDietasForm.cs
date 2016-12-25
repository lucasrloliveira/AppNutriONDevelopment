using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GerenciadorDietasForm : MonoBehaviour {

    [SerializeField]
    Dropdown CafeManhaPeso;
    [SerializeField]
    Dropdown ColacaoPeso;
    [SerializeField]
    Dropdown AlmocoPeso;
    [SerializeField]
    Dropdown LancheTardePeso;
    [SerializeField]
    Dropdown JantarPeso;
    [SerializeField]
    Dropdown CeiaPeso;
    [SerializeField]
    GameObject ColacaoIcone;
    [SerializeField]
    GameObject LancheTardeIcone;
    [SerializeField]
    GameObject CeiaIcone;


    #region Unity Functions

    protected virtual void OnEnable()
    {
        //Registra Callback do manager
        RefeicaoManager.OnValueChanged += MyOnValueChanged;
    }

    protected virtual void OnDisable()
    {
        //Desregistra Callback do manager
        RefeicaoManager.OnValueChanged -= MyOnValueChanged;
    }

    protected virtual void Update()
    {
        if (_isDirty)
        {
            _isDirty = false;
        }
    }

    #endregion

    #region Receivers

    public virtual void MyOnValueChanged()
    {
        SetDirty();
    }

    #endregion

    #region Helper Functions

    bool _isDirty = false;
    public void SetDirty()
    {
        _isDirty = true;
    }

    public void SetaCafeManhaPercent()
    {
        if (CafeManhaPeso != null)
        {
            foreach (var v_prefixoPorPercentual in RefeicaoManager.Instance.PrefixosPorPercentual)
            {
                if (v_prefixoPorPercentual.Prefixo.Contains("CafeManha"))
                {
                    if (CafeManhaPeso.value == 0)
                    {
                        v_prefixoPorPercentual.Percentual = 0.1f;
                    }
                    else if (CafeManhaPeso.value == 1)
                    {
                        v_prefixoPorPercentual.Percentual = 0.15f;
                    }
                    else if (CafeManhaPeso.value == 2)
                    {
                        v_prefixoPorPercentual.Percentual = 0.2f;
                    }
                }
                else { break; }
                    Debug.Log("Executou?");
                    PlayerPrefs.SetInt("CafeManhaPeso", CafeManhaPeso.value);
            }
        }
    }
    

    public void SetaColacaoPercent()
    {
        foreach (var v_prefixoPorPercentual in RefeicaoManager.Instance.PrefixosPorPercentual)
        {
            if (v_prefixoPorPercentual.Prefixo.Contains("Colacao"))
            {

                if (ColacaoPeso.value == 0)
                {
                    v_prefixoPorPercentual.Percentual = 0.0f;
                    ColacaoIcone.SetActive(false);
                    PlayerPrefs.SetString("StateColacao", "ColacaoOFF");
                }
                else if (ColacaoPeso.value == 1)
                {
                    v_prefixoPorPercentual.Percentual = 0.08f;
                    ColacaoIcone.SetActive(true);
                    PlayerPrefs.SetString("StateColacao", "ColacaoON");
                }
                else if (ColacaoPeso.value == 2)
                {
                    v_prefixoPorPercentual.Percentual = 0.1f;
                    ColacaoIcone.SetActive(true);
                    PlayerPrefs.SetString("StateColacao", "ColacaoON");
                }
                else if (ColacaoPeso.value == 3)
                {
                    v_prefixoPorPercentual.Percentual = 0.12f;
                    ColacaoIcone.SetActive(true);
                    PlayerPrefs.SetString("StateColacao", "ColacaoON");
                }
                PlayerPrefs.SetInt("ColacaoPeso", ColacaoPeso.value);
            }
        }
        PlayerPrefs.Save();
    }

    public void SetaAlmocoPercent()
    {
        foreach (var v_prefixoPorPercentual in RefeicaoManager.Instance.PrefixosPorPercentual)
        {
            if (v_prefixoPorPercentual.Prefixo.Contains("Almoco"))
            {

                if (AlmocoPeso.value == 0)
                {
                    v_prefixoPorPercentual.Percentual = 0.2f;
                }
                else if (AlmocoPeso.value == 1)
                {
                    v_prefixoPorPercentual.Percentual = 0.3f;
                }
                else if (AlmocoPeso.value == 2)
                {
                    v_prefixoPorPercentual.Percentual = 0.4f;
                }
                PlayerPrefs.SetInt("AlmocoPeso", AlmocoPeso.value);
            }
        } 
        PlayerPrefs.Save();
    }

    public void SetaLancheTardePercent()
    {
        foreach (var v_prefixoPorPercentual in RefeicaoManager.Instance.PrefixosPorPercentual)
        {
            if (v_prefixoPorPercentual.Prefixo.Contains("LancheTarde"))
            {

                if (LancheTardePeso.value == 0)
                {
                    v_prefixoPorPercentual.Percentual = 0;
                    LancheTardeIcone.SetActive(false);
                    PlayerPrefs.SetString("StateLancheTarde", "LancheTardeOFF");
                    PlayerPrefs.SetInt("LancheTardePeso", LancheTardePeso.value);
                }
                else if (LancheTardePeso.value == 1)
                {
                    v_prefixoPorPercentual.Percentual = 0.067f;
                    LancheTardeIcone.SetActive(true);
                    PlayerPrefs.SetString("StateLancheTarde", "LancheTardeON");
                    PlayerPrefs.SetInt("LancheTardePeso", LancheTardePeso.value);
                }
                else if (LancheTardePeso.value == 2)
                {
                    v_prefixoPorPercentual.Percentual = 0.133f;
                    LancheTardeIcone.SetActive(true);
                    PlayerPrefs.SetString("StateLancheTarde", "LancheTardeON");
                    PlayerPrefs.SetInt("LancheTardePeso", LancheTardePeso.value);
                }
                else if (LancheTardePeso.value == 3)
                {
                    v_prefixoPorPercentual.Percentual = 0.2f;
                    LancheTardeIcone.SetActive(true);
                    PlayerPrefs.SetString("StateLancheTarde", "LancheTardeON");
                    PlayerPrefs.SetInt("LancheTardePeso", LancheTardePeso.value);
                }
            }
        }
        PlayerPrefs.Save();
    }

    public void SetaJantarPercent()
    {
        foreach (var v_prefixoPorPercentual in RefeicaoManager.Instance.PrefixosPorPercentual)
        {
            if (v_prefixoPorPercentual.Prefixo.Contains("Jantar"))
            {

                if (JantarPeso.value == 0)
                {
                    v_prefixoPorPercentual.Percentual = 0.15f;
                    PlayerPrefs.SetInt("JantarPeso", JantarPeso.value);
                }
                else if (JantarPeso.value == 1)
                {
                    v_prefixoPorPercentual.Percentual = 0.2f;
                    PlayerPrefs.SetInt("JantarPeso", JantarPeso.value);
                }
                else if (JantarPeso.value == 2)
                {
                    v_prefixoPorPercentual.Percentual = 0.25f;
                    PlayerPrefs.SetInt("JantarPeso", JantarPeso.value);
                }
            }
        }
        PlayerPrefs.Save();
    }

    public void SetaCeiaPercent()
    {
        foreach (var v_prefixoPorPercentual in RefeicaoManager.Instance.PrefixosPorPercentual)
        {
            if (v_prefixoPorPercentual.Prefixo.Contains("Ceia"))
            {

                if (CeiaPeso.value == 0)
                {
                    v_prefixoPorPercentual.Percentual = 0;
                    CeiaIcone.SetActive(false);
                    PlayerPrefs.SetString("StateCeia", "CeiaOFF");
                    PlayerPrefs.SetInt("CeiaPeso", CeiaPeso.value);
                }
                else if (CeiaPeso.value == 0)
                {
                    v_prefixoPorPercentual.Percentual = 0.05f;
                    CeiaIcone.SetActive(true);
                    PlayerPrefs.SetString("StateCeia", "CeiaON");
                    PlayerPrefs.SetInt("CeiaPeso", CeiaPeso.value);
                }
                else if (CeiaPeso.value == 1)
                {
                    v_prefixoPorPercentual.Percentual = 0.10f;
                    CeiaIcone.SetActive(true);
                    PlayerPrefs.SetString("StateCeia", "CeiaON");
                    PlayerPrefs.SetInt("CeiaPeso", CeiaPeso.value);
                }
                else if (CeiaPeso.value == 2)
                {
                    v_prefixoPorPercentual.Percentual = 0.15f;
                    CeiaIcone.SetActive(true);
                    PlayerPrefs.SetString("StateCeia", "CeiaON");
                    PlayerPrefs.SetInt("CeiaPeso", CeiaPeso.value);
                }
            }
            PlayerPrefs.Save();
        }
    }


    //SALVA INFO DROPDOWN GERENCIADOR DIETAS

    public void SalvaInfoGerenciadorDietasDropdown()
    {
        if (PlayerPrefs.HasKey("CafeManhaPeso") || PlayerPrefs.HasKey("ColacaoPeso") || PlayerPrefs.HasKey("AlmocoPeso") || PlayerPrefs.HasKey("LancheTardePeso") || PlayerPrefs.HasKey("JantarPeso") || PlayerPrefs.HasKey("CeiaPeso"))
        {
            CafeManhaPeso.value = PlayerPrefs.GetInt("CafeManhaPeso");
            ColacaoPeso.value = PlayerPrefs.GetInt("ColacaoPeso");
            AlmocoPeso.value = PlayerPrefs.GetInt("AlmocoPeso");
            LancheTardePeso.value = PlayerPrefs.GetInt("LancheTardePeso");
            JantarPeso.value = PlayerPrefs.GetInt("JantarPeso");
            CeiaPeso.value = PlayerPrefs.GetInt("CeiaPeso");
        }

        else
        {

            CafeManhaPeso.value = 1;
            PlayerPrefs.SetInt("CafeManhaPeso", CafeManhaPeso.value);

            ColacaoPeso.value = 2;
            PlayerPrefs.SetInt("ColacaoPeso", ColacaoPeso.value);


            AlmocoPeso.value = 1;
            PlayerPrefs.SetInt("AlmocoPeso", AlmocoPeso.value);

            LancheTardePeso.value = 2;
            PlayerPrefs.SetInt("LancheTardePeso", LancheTardePeso.value);

            JantarPeso.value = 1;
            PlayerPrefs.SetInt("JantarPeso", JantarPeso.value);

            CeiaPeso.value = 2;
            PlayerPrefs.SetInt("CeiaPeso", CeiaPeso.value);
        }
        PlayerPrefs.Save();
    }

    //REGIAO PARA SALVAR ICONES DE REFEICOES REMOVIDAS

    public void SalvaIconesRefeicaoRemovidos()
    {
        if (PlayerPrefs.HasKey("StateColacao"))
        {
            if (PlayerPrefs.GetString("StateColacao") == "ColacaoON")
                ColacaoIcone.SetActive(true);
            else if (PlayerPrefs.GetString("StateColacao") == "ColacaoOFF")
                ColacaoIcone.SetActive(false);
            else
                Debug.Log("Ta errado a key da colacao");
        }

        if (PlayerPrefs.HasKey("StateLancheTarde"))
        {
            if (PlayerPrefs.GetString("StateLancheTarde") == "LancheTardeON")
                LancheTardeIcone.SetActive(true);
            else if (PlayerPrefs.GetString("StateLancheTarde") == "LancheTardeOFF")
                LancheTardeIcone.SetActive(false);
            else
                Debug.Log("Ta errado a key do lanche da tarde");
        }

        if (PlayerPrefs.HasKey("StateCeia"))
        {
            if (PlayerPrefs.GetString("StateCeia") == "CeiaON")
                CeiaIcone.SetActive(true);
            else if (PlayerPrefs.GetString("StateCeia") == "CeiaOFF")
                CeiaIcone.SetActive(false);
            else
                Debug.Log("Ta errado a key da ceia");
        }
        PlayerPrefs.Save();
    }

    void Start()
    {
        SalvaIconesRefeicaoRemovidos();
        SalvaInfoGerenciadorDietasDropdown();
    }

    #endregion
}
