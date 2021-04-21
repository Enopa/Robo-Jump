using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    public float rocketSpeed;
    private bool move;
    private bool noiseCheck = false;
    private float timer;
    private GameObject player;
    public AudioClip winClip;
    private bool winCheck = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (move)
        {
            timer += Time.deltaTime;
        }
        if (timer >= 2.5f)
        {
            transform.position += Vector3.up * rocketSpeed;
            player.GetComponent<GameStuff>().showWinText = true;
            if (!noiseCheck)
            {
                GetComponent<AudioSource>().Play();
                noiseCheck = true;
            }

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            move = true;
            player = other.gameObject;
            if (!winCheck)
            {
                GetComponent<AudioSource>().PlayOneShot(winClip);
                winCheck = true;
            }
        }
    }
}
