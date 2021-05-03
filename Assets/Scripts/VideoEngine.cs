using RenderHeads.Media.AVProVideo;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.Networking;
using System;

public class VideoEngine : MonoBehaviour
{
    public MediaPlayer VideoPlayer;
    public Breathing Breathing;

    public string MenuName;
    
    public Button BackButton;

    public AudioSource Audio_Voice;
    public AudioSource Audio_Music;

    public GameObject LoadingObject;
    public GameObject BufferingObject;
    public GameObject IssueObject;
    public GameObject InfoObject;

    public Image LoadingFillImage;
    public Text LoadingFillText;

    private int index = 0;

    private bool isStartedAudio = false;

    private void Awake()
    {
        BackButton.onClick.AddListener(GoToMenu);

        VideoPlayer.Events.AddListener(DebugPlayerMessages);
    }

    private IEnumerator Start()
    {
        HowManyTimesLagged = 0;
        isStartedAudio = false;
        BufferingObject.transform.localScale = Vector3.zero;
        if (Audio_Voice == null)
        {
            index = 0;
            string url = PlayerPrefs.GetString("VideoUrl");
            //VideoPlayer.m_Loop = false;
            VideoPlayer.OpenVideoFromFile(MediaPlayer.FileLocation.AbsolutePathOrURL, url, true);
            LoadingObject.SetActive(false);
        }
        else if (PlayerPrefs.GetInt("CanPlayMusic") == 0) {
            index = 0;
            LoadingObject.SetActive(true);
            StartCoroutine(PickUpRandomVoice(PlayerPrefs.GetString("VoiceUrl")));
            string url = PlayerPrefs.GetString("VideoUrl");
            //VideoPlayer.m_Loop = false;
            VideoPlayer.OpenVideoFromFile(MediaPlayer.FileLocation.AbsolutePathOrURL, url, false);
        }
        else
        {
            index = 0;
            LoadingObject.SetActive(true);
            StartCoroutine(PickUpRandomVoice(PlayerPrefs.GetString("VoiceUrl")));
            StartCoroutine(PickUpRandomMusic(PlayerPrefs.GetString("MusicUrl")));
            string url = PlayerPrefs.GetString("VideoUrl");
           // VideoPlayer.m_Loop = false;
            VideoPlayer.OpenVideoFromFile(MediaPlayer.FileLocation.AbsolutePathOrURL, url, false);
        }

        yield return new WaitForSeconds(1f);
        if (InfoObject != null)
        {
            InfoObject.SetActive(true);
        }

        yield return new WaitForSeconds(4f);
        if (InfoObject != null)
        {
            InfoObject.SetActive(false);
        }

        yield return new WaitForSeconds(20.5f);
        if (!Audio_Voice.isPlaying)
        {
            IssueText(true);
        }
        else
        {
            IssueText(false);
        }
    }

    void IssueText(bool value)
    {
        if (IssueObject != null)
        {
            IssueObject.SetActive(value);
        }
    }

    private int HowManyTimesLagged = 0;

    private void DebugPlayerMessages(MediaPlayer arg0, MediaPlayerEvent.EventType arg1, ErrorCode arg2)
    {
        if (arg1 == MediaPlayerEvent.EventType.Stalled)
        {
            HowManyTimesLagged++;

            BufferingObject.SetActive(true);
            if (PlayerPrefs.GetInt("CanPlayMusic") == 0)
            {
                Audio_Voice.Pause();
            }
            else if (PlayerPrefs.GetInt("CanPlayMusic") == 1)
            {
                Audio_Voice.Pause();
                Audio_Music.Pause();
            }

            if (HowManyTimesLagged > 3)
            {
                IssueText(true);
            }
        }
        else if (arg1 == MediaPlayerEvent.EventType.Unstalled)
        {
            IssueText(false);
            BufferingObject.SetActive(false);
            if (PlayerPrefs.GetInt("CanPlayMusic") == 0)
            {
                Audio_Voice.Play();
            }
            else if (PlayerPrefs.GetInt("CanPlayMusic") == 1)
            {
                Audio_Voice.Play();
                Audio_Music.Play();
            }
        }
    }

