﻿using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;


namespace PhysicsBasedCharacterController
{
    [RequireComponent(typeof(CapsuleCollider))]
    [RequireComponent(typeof(Rigidbody))]
    public class CharacterManager : MonoBehaviour
    {
        [Header("Movement specifics")]
        [Tooltip("Layers where the player can stand on")]
        public LayerMask groundMask;
        public void SetGroundMask(LayerMask mask) { groundMask = mask; }

        [Tooltip("Base player speed")]
        public float movementSpeed = 14f;
        public void SetMovementSpeed(float speed) { movementSpeed = speed; }
        
        [Range(0f, 1f)]
        [Tooltip("Minimum input value to trigger movement")]
        public float crouchSpeedMultiplier = 0.248f;
        public void SetCrouchSpeedMultiplier(float multiplier) { crouchSpeedMultiplier = Mathf.Clamp01(multiplier); }
        
        [FormerlySerializedAs("movementThrashold")]
        [Range(0.01f, 0.99f)]
        [Tooltip("Minimum input value to trigger movement")]
        public float movementThreshold = 0.01f;
        public void SetMovementThreshold(float multiplier) { crouchHeightMultiplier = Mathf.Clamp01(multiplier); }
        
        [Space(10)]

        [Tooltip("Speed up multiplier")]
        public float dampSpeedUp = 0.2f;
        public void SetDampSpeedUp(float threshold) { movementThreshold = threshold; }
        
        [Tooltip("Speed down multiplier")]
        public float dampSpeedDown = 0.1f;
        public void SetDampSpeedDown(float damping) { dampSpeedUp = damping; }

        [Header("Jump and gravity specifics")]
        [Tooltip("Jump velocity")]
        public float jumpVelocity = 20f;
        public void SetJumpVelocity(float velocity) { jumpVelocity = velocity; }
        
        [Tooltip("Multiplier applied to gravity when the player is falling")]
        public float fallMultiplier = 1.7f;
        public void SetFallMultiplier(float value) { fallMultiplier = value; }
        
        [Tooltip("Multiplier applied to gravity when the player is holding jump")]
        public float holdJumpMultiplier = 5f;
        public void SetHoldJumpMultiplier(float value) { holdJumpMultiplier = value; }
        
        [Range(0f, 1f)]
        [Tooltip("Player friction against floor")]
        public float frictionAgainstFloor = 0.3f;
        public void SetFrictionAgainstFloor(float value) { frictionAgainstFloor = Mathf.Clamp01(value); }
        
        [Range(0.01f, 0.99f)]
        [Tooltip("Player friction against wall")]
        public float frictionAgainstWall = 0.839f;
        public void SetFrictionAgainstWall(float value) { frictionAgainstWall = Mathf.Clamp01(value); }
        
        [Space(10)]

        [Tooltip("Player can long jump")]
        public bool canLongJump = true;
        public void SetCanLongJump(bool value) { canLongJump = value; }

        [Header("Slope and step specifics")]
        [Tooltip("Distance from the player feet used to check if the player is touching the ground")]
        [FormerlySerializedAs("groundCheckerThrashold")]
        public float groundCheckerThreshold = 0.1f;
        public void SetGroundCheckerThreshold(float value) { groundCheckerThreshold = value; }
        
        [Tooltip("Distance from the player feet used to check if the player is touching a slope")]
        [FormerlySerializedAs("slopeCheckerThrashold")]
        public float slopeCheckerThreshold = 0.51f;
        public void SetSlopeChecker(float value) { slopeCheckerThreshold = value; }
        
        [Tooltip("Distance from the player center used to check if the player is touching a step")]
        [FormerlySerializedAs("stepCheckerThrashold")]
        public float stepCheckerThreshold = 0.6f;
        public void SetStepChecker(float value) { stepCheckerThreshold = value; }
        
        [Space(10)]

        [Range(1f, 89f)]
        [Tooltip("Max climbable slope angle")]
        public float maxClimbableSlopeAngle = 53.6f;
        public void SetMaxClimbableSlopeAngle(float value) { maxClimbableSlopeAngle = value; }
        
        [Tooltip("Max climbable step height")]
        public float maxStepHeight = 0.74f;
        public void SetMaxStepHeight(float value) { maxStepHeight = value; }
        
