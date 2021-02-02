using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class VideoThumbnailContainer : MonoBehaviour
{
    public Button DownloadBtn;
    public Button StreamBtn;

    public DownloadBlockingSystem BlockingSystem;

    private int ID;
    private bool isDownloaded;

    public Text percentageText;

    public GameObject LoadingBar;

    public Slider FillingBar;

    private LargeFileDownloaderExample VideoDownloader;

    private string Path;

    void Awake()
    {
        DownloadBtn.onClick.AddListener(DownloadVideo);
        StreamBtn.onClick.AddListener(StreamVideo);
    }

    void Start()
    {
        //PlayerPrefs.DeleteAll();
        Text text = GetComponentInChildren<Text>();
        if (text != null)
        {
            string[] newText = text.text.Split(' ');
            int id = int.Parse(newText[1]);
            Setup(id);
        }

        GameObject downloader = GameObject.FindGameObjectWithTag("VideoDownloader");
        if (downloader != null)
        {
            VideoDownloader = downloader.GetComponent<LargeFileDownloaderExample>();
        }

        LoadingBar.SetActive(false);
        DownloadBtn.gameObject.SetActive(true);

        if (PlayerPrefs.GetInt("Downloaded" + ID) == 0 || PlayerPrefs.GetInt("Downloaded" + ID) == null)
        {
            StreamBtn.interactable = false;
            DownloadBtn.interactable = true;
        }
        else
        {
            StreamBtn.interactable = true;
            DownloadBtn.interactable = false;
        }
    }

    public void Setup(int id) {
        ID = id;
        
    }

    public void Downloaded() {
        StreamBtn.interactable = true;
        DownloadBtn.interactable = false;
    }

    void DownloadVideo() {
        Debug.Log("Starting " + ID + " Video");
        BlockingSystem.HideButtons();
        VideoDownloader.DownloadVideo(ID, percentageText, FillingBar, LoadingBar, this);
    }

    void StreamVideo() {
        PlayerPrefs.SetInt("ID", ID);
        SceneManager.LoadScene("VideoScene");
    }

    public IEnumerator VideoDownloaded(string path) {
        Debug.Log("Video Successfully Downloaded");

        PlayerPrefs.SetInt("Downloaded" + ID, 1);
        DownloadBtn.gameObject.tag = "ThisBtn";
        BlockingSystem.ShowButtons();
        Path = path;
        isDownloaded = true;
        LoadingBar.SetActive(false);
        StreamBtn.interactable = true;

        yield return new WaitForSeconds(0.5f);
        DownloadBtn.interactable = false;
    }
}
