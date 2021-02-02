using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurvedUICnvasInteractiveChange : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (Application.platform == RuntimePlatform.WindowsPlayer)
        {
            Debug.Log("Windows");
        }
        else
        {
            Debug.Log("Android");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