        [Space(10)]

        [Tooltip("Speed multiplier based on slope angle")]
        public AnimationCurve speedMultiplierOnAngle = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
        public void SetSpeedMultiplierOnAngle(AnimationCurve value) { speedMultiplierOnAngle = value; }
        
        [Range(0.01f, 1f)]
        [Tooltip("Multiplier factor on climbable slope")]
        public float canSlideMultiplierCurve = 0.061f;
        public void SetCanSlideMultiplierCurve(float value) { canSlideMultiplierCurve = Mathf.Clamp01(value); }
        
        [Range(0.01f, 1f)]
        [Tooltip("Multiplier factor on non climbable slope")]
        public float cantSlideMultiplierCurve = 0.039f;
        public void SetCantSlideMultiplierCurve(float value) { cantSlideMultiplierCurve = Mathf.Clamp01(value); }
        
        [Range(0.01f, 1f)]
        [Tooltip("Multiplier factor on step")]
        public float climbingStairsMultiplierCurve = 0.637f;
        public void SetClimbingStairsMultiplier(float value) { climbingStairsMultiplierCurve = Mathf.Clamp01(value); }
        
        [Space(10)]

        [Tooltip("Multiplier factor for gravity")]
        public float gravityMultiplier = 6f;
        public void SetGravityMultiplier(float value) { gravityMultiplier = value; }
        
        [Tooltip("Multiplier factor for gravity used on change of normal")]
        public float gravityMultiplyerOnSlideChange = 3f;
        public void SetGravityMultiplierOnSlideChange(float value) { gravityMultiplyerOnSlideChange = value; }
        
        [Tooltip("Multiplier factor for gravity used on non climbable slope")]
        public float gravityMultiplierIfUnclimbableSlope = 30f;
        public void SetGravityMultiplierIfUnclimbableSlope(float value) { gravityMultiplierIfUnclimbableSlope = value; }
        
        [Space(10)]

        public bool lockOnSlope = false;
        public void SetLockOnSlope(bool value) { lockOnSlope = value; }

        [Header("Wall slide specifics")]
        [Tooltip("Distance from the player head used to check if the player is touching a wall")]
        [FormerlySerializedAs("wallCheckerThrashold")]
        public float wallCheckerThreshold = 0.8f;
        public void SetWallCheckerThreshold(float value) { wallCheckerThreshold = value; }
        
        [Tooltip("Wall checker distance from the player center")]
        [FormerlySerializedAs("hightWallCheckerChecker")]
        public float heightWallChecker = 0.5f;
        public void SetHightWallChecker(float value) { heightWallChecker = value; }
        
        [Space(10)]

        [Tooltip("Multiplier used when the player is jumping from a wall")]
        public float jumpFromWallMultiplier = 30f;
        public void SetJumpFromWallMultiplier(float value) { jumpFromWallMultiplier = value; }
        
        [Tooltip("Factor used to determine the height of the jump")]
        public float multiplierVerticalLeap = 1f;
        public void SetMultiplierVerticalLeap(float value) { multiplierVerticalLeap = value; }
        
        [Header("Sprint and crouch specifics")]
        [Tooltip("Sprint speed")]
        public float sprintSpeed = 20f;
        public void SetSprintSpeed(float value) { sprintSpeed = value; }
        
        [Tooltip("Multiplier applied to the collider when player is crouching")]
        public float crouchHeightMultiplier = 0.5f;
        public void SetCrouchHeightMultiplier(float value) { crouchHeightMultiplier = value; }
        
        [Tooltip("FP camera head height")]
        public Vector3 POV_normalHeadHeight = new Vector3(0f, 0.5f, -0.1f);
        public void SetPovNormalHeadHeight(Vector3 value) { POV_normalHeadHeight = value; }
        
        [Tooltip("FP camera head height when crouching")]
        public Vector3 POV_crouchHeadHeight = new Vector3(0f, -0.1f, -0.1f);
        public void SetPovCrouchHeadHeight(Vector3 value) { POV_crouchHeadHeight = value; }

        [Header("References")]
        [Tooltip("Character camera")]
        public GameObject characterCamera;
        public void SetCharacterCamera(GameObject value) { characterCamera = value; }
        
