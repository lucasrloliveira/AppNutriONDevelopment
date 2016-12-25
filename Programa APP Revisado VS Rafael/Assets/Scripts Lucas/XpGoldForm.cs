using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class XpGoldForm : MonoBehaviour {

    #region Private Variables

    [SerializeField]
    Text m_levelText;
    [SerializeField]
    Image m_xpFillAmount;
    [SerializeField]
    Text m_goldText;
    [SerializeField]
    GameObject m_ChefinhoOficial;
    [SerializeField]
    PopupUI _PopupUINaoTemGold;

    [SerializeField]
    Toggle AtivaPropaganda;
    [SerializeField]
    GameObject BonusPropaganda;


    public void CompraChefinho()
    {
        if (XpGoldManager.Instance.Gold > 10000)
        {
            Debug.Log("Você adquiriu o chefinho!!");
            XpGoldManager.Instance.Gold -= 10000;
            m_ChefinhoOficial.SetActive(true);
            PlayerPrefs.SetInt("gold", XpGoldManager.Instance.Gold);
        }
        else
        {
            _PopupUINaoTemGold.Show();
        }
    }

    #endregion

    #region Unity Functions

    protected virtual void OnEnable()
    {
        //Registra Callback do manager
        XpGoldManager.OnValueChanged += MyOnValueChanged;
        ApplyManagerInformations();
    }

    protected virtual void OnDisable()
    {
        //Desregistra Callback do manager
        XpGoldManager.OnValueChanged -= MyOnValueChanged;
    }

    protected virtual void Update()
    {
        if (_isDirty)
        {
            _isDirty = false;     
            ApplyManagerInformations();
        }
        if (!AtivaPropaganda.isOn)
        {
            BonusPropaganda.SetActive(false);
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
        if (m_xpFillAmount != null)
            m_xpFillAmount.fillAmount = XpGoldManager.Instance.CurrentXp/XpGoldManager.Instance.MaximumXp;
        if (m_levelText != null)
            m_levelText.text = XpGoldManager.Instance.Level.ToString();
        if (m_goldText != null)
            m_goldText.text = XpGoldManager.Instance.Gold.ToString();

    }

    public void AtivaEDesativaPropaganda()
    {
        if (AtivaPropaganda.isOn)
            PlayerPrefs.SetString("PropagandaAtivada", "PropagandaAtivada");
        else
            PlayerPrefs.SetString("PropagandaAtivada", "PropagandaDesativada");
    }

    public void SalvaInfoPropagandaAtivada()
    {
        //VERIFICAR PORQUE NAO FUNCIONA
        string PropagandaAtivada = "PropagandaAtivada";

        if (PlayerPrefs.HasKey("PropagandaAtivada"))
        {
            PropagandaAtivada = PlayerPrefs.GetString("PropagandaAtivada");
            if (PropagandaAtivada == "PropagandaAtivada")
            {
                Debug.Log("A propaganda esta ativada");
                AtivaPropaganda.isOn = true;
            }
            else if (PropagandaAtivada == "PropagandaDesativada")
            {
                Debug.Log("A propaganda está desativada");
                AtivaPropaganda.isOn = false;
            }
            else
            {
                Debug.Log("Voce fez merda no toggle da propaganda");
            }
        }
        else
        {
            PlayerPrefs.SetString("PropagandaAtivada", PropagandaAtivada);
        }
    }

    void Start()
    {
        SalvaInfoPropagandaAtivada();
    }

    #endregion

}
