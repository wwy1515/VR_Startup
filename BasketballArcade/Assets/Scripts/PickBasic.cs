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
        Debug.DrawRay(GameMode.GetInstance().RightHand.transform.position, -GameMode.GetInstance().RightHand.transform.up * 10.0f, Color.yellow);

        InputDevice inputDevice = UnityEngine.XR.InputDevices.GetDeviceAtXRNode(UnityEngine.XR.XRNode.RightHand);
        Vector3 deviceVelocity, deviceAngularVelocity;
        inputDevice.TryGetFeatureValue(CommonUsages.deviceVelocity, out deviceVelocity);
        inputDevice.TryGetFeatureValue(CommonUsages.deviceAngularVelocity, out deviceAngularVelocity);

        if(GameMode.GetInstance().playerController.hasBall)
        {
            GameMode.GetInstance().arrow.gameObject.SetActive(true);

            historyVel.Enqueue(deviceVelocity);
            if(historyVel.Count > 3)
            {
                historyVel.Dequeue();
            }
        }
        else
        {
            GameMode.GetInstance().arrow.gameObject.SetActive(false);
        }

        // Set Arrow Properties
        {
            var velArr = historyVel.ToArray();
            Vector3 totalVel = Vector3.zero;
            for(int i = 0; i < velArr.Length; i++)
            {
                var item = velArr[i];
                totalVel += item;
            }

            var rightHandPos = GameMode.GetInstance().RightHand.position;
            // raycaLayerMask.NameToLayer

            Vector3 toHoop = (GameMode.GetInstance().targetPos.position - attachPos.transform.position).normalized;
            Vector3 trickDir = (totalVel).normalized;

            GameMode.GetInstance().arrow.direction = trickDir;

            float powerScaler = 2.0f * (totalVel / (float)velArr.Length).sqrMagnitude;
            if(float.IsNaN(powerScaler))
            {
                GameMode.GetInstance().arrow.value = 0.0f;
            }
            else
            {
                GameMode.GetInstance().arrow.value = powerScaler;
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

            var rightHandPos = GameMode.GetInstance().RightHand.position;
            // raycaLayerMask.NameToLayer

            Vector3 toHoop = (GameMode.GetInstance().targetPos.position - attachPos.transform.position).normalized;
            Vector3 trickDir = (totalVel).normalized;
            // trickDir = new Vector3(Mathf.Lerp(trickDir.x, toHoop.x, 0.3f), Mathf.Lerp(trickDir.y, toHoop.y, 0.2f), Mathf.Lerp(trickDir.z, toHoop.z, 0.3f));
            // trickDir.Normalize();
            // trickDir = trickDir + Vector3.up * 0.25f;
            // trickDir.Normalize();

            float powerScaler = 2.0f * (totalVel / (float)velArr.Length).sqrMagnitude;
                        

            // ball.AddForce(powerScaler * trickDir, ForceMode.Impulse);
        
            ball.AddForceAtPosition(powerScaler * trickDir, ball.transform.position - 0.5f * trickDir,ForceMode.VelocityChange);
            ball.GetComponent<Ball>().state = Ball.State.Outside;
            ball.useGravity = true;
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
