using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody playerRB;
    private Vector3 movement;
    public float movementSpeed;
    public float jumpForce; //bad game
    public float dashForce;
    private float x = 0;
    public float dashTimer;
    private GameObject stamina;
    private float sonicSpeed; 
    private float memberSpeed;

    //testing variables
    private float wallJumpx = 5;
    private float wallJumpy = 5;


    [HideInInspector]
    public bool dead;
    [HideInInspector]
    public bool cameraReady;
    [HideInInspector]
    public bool textPlace;
    [HideInInspector]
    public GameObject startText;
    [HideInInspector]
    public bool pausePossible;


    public bool win;
    private GameObject goal;


    //Sound Stuff
    private AudioController aux;
    [Header("Audio Clips")]
    public AudioClip dashNoise;
    public AudioClip jumpNoise;
    public AudioClip landNoise;
    private bool grounded;
    private bool hung;
    public AudioClip wallHang;
    public AudioClip moveNoise;
    public AudioClip sprintNoise;
    public AudioClip deathNoise;
    public AudioClip staminaEmpty;
    public AudioClip staminaFull;
    private bool staminaFullCheck;
    private bool staminaEmptyCheck;
    public AudioClip startNoise;


    private void OnEnable()
    {
        startText = GameObject.Find("StartText");
        startText.SetActive(false);
        playerRB = gameObject.GetComponent<Rigidbody>();
        stamina = GameObject.Find("StaminaBar");
        sonicSpeed = movementSpeed * 2;
        memberSpeed = movementSpeed;
        dead = false;
        aux = GetComponent<AudioController>();


    }
    // Update is called once per frame
    void Update()
    {
        if (!dead && cameraReady && !win && !GetComponent<GameStuff>().pause.activeSelf)
        {
            pausePossible = true;
            //Basic Ground Movement and Wall Check
            if (!isGrounded() && (onWall() != "Null"))
            {
                playerRB.velocity = new Vector3(Input.GetAxisRaw("Horizontal") * movementSpeed, 0, 0);
            }
            movement = new Vector3(Input.GetAxisRaw("Horizontal") * movementSpeed, playerRB.velocity.y, 0);



            //Jumping branches
            if (Input.GetButtonDown("Jump"))
            {
                if (isGrounded())
                {
                    playerRB.AddForce(new Vector3(0f, jumpForce, 0f), ForceMode.Impulse);
                    aux.playSound(jumpNoise);
                }
                else if (!isGrounded() && onWall() != "Null")
                {
                    if (onWall() == "Right")
                    {
                        playerRB.AddForce(new Vector3(-wallJumpx, wallJumpy, 0f), ForceMode.Impulse);
                    }
                    else
                    {
                        playerRB.AddForce(new Vector3(wallJumpx, wallJumpy, 0f), ForceMode.Impulse);
                    }
                    aux.playSound(jumpNoise);
                }
            }

            //Dash checks
            if (Input.GetButtonDown("Fire1") && (x <= 0))
            {
                if (Input.GetAxis("Horizontal") < 0)
                {
                    playerRB.AddForce(Vector3.left * dashForce, ForceMode.Impulse);
                    x = dashTimer;
                }
                else
                {
                    playerRB.AddForce(Vector3.right * dashForce, ForceMode.Impulse);
                    x = dashTimer;
                }
                aux.playSound(dashNoise);
            }

            //Dash Cooldowns
            if (x >= 0)
            {
                x -= Time.deltaTime;
            }

            if (Input.GetButton("Fire2") && (onWall() == "Null"))
            {
                stamina.GetComponent<Slider>().value -= (GetComponent<GameStuff>().staminaRecoverySpeed * 4);
                staminaFullCheck = false;
                aux.whatNoise = sprintNoise;
            }
            else
            {
                stamina.GetComponent<Slider>().value += GetComponent<GameStuff>().staminaRecoverySpeed;
                aux.whatNoise = moveNoise;
            }

            //Sprint Functions and Stamina Checks
            //dont want stamina to deplete if you are against a wall
            if (Input.GetButtonDown("Fire2") && (stamina.GetComponent<Slider>().value != 0))
            {
                movementSpeed = sonicSpeed;
                staminaEmptyCheck = false;

            }
            if (Input.GetButtonUp("Fire2") || (stamina.GetComponent<Slider>().value == 0))
            {
                movementSpeed = memberSpeed;
                aux.whatNoise = moveNoise;
            }
            playerRB.velocity = movement;
        } else if (dead)
        {
            playerRB.velocity = new Vector3(0f, playerRB.velocity.y / 10f, 0f);
        } 

        if (textPlace == true)
        {
            startText.SetActive(true);
            if (Input.GetButtonDown("Jump"))
            {
                cameraReady = true;
                textPlace = false;
                startText.SetActive(false);
                aux.playSound(startNoise);
                
            }
        }

        if (win)
        {
            stamina.SetActive(false);
            transform.position = goal.transform.position;
        }

        if (isGrounded() && !grounded)
        {
            grounded = true;
            aux.playSound(landNoise);
        } else if (!isGrounded())
        {
            grounded = false;
        }

        if ((onWall() != "Null") && !hung)
        {
            hung = true;
            aux.playSound(wallHang);
        } else if (onWall() == "Null")
        {
            hung = false;
        }


        if (Input.GetButton("Fire2") && stamina.GetComponent<Slider>().value == 0 && !staminaEmptyCheck) 
        {
            aux.playSound(staminaEmpty);
            staminaEmptyCheck = true;
        } else if ((stamina.GetComponent<Slider>().value == stamina.GetComponent<Slider>().maxValue) && !staminaFullCheck)
        {
            staminaFullCheck = true;
            aux.playSound(staminaFull);
        }
    }

    //Checks is the PLayer is on the ground
    public bool isGrounded()
    {
        float distance = GetComponent<Collider>().bounds.extents.y + 0.01f;
        //we check three points on the player, left, right and centre
        Ray rayL = new Ray(new Vector3(transform.position.x - GetComponent<Collider>().bounds.extents.x, transform.position.y, transform.position.z), Vector3.down);
        Ray rayR = new Ray(new Vector3(transform.position.x + GetComponent<Collider>().bounds.extents.x, transform.position.y, transform.position.z), Vector3.down);
        Ray rayC = new Ray(transform.position, Vector3.down);
        return ((Physics.Raycast(rayL, distance)) || (Physics.Raycast(rayR, distance)) || (Physics.Raycast(rayC, distance)));
    }


    //Returns if the player is touching a wall or not and on what side the wall is
    string onWall()
    {
        float distance = GetComponent<Collider>().bounds.extents.x + 0.01f;
        Ray rayWallL = new Ray(transform.position, Vector3.left);
        Ray rayWallR = new Ray(transform.position, Vector3.right);
        if (Physics.Raycast(rayWallL, distance)) 
        {
            return "Left";
        } 
        else if (Physics.Raycast(rayWallR, distance))
        {
            return "Right";
        } else
        {
            return "Null";
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Bad")
        {
            dead = true;
            aux.playSound(deathNoise);
        }
        if (other.tag == "Good")
        {
            GetComponent<MeshRenderer>().enabled = false;
            goal = other.gameObject;
            win = true;
        }
    }
}
