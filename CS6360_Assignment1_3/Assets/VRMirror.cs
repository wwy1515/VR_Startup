using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRMirror : MonoBehaviour
{
    public CharacterController Guard;

    public enum GuardState
    {
        Matching,
        Mirroring
    }
    public GuardState guardState = GuardState.Mirroring;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        switch (guardState)
        {
            case GuardState.Matching:
                {
                    if (Input.GetKeyDown(KeyCode.M))
                    {
                        guardState = GuardState.Mirroring;
                    }
                }
                break;
            case GuardState.Mirroring:
                {
                    if (Input.GetKeyDown(KeyCode.M))
                    {
                        guardState = GuardState.Matching;
                    }
                }
                break;
            default:
                break;
        }
    }

    public void Move(Vector2 movement)
    {
        Vector3 forward = Camera.main.transform.forward;
        Vector3 right = Camera.main.transform.right;

        Guard.transform.rotation = Camera.main.transform.rotation;

        if (guardState == GuardState.Mirroring)
        {
            forward = -forward;
            right = -right;
            Guard.transform.rotation = Camera.main.transform.rotation * Quaternion.Euler(0.0f, 180.0f, 0.0f);
        }

        Vector3 move = movement.x * forward + movement.y * right;
        Debug.Log(move);

        Guard.Move(move * Time.deltaTime);
    }
}
