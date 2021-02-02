using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DownloadBlockingSystem : MonoBehaviour
{
    public List<Button> DownloadButtons = new List<Button>();

    public Button GoToGenMenuBtn;
    public string GenMenuName;

    void Awake()
    {
        GoToGenMenuBtn.onClick.AddListener(GoToGenMenu);
    }

    void GoToGenMenu() {
        SceneManager.LoadScene(GenMenuName);
    }

    public void ShowButtons() {
        foreach (var item in DownloadButtons)
        {
            if (item.gameObject.tag != "ThisBtn")
            {
                item.interactable = true;
            }
        }
    }
    public void HideButtons()
    {
        foreach (var item in DownloadButtons)
        {
            
            item.interactable = false;
        }
    }
}
