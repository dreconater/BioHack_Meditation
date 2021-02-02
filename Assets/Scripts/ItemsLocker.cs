using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemsLocker : MonoBehaviour
{
    public bool IsLocked = false;
    public bool IsDays = false;

    public Toggle Toggle;
    public Text Title;
    public Image Thumbnail;

    public GameObject LockIcon;

    private Color LockedTextColor = new Color32(150,150,150,255);
    private Color UnLockedTextColor = new Color32(255, 255, 255, 255);

    private Color LockedThumbnailColor = new Color32(250,250,250,130);
    private Color UnLockedThumbnailColor = new Color32(255, 255, 255, 255);

    private void Start()
    {
        if (IsLocked)
        {
            LockItem();
        }
        else
        {
            UnLockItem();
        }
    }

    void LockItem() {
        Toggle.interactable = false;
        Title.color = LockedTextColor;
        LockIcon.SetActive(true);
        Thumbnail.color = new Color(Thumbnail.color.r, Thumbnail.color.g, Thumbnail.color.b, 0.5f);
    }

    void UnLockItem()
    {
        Toggle.interactable = true;
        Title.color = UnLockedTextColor;
        LockIcon.SetActive(false);
        if (IsDays)
        {
            Thumbnail.color = new Color(Thumbnail.color.r, Thumbnail.color.g, Thumbnail.color.b, 0.7f);
        }
        else
        {
            Thumbnail.color = new Color(Thumbnail.color.r, Thumbnail.color.g, Thumbnail.color.b, 1f);
        }
     }
}
