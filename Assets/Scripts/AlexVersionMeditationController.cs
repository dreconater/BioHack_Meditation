using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

public class AlexVersionMeditationController : MonoBehaviour
{
    private string[] MusicUrls;
    private string[] MusicUrlsOffline;

    public string VideoSceneName;

    public CurvedUIInputModule Canvas;

    public LargeFileDownloaderExample Downloader;

    private int VideoId;
    private int VoiceId;
    private Text ProgressBar;
    private Slider ProgressSlider;
    private Button OnlineButton;

    private void Start()
    {
        MusicUrlsOffline = new string[10] { Application.streamingAssetsPath + "/CustomMusic/1.mp3", Application.streamingAssetsPath + "/CustomMusic/2.mp3", Application.streamingAssetsPath + "/CustomMusic/3.mp3", Application.streamingAssetsPath + "/CustomMusic/4.mp3", Application.streamingAssetsPath + "/CustomMusic/5.mp3", Application.streamingAssetsPath + "/CustomMusic/6.mp3", Application.streamingAssetsPath + "/CustomMusic/7.mp3", Application.streamingAssetsPath + "/CustomMusic/8.mp3", Application.streamingAssetsPath + "/CustomMusic/9.mp3", Application.streamingAssetsPath + "/CustomMusic/10.mp3" };
        MusicUrls = new string[10] { "http://biohack.network/Musics/Low/1.mp3", "http://biohack.network/Musics/Low/2.mp3", "http://biohack.network/Musics/Low/3.mp3", "http://biohack.network/Musics/Low/4.mp3", "http://biohack.network/Musics/Low/5.mp3", "http://biohack.network/Musics/Low/6.mp3", "http://biohack.network/Musics/Low/7.mp3", "http://biohack.network/Musics/Low/8.mp3", "http://biohack.network/Musics/Low/9.mp3", "http://biohack.network/Musics/Low/10.mp3" };
    }

    public void OpenCustomMeditation(string VideoUrl, string VoiceUrl, bool Music, bool download)
    {
        /*PlayerPrefs.DeleteKey("VoiceUrl");
        PlayerPrefs.DeleteKey("MusicUrl");
        PlayerPrefs.DeleteKey("VideoUrl");*/
        PlayerPrefs.DeleteKey("CanPlayMusic");
        PlayerPrefs.DeleteKey("CanPlayMusicOffline");

        PlayerPrefs.SetInt("CanPlayMusic", 1);
        PlayerPrefs.SetString("VideoUrl", VideoUrl);
        PlayerPrefs.SetString("VoiceUrl", VoiceUrl);

        if (Music)
        {
            if (!download)
            {
                var musicId = Random.Range(1, 10);
                var newMusicId = Random.Range(musicId, 10);
                PlayerPrefs.SetString("MusicUrl", MusicUrls[newMusicId]);

            }
            else
            {
                var musicId = Random.Range(1, 10);
                var newMusicId = Random.Range(musicId, 10);
                PlayerPrefs.SetString("MusicUrl", MusicUrlsOffline[newMusicId]);
            }

            PlayerPrefs.SetInt("CanPlayMusic", 1);
        }
        else
        {
            PlayerPrefs.SetInt("CanPlayMusic", 0);
        }


        SceneManager.LoadScene(VideoSceneName);
    }

    public void DownloadCustomMeditation(Button onlineBtn, Text progressBar, Slider progressSlider, string videoUrl, int videoId, int voiceId)
    {
        onlineBtn.interactable = false;
        Canvas.enabled = false;

        VideoId = videoId;
        VoiceId = voiceId;
        ProgressBar = progressBar;
        ProgressSlider = progressSlider;
        OnlineButton = onlineBtn;

        string filePath = Application.persistentDataPath + "Alex'Version";
        if (!Directory.Exists(filePath))
        {
            Directory.CreateDirectory(filePath);
        }
        string VideoPath = Path.Combine(filePath, Path.GetFileName("https://storage.googleapis.com/mindfulness_vr_bucket/Videos/2K/" + VideoId + ".mp4"));
        string VoicePath = Path.Combine(filePath, Path.GetFileName("https://storage.googleapis.com/mindfulness_vr_bucket/Voices/Low/" + VoiceId + ".mp3"));

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
        Downloader.DownloadOnlyVoice(VoiceId, ProgressBar, ProgressSlider, null, this);
    }

    void DownloadOnlyVideo()
    {
        Downloader.DownloadOnlyVideo(VideoId, ProgressBar, ProgressSlider, null, this);
    }

    void DownloadVideoAndVoice()
    {
        StartCoroutine(Downloader.DownloadVideoAndVoice(VideoId, VoiceId, ProgressBar, ProgressSlider, "voice", null, this));
    }

    void DownloadAlexVideo()
    {
        Downloader.DownloadAlexVideo(VideoId, ProgressBar, ProgressSlider, this);
    }

    public void DownloadComplated()
    {
        OnlineButton.interactable = true;
        Canvas.enabled = true;
    }
}

