using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GeneralMenu : MonoBehaviour
{
    public Button FirstAppBtn;
    public Button SecondAppBtn;

    public string FirstAppSceneName;
    public string SecondAppSceneName;

    void Awake()
    {
        FirstAppBtn.onClick.AddListener(OpenFirstApp);
        SecondAppBtn.onClick.AddListener(OpenSecondApp);
    }

    void OpenFirstApp() {
        SceneManager.LoadScene(FirstAppSceneName);
    }

    void OpenSecondApp()
    {
        SceneManager.LoadScene(SecondAppSceneName);
    }
}
