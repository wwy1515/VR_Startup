using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickBasic : PickInterface
{
    
    public void Init()
    {

    }

    public void Update()
    {   
        if(!GameMode.GetInstance().playerController.hasBall && Input.GetKeyDown(KeyCode.P))
        {
            GameMode.GetInstance().playerController.hasBall = true;
            GameMode.GetInstance().handledBall = GameObject.Instantiate(GameMode.GetInstance().ballPrefab);
        }

        if (GameMode.GetInstance().playerController.hasBall && GameMode.GetInstance().handledBall != null)
        {
            GameMode.GetInstance().handledBall.transform.position = GameMode.GetInstance().RightHand.transform.position;

            if(Input.GetKeyDown(KeyCode.L))
            {
                GameMode.GetInstance().playerController.hasBall = false;
                GameMode.GetInstance().handledBall.GetComponent<Rigidbody>().AddForce(Camera.main.transform.forward * 5.0f + Vector3.up * 1.5f, ForceMode.Impulse);
                GameMode.GetInstance().handledBall = null;
            }
        }
    }
}