        [Tooltip("Character model")]
        public GameObject characterModel;
        public void SetCharacterModel(GameObject value) { characterModel = value; }
        
        [Tooltip("Character rotation speed when the forward direction is changed")]
        public float characterModelRotationSmooth = 0.1f;
        public void SetCharacterModelRotationSmooth(float value) { characterModelRotationSmooth = value; }
        
        [Space(10)]

        [Tooltip("Default character mesh")]
        public GameObject meshCharacter;
        public void SetMeshCharacter(GameObject value) { meshCharacter = value; }
        
        [Tooltip("Crouch character mesh")]
        public GameObject meshCharacterCrouch;
        public void SetMeshCharacterCrouch(GameObject value) { meshCharacterCrouch = value; }
        
        [Tooltip("Head reference")]
        public Transform headPoint;
        public void SetHeadPoint(Transform value) { headPoint = value; }
        
        [Space(10)]

        [Tooltip("Input reference")]
        public InputReader input;
        
        [Space(10)]

        public bool debug = true;

        [Header("Events")]
        [SerializeField] UnityEvent OnJump;
        [Space(15)]

        public float minimumVerticalSpeedToLandEvent;
        [SerializeField] UnityEvent OnLand;
        [Space(15)]

        public float minimumHorizontalSpeedToFastEvent;
        [SerializeField] UnityEvent OnFast;
        [Space(15)]

        [SerializeField] UnityEvent OnWallSlide;
        [Space(15)]

        [SerializeField] UnityEvent OnSprint;
        [Space(15)]

        [SerializeField] UnityEvent OnCrouch;
        [Space(15)]

        private Vector3 forward;
        private Vector3 globalForward;
        private Vector3 reactionForward;
        private Vector3 down;
        private Vector3 globalDown;
        private Vector3 reactionGlobalDown;

        private float currentSurfaceAngle;
        private bool currentLockOnSlope;

        private Vector3 wallNormal;
        private Vector3 groundNormal;
        private Vector3 prevGroundNormal;
        private bool prevGrounded;

        private float coyoteJumpMultiplier = 1f;

        private bool isGrounded = false;
        private bool isTouchingSlope = false;
        private bool isTouchingStep = false;
        private bool isTouchingWall = false;
        private bool isJumping = false;
        private bool isCrouch = false;

        private Vector2 axisInput;
        private bool jump;
        private bool jumpHold;
        private bool sprint;
        private bool crouch;

        [HideInInspector]
        public float targetAngle;
        private new Rigidbody rigidbody;
        private new CapsuleCollider collider;
        private float originalColliderHeight;

        private Vector3 currVelocity = Vector3.zero;
        private float turnSmoothVelocity;
        private bool lockRotation = false;


        #region GettersSetters
        
        public bool GetGrounded() { return isGrounded; }
        public bool GetTouchingSlope() { return isTouchingSlope; }
        public bool GetTouchingStep() { return isTouchingStep; }
        public bool GetTouchingWall() { return isTouchingWall; }
        public bool GetJumping() { return isJumping; }
        public bool GetCrouching() { return isCrouch; }
        public float GetOriginalColliderHeight() { return originalColliderHeight; }

        public void SetLockRotation(bool _lock) { lockRotation = _lock; }

        #endregion
        
        /**/


        private void Awake()
        {
            if (input == null)
            {
                input = FindObjectOfType<InputReader>();
            }

            if (characterCamera == null)
            {
                characterCamera = GameObject.FindGameObjectWithTag("MainCamera");
            }

            rigidbody = this.GetComponent<Rigidbody>();
            collider = this.GetComponent<CapsuleCollider>();
            originalColliderHeight = collider.height;

            SetFriction(frictionAgainstFloor, true);
            currentLockOnSlope = lockOnSlope;
        }


        private void Update()
        {
            //input
            axisInput = input.axisInput;
            jump = input.jump;
            jumpHold = input.jumpHold;
            sprint = input.sprint;
            crouch = input.crouch;
        }


        private void FixedUpdate()
        {
            //local vectors
            CheckGrounded();
            CheckStep();
            CheckWall();
            CheckSlopeAndDirections();

            //movement
            MoveCrouch();
            MoveWalk();
            MoveRotation();
            MoveJump();

            //gravity
            ApplyGravity();

            //events
            UpdateEvents();
        }


