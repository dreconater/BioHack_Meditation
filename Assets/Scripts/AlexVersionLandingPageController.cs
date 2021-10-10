



using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using TalesFromTheRift;
using UnityEngine;
using UnityEngine.UI;

public class AlexVersionLandingPageController : MonoBehaviour
{
    [Header("General")]
    public OVRRaycaster Raycaster;

    [Header("Containers")]
    public GameObject IntroContainer;
    public GameObject MeditationContainer;
    public GameObject SettingsContainer;
    public GameObject VideosContainer;
    public GameObject VoicesContainer;
    public GameObject VRBrowserContainer;
    public GameObject Voices20Container;
    public GameObject Voices12Container;
    public GameObject Voices6Container;
    public GameObject CourdesContainer;

    public Animator TrialWindowContainer;

    public Toggle QualityToggle;

    [Header("Email Part")]
    public Button CloseBtn;
    public Button SendBtn;

    public InputField EmailField;

    public GameObject EmailErrorText;
    public GameObject EmailSuccesText;

    public OpenCanvasKeyboard VRKeyboard;

    [Header("Settings Part")]
    public SettingsPageController Settings;
    public Button SettingsButton;

    private int VideoId;
    private int VoiceId;
    private int IntroCount;
    private string videoUrl;
    private string voiceUrl;
    private string emailPassword = "";
    private bool IsMusicOn = true;

    public AlexVersionMeditationController MeditationController;

    [Header("Buttons Part")]
    public Button DownloadMeditationBtn;
    public Button OnlineMeditationBtn;
    public GameObject AlreadyDownloadedText;
    public Text ProgressBar;
    public Slider ProgressSlider;

    [Header("PoPup Part")]
    public Button OpenPopUpBtn;
    public GameObject PopupinfoWindow;

    void Awake()
    {
        DownloadMeditationBtn.onClick.AddListener(DownloadingCustomMeditation);
        OnlineMeditationBtn.onClick.AddListener(SetCustomMeditationOptions);
        CloseBtn.onClick.AddListener(CloseEmailSection);
        SendBtn.onClick.AddListener(() => { StartCoroutine(SendEmail()); });
        SettingsButton.onClick.AddListener(() => { OpenSettingsPage(); });
        OpenPopUpBtn.onClick.AddListener(() => { OpenPopUpWindow(); });
        Settings.SettingsPanelClosed += CloseSettingsPage;
    }

    private void Start()
    {
        IntroCount = PlayerPrefs.GetInt("IntroCount");
        PlayerPrefs.SetString("Breathing", "particle");
        IntroContainer.SetActive(false);
        MeditationContainer.SetActive(false);
        VideoId = 1;
        VoiceId = 1;
        CheckOfflineVersion();
        IsMusicOn = true;
        if (PlayerPrefs.GetInt("IsBackFromVideo") == 1)
        {
            IntroContainer.SetActive(false);
            MeditationContainer.SetActive(true);
            VideosContainer.SetActive(true);
            SettingsContainer.SetActive(false);
            VoicesContainer.SetActive(false);
            PlayerPrefs.SetInt("IsBackFromVideo", 0);
        }
        else
        {
            PlayerPrefs.SetInt("IsBackFromVideo", 0);
            StartCoroutine(CheckAndOpenEmailSection());
        }

        StartCoroutine(GetPassword());
    }

    IEnumerator GetPassword() {
        WWW www = new WWW("https://drive.google.com/uc?export=download&id=1gIC4sBROMN4vgwKzWS1uVLr1ktTNIEha");
        yield return www;
        emailPassword = www.text;
        Settings.emailPassword = www.text;
    }

    IEnumerator CheckAndOpenEmailSection()
    {
        yield return new WaitForSeconds(0.8f);
        if (PlayerPrefs.GetInt("IntroCount") != 3)
        {
            IntroCount++;
            PlayerPrefs.SetInt("IntroCount", IntroCount);
            IntroContainer.SetActive(true);
            MeditationContainer.SetActive(false);
        }
        else
        {
            IntroContainer.SetActive(false);
            MeditationContainer.SetActive(true);
            VideosContainer.SetActive(true);
            SettingsContainer.SetActive(false);
            VoicesContainer.SetActive(false);
        }
    }

