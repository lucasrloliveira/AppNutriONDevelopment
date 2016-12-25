using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class XpGoldManager : Kilt.Singleton<XpGoldManager>
{
    public const string GOLD_15000_KEY = "15000gold";
    public const string GOLD_25000_KEY = "25000gold";

    public static event System.Action OnValueChanged;

    #region Private Variables
    [SerializeField]
    float m_currentXp = 0.0f;
    [SerializeField]
    float m_maximumXp = 500.0f;
    [SerializeField]
    int m_level = 1;
    [SerializeField]
    int m_gold = 25000;

    #endregion

    #region Properties

    public float CurrentXp
    {
        get { return m_currentXp; }
        set {
            if (m_currentXp == value)
                return;
            m_currentXp = value;
            SetDirty();
        }
    }
    public float MaximumXp
    {
        get { return m_maximumXp; }
        set
        {
            if (m_maximumXp == value)
                return;
            m_maximumXp = value;
            SetDirty();
        }
    }
    public int Level
    {
        get { return m_level; }
        set
        {
            if (m_level == value)
                return;
            m_level = value;
            SetDirty();
        }
    }
    public int Gold
    {
        get { return m_gold; }
        set
        {
            if (m_gold == value)
                return;
            m_gold = value;
            SetDirty();
        }
    }

    #endregion

    #region Unity Functions

    protected virtual void OnEnable()
    {
        RegisterProducts();
        IAPManager.OnPurchaseSucessEvent += OnPurchaseSucessReceived;
        IAPManager.OnPurchaseFailedEvent += OnPurchaseFailedReceived;
    }

    protected virtual void OnDisable()
    {
        IAPManager.OnPurchaseSucessEvent -= OnPurchaseSucessReceived;
        IAPManager.OnPurchaseFailedEvent -= OnPurchaseFailedReceived;
    }

    protected virtual void Update()
    {
        if (_isDirty)
        {
            LevelUP();
            _isDirty = false;
            if (OnValueChanged != null)
                OnValueChanged();
        }
    }

    #endregion

    #region Receivers

    public void OnPurchaseSucessReceived(string p_received)
    {
        if (p_received == GOLD_15000_KEY)
        {
            Gold += 15000;
        }
        else if (p_received == GOLD_25000_KEY)
        {
            Gold += 25000;
        }
    }

    public void OnPurchaseFailedReceived(string p_received)
    {
        Debug.Log("A compra nao foi efetuada");
    }

    #endregion

    #region Helper Functions

    public void RegisterProducts()
    {
        if (!IAPManager.Instance.ProductKeys.Contains(GOLD_15000_KEY))
            IAPManager.Instance.ProductKeys.Add(GOLD_15000_KEY);
        if (!IAPManager.Instance.ProductKeys.Contains(GOLD_25000_KEY))
            IAPManager.Instance.ProductKeys.Add(GOLD_25000_KEY);
    }

    bool _isDirty = false;
    public void SetDirty()
    {
        _isDirty = true;
    }

    //Local destinado para as funcionalidades
    public void LevelUP()
    {
        if (m_currentXp >= m_maximumXp)
        {

            m_level = m_level + 1;
            m_currentXp = 0;
            PlayerPrefs.GetInt("level", Level);
            PlayerPrefs.GetFloat("currentXP", CurrentXp);
            PlayerPrefs.GetFloat("maximumXP", MaximumXp);

            switch (m_level)
            {
                case 1:
                    m_maximumXp = 500;
                    break;
                case 2:
                    m_maximumXp = 1000;
                    break;
                case 3:
                    m_maximumXp = 1700;
                    break;
                case 4:
                    m_maximumXp = 2600;
                    break;
                case 5:
                    m_maximumXp = 3700;
                    break;
                case 6:
                    m_maximumXp = 5000;
                    break;
                case 7:
                    m_maximumXp = 6500;
                    break;
                case 8:
                    m_maximumXp = 8500;
                    break;
                case 9:
                    m_maximumXp = 11000;
                    break;
                case 10:
                    m_maximumXp = 13500;
                    break;
                case 11:
                    m_maximumXp = 16500;
                    break;
                case 12:
                    m_maximumXp = 19500;
                    break;
                case 13:
                    m_maximumXp = 23000;
                    break;
                case 14:
                    m_maximumXp = 26000;
                    break;
                case 15:
                    m_maximumXp = 29000;
                    break;
                case 16:
                    m_maximumXp = 32000;
                    break;
                case 17:
                    m_maximumXp = 35000;
                    break;
                case 18:
                    m_maximumXp = 38000;
                    break;
                case 19:
                    m_maximumXp = 41000;
                    break;
                case 20:
                    m_maximumXp = 44000;
                    break;
                case 21:
                    m_maximumXp = 47000;
                    break;
                case 22:
                    m_maximumXp = 50000;
                    break;
                case 23:
                    m_maximumXp = 53000;
                    break;
                case 24:
                    m_maximumXp = 56000;
                    break;
                case 25:
                    m_maximumXp = 59000;
                    break;
                case 26:
                    m_maximumXp = 62000;
                    break;
                case 27:
                    m_maximumXp = 65000;
                    break;
                case 28:
                    m_maximumXp = 68000;
                    break;
                case 29:
                    m_maximumXp = 71000;
                    break;
                case 30:
                    m_maximumXp = 74000;
                    break;
            }

            PlayerPrefs.SetInt("level", Level);
            PlayerPrefs.SetFloat("currentXP", CurrentXp);
            PlayerPrefs.SetFloat("maximumXP", MaximumXp);
        }
    }

    public virtual void OnBotaoRefeicaoClicked()
    {
        CurrentXp += 50;
        Gold += 10;
        PlayerPrefs.SetInt("gold", Gold);
        PlayerPrefs.SetFloat("currentXP", CurrentXp);
    }

    public void CompraGold15000()
    {
        IAPManager.Instance.BuyProductID(GOLD_15000_KEY);
        PlayerPrefs.SetInt("gold", Gold);
    }

    public void CompraGold25000()
    {
        IAPManager.Instance.BuyProductID(GOLD_25000_KEY);
        PlayerPrefs.SetInt("gold", Gold);
    }

    public void SalvaInfoXPGoldLevel()
    {
        if (PlayerPrefs.HasKey("gold") || PlayerPrefs.HasKey("currentXP") || PlayerPrefs.HasKey("maximumXP") || PlayerPrefs.HasKey("level"))
        {
            Gold = PlayerPrefs.GetInt("gold");
            Level = PlayerPrefs.GetInt("level");
            CurrentXp = PlayerPrefs.GetFloat("currentXP");
            MaximumXp = PlayerPrefs.GetFloat("maximumXP");
        }
        else
        {
            PlayerPrefs.SetInt("gold", Gold);
            PlayerPrefs.SetInt("level", Level);
            PlayerPrefs.SetFloat("currentXP", CurrentXp);
            PlayerPrefs.SetFloat("maximumXP", MaximumXp);
        }
    }

    void Start()
    {
        SalvaInfoXPGoldLevel();
    }

    #endregion
}