        #region Checks

        private void CheckGrounded()
        {
            prevGrounded = isGrounded;
            isGrounded = Physics.CheckSphere(transform.position - new Vector3(0, originalColliderHeight / 2f, 0), groundCheckerThreshold, groundMask);
        }


        private void CheckStep()
        {
            bool tmpStep = false;
            Vector3 bottomStepPos = transform.position - new Vector3(0f, originalColliderHeight / 2f, 0f) + new Vector3(0f, 0.05f, 0f);

            RaycastHit stepLowerHit;
            if (Physics.Raycast(bottomStepPos, globalForward, out stepLowerHit, stepCheckerThreshold, groundMask))
            {
                RaycastHit stepUpperHit;
                if (RoundValue(stepLowerHit.normal.y) == 0 && !Physics.Raycast(bottomStepPos + new Vector3(0f, maxStepHeight, 0f), globalForward, out stepUpperHit, stepCheckerThreshold + 0.05f, groundMask))
                {
                    //rigidbody.position -= new Vector3(0f, -stepSmooth, 0f);
                    tmpStep = true;
                }
            }

            RaycastHit stepLowerHit45;
            if (Physics.Raycast(bottomStepPos, Quaternion.AngleAxis(45, transform.up) * globalForward, out stepLowerHit45, stepCheckerThreshold, groundMask))
            {
                RaycastHit stepUpperHit45;
                if (RoundValue(stepLowerHit45.normal.y) == 0 && !Physics.Raycast(bottomStepPos + new Vector3(0f, maxStepHeight, 0f), Quaternion.AngleAxis(45, Vector3.up) * globalForward, out stepUpperHit45, stepCheckerThreshold + 0.05f, groundMask))
                {
                    //rigidbody.position -= new Vector3(0f, -stepSmooth, 0f);
                    tmpStep = true;
                }
            }

            RaycastHit stepLowerHitMinus45;
            if (Physics.Raycast(bottomStepPos, Quaternion.AngleAxis(-45, transform.up) * globalForward, out stepLowerHitMinus45, stepCheckerThreshold, groundMask))
            {
                RaycastHit stepUpperHitMinus45;
                if (RoundValue(stepLowerHitMinus45.normal.y) == 0 && !Physics.Raycast(bottomStepPos + new Vector3(0f, maxStepHeight, 0f), Quaternion.AngleAxis(-45, Vector3.up) * globalForward, out stepUpperHitMinus45, stepCheckerThreshold + 0.05f, groundMask))
                {
                    //rigidbody.position -= new Vector3(0f, -stepSmooth, 0f);
                    tmpStep = true;
                }
            }

            isTouchingStep = tmpStep;
        }


        private void CheckWall()
        {
            bool tmpWall = false;
            Vector3 tmpWallNormal = Vector3.zero;
            Vector3 topWallPos = new Vector3(transform.position.x, transform.position.y + heightWallChecker, transform.position.z);

            RaycastHit wallHit;
            if (Physics.Raycast(topWallPos, globalForward, out wallHit, wallCheckerThreshold, groundMask))
            {
                tmpWallNormal = wallHit.normal;
                tmpWall = true;
            }
            else if (Physics.Raycast(topWallPos, Quaternion.AngleAxis(45, transform.up) * globalForward, out wallHit, wallCheckerThreshold, groundMask))
            {
                tmpWallNormal = wallHit.normal;
                tmpWall = true;
            }
            else if (Physics.Raycast(topWallPos, Quaternion.AngleAxis(90, transform.up) * globalForward, out wallHit, wallCheckerThreshold, groundMask))
            {
                tmpWallNormal = wallHit.normal;
                tmpWall = true;
            }
            else if (Physics.Raycast(topWallPos, Quaternion.AngleAxis(135, transform.up) * globalForward, out wallHit, wallCheckerThreshold, groundMask))
            {
                tmpWallNormal = wallHit.normal;
                tmpWall = true;
            }
            else if (Physics.Raycast(topWallPos, Quaternion.AngleAxis(180, transform.up) * globalForward, out wallHit, wallCheckerThreshold, groundMask))
            {
                tmpWallNormal = wallHit.normal;
                tmpWall = true;
            }
            else if (Physics.Raycast(topWallPos, Quaternion.AngleAxis(225, transform.up) * globalForward, out wallHit, wallCheckerThreshold, groundMask))
            {
                tmpWallNormal = wallHit.normal;
                tmpWall = true;
            }
            else if (Physics.Raycast(topWallPos, Quaternion.AngleAxis(270, transform.up) * globalForward, out wallHit, wallCheckerThreshold, groundMask))
            {
                tmpWallNormal = wallHit.normal;
                tmpWall = true;
            }
            else if (Physics.Raycast(topWallPos, Quaternion.AngleAxis(315, transform.up) * globalForward, out wallHit, wallCheckerThreshold, groundMask))
            {
                tmpWallNormal = wallHit.normal;
                tmpWall = true;
            }

            isTouchingWall = tmpWall;
            wallNormal = tmpWallNormal;
        }


