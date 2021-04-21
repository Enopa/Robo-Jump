using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStuff : MonoBehaviour
{
    private GameObject deathText;
    [HideInInspector]
    public bool dead;
    [HideInInspector]
    public bool showWinText;
    private GameObject winText;
    private GameObject choicesText;
    private float textTimer;
    private AudioController aux;

    [HideInInspector]
    public GameObject pause;
    private bool pauseOn;
    [HideInInspector]
    public float staminaRecoverySpeed;


    public AudioClip pausePress;
    public AudioClip pauseNoise;
    private float pauseNoiseTimer = 2;

    private void OnEnable()
    {
        deathText = GameObject.Find("DeathText");
        deathText.SetActive(false);
        winText = GameObject.Find("WinText");
        choicesText = GameObject.Find("Choices");
        choicesText.SetActive(false);
        pause = GameObject.Find("Pause");
        pause.SetActive(false);
        aux = GetComponent<AudioController>();
    }

    // Update is called once per frame
    void Update()
    {
       if (deathText.activeSelf && Input.GetButtonDown("Restart"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            dead = false;
        }
        if (deathText.activeSelf && Input.GetButtonDown("Cancel"))
        {
            SceneManager.LoadScene("Menu");
            dead = false;
        }
        winText.SetActive(showWinText);

        if (winText.activeSelf)
        {
            textTimer += Time.deltaTime;
        }

        if (textTimer > 1)
        {
            choicesText.SetActive(true);
        }

        if (choicesText.activeSelf)
        {
            if (Input.GetButtonDown("Cancel"))
            {
                SceneManager.LoadScene("Menu");
            } else if (Input.GetButtonDown("Jump"))
            {
                if ((int.Parse(SceneManager.GetActiveScene().name) + 1) > 8) 
                {
                    SceneManager.LoadScene("Menu");
                } else
                {
                    SceneManager.LoadScene((((int.Parse(SceneManager.GetActiveScene().name) + 1).ToString())));
                }
            }
            PlayerPrefs.SetString((int.Parse(SceneManager.GetActiveScene().name)).ToString(), "true");
        }

        if (!deathText.activeSelf && !GetComponent<PlayerMovement>().win && GetComponent<PlayerMovement>().pausePossible && Input.GetButtonDown("Cancel"))
        {
            pauseOn = !pauseOn;
            aux.playSound(pausePress);
            pause.SetActive(pauseOn);
        }

        if (pause.activeSelf)
        {
            Time.timeScale = 0;
            staminaRecoverySpeed = 0;
            if (pauseNoiseTimer >= 1)
            {
                pauseNoiseTimer = 0;
                aux.playSound(pauseNoise);
            } else
            {
                pauseNoiseTimer += 0.001f;
            }
        } else
        {
            Time.timeScale = 1;
            staminaRecoverySpeed = 0.1f;
            pauseNoiseTimer = 1;
        }


        if (Input.GetButtonDown("Cancel") && !pause.activeSelf)
        {
            aux.source2.Stop();
            aux.playSound(pausePress);
            pauseNoiseTimer = 2;
        }


        if (GetComponent<PlayerMovement>().win)
        {
            aux.source.Stop();
            aux.source2.Stop();
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Bad")
        {
            deathText.SetActive(true);
            dead = true;
        }
    }
}
