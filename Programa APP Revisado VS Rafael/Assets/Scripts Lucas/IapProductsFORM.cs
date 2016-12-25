using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class IapProductsFORM : MonoBehaviour
{
    public StringUnityEvent OnPurchaseSucess;
    public StringUnityEvent OnPurchaseFailed;

    #region Unity Events

    protected virtual void OnEnable()
    {
        IAPManager.OnPurchaseSucessEvent += OnPurchaseSucessReceiver;
        IAPManager.OnPurchaseFailedEvent += OnPurchaseFailedReceiver;
    }

    protected virtual void OnDisable()
    {
        IAPManager.OnPurchaseSucessEvent -= OnPurchaseSucessReceiver;
        IAPManager.OnPurchaseFailedEvent -= OnPurchaseFailedReceiver;
    }

    #endregion

    #region Receivers

    protected virtual void OnPurchaseSucessReceiver(string p_key)
    {
        if (OnPurchaseSucess != null)
            OnPurchaseSucess.Invoke(p_key);
    }

    protected virtual void OnPurchaseFailedReceiver(string p_key)
    {
        if (OnPurchaseFailed != null)
            OnPurchaseFailed.Invoke(p_key);
    }

    #endregion

    #region Buy Functions

    public void CompraGold15000()
    {
        XpGoldManager.Instance.CompraGold15000();
    }

    public void CompraGold25000()
    {
        XpGoldManager.Instance.CompraGold25000();
    }

    #endregion
}

[System.Serializable]
public class StringUnityEvent : UnityEngine.Events.UnityEvent<string> { }