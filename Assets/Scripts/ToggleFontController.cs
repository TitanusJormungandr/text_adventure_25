using UnityEngine;
using UnityEngine.UI;

public class ToggleFontController : MonoBehaviour
{
    public Text targetText;
    public Toggle fontSizeToggle;

    private float originalFontSize; 
    private float currentFontSize;  
    private const float min_float_size = 15f;

    void Start()
    {
        //Save the initial font size when the game starts
        originalFontSize = targetText.fontSize;
        currentFontSize = originalFontSize;

        //Set the initial font size
        SetFontSize();

        //Add listener to the toggle to adjust the font size when changed
        fontSizeToggle.onValueChanged.AddListener(ChangeFontSize);
    }

 
    void SetFontSize()
    {
        targetText.fontSize = (int)currentFontSize;
    }

    //Change the font size when the toggle is switched
    void ChangeFontSize(bool isOn)
    {
        if (isOn)
        {
            //Double the font size when turned on
            currentFontSize = originalFontSize * 2f;
        }
        else
        {
            //Reset to the original font size when turned off
            currentFontSize = originalFontSize;
        }

        PlayerPrefs.SetFloat("fontSize", currentFontSize);

        SetFontSize();
    }
}
