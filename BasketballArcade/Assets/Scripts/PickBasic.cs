using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class PickBasic : MonoBehaviour, PickInterface
{
    // point the ball is attach to
    public Rigidbody attachPoint;
    public handanimations handanimationsScript;
    private GameObject collidingObject;
    FixedJoint joint;

    public void PickThrowPC()
    {
        if (!GameMode.GetInstance().playerController.hasBall && Input.GetKeyDown(KeyCode.P))
        {
            GameMode.GetInstance().playerController.hasBall = true;
            GameMode.GetInstance().handledBall = GameObject.Instantiate(GameMode.GetInstance().ballPrefab);
        }

        if (GameMode.GetInstance().playerController.hasBall && GameMode.GetInstance().handledBall != null)
        {
            GameMode.GetInstance().handledBall.transform.position = GameMode.GetInstance().RightHand.transform.position;

            if (Input.GetKeyDown(KeyCode.L))
            {
                GameMode.GetInstance().playerController.hasBall = false;
                GameMode.GetInstance().handledBall.GetComponent<Rigidbody>().AddForce(Camera.main.transform.forward * 5.0f + Vector3.up * 1.5f, ForceMode.Impulse);
                GameMode.GetInstance().handledBall = null;
            }
        }
    }

    public void PickThrowVR()
    {
        // when press the trigger button
        if (joint == null && !GameMode.GetInstance().playerController.hasBall && 
            GameMode.GetInstance().playerController.playerControllerPlatform.HandleInteractiveButton() && collidingObject)
        {
            handanimationsScript.setAnim(Animator.StringToHash("GrabLarge"));

            GameMode.GetInstance().playerController.hasBall = true;
            //GameMode.GetInstance().handledBall = GameObject.Instantiate(GameMode.GetInstance().ballPrefab);
            GameMode.GetInstance().handledBall = collidingObject;
            GameMode.GetInstance().handledBall.transform.position = attachPoint.transform.position;
            collidingObject = null;

            joint = GameMode.GetInstance().handledBall.AddComponent<FixedJoint>();
            joint.breakForce = 5000;
            joint.breakTorque = 5000;
            joint.connectedBody = attachPoint;
        }
        // when releasing the trigger button
        else if (joint != null && GameMode.GetInstance().playerController.hasBall && 
            !GameMode.GetInstance().playerController.playerControllerPlatform.HandleInteractiveButton())
        {
            handanimationsScript.setAnim(Animator.StringToHash("Idle"));

            GameObject go = joint.gameObject;
            Rigidbody ball = go.GetComponent<Rigidbody>();

            Object.DestroyImmediate(joint);
            joint = null;
            Object.Destroy(go, 15.0f);

            InputDevice inputDevice = UnityEngine.XR.InputDevices.GetDeviceAtXRNode(UnityEngine.XR.XRNode.RightHand);
            Vector3 deviceVelocity, deviceAngularVelocity;
            inputDevice.TryGetFeatureValue(CommonUsages.deviceVelocity, out deviceVelocity);
            inputDevice.TryGetFeatureValue(CommonUsages.deviceAngularVelocity, out deviceAngularVelocity);

            ball.velocity = deviceVelocity;
            ball.angularVelocity = deviceAngularVelocity;
            ball.maxAngularVelocity = ball.angularVelocity.magnitude;

            GameMode.GetInstance().playerController.hasBall = false;
            GameMode.GetInstance().handledBall = null;
        }
    }

    private void SetCollidingObject(Collider col)
    {
        if (collidingObject || !col.GetComponent<Rigidbody>())
        {
            return;
        }
        collidingObject = col.gameObject;
    }

    public void OnTriggerEnter(Collider other)
    {
        SetCollidingObject(other);
    }

    public void OnTriggerStay(Collider other)
    {
        SetCollidingObject(other);
    }

    public void OnTriggerExit(Collider other)
    {
        if (!collidingObject)
        {
            return;
        }
        collidingObject = null;
    }

    public void Init()
    {
        //if (GameMode.GetInstance().enviromentType == GameMode.ENVIRONMENT_TYPE.VR)
        //{
        //    // set the attachpoint to righthand
        //    GameObject righthand = GameObject.Find("vr_cartoon_hand_prefab_right").transform.GetChild(0).gameObject;
        //    attachPoint = righthand.GetComponent<Rigidbody>();
        //}
    }

    public void Update()
    {   
        // For PC pick and throw logic 
        if (GameMode.GetInstance().enviromentType == GameMode.ENVIRONMENT_TYPE.PC)
        {
            PickThrowPC();
        }
        // For VR pick and throw logic
        else
        {
            PickThrowVR();
        }
    }

    
}
