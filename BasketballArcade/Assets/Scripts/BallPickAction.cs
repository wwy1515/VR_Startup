using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class BallPickAction : MonoBehaviour
{
    public SteamVR_Input_Sources handType;
    public SteamVR_Action_Boolean pickball = SteamVR_Input.GetAction<SteamVR_Action_Boolean>("PickBall");
    SteamVR_Behaviour_Pose trackedObj;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerStay(Collider other)
    {
        if (pickball.GetStateDown(handType))
        {
            // apply the pick ball action
            // transfer the ball position to trackedObj position
            // pass the ball GameObject to TestThrow script
            PickThrowBasic pickThrowBasicScript = GameObject.Find("Controller (right)").GetComponent<PickThrowBasic>();
            pickThrowBasicScript.setPickedObj(gameObject);

        }
    }
}
