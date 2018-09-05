using UnityEngine;
using UnityEngine.UI;

public class bl_Key : MonoBehaviour {

    [HideInInspector]public string Char;
    [Header("Settings")]
    [SerializeField]private Color FailColor;
    [SerializeField]private Color SuccessColor;
    [Header("References")]
    [SerializeField]private AudioClip PressSound;
    [SerializeField]private Text KeyText;
    [SerializeField]private Button KeyButton;
   
    private KeyCode cacheKey;
    private Color DefaultTextColor;

    public void GetInfo(KeyCode k)
    {
        cacheKey = k;
        Char = k.ToString().ToLower();
        KeyText.text = k.ToString().ToUpper();
        KeyButton.onClick.AddListener(() => this.Select(cacheKey));
        DefaultTextColor = KeyText.color;
    }

    public void Select(KeyCode k)
    {

        bool exist = bl_GameManager.Instance.NewSelect(k);

        ColorBlock cb = KeyButton.colors;
        cb.disabledColor = (exist) ? SuccessColor : FailColor;
        KeyButton.colors = cb;

        KeyButton.interactable = false;
        KeyText.color = KeyButton.colors.disabledColor;
        if (PressSound != null)
        {
            GetComponent<AudioSource>().clip = PressSound;
            GetComponent<AudioSource>().Play();
        }
    }

    public void Reset()
    {
        KeyButton.interactable = true;
        KeyButton.image.CrossFadeColor(KeyButton.colors.normalColor, 1, true, true);
        KeyText.color = DefaultTextColor;
    }

    public void DesactiveKey()
    {
        KeyButton.interactable = false;
        KeyText.color = KeyButton.colors.disabledColor;
    }

}