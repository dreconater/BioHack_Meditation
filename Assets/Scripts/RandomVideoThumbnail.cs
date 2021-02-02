using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RandomVideoThumbnail : MonoBehaviour
{
    public Button StreamBtn;

    private int ID;

    private string[] VideoUrls;

    private string myVideoUrl;

    public Action<int, string> OnClickedVideo;

    void Awake()
    {
        StreamBtn.onClick.AddListener(StreamVideo);
    }

    void Start()
    {
        PlayerPrefs.DeleteKey("VideoUrl");
        PlayerPrefs.DeleteKey("VoiceUrl");
        PlayerPrefs.DeleteKey("MusicUrl");

        VideoUrls = new string[10] { "http://Biohack.network/1.mp4", "http://Biohack.network/2.mp4", "http://biohack.network/3.mp4", "http://Biohack.network/4.mp4", "http://Biohack.network/5.mp4", "http://Biohack.network/6.mp4", "http://Biohack.network/7.mp4", "http://Biohack.network/8.mp4", "http://Biohack.network/9.mp4", "http://Biohack.network/10.mp4" };

        Text text = GetComponentInChildren<Text>();
        if (text != null)
        {
            string[] newText = text.text.Split(' ');
            int id = int.Parse(newText[1]);
            Setup(id);
        }
    }

    void StreamVideo()
    {
        OnClickedVideo.Invoke(ID, myVideoUrl);
    }

    void Setup(int id) {
        ID = id;
        myVideoUrl = VideoUrls[id - 1];
    }
}
