using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
Create a script named CameraReset that accomplishes the
following: pressing the Tab Key resets the main camera position’s global coordinates to (0,
0, 0). Pressing the Tab Key multiples times should move the camera back to the origin each
time, regardless of where the player moves their head between presses. 
*/
public class CameraReset : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            GameMode.playerController.characterController.enabled = false;
            GameMode.playerController.transform.position = Vector3.zero;
            GameMode.playerController.characterController.enabled = true;
        }
    }
}
