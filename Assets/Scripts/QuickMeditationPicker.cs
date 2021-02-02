using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class QuickMeditationPicker : MonoBehaviour
{
    private string[] VideoUrls;
    private string FirstDayVoiceUrl = "";
    private string[] MusicUrl;

    public string VideoSceneName;

    private void Start()
    {
        VideoUrls = new string[9] { "http://biohack.network/1.mp4",
                                     "http://biohack.network/3.mp4",
                                     "http://biohack.network/4.mp4",
                                     "http://biohack.network/5.mp4",
                                     "http://biohack.network/6.mp4",
                                     "http://biohack.network/7.mp4",
                                     "http://biohack.network/8.mp4",
                                     "http://biohack.network/9.mp4",
                                     "http://biohack.network/10.mp4", };

        FirstDayVoiceUrl = "http://biohack.network/Voices/1.mp3";

        MusicUrl = new string[3] { "http://biohack.network/Musics/1.mp3", "http://biohack.network/Musics/2.mp3", "http://biohack.network/Musics/3.mp3" };
    }

    public void OpenQuickMeditation() {
        /*PlayerPrefs.DeleteKey("VoiceUrl");
        PlayerPrefs.DeleteKey("MusicUrl");
        PlayerPrefs.DeleteKey("VideoUrl");*/
        PlayerPrefs.DeleteKey("CanPlayMusic");
        PlayerPrefs.DeleteKey("CanPlayMusicOffline");

        int videoId = Random.Range(1, 9);

        int MusicId = Random.Range(1, 3);

        PlayerPrefs.SetInt("CanPlayMusic", 1);
        PlayerPrefs.SetString("VoiceUrl", FirstDayVoiceUrl);
        PlayerPrefs.SetString("MusicUrl", MusicUrl[MusicId]);
        PlayerPrefs.SetString("VideoUrl", VideoUrls[videoId - 1]);

        SceneManager.LoadScene(VideoSceneName);
    }
}
