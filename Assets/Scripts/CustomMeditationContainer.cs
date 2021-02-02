using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

public class CustomMeditationContainer : MonoBehaviour
{
    private string[] MusicUrls;
    private string[] MusicUrlsOffline;

    public string VideoSceneName;

    private string[] VideoUrls;

    public OVRRaycaster Canvas;

    public LargeFileDownloaderExample Downloader;

    private int VideoId;
    private int VoiceId;

    private Text ProgressBar;

    private Slider ProgressSlider;

    private Button OnlineButton;

    private void Start()
    {
        MusicUrls = new string[10] { "http://biohack.network/Musics/1.mp3", "http://biohack.network/Musics/2.mp3", "http://biohack.network/Musics/3.mp3", "http://biohack.network/Musics/4.mp3", "http://biohack.network/Musics/5.mp3", "http://biohack.network/Musics/6.mp3", "http://biohack.network/Musics/7.mp3", "http://biohack.network/Musics/8.mp3", "http://biohack.network/Musics/9.mp3", "http://biohack.network/Musics/10.mp3" };
        MusicUrlsOffline = new string[4] { Application.streamingAssetsPath + "/CustomMusic/1.mp3", Application.streamingAssetsPath + "/CustomMusic/2.mp3", Application.streamingAssetsPath + "/CustomMusic/3.mp3", Application.streamingAssetsPath + "/CustomMusic/4.mp3" };

        VideoUrls = new string[10] { "http://biohack.network/1.mp4",
                                     "http://biohack.network/2.mp4",
                                     "http://biohack.network/3.mp4",
                                     "http://biohack.network/4.mp4",
                                     "http://biohack.network/5.mp4",
                                     "http://biohack.network/6.mp4",
                                     "http://biohack.network/7.mp4",
                                     "http://biohack.network/8.mp4",
                                     "http://biohack.network/9.mp4",
                                     "http://biohack.network/10.mp4", };
    }

    public void OpenCustomMeditation(string VideoUrl, string VoiceUrl, bool Music, bool download) {
        /*PlayerPrefs.DeleteKey("VoiceUrl");
        PlayerPrefs.DeleteKey("MusicUrl");
        PlayerPrefs.DeleteKey("VideoUrl");*/
        PlayerPrefs.DeleteKey("CanPlayMusic");
        PlayerPrefs.DeleteKey("CanPlayMusicOffline");

        if (VideoUrl == "random")
        {
            int videoId = Random.Range(1, 10);
            PlayerPrefs.SetString("VideoUrl", VideoUrls[videoId - 1]);
        }
        else
        {
            PlayerPrefs.SetString("VideoUrl", VideoUrl);
        }

        PlayerPrefs.SetString("VoiceUrl", VoiceUrl);

        if (Music)
        {
            if (!download)
            {
                int musicId = Random.Range(1, 10);
                PlayerPrefs.SetString("MusicUrl", MusicUrls[musicId]);
            }
            else
            {
                int musicId = Random.Range(1, 4);
                PlayerPrefs.SetString("MusicUrl", MusicUrlsOffline[musicId]);
            }
            
            PlayerPrefs.SetInt("CanPlayMusic", 1);
        }
        else
        {
            PlayerPrefs.SetInt("CanPlayMusic", 0);
        }

        SceneManager.LoadScene(VideoSceneName);
    }

    public void DownloadCustomMeditation(Button onlineBtn, Text progressBar, Slider progressSlider, string videoUrl, bool music, int videoId, int voiceId) {
        onlineBtn.interactable = false;
        Canvas.enabled = false;

        VideoId = videoId;
        VoiceId = voiceId;
        ProgressBar = progressBar;
        ProgressSlider = progressSlider;
        OnlineButton = onlineBtn;

        string VideoPath = Path.Combine(Application.persistentDataPath, Path.GetFileName("http://biohack.network/" + VideoId + ".mp4"));
        string VoicePath = Path.Combine(Application.persistentDataPath, Path.GetFileName("http://biohack.network/" + VoiceId + ".mp3"));

        if (File.Exists(VideoPath) && File.Exists(VoicePath))
        {
            //DoNoting
        }
        else if (File.Exists(VideoPath) && !File.Exists(VoicePath))
        {
            DownloadOnlyVoice();
        }
        else if (!File.Exists(VideoPath) && File.Exists(VoicePath))
        {
            DownloadOnlyVideo();
        }
        else if (!File.Exists(VideoPath) && !File.Exists(VoicePath))
        {
            DownloadVideoAndVoice();
        }
    }

    void DownloadOnlyVoice()
    {
        Downloader.DownloadOnlyVoice(VoiceId, ProgressBar, ProgressSlider, this, null);
    }

    void DownloadOnlyVideo()
    {
        Downloader.DownloadOnlyVideo(VideoId, ProgressBar, ProgressSlider, this, null);
    }

    void DownloadVideoAndVoice() {
        StartCoroutine(Downloader.DownloadVideoAndVoice(VideoId, VoiceId, ProgressBar, ProgressSlider, "voice", this, null));
    }

    public void DownloadComplated() {
        OnlineButton.interactable = true;
        Canvas.enabled = true;
    }
}
