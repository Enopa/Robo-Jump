using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{

    public AudioSource source;
    public AudioSource source2;
    private float moveSoundTimer;
    private PlayerMovement p;
    [HideInInspector]
    public AudioClip whatNoise;

    
    public AudioClip cameraPan;
    public AudioClip restart;
    private float panSoundTimer = 3;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnEnable()
    {
        source = GetComponent<AudioSource>();
        moveSoundTimer = 0f;
        p = GetComponent<PlayerMovement>();
        if (p.cameraReady)
        {
            playSound(restart);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!p.win && p.cameraReady && !p.dead && !GetComponent<GameStuff>().pause.activeSelf)
        {
            if ((Input.GetAxisRaw("Horizontal") != 0) && (moveSoundTimer <= 0))
            {
                source.PlayOneShot(whatNoise);
                moveSoundTimer = 4;
            }
            else if (Input.GetAxisRaw("Horizontal") == 0)
            {
                source.Stop();
                moveSoundTimer = 0;
            }

            if (moveSoundTimer > 0)
            {
                moveSoundTimer -= Time.deltaTime;
            }

            if (Input.GetButtonDown("Fire2"))
            {
                source.Stop();
                moveSoundTimer = 0;
            }
            if (Input.GetButtonUp("Fire2"))
            {
                source.Stop();
                moveSoundTimer = 0;
            }
        } else if (p.dead)
        {
            source.Stop();
        } else if (!p.textPlace)
        {
            if (panSoundTimer >= 2.5)
            {
                panSoundTimer = 0;
                source.PlayOneShot(cameraPan);
            } else
            {
                panSoundTimer += Time.deltaTime;
            }


        }
    }


    public void playSound(AudioClip sound)
    {
        source2.PlayOneShot(sound);
    }
}
