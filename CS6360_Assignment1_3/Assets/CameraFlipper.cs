using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFlipper : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            GameMode.playerController.transform.rotation *= Quaternion.Euler(0.0f, 180.0f, 0.0f);
        }
    }
}
