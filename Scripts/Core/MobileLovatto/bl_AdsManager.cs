using UnityEngine;
using System.Collections;
using UnityEngine.Advertisements;

public class bl_AdsManager : Singleton<bl_AdsManager> {

    [SerializeField,Range(0,10)]private int ShowOnMatch = 3;
    [SerializeField]private string GameID;

    private int currentMathc = 0;

	void Awake()
    {
        Advertisement.debugLevel = Advertisement.DebugLevel.Error;
        if (!Advertisement.isInitialized)
        {
            Advertisement.Initialize(GameID);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public void ShowUnityADS()
    {
        if (Advertisement.IsReady())
        {
            Advertisement.Show();
        }
    }

    public void AddMatch()
    {
        currentMathc++;
        if(currentMathc >= ShowOnMatch)
        {
            currentMathc = 0;
            StartCoroutine(WaitForShow());
        }
    }

    IEnumerator WaitForShow()
    {
        yield return new WaitForSeconds(1);
        ShowUnityADS();
    }
    /// <summary>
    /// 
    /// </summary>
    public static bl_AdsManager Instance
    {
        get
        {
            return ((bl_AdsManager)mInstance);
        }
        set
        {
            mInstance = value;
        }
    }
}