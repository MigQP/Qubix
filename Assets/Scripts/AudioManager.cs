using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource audioLoop;
    public float fadeInTime;
    // Start is called before the first frame update
    void Awake()
    {
        StartCoroutine(FadeIn(audioLoop, fadeInTime));
    }

    public void GameOverFade()
    {
        StartCoroutine(FadeOutGameOver(audioLoop, 1f));
    }

    public void NormalFadeOut()
    {
        StartCoroutine(FadeOut(audioLoop, 1f));
    }

    public  IEnumerator FadeOut(AudioSource audioSource, float FadeTime)
    {
        float startVolume = audioSource.volume;

        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / FadeTime;

            yield return null;
        }

        audioSource.Stop();
        audioSource.volume = startVolume;
    }

    public IEnumerator FadeOutGameOver(AudioSource audioSource, float FadeTime)
    {
        float startVolume = audioSource.volume;

        while (audioSource.volume > 0.2)
        {
            audioSource.volume -= startVolume * Time.deltaTime / FadeTime;

            yield return null;
        }

        //audioSource.Stop();
        //audioSource.volume = startVolume;
    }

    public  IEnumerator FadeIn(AudioSource audioSource, float FadeTime)
    {
        float startVolume = 0.2f;

        audioSource.volume = 0;
        audioSource.Play();

        while (audioSource.volume < 0.6f)
        {
            audioSource.volume += startVolume * Time.deltaTime / FadeTime;

            yield return null;
        }

        audioSource.volume = .6f;
    }
}
