using UnityEngine;
using System.Collections;
using UnityEngine.Advertisements;

public class AdManager : MonoBehaviour
{
    public void ShowAd()
    {
        if(Advertisement.IsReady())
        Advertisement.Show("rewardedVideo",new ShowOptions() { resultCallback = HandleResult});

    }

    public void HandleResult(ShowResult result)
    {
        switch(result)
        {
            case ShowResult.Finished:
                Debug.Log("Usuário ganhou 50 moedas");
                XpGoldManager.Instance.CurrentXp += 250;
                XpGoldManager.Instance.Gold += 50;
                break;

            case ShowResult.Skipped:
                Debug.Log("Usuário não assistiu vídeo completo.");
                break;

            case ShowResult.Failed:
                Debug.Log("Usuário não conseguiu iniciar o vídeo.");
                break;
        }
    }
    
}
