using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Collections;

public class bl_GameManager : Singleton<bl_GameManager> {

    public string CurrentWord;
    [Header("Settings")]
    public DifficultMode m_DifficultMode = DifficultMode.EASY;
    public Mode m_Mode = Mode.SinglePlayer;
    [Range(2, 8)]
    public int MaxTrys = 8;
    [Range(0,8)]
    public int Trys = 8;
    [Header("Effects")]
    [SerializeField]private AudioSource FSXSource;
    [SerializeField] private AudioClip HitAudio;
    [SerializeField] private AudioClip SuccessAudio;
    [SerializeField] private AudioClip FailHitAudio;
    [SerializeField] private AudioClip FailAudio;
    [Header("References")]
	[SerializeField]private Image[] HangManParts;
    [SerializeField]private GameObject CharPrefab;
    [SerializeField]private GameObject SpacePrefab;
    [SerializeField]private GameObject CustomWordCreatorPanel;
    [SerializeField]private Transform WordPanel;
    [SerializeField]private Text TryText;
    [SerializeField]private Animator FlashSentenceAnim;
    [SerializeField]private Animator LoadAnimator;
    [SerializeField]private Rigidbody2D[] Rigid2D;
    [SerializeField]private Text WordCreatorLogText;
    [SerializeField]private InputField WordInput;
    [SerializeField]private InputField Clue1Input;
    [SerializeField]private InputField Clue2Input;
    [SerializeField]private Text Clue1Text;
    [SerializeField]private Text Clue2Text;
    [SerializeField]private Text WordCreatorInfoText;

    private List<bl_Char> AllChars = new List<bl_Char>();
    private List<bl_Char> cacheChars = new List<bl_Char>();
    private List<GameObject> cacheSpace = new List<GameObject>();
    private int CurrentPart = 0;
    private Vector2[] HangmanPartPositions;

    /// <summary>
    /// 
    /// </summary>
    void Awake()
    {
        m_DifficultMode = bl_GameInfo.Instance.Category;
        m_Mode = bl_GameInfo.Instance.GameMode;
    }

