using UnityEngine;
using System.Collections;

public class ChangeDesativaNotificacao : MonoBehaviour {

    public const string DESATIVA_NOTIFICACAO_KEY = "DesativaNotificacao";

    public virtual void TryChange(bool p_value)
    {
        PlayerPrefs.SetInt(DESATIVA_NOTIFICACAO_KEY, p_value ? 1 : 0);
    }
}
