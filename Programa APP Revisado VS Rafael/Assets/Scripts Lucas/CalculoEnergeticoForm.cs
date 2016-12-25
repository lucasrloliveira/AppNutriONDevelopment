using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CalculoEnergeticoForm : MonoBehaviour {

    #region Private Variables
    //Variáveis presentes para setar os valores iniciais do usuário
    [SerializeField]
    Text ExibePeso;
    [SerializeField]
    Text ExibeIdade;
    [SerializeField]
    Text ExibeAltura;
    [SerializeField]
    Slider PesoSlider;
    [SerializeField]
    Slider IdadeSlider;
    [SerializeField]
    Slider AlturaSlider;


    //Dropdown presente no Perfil do usuário, que determina se o usuario irá emagrecer 1,2,3 ou 4 kg/mes
    [SerializeField]
    Dropdown AceleraEmagrecimento;

    #endregion


    #region Public Properties



    #endregion

    #region Unity Functions

    protected virtual void OnEnable()
    {
        //Registra Callback do manager
        CalculoEnergeticoManager.OnValueChanged += MyOnValueChanged;
        ApplyManagerInformations();
    }

    protected virtual void OnDisable()
    {
        //Desregistra Callback do manager
        CalculoEnergeticoManager.OnValueChanged -= MyOnValueChanged;
    }

    protected virtual void Update()
    {
        if (_isDirty)
        {
            _isDirty = false;
            ApplyManagerInformations();
            SetInformationsInManager();
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

    //Pega informacoes do manager e aplica no form
    public virtual void ApplyManagerInformations()
    {
        if (ExibePeso != null)
            ExibePeso.text = CalculoEnergeticoManager.Instance.Peso.ToString();
        if (ExibeIdade != null)
            ExibeIdade.text = CalculoEnergeticoManager.Instance.Idade.ToString();
        if (ExibeAltura != null)
            ExibeAltura.text = CalculoEnergeticoManager.Instance.Altura.ToString();
        if (PesoSlider != null)
        {
            PesoSlider.minValue = 40;
            PesoSlider.maxValue = 150;
            PesoSlider.value = CalculoEnergeticoManager.Instance.Peso;
        }
        if (AlturaSlider != null)
        {
            AlturaSlider.minValue = 140;
            AlturaSlider.maxValue = 210;
            AlturaSlider.value = CalculoEnergeticoManager.Instance.Altura;
        }
        if (IdadeSlider != null)
        {
            IdadeSlider.minValue = 15;
            IdadeSlider.maxValue = 100;
            IdadeSlider.value = CalculoEnergeticoManager.Instance.Idade;
        }
        if (AceleraEmagrecimento != null)
        {
            int v_index = 0;
            if(CalculoEnergeticoManager.Instance.AceleraEmagrecimento < 4)
                v_index = CalculoEnergeticoManager.Instance.AceleraEmagrecimento;
            AceleraEmagrecimento.value = v_index;
        }
    }


    public virtual void OnManterClicked()
    {
        CalculoEnergeticoManager.Instance.Objective = "manter";
    }

    public virtual void OnPerderClicked()
    {
        CalculoEnergeticoManager.Instance.Objective = "perder";
    }

    public virtual void OnGanharClicked()
    {
        CalculoEnergeticoManager.Instance.Objective = "ganhar";
    }

    public virtual void OnMascClicked()
    {
        CalculoEnergeticoManager.Instance.Sex = "m";
    }

    public virtual void OnFemClicked()
    {
        CalculoEnergeticoManager.Instance.Sex = "f";
    }

    //Joga de volta pro manager
    public virtual void SetInformationsInManager()
    {
        if (PesoSlider != null)
        {
            CalculoEnergeticoManager.Instance.Peso = PesoSlider.value;
        }
        if (AlturaSlider != null)
        {
            CalculoEnergeticoManager.Instance.Altura = AlturaSlider.value;
        }
        if (IdadeSlider != null)
        {
            CalculoEnergeticoManager.Instance.Idade = IdadeSlider.value;
        }
        if (AceleraEmagrecimento != null)
        {
            CalculoEnergeticoManager.Instance.AceleraEmagrecimento = AceleraEmagrecimento.value;
        }
    }
    public void SetaInformacoesIniciais()
    {
        PlayerPrefs.DeleteKey("Sex");
        PlayerPrefs.DeleteKey("Objective");
        PlayerPrefs.DeleteKey("Peso");
        PlayerPrefs.DeleteKey("Idade");
        PlayerPrefs.DeleteKey("Altura");
    }
    #endregion
}
