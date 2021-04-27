using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breathing : MonoBehaviour
{
    public ParticleSystem FrostEffect;
    public ParticleSystem BreathIn;
    public ParticleSystem BreathOut;

    public AudioSource BreathingSoundEffect;

    private void Start()
    {
        /*if (PlayerPrefs.GetString("Breathing") == "on")
        {
            BreathingSoundEffect.gameObject.SetActive(true);
            FrostEffect.gameObject.SetActive(true);

            BreathingSoundEffect.Pause();
            FrostEffect.Stop();
        }
        else if (PlayerPrefs.GetString("Breathing") == "off")
        {
            BreathingSoundEffect.gameObject.SetActive(false);
            FrostEffect.gameObject.SetActive(false);
        }*/

        BreathingSoundEffect.gameObject.SetActive(true);
        FrostEffect.Stop();
        BreathIn.Stop();
        BreathOut.Stop();
    }

    public IEnumerator StartBreathing()
    {
        /*if (PlayerPrefs.GetString("Breathing") == "on")
        {
            BreathingSoundEffect.Play();
            yield return new WaitForSeconds(4f);
            FrostEffect.Play();
            yield return new WaitForSeconds(5);
            StartCoroutine(StartBreathing());
        }*/

        BreathingSoundEffect.Play();
        BreathingSoundEffect.volume = 0.05f;

        if (PlayerPrefs.GetString("Breathing") == "vapor")
        {
            FrostEffect.gameObject.SetActive(true);
            BreathIn.gameObject.SetActive(false);
            BreathOut.gameObject.SetActive(false);

            BreathingSoundEffect.Play();
            yield return new WaitForSeconds(4f);
            FrostEffect.Play();
            yield return new WaitForSeconds(5);
            StartCoroutine(StartBreathing());
        }
        else if (PlayerPrefs.GetString("Breathing") == "particle")
        {
            FrostEffect.gameObject.SetActive(false);
            BreathIn.gameObject.SetActive(true);
            BreathOut.gameObject.SetActive(true);
            BreathIn.Play();
            yield return new WaitForSeconds(5);
            BreathOut.Play();
            yield return new WaitForSeconds(4f);
            StartCoroutine(StartBreathing());

        }
    }
}