        private void CheckSlopeAndDirections()
        {
            prevGroundNormal = groundNormal;

            RaycastHit slopeHit;
            if (Physics.SphereCast(transform.position, slopeCheckerThreshold, Vector3.down, out slopeHit, originalColliderHeight / 2f + 0.5f, groundMask))
            {
                groundNormal = slopeHit.normal;

                if (slopeHit.normal.y == 1)
                {

                    forward = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                    globalForward = forward;
                    reactionForward = forward;

                    SetFriction(frictionAgainstFloor, true);
                    currentLockOnSlope = lockOnSlope;

                    currentSurfaceAngle = 0f;
                    isTouchingSlope = false;

                }
                else
                {
                    //set forward
                    Vector3 tmpGlobalForward = transform.forward.normalized;
                    Vector3 tmpForward = new Vector3(tmpGlobalForward.x, Vector3.ProjectOnPlane(transform.forward.normalized, slopeHit.normal).normalized.y, tmpGlobalForward.z);
                    Vector3 tmpReactionForward = new Vector3(tmpForward.x, tmpGlobalForward.y - tmpForward.y, tmpForward.z);

                    if (currentSurfaceAngle <= maxClimbableSlopeAngle && !isTouchingStep)
                    {
                        //set forward
                        forward = tmpForward * ((speedMultiplierOnAngle.Evaluate(currentSurfaceAngle / 90f) * canSlideMultiplierCurve) + 1f);
                        globalForward = tmpGlobalForward * ((speedMultiplierOnAngle.Evaluate(currentSurfaceAngle / 90f) * canSlideMultiplierCurve) + 1f);
                        reactionForward = tmpReactionForward * ((speedMultiplierOnAngle.Evaluate(currentSurfaceAngle / 90f) * canSlideMultiplierCurve) + 1f);

                        SetFriction(frictionAgainstFloor, true);
                        currentLockOnSlope = lockOnSlope;
                    }
                    else if (isTouchingStep)
                    {
                        //set forward
                        forward = tmpForward * ((speedMultiplierOnAngle.Evaluate(currentSurfaceAngle / 90f) * climbingStairsMultiplierCurve) + 1f);
                        globalForward = tmpGlobalForward * ((speedMultiplierOnAngle.Evaluate(currentSurfaceAngle / 90f) * climbingStairsMultiplierCurve) + 1f);
                        reactionForward = tmpReactionForward * ((speedMultiplierOnAngle.Evaluate(currentSurfaceAngle / 90f) * climbingStairsMultiplierCurve) + 1f);

                        SetFriction(frictionAgainstFloor, true);
                        currentLockOnSlope = true;
                    }
                    else
                    {
                        //set forward
                        forward = tmpForward * ((speedMultiplierOnAngle.Evaluate(currentSurfaceAngle / 90f) * cantSlideMultiplierCurve) + 1f);
                        globalForward = tmpGlobalForward * ((speedMultiplierOnAngle.Evaluate(currentSurfaceAngle / 90f) * cantSlideMultiplierCurve) + 1f);
                        reactionForward = tmpReactionForward * ((speedMultiplierOnAngle.Evaluate(currentSurfaceAngle / 90f) * cantSlideMultiplierCurve) + 1f);

                        SetFriction(0f, true);
                        currentLockOnSlope = lockOnSlope;
                    }

                    currentSurfaceAngle = Vector3.Angle(Vector3.up, slopeHit.normal);
                    isTouchingSlope = true;
                }

                //set down
                down = Vector3.Project(Vector3.down, slopeHit.normal);
                globalDown = Vector3.down.normalized;
                reactionGlobalDown = Vector3.up.normalized;
            }
            else
            {
                groundNormal = Vector3.zero;

                forward = Vector3.ProjectOnPlane(transform.forward, slopeHit.normal).normalized;
                globalForward = forward;
                reactionForward = forward;

                //set down
                down = Vector3.down.normalized;
                globalDown = Vector3.down.normalized;
                reactionGlobalDown = Vector3.up.normalized;

                SetFriction(frictionAgainstFloor, true);
                currentLockOnSlope = lockOnSlope;
            }
        }

