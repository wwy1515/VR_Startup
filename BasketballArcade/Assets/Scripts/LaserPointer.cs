using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class LaserPointer : MonoBehaviour
{
    public GameObject laserPrefab;
    private GameObject laser;
    private Transform laserTransform;
    private Vector3 hitPoint;
    public Transform playerTransform;
    public GameObject teleportReticlePrefab;
    private GameObject reticle;
    private Transform teleportReticleTransform;
    public Transform headTransform;
    public Vector3 teleportReticleOffset;
    public LayerMask teleportMask;
    private bool shouldTeleport;
    private bool prevState;

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
        InputDevice inputDevice = UnityEngine.XR.InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
        inputDevice.TryGetFeatureValue(CommonUsages.primaryButton, out bool primaryButton);

        if (primaryButton)
        {
            prevState = true;
            Debug.Log("teleport pressed.");
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, 100, teleportMask))
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
            if (prevState && shouldTeleport)
            {
                prevState = false;
                Debug.Log("teleport success");
                Teleport();
            }
        }

    }

    private void ShowLaser(RaycastHit hit)
    {
        laser.SetActive(true);
        laserTransform.position = Vector3.Lerp(transform.position, hitPoint, .5f);
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
        Vector3 difference = playerTransform.position - headTransform.position;
        Debug.Log(difference);
        difference.y = 0;
        hitPoint.y = playerTransform.position.y;
        playerTransform.position = hitPoint + difference;
    }
}