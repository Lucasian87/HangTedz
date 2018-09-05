using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Advertisements;

public class bl_MenuManager : MonoBehaviour {

	[Header("Settings")]
    [SerializeField]private string GameID = "81072";
    [Header("References")]
    [SerializeField]private AudioClip ChangePage;
    [SerializeField]private GameObject[] MenuPanels;
    [SerializeField]private Animator PanelAnimator;
    [SerializeField]private GameObject GameInfoPrefab;
    [SerializeField]private GameObject AdmobPrefab;
    [SerializeField]private GameObject GoButton;
    [SerializeField]private GameObject GoPlayerNameButton;
    [SerializeField]private Animator LoadAnimator;
    [SerializeField]private Text AudioEnableText;
    [SerializeField]private Text VibrateText;
    [SerializeField]private Slider VolumenSlider;
    [SerializeField]private Text MaxRoundsText;

    private string _player1;
    private string _player2;
    private bool FirtsLoad = true;

    /// <summary>
    /// 
    /// </summary>
    void Awake()
    {
        if(bl_GameInfo.Instance == null)
        {
            Instantiate(GameInfoPrefab);
        }
        if(GameObject.Find(bl_AdMod.NAMEGO) == null)
        {
            Instantiate(AdmobPrefab);
        }
    }

    void Start()
    {
        if (Advertisement.isSupported && !Advertisement.isInitialized)
        {
            Advertisement.Initialize(GameID);
        }

        ChangeWindow(0);
        GoButton.SetActive(false);
        GoPlayerNameButton.SetActive(false);
        VolumenSlider.value = PlayerPrefs.GetFloat(bl_GameInfo.VolumenKey, 1);
        AudioEnableText.text = (PlayerPrefs.GetInt(bl_GameInfo.AudioKey, 1) == 1) ? "ON" : "OFF";
        VibrateText.text = (PlayerPrefs.GetInt(bl_GameInfo.VibrateKey, 1) == 1) ? "ON" : "OFF";
        MaxRoundsText.text = PlayerPrefs.GetInt(bl_GameInfo.RoundsKey, 7).ToString();
    }

    public void ButtonGroup(Button b)
    {
        Button[] bts = b.transform.parent.GetComponentsInChildren<Button>();
        foreach(Button bb in bts) { bb.interactable = true; }
        b.interactable = false;
    }

    public void ChangeWindow(int id)
    {
        AudioSource.PlayClipAtPoint(ChangePage, transform.position); 
        if (!FirtsLoad)
        {
            PanelAnimator.Play("Change", 0, 0);
            StartCoroutine(WaitForChange(id));
        }
        else
        {
            for (int i = 0; i < MenuPanels.Length; i++)
            {
                MenuPanels[i].SetActive(false);
            }
            MenuPanels[id].SetActive(true);
            FirtsLoad = false;
        }
    }

    public void Volumen(float v)
    {
        bl_GameInfo.Instance.Volumen = v;
        PlayerPrefs.SetFloat(bl_GameInfo.VolumenKey, v);
    }

    public void GameMode(int typ)
    {
        bl_GameInfo.Instance.GameMode = (Mode)typ;
        ChangeWindow(3);
    }

    public void Category(int mod)
    {
        bl_GameInfo.Instance.Category = (DifficultMode)mod;
        if (bl_GameInfo.Instance.GameMode == Mode.SinglePlayer)
        {
            GoButton.SetActive(true);
        }
        else
        {
            ChangeWindow(5);
        }
    }
    public void CallRate() { bl_GameInfo.Instance.RateApp(); }

    public void ActiveAudio(bool b)
    {
        bl_GameInfo.Instance.Audio = b;
        AudioEnableText.text = (b) ? "ON" : "OFF";
        PlayerPrefs.SetInt(bl_GameInfo.AudioKey, (b) ? 1 : 0);
    }

    public void Vibrate(bool b)
    {
        bl_GameInfo.Instance.UseVibrate = b;
        VibrateText.text = (b) ? "ON" : "OFF";
        PlayerPrefs.SetInt(bl_GameInfo.VibrateKey, (b) ? 1 : 0);
    }

    public void OnExit()
    {
        Application.Quit();
    }

    public void TwoPlayerRounds(bool b)
    {
        if (b)
        {
            if (bl_GameInfo.Instance.TwoPlayerMaxRounds < 99)
            {
                bl_GameInfo.Instance.TwoPlayerMaxRounds++;
            }
        }
        else
        {
            if (bl_GameInfo.Instance.TwoPlayerMaxRounds > 1)
            {
                bl_GameInfo.Instance.TwoPlayerMaxRounds--;
            }
        }
        MaxRoundsText.text = bl_GameInfo.Instance.TwoPlayerMaxRounds.ToString();
        PlayerPrefs.SetInt(bl_GameInfo.RoundsKey, bl_GameInfo.Instance.TwoPlayerMaxRounds);
    }

    public void GoPlay()
    {
        LoadAnimator.gameObject.SetActive(true);
        StartCoroutine(WaitForLoad());
    }

    public void SetNames()
    {
        if(string.IsNullOrEmpty(_player1) || string.IsNullOrEmpty(_player2))
        {
            Debug.LogWarning("Players names can't be empty!");
            return;
        }
        bl_GameInfo.Instance.Player1 = _player1;
        bl_GameInfo.Instance.Player2 = _player2;
        GoPlayerNameButton.SetActive(true);
    }

    public void Player1Name(InputField f) { _player1 = f.text; }
    public void Player2Name(InputField f) { _player2 = f.text; }

    IEnumerator WaitForLoad()
    {
        float w = LoadAnimator.GetCurrentAnimatorClipInfo(0).Length;
        yield return new WaitForSeconds(w);
        Application.LoadLevel("Game");
    }

    IEnumerator WaitForChange(int id)
    {
        yield return new WaitForSeconds(0.4f);
        AudioSource.PlayClipAtPoint(ChangePage, transform.position);
        for (int i = 0; i < MenuPanels.Length; i++)
        {
            MenuPanels[i].SetActive(false);
        }
        MenuPanels[id].SetActive(true);
    }

   public void OpenUrl(string url)
    {
        Application.OpenURL(url);
    }
}