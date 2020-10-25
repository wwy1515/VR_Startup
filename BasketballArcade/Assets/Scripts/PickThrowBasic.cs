using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

[RequireComponent(typeof(SteamVR_TrackedObject))]
public class PickThrowBasic : MonoBehaviour
{
    public GameObject prefab;
    public GameObject pickedObj;
    public Rigidbody attachPoint;

    public SteamVR_Action_Boolean throwBall = SteamVR_Input.GetAction<SteamVR_Action_Boolean>("ThrowBall");
    public SteamVR_Action_Boolean pickball = SteamVR_Input.GetAction<SteamVR_Action_Boolean>("PickBall");

    SteamVR_Behaviour_Pose trackedObj;
    FixedJoint joint;

    private void Awake()
    {
        trackedObj = GetComponent<SteamVR_Behaviour_Pose>();
    }

    private void FixedUpdate()
    {
        if (pickedObj != null && joint == null && pickball.GetStateDown(trackedObj.inputSource))
        {
            GameObject go = pickedObj;
            go.transform.position = attachPoint.transform.position;

            joint = go.AddComponent<FixedJoint>();
            joint.connectedBody = attachPoint;
        }
        if (joint == null && throwBall.GetStateDown(trackedObj.inputSource))
        {
            GameObject go = GameObject.Instantiate(prefab);
            go.transform.position = attachPoint.transform.position;

            joint = go.AddComponent<FixedJoint>();
            joint.connectedBody = attachPoint;
        }
        else if (joint != null && throwBall.GetStateUp(trackedObj.inputSource))
        {
            GameObject go = joint.gameObject;
            Rigidbody rigidbody = go.GetComponent<Rigidbody>();
            Object.DestroyImmediate(joint);
            joint = null;
            Object.Destroy(go, 10.0f);

            Transform origin = trackedObj.origin ? trackedObj.origin : trackedObj.transform.parent;
            if (origin != null) // origin = trackedObj.origin
            {
                rigidbody.velocity = origin.TransformVector(trackedObj.GetVelocity());
                rigidbody.angularVelocity = origin.TransformVector(trackedObj.GetAngularVelocity());
                Debug.Log("velocity: " + rigidbody.velocity + "angular velocity: " + rigidbody.angularVelocity);
            }
            else // origin = trackedObj.transform.parent -> world space 
            {
                rigidbody.velocity = trackedObj.GetVelocity();
                rigidbody.angularVelocity = trackedObj.GetAngularVelocity();
                Debug.Log("velocity: " + rigidbody.velocity + "angular velocity: " + rigidbody.angularVelocity);

            }

            rigidbody.maxAngularVelocity = rigidbody.angularVelocity.magnitude;
        }
    }

    // The ball be picked should call this setter
    public void setPickedObj(GameObject obj)
    {
        pickedObj = obj;
    }
}