        #endregion


        #region Move

        private void MoveCrouch()
        {
            if (crouch && isGrounded)
            {
                isCrouch = true;
                if (meshCharacterCrouch != null && meshCharacter != null) meshCharacter.SetActive(false);
                if (meshCharacterCrouch != null) meshCharacterCrouch.SetActive(true);

                float newHeight = originalColliderHeight * crouchHeightMultiplier;
                collider.height = newHeight;
                collider.center = new Vector3(0f, -newHeight * crouchHeightMultiplier, 0f);

                headPoint.position = new Vector3(transform.position.x + POV_crouchHeadHeight.x, transform.position.y + POV_crouchHeadHeight.y, transform.position.z + POV_crouchHeadHeight.z);
            }
            else
            {
                isCrouch = false;
                if (meshCharacterCrouch != null && meshCharacter != null) meshCharacter.SetActive(true);
                if (meshCharacterCrouch != null) meshCharacterCrouch.SetActive(false);

                collider.height = originalColliderHeight;
                collider.center = Vector3.zero;

                headPoint.position = new Vector3(transform.position.x + POV_normalHeadHeight.x, transform.position.y + POV_normalHeadHeight.y, transform.position.z + POV_normalHeadHeight.z);
            }
        }


        private void MoveWalk()
        {
            float crouchMultiplier = 1f;
            if (isCrouch) crouchMultiplier = crouchSpeedMultiplier;

            if (axisInput.magnitude > movementThreshold)
            {
                targetAngle = Mathf.Atan2(axisInput.x, axisInput.y) * Mathf.Rad2Deg + characterCamera.transform.eulerAngles.y;

                if (!sprint) rigidbody.linearVelocity = Vector3.SmoothDamp(rigidbody.linearVelocity, forward * (movementSpeed * crouchMultiplier), ref currVelocity, dampSpeedUp);
                else rigidbody.linearVelocity = Vector3.SmoothDamp(rigidbody.linearVelocity, forward * (sprintSpeed * crouchMultiplier), ref currVelocity, dampSpeedUp);
            }
            else rigidbody.linearVelocity = Vector3.SmoothDamp(rigidbody.linearVelocity, Vector3.zero * crouchMultiplier, ref currVelocity, dampSpeedDown);
        }


        private void MoveRotation()
        {
            float angle = Mathf.SmoothDampAngle(characterModel.transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, characterModelRotationSmooth);
            transform.rotation = Quaternion.Euler(0f, targetAngle, 0f);

            if (!lockRotation) characterModel.transform.rotation = Quaternion.Euler(0f, angle, 0f);
            else
            {
                var lookPos = -wallNormal;
                lookPos.y = 0;
                var rotation = Quaternion.LookRotation(lookPos);
                characterModel.transform.rotation = rotation;
            }
        }


        private void MoveJump()
        {
            //jumped
            if (jump && isGrounded && ((isTouchingSlope && currentSurfaceAngle <= maxClimbableSlopeAngle) || !isTouchingSlope) && !isTouchingWall)
            {
                rigidbody.linearVelocity += Vector3.up * jumpVelocity;
                isJumping = true;
            }
            //jumped from wall
            else if (jump && !isGrounded && isTouchingWall)
            {
                rigidbody.linearVelocity += wallNormal * jumpFromWallMultiplier + Vector3.up * (jumpFromWallMultiplier * multiplierVerticalLeap);
                isJumping = true;

                targetAngle = Mathf.Atan2(wallNormal.x, wallNormal.z) * Mathf.Rad2Deg;

                forward = wallNormal;
                globalForward = forward;
                reactionForward = forward;
            }

            //is falling
            if (rigidbody.linearVelocity.y < 0 && !isGrounded) coyoteJumpMultiplier = fallMultiplier;
            else if (rigidbody.linearVelocity.y > 0.1f && (currentSurfaceAngle <= maxClimbableSlopeAngle || isTouchingStep))
            {
                //is short jumping
                if (!jumpHold || !canLongJump) coyoteJumpMultiplier = 1f;
                //is long jumping
                else coyoteJumpMultiplier = 1f / holdJumpMultiplier;
            }
            else
            {
                isJumping = false;
                coyoteJumpMultiplier = 1f;
            }
        }