    void Update() {
        /*if (VideoPlayer.Control.IsFinished())
        {
            Audio_Music.volume = 0;
            Audio_Voice.volume = 0;
            GoToMenu();
        }*/

        if (Audio_Voice.isPlaying)
        {
            isStartedAudio = true;
        }
        
        if (isStartedAudio)
        {
            if (Audio_Voice.time > (Audio_Voice.clip.length - 0.5f))
            {
                GoToMenu();
            }

        }

        if (OVRInput.Get(OVRInput.Button.One))
        {
            GoToMenu();
        }

        if (Input.GetKey(KeyCode.Space))
        {
            GoToMenu();
        }
    }

    void GoToMenu() {
        SceneManager.LoadScene(MenuName);
    }

    private IEnumerator PickUpRandomVoice(string url) {
        Debug.LogError(url);
        UnityWebRequest request = UnityWebRequestMultimedia.GetAudioClip(url, AudioType.OGGVORBIS);
        
        if (PlayerPrefs.GetInt("CanPlayMusic") == 0)
        {
            AsyncOperation operation = request.SendWebRequest();

            while (!operation.isDone)
            {
                LoadingFillImage.fillAmount = operation.progress;
                float progress = operation.progress / 1f;
                LoadingFillText.text = string.Format("{0:0}%", progress * 100);
                yield return null;
            }
        }
        else
        {
            yield return request.SendWebRequest();
        }


        AudioClip audio = null;
        try
        {
            audio = DownloadHandlerAudioClip.GetContent(request);
        }
        catch (Exception e)
        {
            Debug.LogErrorFormat("{0} is wrong audio url. Got exception {1}", url, e.Message);
        }
        Audio_Voice.clip = audio;
        if (PlayerPrefs.GetInt("CanPlayMusic") == 1)
        {
            index += 1;
            if (index == 2)
            {
                VideoPlayer.Play();
                BufferingObject.transform.localScale = new Vector3(-0.2f, 0.2f, 0.2f);
                if (LoadingObject != null)
                {
                    LoadingObject.SetActive(false);
                    if (IssueObject != null)
                    {
                        IssueObject.SetActive(false);
                    }
                    yield return new WaitForSeconds(2f);
                    StartCoroutine(Breathing.StartBreathing());
                    if (!Audio_Voice.isPlaying)
                    {
                        Audio_Voice.Play();
                    }
                    Audio_Music.Play();
                }
            }
        }
        else
        {
            index = 1;
            if (index == 1)
            {
                VideoPlayer.Play();
                BufferingObject.transform.localScale = new Vector3(-0.2f, 0.2f, 0.2f);
                if (LoadingObject != null)
                {
                    LoadingObject.SetActive(false);
                    if (IssueObject != null)
                    {
                        IssueObject.SetActive(false);
                    }
                    yield return new WaitForSeconds(2f);
                    StartCoroutine(Breathing.StartBreathing());
                    if (!Audio_Voice.isPlaying)
                    {
                        Audio_Voice.Play();
                    }
                }
            }
        }
        
        yield return new WaitForSeconds(0f);
    }

    private IEnumerator PickUpRandomMusic(string url)
    {
        Debug.Log(url);
        UnityWebRequest request = UnityWebRequestMultimedia.GetAudioClip(url, AudioType.OGGVORBIS);
        AsyncOperation operation = request.SendWebRequest();

        if (PlayerPrefs.GetInt("CanPlayMusic") == 1)
        {
            while (!operation.isDone)
            {
                LoadingFillImage.fillAmount = operation.progress;
                float progress = operation.progress / 1f;
                LoadingFillText.text = string.Format("{0:0}%", progress * 100);
                yield return null;
            }
        }

        //yield return request.SendWebRequest();

        AudioClip audio = null;
        try
        {
            audio = DownloadHandlerAudioClip.GetContent(request);
        }
        catch (Exception e)
        {
            Debug.LogErrorFormat("{0} is wrong audio url. Got exception {1}", url, e.Message);
        }
        Audio_Music.clip = audio;
        index += 1;
        if (index == 2)
        {
            VideoPlayer.Play();
            BufferingObject.transform.localScale = new Vector3(-0.2f, 0.2f, 0.2f);
            if (LoadingObject != null)
            {
                LoadingObject.SetActive(false);
                if (IssueObject != null)
                {
                    IssueObject.SetActive(false);
                }
                yield return new WaitForSeconds(2f);
                if (!Audio_Voice.isPlaying)
                {
                    Audio_Voice.Play();
                }
                Audio_Music.Play();
                StartCoroutine(Breathing.StartBreathing());
            }
        }
        yield return new WaitForSeconds(0f);
    }

}
