using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tilt : MonoBehaviour
{
    private bool rotateForwards;
    public float tiltSpeed;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (transform.rotation.z >= 0.02)
        {
            rotateForwards = false;

        }
        else if (transform.rotation.z <= -0.02)
        {
            rotateForwards = true;
        }

        if (rotateForwards)
        {
            transform.Rotate(new Vector3(0, 0, tiltSpeed * Time.deltaTime));
        } else
        {
            transform.Rotate(new Vector3(0, 0, -tiltSpeed * Time.deltaTime));
        }
    }
}