        #endregion


        #region Gravity

        private void ApplyGravity()
        {
            Vector3 gravity = Vector3.zero;

            if (currentLockOnSlope || isTouchingStep) gravity = down * (gravityMultiplier * -Physics.gravity.y * coyoteJumpMultiplier);
            else gravity = globalDown * (gravityMultiplier * -Physics.gravity.y * coyoteJumpMultiplier);

            //avoid little jump
            if (groundNormal.y != 1 && groundNormal.y != 0 && isTouchingSlope && prevGroundNormal != groundNormal)
            {
                //Debug.Log("Added correction jump on slope");
                gravity *= gravityMultiplyerOnSlideChange;
            }

            //slide if angle too big
            if (groundNormal.y != 1 && groundNormal.y != 0 && (currentSurfaceAngle > maxClimbableSlopeAngle && !isTouchingStep))
            {
                //Debug.Log("Slope angle too high, character is sliding");
                if (currentSurfaceAngle > 0f && currentSurfaceAngle <= 30f) gravity = globalDown * (gravityMultiplierIfUnclimbableSlope * -Physics.gravity.y);
                else if (currentSurfaceAngle > 30f && currentSurfaceAngle <= 89f) gravity = globalDown * gravityMultiplierIfUnclimbableSlope / 2f * -Physics.gravity.y;
            }

            //friction when touching wall
            if (isTouchingWall && rigidbody.linearVelocity.y < 0) gravity *= frictionAgainstWall;

            rigidbody.AddForce(gravity);
        }

        #endregion


        #region Events

        private void UpdateEvents()
        {
            if ((jump && isGrounded && ((isTouchingSlope && currentSurfaceAngle <= maxClimbableSlopeAngle) || !isTouchingSlope)) || (jump && !isGrounded && isTouchingWall)) OnJump.Invoke();
            if (isGrounded && !prevGrounded && rigidbody.linearVelocity.y > -minimumVerticalSpeedToLandEvent) OnLand.Invoke();
            if (Mathf.Abs(rigidbody.linearVelocity.x) + Mathf.Abs(rigidbody.linearVelocity.z) > minimumHorizontalSpeedToFastEvent) OnFast.Invoke();
            if (isTouchingWall && rigidbody.linearVelocity.y < 0) OnWallSlide.Invoke();
            if (sprint) OnSprint.Invoke();
            if (isCrouch) OnCrouch.Invoke();
        }

        #endregion


        #region Friction and Round

        private void SetFriction(float _frictionWall, bool _isMinimum)
        {
            collider.material.dynamicFriction = 0.6f * _frictionWall;
            collider.material.staticFriction = 0.6f * _frictionWall;

            if (_isMinimum) collider.material.frictionCombine = PhysicsMaterialCombine.Minimum;
            else collider.material.frictionCombine = PhysicsMaterialCombine.Maximum;
        }

        private float RoundValue(float _value)
        {
            float unit = (float)Mathf.Round(_value);

            if (_value - unit < 0.000001f && _value - unit > -0.000001f) return unit;
            else return _value;
        }

        #endregion

        #region Gizmos