    IEnumerator SendEmail()
    {
        if (EmailField.text.Contains("@") && EmailField.text.Contains("."))
        {
            //Send To Server
            Debug.Log("Sending Email");
            SendingEmail();
            EmailErrorText.SetActive(false);
            EmailSuccesText.SetActive(true);
            yield return new WaitForSeconds(1.5f);
            CloseEmailSection();
        }
        else
        {
            Debug.Log("Wrong Email");
            EmailErrorText.SetActive(true);
            EmailSuccesText.SetActive(false);
        }
    }

    void SendingEmail()
    {
        Debug.Log("Sending Email");

        MailMessage mail = new MailMessage();

        mail.From = new MailAddress("team@biohack.network");
        mail.To.Add("team@biohack.network");
        mail.Subject = "VR subscriber from " + EmailField.text;
        mail.Body = "VR subscriber from " + EmailField.text;

        SmtpClient smtpServer = new SmtpClient("smtp.gmail.com");
        smtpServer.Port = 587;
        smtpServer.Credentials = new System.Net.NetworkCredential("team@biohack.network", emailPassword) as ICredentialsByHost;
        smtpServer.EnableSsl = true;
        ServicePointManager.ServerCertificateValidationCallback =
            delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
            { return true; };
        smtpServer.Send(mail);
        
    }

    void CloseEmailSection() {
        IntroContainer.SetActive(false);
        MeditationContainer.SetActive(true);
        VideosContainer.SetActive(true);
        VoicesContainer.SetActive(false);
        SettingsContainer.SetActive(false);
        VRKeyboard.CloseKeyboard();
    }

    private GameObject mainGamobject;
    private GameObject secondGamobject;

    void OpenSettingsPage() {
        if (VideosContainer.activeInHierarchy)
        {
            mainGamobject = VideosContainer;
            secondGamobject = VoicesContainer;
        }
        else if(VoicesContainer.activeInHierarchy)
        {
            mainGamobject = VoicesContainer;
            secondGamobject = VideosContainer;
        }
        VideosContainer.SetActive(false);
        VoicesContainer.SetActive(false);
        SettingsContainer.SetActive(true);
        SettingsButton.gameObject.SetActive(false);
    }

    void CloseSettingsPage()
    {
        mainGamobject.SetActive(true);
        secondGamobject.SetActive(false);
        SettingsContainer.SetActive(false);
        SettingsButton.gameObject.SetActive(true);
    }

    void CheckOfflineVersion()
    {
        string filePath = Application.persistentDataPath + "Alex'Version";
        if (!Directory.Exists(filePath))
        {
            Directory.CreateDirectory(filePath);
        }
        string VideoPath = Path.Combine(filePath, Path.GetFileName("https://storage.googleapis.com/mindfulness_vr_bucket/Videos/2K/" + VideoId + ".mp4"));
        string VoicePath = Path.Combine(filePath, Path.GetFileName("https://storage.googleapis.com/mindfulness_vr_bucket/Voices/Low/" + VoiceId + ".mp3"));

        if (File.Exists(VideoPath) && File.Exists(VoicePath))
        {
            DownloadMeditationBtn.gameObject.SetActive(false);
            AlreadyDownloadedText.SetActive(true);
            ProgressBar.gameObject.SetActive(false);
            ProgressSlider.gameObject.SetActive(false);
        }
        else
        {
            DownloadMeditationBtn.gameObject.SetActive(true);
            AlreadyDownloadedText.SetActive(false);
            ProgressBar.gameObject.SetActive(false);
            ProgressSlider.gameObject.SetActive(false);
        }
    }

    public void SetVideo(Text text)
    {
        string[] videoId = text.text.Split(' ');
        VideoId = int.Parse(videoId[1]);
        CheckOfflineVersion();
        OpenVoiceContainer();
    }

