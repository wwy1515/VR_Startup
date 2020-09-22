using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    private float gravityValue = -9.81f;
    private bool groundedPlayer;
    private float verticalVelocity = 0.0f;
    public CharacterController characterController;

    public Camera playerCamera;
    public enum CONTROLLER_TYPE
    {
        PC,
        WMR,
        VALVE
    }
    public CONTROLLER_TYPE DebugType = CONTROLLER_TYPE.PC;

    public interface PlayerControllerPlatform
    {
        void HandleMovement(CharacterController characterController, Transform playerTransform);
        void HandleLookDir(Transform cameraTransform, Transform playerTransform);

        bool HandleInteractiveButton();
        bool HandleCancelButton();
    }

    /*
    Debug Only
    */
    class PlayerControllerPC : PlayerControllerPlatform
    {
        float rotationX = 0;
        public float lookSpeed = 2.0f;
        public float lookXLimit = 45.0f;

        float movementVelocity = 5.0f;

        public void HandleLookDir(Transform cameraTransform, Transform playerTransform)
        {
            rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            cameraTransform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            playerTransform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
        }

        public void HandleMovement(CharacterController characterController, Transform playerTransform)
        {
            // We are grounded, so recalculate move direction based on axes
            Vector3 forward = playerTransform.TransformDirection(Vector3.forward);
            Vector3 right = playerTransform.TransformDirection(Vector3.right);

            float curSpeedX = movementVelocity * Input.GetAxis("Vertical");
            float curSpeedY = 0.6f * movementVelocity * Input.GetAxis("Horizontal");

            Vector3 moveDirection = (forward * curSpeedX) + (right * curSpeedY);

            characterController.Move(Time.deltaTime * moveDirection);
        }

        public bool HandleInteractiveButton()
        {
            return Input.GetKeyDown(KeyCode.F);
        }

        public bool HandleCancelButton()
        {
            return Input.GetKeyDown(KeyCode.Escape);
        }
    }

    class PlayerControllerWMR : PlayerControllerPlatform
    {
        public bool HandleCancelButton()
        {
            throw new System.NotImplementedException();
        }

        public bool HandleInteractiveButton()
        {
            throw new System.NotImplementedException();
        }

        public void HandleLookDir(Transform cameraTransform, Transform playerTransform)
        {
            throw new System.NotImplementedException();
        }

        public void HandleMovement(CharacterController characterController, Transform playerTransform)
        {
            throw new System.NotImplementedException();
        }
    }

    class PlayerControllerValve : PlayerControllerPlatform
    {
        public bool HandleCancelButton()
        {
            throw new System.NotImplementedException();
        }

        public bool HandleInteractiveButton()
        {
            throw new System.NotImplementedException();
        }

        public void HandleLookDir(Transform cameraTransform, Transform playerTransform)
        {
            throw new System.NotImplementedException();
        }

        public void HandleMovement(CharacterController characterController, Transform playerTransform)
        {
            throw new System.NotImplementedException();
        }
    }

    public PlayerControllerPlatform playerControllerPlatform;

    // Start is called before the first frame update
    void Start()
    {
#if WMR_HEADSET
        playerControllerPlatform = new PlayerControllerWMR();
#elif VALVE_HEADSET
        playerControllerPlatform = new PlayerControllerValve();
#else
        if (DebugType == CONTROLLER_TYPE.PC)
        {
            playerControllerPlatform = new PlayerControllerPC();
        }
        else if (DebugType == CONTROLLER_TYPE.WMR)
        {
            playerControllerPlatform = new PlayerControllerWMR();
        }
        else if (DebugType == CONTROLLER_TYPE.VALVE)
        {
            playerControllerPlatform = new PlayerControllerValve();
        }
#endif
        GameMode.playerController = this;

        characterController = GetComponent<CharacterController>();

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        groundedPlayer = characterController.isGrounded;
        if (groundedPlayer && verticalVelocity < 0)
        {
            verticalVelocity = 0f;
        }

        // Vector3 movement = playerControllerPlatform.HandleMovement();
        playerControllerPlatform.HandleLookDir(playerCamera.transform, transform);
        playerControllerPlatform.HandleMovement(characterController, transform);

        // Add gravity
        verticalVelocity += gravityValue * Time.deltaTime;

        // Vertical Movement
        characterController.Move(verticalVelocity * Time.deltaTime * Vector3.up);

        if(playerControllerPlatform.HandleCancelButton())
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}
