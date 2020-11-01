using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class PickBasic : MonoBehaviour, PickInterface
{
    // point the ball is attach to
    public Rigidbody attachPoint;
    public Transform attachPos;
    public handanimations handanimationsScript;
    private GameObject collidingObject;
    FixedJoint joint;
    Queue<Vector3> historyVel = new Queue<Vector3>();

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
        InputDevice inputDevice = UnityEngine.XR.InputDevices.GetDeviceAtXRNode(UnityEngine.XR.XRNode.RightHand);
        Vector3 deviceVelocity, deviceAngularVelocity;
        inputDevice.TryGetFeatureValue(CommonUsages.deviceVelocity, out deviceVelocity);
        inputDevice.TryGetFeatureValue(CommonUsages.deviceAngularVelocity, out deviceAngularVelocity);

        if(GameMode.GetInstance().playerController.hasBall)
        {
            historyVel.Enqueue(deviceVelocity);
            if(historyVel.Count > 8)
            {
                historyVel.Dequeue();
            }
        }

        if(joint == null && !GameMode.GetInstance().playerController.hasBall && GameMode.GetInstance().playerController.playerControllerPlatform.HandleInteractiveButton() && (collidingObject == null))
        {
            bool noTheForce = true;
            foreach(var ball in GameMode.GetInstance().balls)
            {
                if(ball.state == Ball.State.TheForce)
                {
                    noTheForce = false;
                    break;
                }
            }

            if(noTheForce)
            {
                Ball nearestBall = null;
                float nearestDistance = 9999.9f;

                foreach(var ball in GameMode.GetInstance().balls)
                {
                    float dist = (ball.transform.position - this.transform.position).sqrMagnitude;
                    if(dist < nearestDistance)
                    {
                        nearestDistance = dist;
                        nearestBall = ball;
                    }
                }

                if(nearestBall.state == Ball.State.Outside)
                {
                    nearestBall.GetComponent<Rigidbody>().velocity = Vector3.zero;
                    nearestBall.state = Ball.State.TheForce;
                }
            }
        }
        // when press the trigger button
        else if (joint == null && !GameMode.GetInstance().playerController.hasBall && 
            GameMode.GetInstance().playerController.playerControllerPlatform.HandleInteractiveButton() && collidingObject)
        {
            handanimationsScript.setAnim(Animator.StringToHash("GrabLarge"));

            GameMode.GetInstance().playerController.hasBall = true;
            // GameMode.GetInstance().handledBall = GameObject.Instantiate(GameMode.GetInstance().ballPrefab);
            GameMode.GetInstance().handledBall = collidingObject;
            GameMode.GetInstance().handledBall.transform.position = attachPos.transform.position;
            collidingObject = null;

            joint = GameMode.GetInstance().handledBall.AddComponent<FixedJoint>();
            joint.breakForce = 5000;
            joint.breakTorque = 5000;
            joint.connectedBody = attachPos.GetComponent<Rigidbody>();
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
            // Object.Destroy(go, 15.0f);

            var velArr = historyVel.ToArray();
            Vector3 totalVel = Vector3.zero;
            for(int i = 0; i < velArr.Length; i++)
            {
                var item = velArr[i];
                totalVel += item;
            }

            Vector3 toHoop = (GameMode.GetInstance().targetPos.position - attachPos.transform.position).normalized;
            Vector3 trickDir = (totalVel).normalized;
            trickDir = new Vector3(Mathf.Lerp(trickDir.x, toHoop.x, 0.3f), Mathf.Lerp(trickDir.y, toHoop.y, 0.2f), Mathf.Lerp(trickDir.z, toHoop.z, 0.3f));
            trickDir.Normalize();
            trickDir = trickDir + Vector3.up * 0.25f;
            trickDir.Normalize();
            float powerScaler = Mathf.Clamp(2.5f * (totalVel / (float)velArr.Length).sqrMagnitude, 3.7f, 6.7f);
            ball.AddForce(powerScaler * trickDir, ForceMode.Impulse);

            GameMode.GetInstance().playerController.hasBall = false;
            GameMode.GetInstance().handledBall = null;

            historyVel.Clear();
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

    void OnTriggerEnter(Collider other)
    {
        SetCollidingObject(other);
    }

    void OnTriggerStay(Collider other)
    {
        SetCollidingObject(other);
    }

    void OnTriggerExit(Collider other)
    {
        if (!collidingObject)
        {
            return;
        }
        collidingObject = null;
    }

    public void Init()
    {
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