    public void SetCourse(string text) {
        CourdesContainer.SetActive(false);

        if (text == "20")
        {
            Voices20Container.SetActive(true);
        }
        else if (text == "12")
        {
            Voices12Container.SetActive(true);
        }
        else if (text == "6")
        {
            Voices6Container.SetActive(true);
        }
    }

    public void Call12MonthContent(int value) {
        
    }

    public void SetVoice(Text text)
    {
        string[] voiceId = text.text.Split(' ');
        VoiceId = int.Parse(voiceId[1]);
        CheckOfflineVersion();
    }

    public void SetMusic(Toggle toggle)
    {
        bool value = toggle.isOn;
        IsMusicOn = value;
    }

    public void SetBreath(string toggleName)
    {
        string name = toggleName;

        if (name == "vapor")
        {
            PlayerPrefs.SetString("Breathing", "vapor");
        }
        else if (name == "particle")
        {
            PlayerPrefs.SetString("Breathing", "particle");
        }
    }

    void OpenVoiceContainer() {
        VideosContainer.SetActive(false);
        SettingsContainer.SetActive(false);
        VoicesContainer.SetActive(true);
    }

    void SetCustomMeditationOptions()
    {
        string filePath = Application.persistentDataPath + "Alex'Version";
        if (!Directory.Exists(filePath))
        {
            Directory.CreateDirectory(filePath);
        }

        string VideoPath = Path.Combine(filePath, Path.GetFileName("https://storage.googleapis.com/mindfulness_vr_bucket/Videos/2K/" + VideoId + ".mp4"));
        
        string VoicePath = Path.Combine(filePath, Path.GetFileName("https://storage.googleapis.com/mindfulness_vr_bucket/Voices/Low/" + VoiceId + ".mp3"));
        
        if (File.Exists(VideoPath) && File.Exists(VoicePath))
        {
            Debug.Log("Starting Offline Version");
            MeditationController.OpenCustomMeditation(VideoPath, VoicePath, IsMusicOn, true);
            PlayerPrefs.SetInt("IsBackFromVideo", 1);
        }
        else
        {
            Debug.Log("Starting Online Version");

            if (QualityToggle.isOn)
            {
                videoUrl = "https://storage.googleapis.com/mindfulness_vr_bucket/Videos/4K/" + VideoId + ".mp4";
            }
            else
            {
                videoUrl = "https://storage.googleapis.com/mindfulness_vr_bucket/Videos/2K/" + VideoId + ".mp4";
            }

            voiceUrl = "https://storage.googleapis.com/mindfulness_vr_bucket/Voices/Low/" + VoiceId + ".mp3";

            MeditationController.OpenCustomMeditation(videoUrl, voiceUrl, IsMusicOn, false);
            PlayerPrefs.SetInt("IsBackFromVideo", 1);
        }
    }

    void DownloadingCustomMeditation()
    {
        videoUrl = "https://storage.googleapis.com/mindfulness_vr_bucket/Videos/4K/" + VideoId + ".mp4";
        voiceUrl = "https://storage.googleapis.com/mindfulness_vr_bucket/Voices/Low/" + VoiceId + ".mp3";


        DownloadMeditationBtn.gameObject.SetActive(false);
        ProgressBar.gameObject.SetActive(true);
        ProgressSlider.gameObject.SetActive(true);

        MeditationController.DownloadCustomMeditation(DownloadMeditationBtn, ProgressBar, ProgressSlider, videoUrl, VideoId, VoiceId);
    }

    void OpenPopUpWindow() {
        PopupinfoWindow.SetActive(true);
    }

    public void OpenVRBrowser()
    {
        Debug.Log("Opening VR Browser");

        if (VRBrowserContainer != null)
        {
            VRBrowserContainer.SetActive(true);
            MeditationContainer.SetActive(false);
        }

        if (TrialWindowContainer != null)
        {
            TrialWindowContainer.gameObject.SetActive(true);
            TrialWindowContainer.Play("CanvasAplhaIn");
        }
    }

    public void CloseVRBrowser()
    {
        Debug.Log("Opening VR Browser");

        if (VRBrowserContainer != null)
        {
            VRBrowserContainer.SetActive(false);
            MeditationContainer.SetActive(true);
        }
    }
}
