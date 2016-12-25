﻿using UnityEngine;
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

    [SerializeField]
    float v_cafeManhaKcal;
    [SerializeField]
    float v_colacaoKcal;
    [SerializeField]
    float v_almocoKcal;
    [SerializeField]
    float v_lancheTardeKcal;
    [SerializeField]
    float v_jantarKcal;
    [SerializeField]
    float v_ceiaKcal;

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
            SetInformationsInManager();
        }
    }

    #endregion

    #region SetInformationsInManager

    //Joga de volta pro manager
    public virtual void SetInformationsInManager()
    {
        if (CafeManhaPeso != null)
        {
            RefeicaoManager.Instance.CafeDaManhaKcal = v_cafeManhaKcal;
        }
        if (ColacaoPeso != null)
        {
            RefeicaoManager.Instance.ColacaoKcal = v_colacaoKcal;
        }
        if (AlmocoPeso != null)
        {
            RefeicaoManager.Instance.AlmocoKcal = v_almocoKcal;
        }
        if (LancheTardePeso != null)
        {
            RefeicaoManager.Instance.LancheTardeKcal = v_lancheTardeKcal;
        }
        if (JantarPeso != null)
        {
            RefeicaoManager.Instance.JantarKcal = v_jantarKcal;
        }
        if (CeiaPeso != null)
        {
            RefeicaoManager.Instance.CeiaKcal = v_ceiaKcal;
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
            if (CafeManhaPeso.value == 0)
            {
                v_cafeManhaKcal = 0.1f;
                RefeicaoManager.Instance.CafeDaManhaKcal = v_cafeManhaKcal;
                Debug.Log("kcal cafe manha" + RefeicaoManager.Instance.CafeDaManhaKcal);
                SetDirty();
            }
            else if (CafeManhaPeso.value == 1)
            {
                v_cafeManhaKcal = 0.15f;
                RefeicaoManager.Instance.CafeDaManhaKcal = v_cafeManhaKcal;
                Debug.Log("kcal cafe manha" + RefeicaoManager.Instance.CafeDaManhaKcal);
                SetDirty();
            }
            else if (CafeManhaPeso.value == 2)
            {
                v_cafeManhaKcal = 0.2f;
                RefeicaoManager.Instance.CafeDaManhaKcal = v_cafeManhaKcal;
                Debug.Log("kcal cafe manha" + RefeicaoManager.Instance.CafeDaManhaKcal);
                SetDirty();
            }

            PlayerPrefs.SetInt("CafeManhaPeso", CafeManhaPeso.value);
        }
    }

    public void SetaColacaoPercent()
    {
        if (ColacaoPeso != null)
        {
            if (ColacaoPeso.value == 0)
            {
                v_colacaoKcal = 0f;
                RefeicaoManager.Instance.ColacaoKcal = v_colacaoKcal;
                ColacaoIcone.SetActive(false);
                PlayerPrefs.SetString("StateColacao", "ColacaoOFF");
                SetDirty();
            }
            else if (ColacaoPeso.value == 1)
            {
                v_colacaoKcal = 0.08f;
                RefeicaoManager.Instance.ColacaoKcal = v_colacaoKcal;
                ColacaoIcone.SetActive(true);
                PlayerPrefs.SetString("StateColacao", "ColacaoON");
                SetDirty();
            }
            else if (ColacaoPeso.value == 2)
            {
                v_colacaoKcal = 0.1f;
                RefeicaoManager.Instance.ColacaoKcal = v_colacaoKcal;
                ColacaoIcone.SetActive(true);
                PlayerPrefs.SetString("StateColacao", "ColacaoON");
                SetDirty();
            }
            else if (ColacaoPeso.value == 3)
            {
                v_colacaoKcal = 1.2f;
                RefeicaoManager.Instance.ColacaoKcal = v_colacaoKcal;
                ColacaoIcone.SetActive(true);
                PlayerPrefs.SetString("StateColacao", "ColacaoON");
                SetDirty();
            }
            PlayerPrefs.SetInt("ColacaoPeso", ColacaoPeso.value);
        }
        PlayerPrefs.Save();
    }

    public void SetaAlmocoPercent()
    {
        if (AlmocoPeso != null)
        {

            if (AlmocoPeso.value == 0)
            {
                v_almocoKcal = 0.2f;
                RefeicaoManager.Instance.AlmocoKcal = v_almocoKcal;
                SetDirty();
            }
            else if (AlmocoPeso.value == 1)
            {
                v_almocoKcal = 0.3f;
                RefeicaoManager.Instance.AlmocoKcal = v_almocoKcal;
                SetDirty();
            }
            else if (AlmocoPeso.value == 2)
            {
                v_almocoKcal = 0.4f;
                RefeicaoManager.Instance.AlmocoKcal = v_almocoKcal;
                SetDirty();
            }
            PlayerPrefs.SetInt("AlmocoPeso", AlmocoPeso.value);
        }
        PlayerPrefs.Save();
    }

    public void SetaLancheTardePercent()
    {
        if (LancheTardePeso != null)
        {

            if (LancheTardePeso.value == 0)
            {
                v_lancheTardeKcal = 0;
                RefeicaoManager.Instance.LancheTardeKcal = v_lancheTardeKcal;
                LancheTardeIcone.SetActive(false);
                PlayerPrefs.SetString("StateLancheTarde", "LancheTardeOFF");
                PlayerPrefs.SetInt("LancheTardePeso", LancheTardePeso.value);
                SetDirty();
            }
            else if (LancheTardePeso.value == 1)
            {
                v_lancheTardeKcal = 0.067f;
                RefeicaoManager.Instance.LancheTardeKcal = v_lancheTardeKcal;
                LancheTardeIcone.SetActive(true);
                PlayerPrefs.SetString("StateLancheTarde", "LancheTardeON");
                PlayerPrefs.SetInt("LancheTardePeso", LancheTardePeso.value);
                SetDirty();
            }
            else if (LancheTardePeso.value == 2)
            {
                v_lancheTardeKcal = 0.133f;
                RefeicaoManager.Instance.LancheTardeKcal = v_lancheTardeKcal;
                LancheTardeIcone.SetActive(true);
                PlayerPrefs.SetString("StateLancheTarde", "LancheTardeON");
                PlayerPrefs.SetInt("LancheTardePeso", LancheTardePeso.value);
                SetDirty();
            }
            else if (LancheTardePeso.value == 3)
            {
                v_lancheTardeKcal = 0.2f;
                RefeicaoManager.Instance.LancheTardeKcal = v_lancheTardeKcal;
                LancheTardeIcone.SetActive(true);
                PlayerPrefs.SetString("StateLancheTarde", "LancheTardeON");
                PlayerPrefs.SetInt("LancheTardePeso", LancheTardePeso.value);
                SetDirty();
            }
        }
        PlayerPrefs.Save();
    }

    public void SetaJantarPercent()
    {
        if (JantarPeso != null)
        {

            if (JantarPeso.value == 0)
            {
                v_jantarKcal = 0.15f;
                RefeicaoManager.Instance.JantarKcal = v_jantarKcal;
                PlayerPrefs.SetInt("JantarPeso", JantarPeso.value);
                SetDirty();
            }
            else if (JantarPeso.value == 1)
            {
                v_jantarKcal = 0.2f;
                RefeicaoManager.Instance.JantarKcal = v_jantarKcal;
                PlayerPrefs.SetInt("JantarPeso", JantarPeso.value);
                SetDirty();
            }
            else if (JantarPeso.value == 2)
            {
                v_jantarKcal = 0.25f;
                RefeicaoManager.Instance.JantarKcal = v_jantarKcal;
                PlayerPrefs.SetInt("JantarPeso", JantarPeso.value);
                SetDirty();
            }
        }
        PlayerPrefs.Save();
    }

    public void SetaCeiaPercent()
    {
        if (CeiaPeso != null)
        {

            if (CeiaPeso.value == 0)
            {
                v_ceiaKcal = 0;
                RefeicaoManager.Instance.CeiaKcal = v_ceiaKcal;
                CeiaIcone.SetActive(false);
                PlayerPrefs.SetString("StateCeia", "CeiaOFF");
                PlayerPrefs.SetInt("CeiaPeso", CeiaPeso.value);
                SetDirty();
            }
            else if (CeiaPeso.value == 0)
            {
                v_ceiaKcal = 0.05f;
                RefeicaoManager.Instance.CeiaKcal = v_ceiaKcal;
                CeiaIcone.SetActive(true);
                PlayerPrefs.SetString("StateCeia", "CeiaON");
                PlayerPrefs.SetInt("CeiaPeso", CeiaPeso.value);
                SetDirty();
            }
            else if (CeiaPeso.value == 1)
            {
                v_ceiaKcal = 0.10f;
                RefeicaoManager.Instance.CeiaKcal = v_ceiaKcal;
                CeiaIcone.SetActive(true);
                PlayerPrefs.SetString("StateCeia", "CeiaON");
                PlayerPrefs.SetInt("CeiaPeso", CeiaPeso.value);
                SetDirty();
            }
            else if (CeiaPeso.value == 2)
            {
                v_ceiaKcal = 0.15f;
                RefeicaoManager.Instance.CeiaKcal = v_ceiaKcal;
                CeiaIcone.SetActive(true);
                PlayerPrefs.SetString("StateCeia", "CeiaON");
                PlayerPrefs.SetInt("CeiaPeso", CeiaPeso.value);
                SetDirty();
            }
        }
        PlayerPrefs.Save();
    }


    //SALVA INFO DROPDOWN GERENCIADOR DIETAS

    public void SalvaInfoGerenciadorDietasKcalPercentAndDropdown()
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
            RefeicaoManager.Instance.CafeDaManhaKcal = 0.15f;
            PlayerPrefs.SetFloat("CafeManhaValor", RefeicaoManager.Instance.CafeDaManhaKcal);

            ColacaoPeso.value = 2;
            PlayerPrefs.SetInt("ColacaoPeso", ColacaoPeso.value);
            RefeicaoManager.Instance.ColacaoKcal = 0.1f;
            PlayerPrefs.SetFloat("ColacaoValor", RefeicaoManager.Instance.ColacaoKcal);


            AlmocoPeso.value = 1;
            PlayerPrefs.SetInt("AlmocoPeso", AlmocoPeso.value);
            RefeicaoManager.Instance.AlmocoKcal = 0.3f;
            PlayerPrefs.SetFloat("AlmocoValor", RefeicaoManager.Instance.AlmocoKcal);

            LancheTardePeso.value = 2;
            PlayerPrefs.SetInt("LancheTardePeso", LancheTardePeso.value);
            RefeicaoManager.Instance.LancheTardeKcal = 0.15f;
            PlayerPrefs.SetFloat("LancheTardeValor", RefeicaoManager.Instance.LancheTardeKcal);

            JantarPeso.value = 1;
            PlayerPrefs.SetInt("JantarPeso", JantarPeso.value);
            RefeicaoManager.Instance.JantarKcal = 0.2f;
            PlayerPrefs.SetFloat("JantarValor", RefeicaoManager.Instance.JantarKcal);

            CeiaPeso.value = 2;
            PlayerPrefs.SetInt("CeiaPeso", CeiaPeso.value);
            RefeicaoManager.Instance.CeiaKcal = 0.1f;
            PlayerPrefs.SetFloat("CeiaValor", RefeicaoManager.Instance.CeiaKcal);
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
        SalvaInfoGerenciadorDietasKcalPercentAndDropdown();
    }
    #endregion
}
