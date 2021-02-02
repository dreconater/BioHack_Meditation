using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breathing : MonoBehaviour
{
    public ParticleSystem FrostEffect;

    public AudioSource BreathingSoundEffect;

    private void Start()
    {
        if (PlayerPrefs.GetString("Breathing") == "on")
        {
            BreathingSoundEffect.gameObject.SetActive(true);
            FrostEffect.gameObject.SetActive(true);

            BreathingSoundEffect.Pause();
            FrostEffect.Stop();
        }
        else if ((PlayerPrefs.GetString("Breathing") == "off"))
        {
            BreathingSoundEffect.gameObject.SetActive(false);
            FrostEffect.gameObject.SetActive(false);
        }
    }

    public IEnumerator StartBreathing()
    {
        if (PlayerPrefs.GetString("Breathing") == "on")
        {
            BreathingSoundEffect.Play();
            yield return new WaitForSeconds(4f);
            FrostEffect.Play();
            yield return new WaitForSeconds(5);
            StartCoroutine(StartBreathing());
        }
    }
}
