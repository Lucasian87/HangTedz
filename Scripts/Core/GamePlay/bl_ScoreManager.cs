using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class bl_ScoreManager : Singleton<bl_ScoreManager> {

    [Header("Global")]
    public int CurrentScore = 0;
    public int TotalScore = 0;
    public int BestScore = 0;
    [Header("Settings")]
    [Range(1,200)]
    public int AmountPerLet = 20;
    [Range(1, 1000)]
    public int AmountPerSucces = 100;
    [Header("References")]
    [SerializeField]private Text CurrentScoreText;
    [SerializeField]private GameObject WonUI;
    [SerializeField]private GameObject FailUI;
    [SerializeField]private Text WonDescription;
    [SerializeField]private Text WonScoreText;
    [SerializeField]private Text WonTotalScoreText;
    [SerializeField]private Text WonBestScoreText;
    [SerializeField]private Text TotalText;
    [SerializeField]private Text FaileWordText;
    [SerializeField]private Text WordCreatorPlayerText;
    [SerializeField]private Text FullTwoWonText;
    [SerializeField]private GameObject FullTwoWon;

    public const string BestScoreKey = "HangManLovattoBS";
    private int FollowSucess = 0;

    /// <summary>
    /// 
    /// </summary>
    void Start()
    {
        BestScore = PlayerPrefs.GetInt(BestScoreKey, 0);
        if (bl_GameInfo.Instance.GameMode == Mode.TwoPlayers)
        {
            WordCreatorPlayerText.text = bl_GameInfo.Instance.CurrentWordCreatorPlayer.ToUpper();
            CurrentScoreText.text = string.Format("{0}: {1}", bl_GameInfo.Instance.Player1.ToUpper(), bl_GameInfo.Instance.Player1Score);
            TotalText.text = string.Format("{0}: {1}", bl_GameInfo.Instance.Player2.ToUpper(), bl_GameInfo.Instance.Player2Score);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public void SuccessLetter()
    {
        if (bl_GameInfo.Instance.GameMode == Mode.SinglePlayer)
        {
            FollowSucess++;
            //calculate the xp give for success.
            int s = (AmountPerLet - (bl_GameManager.Instance.TryDone * 2)) * FollowSucess;
            CurrentScore += s;
            CurrentScoreText.text = string.Format("SCORE: {0}", CurrentScore);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public void TwoRoundEnd()
    {
        FullTwoWonText.text = string.Format("PLAYER: \n <size=65>{0}</size> \n WON THE MATCH!", bl_GameInfo.Instance.GetWinner);
        FullTwoWon.SetActive(true);
    }

    public void ReturnToMenu() { Application.LoadLevel("Menu"); }

    /// <summary>
    /// 
    /// </summary>
    public void Reset()
    {
        if (bl_GameInfo.Instance.GameMode == Mode.SinglePlayer)
        {
            CurrentScore = 0;
            CurrentScoreText.text = string.Format("SCORE: {0}", CurrentScore);
        }
        else
        {
            CurrentScoreText.text = string.Format("{0}: {1}", bl_GameInfo.Instance.Player1.ToUpper(), bl_GameInfo.Instance.Player1Score);
            TotalText.text = string.Format("{0}: {1}", bl_GameInfo.Instance.Player2.ToUpper(), bl_GameInfo.Instance.Player2Score);
            WordCreatorPlayerText.text = bl_GameInfo.Instance.CurrentWordCreatorPlayer.ToUpper();
        }

        if (WonUI.activeSelf) { WonUI.GetComponent<Animator>().SetBool("show", false); StartCoroutine(DesactiveInTime(WonUI, WonUI.GetComponent<Animator>().GetCurrentAnimatorClipInfo(0).Length)); }
        if (FailUI.activeSelf) { FailUI.GetComponent<Animator>().SetBool("show", false); StartCoroutine(DesactiveInTime(FailUI, FailUI.GetComponent<Animator>().GetCurrentAnimatorClipInfo(0).Length)); }
    }

    /// <summary>
    /// 
    /// </summary>
    public void FailLetter()
    {
        FollowSucess = 0;
    }

    /// <summary>
    /// 
    /// </summary>
    public void SuccesSentence()
    {
        if (bl_GameInfo.Instance.GameMode == Mode.SinglePlayer)
        {
            CurrentScore += (AmountPerSucces - (bl_GameManager.Instance.TryDone * 5));
            //in more large sentence, more xp.
            CurrentScore += bl_GameManager.Instance.CurrentWord.Length * 3;
            TotalScore += CurrentScore;
            TotalText.text = string.Format("TOTAL: {0}", TotalScore);
            if (TotalScore > BestScore)
            {
                PlayerPrefs.SetInt(BestScoreKey, TotalScore);
                BestScore = TotalScore;
            }
            string cu = CurrentScore.ToString();
            CurrentScoreText.text = string.Format("SCORE: {0}", cu);
            WonDescription.text = string.Format("YOU WON THAT WORD ITS <b>\"{0}\"</b>", bl_GameManager.Instance.CurrentWord.ToUpper());
            WonScoreText.text = string.Format("SCORE: {0}", cu);
            WonTotalScoreText.text = string.Format("TOTAL ROUND: {0}", TotalScore);
            WonBestScoreText.text = string.Format("BEST SCORE: {0}", BestScore);
            WonUI.SetActive(true);
            WonUI.GetComponent<Animator>().SetBool("show", true);
        }
        else//two player mode
        {
            bl_GameInfo.Instance.MultiplayerAddScore();
            CurrentScoreText.text = string.Format("{0}: {1}", bl_GameInfo.Instance.Player1.ToUpper(), bl_GameInfo.Instance.Player1Score);
            TotalText.text = string.Format("{0}: {1}", bl_GameInfo.Instance.Player2.ToUpper(), bl_GameInfo.Instance.Player2Score);
            WonDescription.text = string.Format("{0} 2 BRAINS BETA N 1, THE WORD IS <b>\"{1}\"</b>", bl_GameInfo.Instance.CurrentPlayerTurn.ToUpper(), bl_GameManager.Instance.CurrentWord.ToUpper());
            WonScoreText.text = string.Format("NEXT TURN IS FOR {0}", bl_GameInfo.Instance.NextPlayerTurn.ToUpper());
            WonTotalScoreText.text = string.Format("MATCH IS: {0}: {1} FOR {2}: {3}", bl_GameInfo.Instance.Player1.ToUpper(), bl_GameInfo.Instance.Player1Score, bl_GameInfo.Instance.Player2.ToUpper(), bl_GameInfo.Instance.Player2Score);
            WonBestScoreText.text = "";
            WonUI.SetActive(true);
            WonUI.GetComponent<Animator>().SetBool("show", true);
            bl_GameInfo.Instance.NextTurnPlayer();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public void FailSentence()
    {
        if (bl_GameInfo.Instance.GameMode == Mode.SinglePlayer)
        {
            FaileWordText.text = string.Format("FAILED!, THE WORD WAS:\n<size=55>\"{0}\"</size>", bl_GameManager.Instance.CurrentWord.ToUpper());
        }
        else
        {
            FaileWordText.text = string.Format("FAILED!, THE WORD WAS:\n <size=55>\"{0}\"</size> \n NEXT TURN IS FOR GUESS IS FOR: \n <size=65>{1}</size>", bl_GameManager.Instance.CurrentWord.ToUpper(), bl_GameInfo.Instance.NextPlayerTurn.ToUpper());
            bl_GameInfo.Instance.NextTurnPlayer();
        }
        FailUI.SetActive(true);
        FailUI.GetComponent<Animator>().SetBool("show", true);
    }

    IEnumerator DesactiveInTime(GameObject go,float t)
    {
        yield return new WaitForSeconds(t);
        go.SetActive(false);
    }

    public static bl_ScoreManager Instance
    {
        get
        {
            return ((bl_ScoreManager)mInstance);
        }
        set
        {
            mInstance = value;
        }
    }
}