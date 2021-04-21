using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    public float rotationSpeedX;
    public float rotationSpeedY;
    public float rotationSpeedZ;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(rotationSpeedX/100, rotationSpeedY/100, rotationSpeedZ/100); 
    }

    private void quit()
    {
        Application.Quit();
    }
}
