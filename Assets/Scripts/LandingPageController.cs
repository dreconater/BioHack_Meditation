
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using TalesFromTheRift;

public class LandingPageController : MonoBehaviour
{
    public GameObject MassageContainer;
    public GameObject MenuContainer;
    public GameObject CustomMedContainer;
    public GameObject SettingsContainer;

    public Button QuickStartBtn;
    public Button CustomStartBtn;

    public QuickMeditationPicker QuickMeditationPicker;

    public OVRRaycaster Canvas;

    [Header("Intro Part")]
    public Button CloseBtn;

    [Header("Custom Med Part")]
    public CustomMeditationContainer CustomMeditationPicker;
    private int VideoId = 0;
    private int DayId = 0;
    private bool IsMusicOn = true;
    private string videoUrl;
    private string voiceUrl;
    public GameObject VideoContainer;
    public GameObject DaysContainer;

    [Header("Online Offline Part")]

    public Button StartCustomMeditationBtn;
    public Button DownloadCustomMeditationBtn;
    public GameObject AlreadyDownloadedText;
    public Text ProgressBar;
    public Slider ProgressSlider;

    private int IntroCount;
    private int FromWhereOpenedStettings;

    [Header("Settings Part")]
    public Button SettingsBtn1;
    public Button SettingsBtn2;
    public SettingsPageController Settings;

    void Awake()
    {
        QuickStartBtn.onClick.AddListener(QuickStart);
        CustomStartBtn.onClick.AddListener(ShowCustomPanel);
        StartCustomMeditationBtn.onClick.AddListener(SetCustomMeditationOptions);
        DownloadCustomMeditationBtn.onClick.AddListener(DownloadingCustomMeditation);
        CloseBtn.onClick.AddListener(() => { StartCoroutine(CloseIntro()); });
        SettingsBtn1.onClick.AddListener(() => { OpenSettingsPage(1); });
        SettingsBtn2.onClick.AddListener(() => {OpenSettingsPage(2); });
        Settings.SettingsPanelClosed += CloseSettingsPage;
    }

    void Start()
    {
        //PlayerPrefs.DeleteKey("IntroCount");
        IntroCount = PlayerPrefs.GetInt("IntroCount");
        MassageContainer.SetActive(false);
        MenuContainer.SetActive(false);
        CustomMedContainer.SetActive(false);
        StartCoroutine(CheckAndOpenMassage());

        VideoId = 1;
        DayId = 1;
        IsMusicOn = true;

        CheckOfflineVersion();

        VideoContainer.SetActive(true);
        DaysContainer.SetActive(false);
    }

    IEnumerator CheckAndOpenMassage()
    {
        yield return new WaitForSeconds(0.8f);
        if (PlayerPrefs.GetInt("IntroCount") != 3)
        {
            IntroCount++;
            PlayerPrefs.SetInt("IntroCount", IntroCount);
            MassageContainer.SetActive(true);
            MenuContainer.SetActive(false);
            CustomMedContainer.SetActive(false);
        }
        else
        {
            MassageContainer.SetActive(false);
            MenuContainer.SetActive(true);
        }
    }

    void OpenSettingsPage(int fromWhere) {
        FromWhereOpenedStettings = fromWhere;
        MenuContainer.SetActive(false);
        CustomMedContainer.SetActive(false);
        SettingsContainer.SetActive(true);
    }

    void CloseSettingsPage() {
        if (FromWhereOpenedStettings == 1)
        {
            MenuContainer.SetActive(true);
            MenuContainer.GetComponent<Animator>().Play("New State");
            CustomMedContainer.SetActive(false);
            SettingsContainer.SetActive(false);
        }
        else if (FromWhereOpenedStettings == 2)
        {
            MenuContainer.SetActive(false);
            CustomMedContainer.SetActive(true);
            SettingsContainer.SetActive(false);
        }
    }

    IEnumerator CloseIntro()
    {
        MassageContainer.GetComponent<Animator>().Play("CanvasAplhaOut");
        yield return new WaitForSeconds(0.7f);
        MassageContainer.SetActive(false);
        MenuContainer.SetActive(true);
    }

    void CheckOfflineVersion() {
        string VideoPath = Path.Combine(Application.persistentDataPath, Path.GetFileName("http://biohack.network/" + VideoId + ".mp4"));
        string VoicePath = Path.Combine(Application.persistentDataPath, Path.GetFileName("http://biohack.network/" + DayId + ".ogg"));

        if (File.Exists(VideoPath) && File.Exists(VoicePath))
        {
            DownloadCustomMeditationBtn.gameObject.SetActive(false);
            AlreadyDownloadedText.SetActive(true);
            ProgressBar.gameObject.SetActive(false);
            ProgressSlider.gameObject.SetActive(false);
        }
        else {
            DownloadCustomMeditationBtn.gameObject.SetActive(true);
            AlreadyDownloadedText.SetActive(false);
            ProgressBar.gameObject.SetActive(false);
            ProgressSlider.gameObject.SetActive(false);
        }
    }

    void QuickStart()
    {
        QuickMeditationPicker.OpenQuickMeditation();
    }

    void ShowCustomPanel()
    {
        MenuContainer.SetActive(false);
        CustomMedContainer.SetActive(true);
    }

    public void SetVideo(Text text) {
        string[] videoId = text.text.Split(' ');
        VideoId = int.Parse(videoId[1]);
        CheckOfflineVersion();
    }

    public void SetRandomVideo() {
        videoUrl = "random";
        CheckOfflineVersion();
    }

    public void SetDay(Text text)
    {
        string[] dayId = text.text.Split(' ');
        DayId = int.Parse(dayId[1]);
        CheckOfflineVersion();
    }

    public void SetMusic(Toggle toggle)
    {
        bool value = toggle.isOn;
        IsMusicOn = value;
    }

    void SetCustomMeditationOptions() {
        string VideoPath = Path.Combine(Application.persistentDataPath, Path.GetFileName("http://biohack.network/" + VideoId + ".mp4"));
        string VoicePath = Path.Combine(Application.persistentDataPath, Path.GetFileName("http://biohack.network/" + DayId + ".ogg"));

        if (File.Exists(VideoPath) && File.Exists(VoicePath))
        {
            Debug.Log("Starting Offline Version");
            CustomMeditationPicker.OpenCustomMeditation(VideoPath, VoicePath, IsMusicOn, true);
        }
        else
        {
            Debug.Log("Starting Online Version");
            if (videoUrl == "random")
            {
                videoUrl = "random";
            }
            else
            {
                videoUrl = "http://biohack.network/" + VideoId + ".mp4";
            }

            voiceUrl = "http://biohack.network/Voices/" + DayId + ".ogg";

            CustomMeditationPicker.OpenCustomMeditation(videoUrl, voiceUrl, IsMusicOn, false);
        }
    }

    void DownloadingCustomMeditation() {
        if (videoUrl == "random")
        {
            videoUrl = "random";
        }
        else
        {
            videoUrl = "http://biohack.network/" + VideoId + ".mp4";
        }

        voiceUrl = "http://biohack.network/Voices/" + DayId + ".ogg";

        DownloadCustomMeditationBtn.gameObject.SetActive(false);
        ProgressBar.gameObject.SetActive(true);
        ProgressSlider.gameObject.SetActive(true);

        CustomMeditationPicker.DownloadCustomMeditation(StartCustomMeditationBtn, ProgressBar, ProgressSlider, videoUrl, IsMusicOn, VideoId, DayId);
    }
}
