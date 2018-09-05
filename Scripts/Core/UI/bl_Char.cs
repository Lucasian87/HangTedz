using UnityEngine;
using UnityEngine.UI;

public class bl_Char : MonoBehaviour {

    public string Char;
    [SerializeField]private Color SuccessColor;
    [SerializeField]private Color FailedColor;
    [SerializeField]private Text CharText;
    [SerializeField]private Image SpaceImage;


    public void GetInfo(string c)
    {
        CharText.CrossFadeAlpha(0, 0.1f, true);
        Char = c;
        CharText.text = c.ToUpper();
    }

    public void ActiveLetter()
    {
        if (CharText != null)
        {
            CharText.CrossFadeAlpha(1, 1, true);
        }
        if (SpaceImage != null)
        {
            SpaceImage.CrossFadeColor(SuccessColor, 1, true, true);
        }
    }

    public void Failed()
    {
        if (SpaceImage != null)
        {
            SpaceImage.CrossFadeColor(FailedColor, 1, true, true);
        }
        if (CharText != null)
        {
            CharText.CrossFadeColor(FailedColor, 1, true, true);
        }
    }
}