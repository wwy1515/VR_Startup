using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class LaserPointer : MonoBehaviour
{
    public SteamVR_Input_Sources handType;
    public SteamVR_Behaviour_Pose controllerPose;
    public SteamVR_Action_Boolean teleportAction;
    public SteamVR_Action_Boolean exitGameAction;
    public GameObject laserPrefab;
    private GameObject laser;
    private Transform laserTransform;
    private Vector3 hitPoint;
    public Transform cameraRigTransform;
<<<<<<< Updated upstream
    public Transform playerTransform;
=======
>>>>>>> Stashed changes
    public GameObject teleportReticlePrefab;
    private GameObject reticle;
    private Transform teleportReticleTransform;
    public Transform headTransform;
    public Vector3 teleportReticleOffset;
    public LayerMask teleportMask;
    private bool shouldTeleport;

    // Start is called before the first frame update
    void Start()
    {
        laser = Instantiate(laserPrefab);
        laserTransform = laser.transform;
        reticle = Instantiate(teleportReticlePrefab);
        teleportReticleTransform = reticle.transform;
    }

    // Update is called once per frame
    void Update()
    {
        // press the "B" button to exit game
//        if (exitGameAction.GetState(handType))
//        {
//#if UNITY_EDITOR
//            UnityEditor.EditorApplication.isPlaying = false;
//#else
//                Application.Quit();
//#endif
//        }

        if (teleportAction.GetState(handType))
        {
            Debug.Log("teleport pressed.");
            RaycastHit hit;
            if (Physics.Raycast(controllerPose.transform.position, transform.forward, out hit, 100, teleportMask))
            {
                Debug.Log("raycast success");
                hitPoint = hit.point;
                ShowLaser(hit);
            }
        }
        else
        {
            laser.SetActive(false);
            reticle.SetActive(false);
        }

        if (teleportAction.GetStateUp(handType) && shouldTeleport)
        {
            Debug.Log("teleport success");
            Teleport();
        }
    }

    private void ShowLaser(RaycastHit hit)
    {
        laser.SetActive(true);
        laserTransform.position = Vector3.Lerp(controllerPose.transform.position, hitPoint, .5f);
        laserTransform.LookAt(hitPoint);
        laserTransform.localScale = new Vector3(laserTransform.localScale.x,
                                                laserTransform.localScale.y,
                                                hit.distance);
        reticle.SetActive(true);
        teleportReticleTransform.position = hitPoint + teleportReticleOffset;
        shouldTeleport = true;
    }

    private void Teleport()
    {
        shouldTeleport = false;
        reticle.SetActive(false);
        Vector3 difference = cameraRigTransform.position - headTransform.position;
        difference.y = 0;
<<<<<<< Updated upstream
        //hitPoint.y = cameraRigTransform.position.y;
        //cameraRigTransform.position = hitPoint + rigHeadDifference;
        //playerTransform.position = new Vector3(cameraRigTransform.position.x, playerTransform.position.y, cameraRigTransform.position.z);
        hitPoint.y = playerTransform.position.y;
        playerTransform.position = hitPoint + difference;
=======
        hitPoint.y = cameraRigTransform.position.y;
        cameraRigTransform.position = hitPoint + difference;
>>>>>>> Stashed changes
    }
}