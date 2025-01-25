using UnityEngine;
using UnityEngine.Events;

//DISABLE if using old input system
using UnityEngine.InputSystem;


namespace PhysicsBasedCharacterController
{
    public class InputReader : MonoBehaviour
    {
        [Header("Input specs")]
        public UnityEvent changedInputToMouseAndKeyboard;
        public UnityEvent changedInputToGamepad;

        [Header("Enable inputs")]
        public bool enableJump = true;
        public bool enableCrouch = true;
        public bool enableSprint = true;

        [HideInInspector]
        public Vector2 axisInput;
        [HideInInspector]
        public Vector2 cameraInput = Vector2.zero;
        [HideInInspector]
        public bool jump;
        [HideInInspector]
        public bool jumpHold;
        [HideInInspector]
        public float zoom;
        [HideInInspector]
        public bool sprint;
        [HideInInspector]
        public bool crouch;

        private bool hasJumped = false;
        private bool skippedFrame = false;
        private bool isMouseAndKeyboard = true;
        private bool oldInput = true;

        private MovementActions movementActions;

        private void Awake()
        {
            movementActions = new MovementActions();

            movementActions.Gameplay.Movement.performed += ctx => OnMove(ctx);

            movementActions.Gameplay.Jump.performed += ctx => OnJump();
            movementActions.Gameplay.Jump.canceled += ctx => JumpEnded();

            movementActions.Gameplay.Camera.performed += ctx => OnCamera(ctx);

            movementActions.Gameplay.Sprint.performed += ctx => OnSprint(ctx);
            movementActions.Gameplay.Sprint.canceled += ctx => SprintEnded(ctx);

            movementActions.Gameplay.Crouch.performed += ctx => OnCrouch(ctx);
            movementActions.Gameplay.Crouch.canceled += ctx => CrouchEnded(ctx);
        }


        private void GetDeviceNew(InputAction.CallbackContext ctx)
        {
            oldInput = isMouseAndKeyboard;

            if (ctx.control.device is Keyboard || ctx.control.device is Mouse) isMouseAndKeyboard = true;
            else isMouseAndKeyboard = false;

            if (oldInput != isMouseAndKeyboard && isMouseAndKeyboard) changedInputToMouseAndKeyboard.Invoke();
            else if (oldInput != isMouseAndKeyboard && !isMouseAndKeyboard) changedInputToGamepad.Invoke();
        }


        #region Actions

        public void OnMove(InputAction.CallbackContext ctx)
        {
            axisInput = ctx.ReadValue<Vector2>();
            GetDeviceNew(ctx);
        }


        public void OnJump()
        {
            if (enableJump)
            {
                jump = true;
                jumpHold = true;

                hasJumped = true;
                skippedFrame = false;
            }
        }


        public void JumpEnded()
        {
            jump = false;
            jumpHold = false;
        }



        private void FixedUpdate()
        {
            if (hasJumped && skippedFrame)
            {
                jump = false;
                hasJumped = false;
            }
            if (!skippedFrame && enableJump) skippedFrame = true;
        }



        public void OnCamera(InputAction.CallbackContext ctx)
        {
            Vector2 pointerDelta = ctx.ReadValue<Vector2>();
            cameraInput.x += pointerDelta.x;
            cameraInput.y += pointerDelta.y;
            GetDeviceNew(ctx);
        }


        public void OnSprint(InputAction.CallbackContext ctx)
        {
            if (enableSprint) sprint = true;
        }


        public void SprintEnded(InputAction.CallbackContext ctx)
        {
            sprint = false;
        }


        public void OnCrouch(InputAction.CallbackContext ctx)
        {
            if (enableCrouch) crouch = true;
        }


        public void CrouchEnded(InputAction.CallbackContext ctx)
        {
            crouch = false;
        }

        #endregion


        #region Enable / Disable

        private void OnEnable()
        {
            movementActions.Enable();
        }


        private void OnDisable()
        {
            movementActions.Disable();
        }

        #endregion
    }
}