using UnityEngine;
using System.Collections;

public class CallPopupIfUserWants : MonoBehaviour
{
    public virtual void TryCall(PopupUI p_popupToCall)
    {
        if (p_popupToCall != null)
        {
            int v_value = !PlayerPrefs.HasKey(ChangeDesativaNotificacao.DESATIVA_NOTIFICACAO_KEY)? 1 : PlayerPrefs.GetInt(ChangeDesativaNotificacao.DESATIVA_NOTIFICACAO_KEY);
            if (v_value != 0)
                p_popupToCall.Show();
        }
    }
}
