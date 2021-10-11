using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VoiceChecker : MonoBehaviour
{
    public Text TextBox;

    public void SetVoice() {
        GameObject Canvas = GameObject.FindGameObjectWithTag("MainCanvas");
        Canvas.GetComponent<AlexVersionLandingPageController>().SetVoice(TextBox);
    }
}
