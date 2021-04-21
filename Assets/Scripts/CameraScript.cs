using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CameraScript : MonoBehaviour
{
    private static string previousScene;
    private GameObject player;
    public float startHeight;

    private float dampingTop = 2;
    private float dampingBot = 1;

    private bool firstStart;
    private GameObject goal;
    private float endPos;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnEnable()
    {
        player = GameObject.Find("Player");
        goal = GameObject.Find("Goal");
        endPos = goal.transform.position.y + 2;
        if (previousScene != SceneManager.GetActiveScene().name)
        {
            transform.position = new Vector3(0f, startHeight, -10f);
            firstStart = true;
        } else
        {
            firstStart = false;
            player.GetComponent<PlayerMovement>().cameraReady = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (player.GetComponent<PlayerMovement>().win == false)
        {
            if (player.GetComponent<GameStuff>().dead)
            {
                previousScene = SceneManager.GetActiveScene().name;
            }
            if (((transform.position.y + dampingTop) < player.transform.position.y) && !player.GetComponent<PlayerMovement>().dead)
            {
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(0f, player.transform.position.y, -10f), .25f);
            }



            if (((transform.position.y - dampingBot) > player.transform.position.y) && !player.GetComponent<PlayerMovement>().dead)
            {
                if (firstStart)
                {
                    transform.position = Vector3.MoveTowards(transform.position, new Vector3(0f, player.transform.position.y, -10f), .25f);
                    player.GetComponent<PlayerMovement>().cameraReady = false;
                }
                else
                {
                    transform.position = Vector3.MoveTowards(transform.position, new Vector3(0f, player.transform.position.y, -10f), .25f);
                }
            }
            else if (firstStart)
            {
                player.GetComponent<PlayerMovement>().textPlace = true;
                firstStart = false;
            }
        } else
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(0f, endPos, -10f), .01f);
        }
    }
}
