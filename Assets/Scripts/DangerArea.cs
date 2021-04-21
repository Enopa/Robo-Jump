using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DangerArea : MonoBehaviour
{
    public float dangerSpeed;
    private Rigidbody dRB;
    private GameObject player;
    private float dangerStore;
    public float maxY;

    // Start is called before the first frame update
    void Start()
    {
        
    }


    private void OnEnable()
    {
        dRB = GetComponent<Rigidbody>();
        dangerStore = dangerSpeed;
        player = GameObject.Find("Player");
    }
    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < maxY)
        {
            dRB.velocity = new Vector3(0f, dangerSpeed * Time.deltaTime, 0f);
        }
        if (!player.GetComponent<PlayerMovement>().cameraReady)
        {
            dangerSpeed = 0f;
        } else
        {
            dangerSpeed = dangerStore;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            dangerStore = 0f;
        }
    }

}
