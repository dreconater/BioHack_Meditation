using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VRBrowserContainer : MonoBehaviour
{
    public AlexVersionLandingPageController Canvas;

    public Button CloseBtn;

    private void Awake()
    {
        CloseBtn.onClick.AddListener(Close);
    }

    void Close() {
        Canvas.CloseVRBrowser();
    }
}
