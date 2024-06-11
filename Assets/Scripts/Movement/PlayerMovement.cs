using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float jumpHeight;
    [SerializeField] private LayerMask groundMask;

    private CharacterController characterController;
    public CharacterController CharacterController{ get{if (characterController == null)
                characterController = GetComponent<CharacterController>(); return characterController;}}

    private Camera characterCamera;
    public Camera CharacterCamera{ get{if (characterCamera == null)
                characterCamera = FindObjectOfType<Camera>(); return characterCamera;}}

    private Animator characterAnimator;
    public Animator CharacterAnimator{get{if (characterAnimator == null)
                characterAnimator = GetComponent<Animator>(); return characterAnimator;}}


    private Vector3 verticalVelocity = Vector3.zero;

    private float gravity = -9.81f;
    private float animationBlandSpeed = 0.02f;
    private float targetAnimationSpeed = 0.0f;
    private float rotationAngle;
    private float curentSpeed;
    private float standingSpeed = 0.0f;

    private bool isJumping;
    public bool IsJumping { get { return isJumping;} private set { isJumping = value; } }

    private bool isStanding;
    private bool isJump;
    private bool isWalk;
    private bool isRun;

    private Vector2 input;
    private Vector3 move;

    private void OnEnable()
    {
        CharacterAnimator.SetFloat("Speed", 0.0f);

        isJumping = false;
    }

    private void Update()
    {
        move = new Vector3(input.x, 0.0f, input.y);

        if (isRun)
        {
            var iAmRun = 1.0f;
            curentSpeed = runSpeed;
            targetAnimationSpeed = iAmRun;
        }
        if(isWalk)
        {
            var iAmWalk = 0.5f;
            curentSpeed = walkSpeed;
            targetAnimationSpeed = iAmWalk;
        }
        if(isStanding)
        {
            move = Vector3.zero;

            var iAmStanding = 0.0f;
            curentSpeed = standingSpeed;
            targetAnimationSpeed = iAmStanding;
        }

        CheckGrounded();

        verticalVelocity.y = verticalVelocity.y + gravity * Time.deltaTime;

        CharacterAnimator.SetFloat("SpeedY", verticalVelocity.y / jumpHeight);

        CheckLanding();

        var rotatedMovement = Quaternion.Euler(0.0f, CharacterCamera.transform.rotation.eulerAngles.y, 0.0f) * move.normalized;
        var verticalMovement = Vector3.up * verticalVelocity.y;

        if (move.magnitude > 0.0f)
        {
            rotationAngle = Mathf.Atan2(rotatedMovement.x, rotatedMovement.z) * Mathf.Rad2Deg;
        }

        CharacterController.Move((verticalMovement + rotatedMovement * curentSpeed) * Time.deltaTime);

        float animSpeed = Mathf.Lerp(CharacterAnimator.GetFloat("Speed"), targetAnimationSpeed, animationBlandSpeed);
        CharacterAnimator.SetFloat("Speed", animSpeed);

        Quaternion currentRotation = CharacterController.transform.rotation;
        Quaternion targetRotation = Quaternion.Euler(0.0f, rotationAngle, 0.0f);

        CharacterController.transform.rotation = Quaternion.Lerp(currentRotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    private void CheckGrounded()
    {
        var isGrounded = Physics.CheckSphere(transform.position, 0.1f, groundMask);
        if (isGrounded)
        {
            verticalVelocity.y = -1.0f;

            if (isJumping == false)
            {
                if (isJump)
                {
                    verticalVelocity.y = Mathf.Sqrt(-2f * jumpHeight * gravity);
                    CharacterAnimator.SetTrigger("Jump");
                    isJumping = true;
                    isJump = false;
                }
            }
        }
    }

    private void CheckLanding()
    {
        if (isJumping && verticalVelocity.y < 0.0f)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, Vector3.down, out hit, 0.1f, groundMask))
            {
                CharacterAnimator.SetTrigger("Land");
                isJumping = false;
                isJump = false;
            }
            
        }
    }

    public void SetInputActions(Vector2 inputActions)
    {
        input = inputActions;
    }

    public void SetMovementProperties(bool jupm, bool run, bool walk, bool standing)
    {
        isJump = jupm;
        isStanding = standing;
        isWalk = walk;
        isRun = run;
    }
}
