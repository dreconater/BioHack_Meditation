using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RandomPicker : MonoBehaviour
{
    public List<RandomVideoThumbnail> VideoThumbnails = new List<RandomVideoThumbnail>();

    private string[] VoiceUrls;
    private string[] MusicUrls;

    public Button BackButton;
    public string GenMenuName;

    void Awake()
    {
        BackButton.onClick.AddListener(GoToGenMenu);

        foreach (var item in VideoThumbnails)
        {
            item.OnClickedVideo += ClickedVideo;
        }    
    }

    void GoToGenMenu() {
        SceneManager.LoadScene(GenMenuName);
    }

    void ClickedVideo(int id, string videoUrl)
    {
        int VoiceId = Random.Range(1, 20);
        int MusicId = Random.Range(1, 3);

        PlayerPrefs.SetString("VideoUrl", videoUrl);
        PlayerPrefs.SetString("VoiceUrl", VoiceUrls[VoiceId-1]);
        PlayerPrefs.SetString("MusicUrl", MusicUrls[MusicId - 1]);

        SceneManager.LoadScene("RandomVideoScene");
    }

    void Start()
    {
        VoiceUrls = new string[20] { "http://biohack.network/Voices/1.mp3",
                                     "http://biohack.network/Voices/2.mp3",
                                     "http://biohack.network/Voices/3.mp3",
                                     "http://biohack.network/Voices/4.mp3",
                                     "http://biohack.network/Voices/5.mp3",
                                     "http://biohack.network/Voices/6.mp3",
                                     "http://biohack.network/Voices/7.mp3",
                                     "http://biohack.network/Voices/8.mp3",
                                     "http://biohack.network/Voices/9.mp3",
                                     "http://biohack.network/Voices/10.mp3",
                                     "http://biohack.network/Voices/11.mp3",
                                     "http://biohack.network/Voices/12.mp3",
                                     "http://biohack.network/Voices/13.mp3",
                                     "http://biohack.network/Voices/14.mp3",
                                     "http://biohack.network/Voices/15.mp3",
                                     "http://biohack.network/Voices/16.mp3",
                                     "http://biohack.network/Voices/17.mp3",
                                     "http://biohack.network/Voices/18.mp3",
                                     "http://biohack.network/Voices/19.mp3",
                                     "http://biohack.network/Voices/20.mp3", };

        MusicUrls = new string[3] { "http://biohack.network/Musics/1.mp3",
                                    "http://biohack.network/Musics/2.mp3",
                                    "http://biohack.network/Musics/3.mp3", };

        //--------------------------------------------------------------------------------------------


    }
}
