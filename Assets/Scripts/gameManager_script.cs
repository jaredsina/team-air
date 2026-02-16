using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro; 

public class GameManager : MonoBehaviour
{
    public Lights[] lights;
    public Button restartButton;

    public float flashDuration = 0.4f;
    public float delayBetweenFlashes = 0.2f;

    List<int> pattern = new List<int>();
    int playerIndex = 0;

    bool inputEnabled;

    public AudioClip yellowFlashSound;
    public AudioClip redFlashSound;
    public AudioClip correctClickSound;
    public AudioClip sequenceCompleteSound;

    private AudioSource audioSource;

    public TMP_Text statusText; 

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        if (restartButton != null)
            restartButton.gameObject.SetActive(false);

        StartNewRound();
    }


    void StartNewRound()
    {
        statusText.text = "";

        pattern.Clear();
        playerIndex = 0;
        inputEnabled = false;

        for (int i = 0; i < 5; i++)
        {
            pattern.Add(Random.Range(0, lights.Length));
        }

        StartCoroutine(PlayPattern());
    }

    IEnumerator PlayPattern()
    {
        yield return new WaitForSeconds(0.5f);

        for (int i = 0; i < pattern.Count; i++)
        {
            audioSource.PlayOneShot(yellowFlashSound);

            yield return StartCoroutine(
                lights[pattern[i]].Flash(flashDuration)
            );


            yield return new WaitForSeconds(delayBetweenFlashes);
        }

        inputEnabled = true;
        Debug.Log("PLAYER TURN");
    }

    public void RegisterPlayerInput(int index)
    {
        if (!inputEnabled) return;

        if (index == pattern[playerIndex])
        {
            Debug.Log("CORRECT");
            audioSource.PlayOneShot(correctClickSound);
            playerIndex++;

            if (playerIndex >= pattern.Count)
            {
                inputEnabled = false;
                StartCoroutine(LevelComplete());
            }

        }
        else
        {
            Debug.Log("WRONG");
            inputEnabled = false;
            StartCoroutine(GameOver());
        }
    }

    IEnumerator GameOver()
    {
        inputEnabled = false;

        statusText.text = "WRONG";

        audioSource.PlayOneShot(redFlashSound);

        foreach (Lights l in lights)
        {
            StartCoroutine(l.FlashRed(0.5f));
        }

        yield return new WaitForSeconds(0.6f);

        restartButton.gameObject.SetActive(true);
    }

    IEnumerator LevelComplete()
    {
        inputEnabled = false;

        statusText.text = "LEVEL COMPLETE";
        audioSource.PlayOneShot(sequenceCompleteSound);

        yield return new WaitForSeconds(1f);

        restartButton.gameObject.SetActive(true);
    }


    public void RestartGame()
    {
        restartButton.gameObject.SetActive(false);
        StartNewRound();
    }
}