    /// <summary>
    /// 
    /// </summary>
    void Start()
    {
        if (bl_GameInfo.Instance.GameMode == Mode.SinglePlayer)
        {
            CreateWord();
        }
        else
        {
            OpenWordCreatorPanel();
        }
        Clue1Text.CrossFadeColor(new Color(0, 1, 0, 0), 0.01f, true, true);
        Clue2Text.CrossFadeColor(new Color(0, 1, 0, 0), 0.01f, true, true);
        foreach (Image im in HangManParts) { im.CrossFadeColor(new Color(1, 0, 0, 0), 0.01f, true, true); }
        StartCoroutine(DesactiveOnTime(LoadAnimator.gameObject));
        AudioListener.volume = bl_GameInfo.Instance.Volumen;
        AudioListener.pause = !bl_GameInfo.Instance.Audio;

        HangmanPartPositions = new Vector2[HangManParts.Length];
        for (int i = 0; i < HangManParts.Length; i++)
        {
            HangmanPartPositions[i] = HangManParts[i].rectTransform.anchoredPosition;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="k"></param>
    public bool NewSelect(KeyCode k)
    {
        string _char = k.ToString().ToLower();
        bool found = false;
        for (int i = 0; i < CurrentWord.Length; i++)
        {
            //If letter exist in the word
            if (CurrentWord[i].ToString().ToLower() == _char)
            {
                //show letter
                ActiveLetters(_char);
                bl_ScoreManager.Instance.SuccessLetter();
                found = true;
            }
        }

        //else if not exist, then descount a try.
        if (!found)
        {
            OnError();
            CheckProgress();
        }
        return found;
    }

    /// <summary>
    /// 
    /// </summary>
    public void OpenWordCreatorPanel()
    {

        WordCreatorInfoText.text = string.Format("Write here the secret word that  {0} must guess (prevents {1} look now), and adds the two clue about the word. <i>(lenght max 10)</i>", bl_GameInfo.Instance.CurrentPlayerTurn.ToUpper(), bl_GameInfo.Instance.CurrentPlayerTurn.ToUpper());
        WordCreatorLogText.CrossFadeAlpha(0, 0.1f, true);
        CustomWordCreatorPanel.SetActive(true);
        CustomWordCreatorPanel.GetComponent<Animator>().SetBool("show", true);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="word"></param>
    public void CreateCustomWord()
    {
        if (string.IsNullOrEmpty(WordInput.text))
        {
            WordCreatorLogText.text = "Please writte a word.";
            WordCreatorLogText.CrossFadeAlpha(1, 0.5f, true);
            return;
        }
        if(WordInput.text.Length < 3) { WordCreatorLogText.text = "Word must have at least 3 characters"; WordCreatorLogText.CrossFadeAlpha(1, 0.5f, true);return; }
        if(string.IsNullOrEmpty(Clue1Input.text) || string.IsNullOrEmpty(Clue2Input.text))
        {
            WordCreatorLogText.text = "Please enter both Clues";
            WordCreatorLogText.CrossFadeAlpha(1, 0.5f, true);
            return;
        }
        WordCreatorLogText.CrossFadeAlpha(0, 0.5f, true);
        //Pass test, then create word.
        CurrentWord = WordInput.text;
        CleanSentence();

        //Instances Letters and spaces
        for (int i = 0; i < CurrentWord.Length; i++)
        {
            if (CurrentWord.ToCharArray()[i] == ' ' || CurrentWord.ToCharArray()[i] == '\0')
            {
                GameObject spa = Instantiate(SpacePrefab) as GameObject;
                cacheSpace.Add(spa);
                spa.transform.SetParent(WordPanel, false);
            }
            else
            {
                GameObject cha = Instantiate(CharPrefab) as GameObject;
                cha.GetComponent<bl_Char>().GetInfo(CurrentWord[i].ToString());
                AllChars.Add(cha.GetComponent<bl_Char>());
                cacheChars.Add(cha.GetComponent<bl_Char>());
                cha.transform.SetParent(WordPanel, false);
            }
        }
        //Hide WordCreator
        CustomWordCreatorPanel.GetComponent<Animator>().SetBool("show", false);
        WordInput.text = string.Empty;
        StartCoroutine(DesactiveOnTime(CustomWordCreatorPanel));
    }

    /// <summary>
    /// 
    /// </summary>
    public void CreateWord()
    {
        if (bl_GameInfo.Instance.GameMode == Mode.SinglePlayer)
        {
            CurrentWord = WordsDataBase.GetWord(m_DifficultMode);
            CleanSentence();

            for (int i = 0; i < CurrentWord.Length; i++)
            {
                if (CurrentWord.ToCharArray()[i] == ' ' || CurrentWord.ToCharArray()[i] == '\0')
                {
                    GameObject spa = Instantiate(SpacePrefab) as GameObject;
                    cacheSpace.Add(spa);
                    spa.transform.SetParent(WordPanel, false);
                }
                else
                {
                    GameObject cha = Instantiate(CharPrefab) as GameObject;
                    cha.GetComponent<bl_Char>().GetInfo(CurrentWord[i].ToString());
                    AllChars.Add(cha.GetComponent<bl_Char>());
                    cacheChars.Add(cha.GetComponent<bl_Char>());
                    cha.transform.SetParent(WordPanel, false);
                }
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public void CleanSentence()
    {
        foreach (GameObject g in cacheSpace)
        {
            Destroy(g);
        }
        foreach(bl_Char c in cacheChars)
        {
            Destroy(c.gameObject);
        }
        foreach(bl_Char ac in AllChars)
        {
            Destroy(ac.gameObject);
        }
        cacheChars.Clear();
        cacheSpace.Clear();
        AllChars.Clear();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="letter"></param>
    void ActiveLetters(string letter)
    {
        for (int i = 0; i < AllChars.Count; i++)
        {
            if(AllChars[i].Char.ToLower() == letter.ToLower())
            {
                AllChars[i].ActiveLetter();
                AllChars.RemoveAt(i);
                FSXSource.clip = HitAudio;
                FSXSource.Play();
                CheckProgress();

            }
        }
    }

    public void ToMainMenu() { Application.LoadLevel("Menu"); }

    /// <summary>
    /// 
    /// </summary>
    void CheckProgress()
    {
        if(Trys <= 0)
        {
            FailedSentence();
            return;    
        }
        if(LettersLeft <= 0)
        {
            SuccessSentence();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    void SuccessSentence()
    {
        FlashSentenceAnim.enabled = true;
        FlashSentenceAnim.Play("Flash", 0, 0);
        bl_ScoreManager.Instance.SuccesSentence();
        FSXSource.clip = SuccessAudio;
        FSXSource.Play();
        bl_AdsManager.Instance.AddMatch();
    }

    /// <summary>
    /// 
    /// </summary>
    void CheckForClues()
    {
        if(Trys <= 5)
        {
            Clue1Text.text = string.Format("<color=#00DFFCFF>CLUE 1:</color> {0}", Clue1Input.text.ToUpper());
            Clue1Text.CrossFadeColor(Color.white, 0.75f, true, true);
        }
        if(Trys <= 2)
        {
            Clue2Text.text = string.Format("<color=#00DFFCFF>CLUE 2:</color> {0}", Clue2Input.text.ToUpper());
            Clue2Text.CrossFadeColor(Color.white, 0.75f, true, true);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    void FailedSentence()
    {
        bl_KeyboardManager.Instance.DesactiveAllKeys();
        FlashSentenceAnim.enabled = true;
        FlashSentenceAnim.Play("Flash", 0, 0);
        foreach (Rigidbody2D r in Rigid2D) { r.AddForce(new Vector2(700, 200), ForceMode2D.Force); }
        for (int i = 0; i < AllChars.Count; i++)
        {
            AllChars[i].Failed();
        }
        FSXSource.clip = FailAudio;
        FSXSource.Play();
        bl_AdsManager.Instance.AddMatch();
        bl_ScoreManager.Instance.FailSentence();
    }

    /// <summary>
    /// 
    /// </summary>
    public void OnError()
    {
        Trys--;
        if (HangManParts.Length - 1 >= CurrentPart)
        {
            HangManParts[CurrentPart].CrossFadeColor(Color.white, 1, true, true);
            CurrentPart++;
        }
        else { Debug.Log("Not have more part of hangman!"); }
        FSXSource.clip = FailHitAudio;
        FSXSource.Play();
        //if (bl_GameInfo.Instance.UseVibrate) { bl_HandleVibration.Vibrate(); }
        TryText.text = string.Format("<b>REMAIN:</b> {0}/{1}", Trys, MaxTrys);
        bl_ScoreManager.Instance.FailLetter();
        if(bl_GameInfo.Instance.GameMode == Mode.TwoPlayers) { CheckForClues(); }
    }

    /// <summary>
    /// 
    /// </summary>
    public void Reset()
    {
        Trys = MaxTrys;
        CurrentPart = 0;
        foreach(Image im in HangManParts) { im.CrossFadeColor(new Color(1, 0, 0, 0), 0.01f, true, true); }
        FlashSentenceAnim.enabled = false;
        bl_KeyboardManager.Instance.ResetKeys();
        bl_ScoreManager.Instance.Reset();
        Clue1Input.text = string.Empty;
        Clue2Input.text = string.Empty;
        Clue1Text.CrossFadeColor(new Color(0, 1, 0, 0), 0.01f, true, true);
        Clue2Text.CrossFadeColor(new Color(0, 1, 0, 0), 0.01f, true, true);
        Clue1Text.text = string.Empty;
        Clue2Text.text = string.Empty;

        for (int i = 0; i < HangManParts.Length; i++)
        {         
            HangManParts[i].rectTransform.anchoredPosition = HangmanPartPositions[i];
            HangManParts[i].GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            HangManParts[i].GetComponent<Rigidbody2D>().angularVelocity = 0;
        }

        if (bl_GameInfo.Instance.GameMode == Mode.SinglePlayer)
        {
            CreateWord();
        }
        else
        {
            OpenWordCreatorPanel();
        }
    }

    IEnumerator DesactiveOnTime(GameObject g)
    {
        yield return new WaitForSeconds(1);
        g.SetActive(false);
    }

    /// <summary>
    /// 
    /// </summary>
    public int LettersLeft
    {
        get
        {
            return AllChars.Count;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public int TryDone
    {
        get
        {
            return MaxTrys - Trys;
        }
    }

    public static bl_GameManager Instance
    {
        get
        {
            return ((bl_GameManager)mInstance);
        }
        set
        {
            mInstance = value;
        }
    }

}