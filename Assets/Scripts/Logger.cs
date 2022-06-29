using TMPro;
using UnityEngine;

public class Logger : MonoBehaviour
{
    [SerializeField] private GameObject textWindow;

    public void LogText(string text, bool append)
    {
        if (append)
        {
            if (!string.IsNullOrWhiteSpace(textWindow.GetComponent<TMP_Text>().text))
                textWindow.GetComponent<TMP_Text>().text += "\n" + text;
            else
                textWindow.GetComponent<TMP_Text>().text += text;
        }
        else
            textWindow.GetComponent<TMP_Text>().text = text;
    }
}
