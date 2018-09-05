using UnityEngine;

public class bl_GameInfo : Singleton<bl_GameInfo> {

    [Header("Global Info")]
    public string Player1 = "None";
    public string Player2 = "None";
    public Mode GameMode = Mode.SinglePlayer;
    public DifficultMode Category = DifficultMode.MEDIUM;
    [Header("Game Options")]
    public float Volumen = 1;
    public bool Audio = true;
    public bool UseVibrate = true;
    [Range(1,10)]
    public int TwoPlayerMaxRounds = 7;

    [HideInInspector]public bool TurnPlayer1 = true;
    [HideInInspector]public int Player1Score = 0;
    [HideInInspector]public int Player2Score = 0;
    [HideInInspector]public int TwoPlayerRound = 0;

    public const string VolumenKey = "HangmanLovattoVol";
    public const string AudioKey = "HangmanLovattoAudio";
    public const string VibrateKey = "HangmanLovattoVibrate";
    public const string RoundsKey = "HangmanLovattoRounds";

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void Start()
    {
        Volumen = PlayerPrefs.GetFloat(VolumenKey, 1);
        Audio = (PlayerPrefs.GetInt(AudioKey, 1) == 1) ? true : false;
        UseVibrate = (PlayerPrefs.GetInt(VibrateKey, 1) == 1) ? true : false;
        TwoPlayerMaxRounds = PlayerPrefs.GetInt(RoundsKey, 7);
    }

    /// <summary>
    /// 
    /// </summary>
    public void MultiplayerAddScore()
    {
        if (TurnPlayer1)
        {
            Player2Score++;
        }
        else
        {
            Player1Score++;
        }
    }

    /// <summary>
    /// change side player
    /// </summary>
    public void NextTurnPlayer()
    {
        if (!TurnPlayer1) { TwoPlayerRound++; }
        TurnPlayer1 = !TurnPlayer1;

        if(TwoPlayerRound >= TwoPlayerMaxRounds)
        {
            TwoPlayerRound = 0;
            bl_ScoreManager.Instance.TwoRoundEnd();
        }
    }

    /// <summary>
    /// Who is the current player to guess
    /// </summary>
    public string CurrentPlayerTurn
    {
        get
        {
            if (TurnPlayer1) { return Player2; } else { return Player1; }
        }
    }

    /// <summary>
    /// Who is the next player to guess?
    /// </summary>
    public string NextPlayerTurn
    {
        get
        {
            if (TurnPlayer1) { return Player1; } else { return Player2; }
        }
    }

    //Current Player who writte the word
    public string CurrentWordCreatorPlayer
    {
        get
        {
            if (TurnPlayer1) { return Player1; } else { return Player2; }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public string GetWinner
    {
        get
        {
            if (Player1Score > Player2Score)
            {
                return Player1;
            }
            else if (Player1Score < Player2Score)
            {
                return Player2;
            }
            else
            {
                return "None";
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public void Reset()
    {
        Player1Score = 0;
        Player2Score = 0;
        TwoPlayerRound = 0;
        TurnPlayer1 = true;
    }

    public void RateApp()
    {
#if UNITY_ANDROID
        Application.OpenURL("market://details?id=com.lovattostudio.lovattohangman");
#elif UNITY_IPHONE
 Application.OpenURL("itms-apps://itunes.apple.com/app/idYOUR_ID");
#endif
    }


    public static bl_GameInfo Instance
    {
        get
        {
            return ((bl_GameInfo)mInstance);
        }
        set
        {
            mInstance = value;
        }
    }
}