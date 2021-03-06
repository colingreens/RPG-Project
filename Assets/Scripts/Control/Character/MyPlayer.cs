using RPG.Control.Core;
using RPG.GameEvents.Events;
using UnityEngine;

namespace RPG.Control.Character
{
    public class MyPlayer : MonoBehaviour
    {
        public PlayerDriver Character;
        public Transform CameraFollowPoint;
        public MyCharacterCameraController CharacterCamera;
        public VoidEvent onZoomEvent;
        public VoidEvent onUnZoomEvent;
        public PlayerController playerController;

        private const string MouseXInput = "Mouse X";
        private const string MouseYInput = "Mouse Y";
        private const string MouseScrollInput = "Mouse ScrollWheel";
        private const string HorizontalInput = "Horizontal";
        private const string VerticalInput = "Vertical";

        private bool isZoomed = false;


        private void Awake()
        {
            Cursor.lockState = CursorLockMode.Locked;
        }

        void Start()
        {    
            // Tell camera to follow transform
            CharacterCamera.SetFollowTransform(CameraFollowPoint);

            // Ignore the character's collider(s) for camera obstruction checks
            CharacterCamera.IgnoredColliders.Clear();
            CharacterCamera.IgnoredColliders.AddRange(Character.GetComponentsInChildren<Collider>());


        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Cursor.lockState = CursorLockMode.Locked;
            }

            //if (Input.GetKeyDown(KeyCode.E))
            //{
            //    playerController.InteractWithComponent();
            //}



            HandleCharacterInput();
        }

        private void LateUpdate()
        {
            // Handle rotating the camera along with physics movers
            if (CharacterCamera.RotateWithPhysicsMover && Character.Motor.AttachedRigidbody != null)
            {
                CharacterCamera.PlanarDirection = Character.Motor.AttachedRigidbody.GetComponent<PhysicsMover>().RotationDeltaFromInterpolation * CharacterCamera.PlanarDirection;
                CharacterCamera.PlanarDirection = Vector3.ProjectOnPlane(CharacterCamera.PlanarDirection, Character.Motor.CharacterUp).normalized;
            }

            HandleCameraInput();

            Character.PostInputUpdate(Time.deltaTime, CharacterCamera.transform.forward);
        }
        private void HandleCameraInput()
        {
            // Create the look input vector for the camera
            float mouseLookAxisUp = Input.GetAxisRaw(MouseYInput);
            float mouseLookAxisRight = Input.GetAxisRaw(MouseXInput);
            Vector3 lookInputVector = new Vector3(mouseLookAxisRight, mouseLookAxisUp, 0f);

            // Prevent moving the camera while the cursor isn't locked
            if (Cursor.lockState != CursorLockMode.Locked)
            {
                lookInputVector = Vector3.zero;
            }

            // Input for zooming the camera (disabled in WebGL because it can cause problems)
            float scrollInput = -Input.GetAxis(MouseScrollInput);
#if UNITY_WEBGL
        scrollInput = 0f;
#endif

            // Apply inputs to the camera
            CharacterCamera.UpdateWithInput(Time.deltaTime, scrollInput, lookInputVector);

            // Handle toggling zoom level
            if (Input.GetMouseButtonDown(1))
            {
                CharacterCamera.TargetDistance = (CharacterCamera.TargetDistance == 0f) ? CharacterCamera.DefaultDistance : 0f;
            }
        }

        private void HandleCharacterInput()
        {
            PlayerCharacterInputs characterInputs = new PlayerCharacterInputs
            {

                // Build the CharacterInputs struct
                MoveAxisForward = Input.GetAxisRaw(VerticalInput),
                MoveAxisRight = Input.GetAxisRaw(HorizontalInput),
                CameraRotation = CharacterCamera.Transform.rotation,
                JumpDown = Input.GetKeyDown(KeyCode.Space),
                CrouchDown = Input.GetKeyDown(KeyCode.C),
                CrouchUp = Input.GetKeyUp(KeyCode.C)
            };

            // Apply inputs to character
            Character.SetInputs(ref characterInputs);

            // Apply impulse
            if (Input.GetKeyDown(KeyCode.Q))
            {
                Character.Motor.ForceUnground(0.1f);
                Character.AddVelocity(Vector3.forward * 25f);
            }

            if (Input.GetMouseButtonDown(1))
            {
                if (isZoomed)
                {
                    onUnZoomEvent.Raise();
                    isZoomed = false;
                    Debug.Log("Zooming Out!");
                }
                else
                {
                    onZoomEvent.Raise();
                    isZoomed = true;
                    Debug.Log("Zooming in!");
                }
                
            }
        }
    }
}