        private void OnDrawGizmos()
        {
            if (debug)
            {
                rigidbody = this.GetComponent<Rigidbody>();
                collider = this.GetComponent<CapsuleCollider>();

                Vector3 bottomStepPos = transform.position - new Vector3(0f, originalColliderHeight / 2f, 0f) + new Vector3(0f, 0.05f, 0f);
                Vector3 topWallPos = new Vector3(transform.position.x, transform.position.y + heightWallChecker, transform.position.z);

                //ground and slope
                Gizmos.color = Color.blue;
                Gizmos.DrawWireSphere(transform.position - new Vector3(0, originalColliderHeight / 2f, 0), groundCheckerThreshold);

                Gizmos.color = Color.green;
                Gizmos.DrawWireSphere(transform.position - new Vector3(0, originalColliderHeight / 2f, 0), slopeCheckerThreshold);

                //direction
                Gizmos.color = Color.blue;
                Gizmos.DrawLine(transform.position, transform.position + forward * 2f);

                Gizmos.color = Color.cyan;
                Gizmos.DrawLine(transform.position, transform.position + globalForward * 2);

                Gizmos.color = Color.cyan;
                Gizmos.DrawLine(transform.position, transform.position + reactionForward * 2f);

                Gizmos.color = Color.red;
                Gizmos.DrawLine(transform.position, transform.position + down * 2f);

                Gizmos.color = Color.magenta;
                Gizmos.DrawLine(transform.position, transform.position + globalDown * 2f);

                Gizmos.color = Color.magenta;
                Gizmos.DrawLine(transform.position, transform.position + reactionGlobalDown * 2f);

                //step check
                Gizmos.color = Color.black;
                Gizmos.DrawLine(bottomStepPos, bottomStepPos + globalForward * stepCheckerThreshold);

                Gizmos.color = Color.black;
                Gizmos.DrawLine(bottomStepPos + new Vector3(0f, maxStepHeight, 0f), bottomStepPos + new Vector3(0f, maxStepHeight, 0f) + globalForward * (stepCheckerThreshold + 0.05f));

                Gizmos.color = Color.black;
                Gizmos.DrawLine(bottomStepPos, bottomStepPos + Quaternion.AngleAxis(45, transform.up) * (globalForward * stepCheckerThreshold));

                Gizmos.color = Color.black;
                Gizmos.DrawLine(bottomStepPos + new Vector3(0f, maxStepHeight, 0f), bottomStepPos + Quaternion.AngleAxis(45, Vector3.up) * (globalForward * stepCheckerThreshold) + new Vector3(0f, maxStepHeight, 0f));

                Gizmos.color = Color.black;
                Gizmos.DrawLine(bottomStepPos, bottomStepPos + Quaternion.AngleAxis(-45, transform.up) * (globalForward * stepCheckerThreshold));

                Gizmos.color = Color.black;
                Gizmos.DrawLine(bottomStepPos + new Vector3(0f, maxStepHeight, 0f), bottomStepPos + Quaternion.AngleAxis(-45, Vector3.up) * (globalForward * stepCheckerThreshold) + new Vector3(0f, maxStepHeight, 0f));

                //wall check
                Gizmos.color = Color.black;
                Gizmos.DrawLine(topWallPos, topWallPos + globalForward * wallCheckerThreshold);

                Gizmos.color = Color.black;
                Gizmos.DrawLine(topWallPos, topWallPos + Quaternion.AngleAxis(45, transform.up) * (globalForward * wallCheckerThreshold));

                Gizmos.color = Color.black;
                Gizmos.DrawLine(topWallPos, topWallPos + Quaternion.AngleAxis(90, transform.up) * (globalForward * wallCheckerThreshold));

                Gizmos.color = Color.black;
                Gizmos.DrawLine(topWallPos, topWallPos + Quaternion.AngleAxis(135, transform.up) * (globalForward * wallCheckerThreshold));

                Gizmos.color = Color.black;
                Gizmos.DrawLine(topWallPos, topWallPos + Quaternion.AngleAxis(180, transform.up) * (globalForward * wallCheckerThreshold));

                Gizmos.color = Color.black;
                Gizmos.DrawLine(topWallPos, topWallPos + Quaternion.AngleAxis(225, transform.up) * (globalForward * wallCheckerThreshold));

                Gizmos.color = Color.black;
                Gizmos.DrawLine(topWallPos, topWallPos + Quaternion.AngleAxis(270, transform.up) * (globalForward * wallCheckerThreshold));

                Gizmos.color = Color.black;
                Gizmos.DrawLine(topWallPos, topWallPos + Quaternion.AngleAxis(315, transform.up) * (globalForward * wallCheckerThreshold));
            }
        }

        #endregion
    }
}