using UnityEngine.InputSystem;
using UnityEngine;
using System.Runtime.InteropServices;

public class PlayerInputController : MonoBehaviour
{
    private InputControlActions inputControlActions;
    private InputControlActions.PlayerActions playerActions;
    private InputControlActions.LifeControlActions lifeControlActions;
    private PlayerMovement playerMovement;
    private LifeControl lifeControl;
    private KickControl kickControl;

    private Vector2 inputMovement;

    private bool isRun;
    private bool isWalk;
    private bool isStanding;
    private bool isJump;
    private bool isSpawn;
    private bool isDeath;

    private void OnEnable()
    {
        inputControlActions = new InputControlActions();
        playerActions = inputControlActions.Player;
        lifeControlActions = inputControlActions.LifeControl;

        playerMovement = GetComponent<PlayerMovement>();
        lifeControl = GetComponent<LifeControl>();
        kickControl = GetComponent<KickControl>();

        if (inputControlActions == null)
        {
            Debug.LogError($"InputControlActions in OnEnable is NULL!");
            return;
        }

        inputControlActions.Enable();

        playerActions.Movement.started += Movement;
        playerActions.Movement.performed += Movement;
        playerActions.Movement.canceled += Movement;

        playerActions.Run.started += Run;
        playerActions.Run.canceled += Run;

        playerActions.Jump.started += Jump;
        playerActions.Jump.performed += Jump;
        playerActions.Jump.canceled += Jump;

        playerActions.Kick.started += Kick;

        lifeControlActions.Spawn.started += Spawn;
        lifeControlActions.Death.started += Death;


    }

    private void Kick(InputAction.CallbackContext obj)
    {
        if (lifeControl.CanBePressedKick)
        {
            kickControl.Kick();
        }
    }

    private void Death(InputAction.CallbackContext obj)
    {
        if (obj.started)
        {
            if(lifeControl.CanBePressedDeath)
            {
                isDeath = true;

                lifeControl.SetLifeControlProp(isDeath, isSpawn);
            }
        }
    }

    private void Spawn(InputAction.CallbackContext obj)
    {
        if(obj.started)
        {
            if (lifeControl.CanBePressedSpawn)
            {
                isSpawn = true;

                lifeControl.SetLifeControlProp(isDeath, isSpawn);
            }
        }
    }

    private void Jump(InputAction.CallbackContext obj)
    {
        if (obj.started) 
        {
            if (lifeControl.CanBePressedJump)
            {
                isJump = true;

                playerMovement.SetMovementProperties(isJump, isRun, isWalk, isStanding);
            }
        }
    }

    private void Run(InputAction.CallbackContext obj)
    {
        if (isStanding)
        {
            return;
        }
        if(isWalk)
        {
            if (obj.started)
            {
                isRun = true;
                isJump = false;
                isStanding = false;
                isWalk = false;

                playerMovement.SetMovementProperties(isJump, isRun, isWalk, isStanding);
            }
        }
        if (isRun)
        {
            if (obj.canceled)
            {
                isRun = false;
                isStanding = false;
                isJump = false;

                isWalk = true;

                playerMovement.SetMovementProperties(isJump, isRun, isWalk, isStanding);
            }
        }
    }

    private void Movement(InputAction.CallbackContext obj)
    {
        if (obj.started)
        {
            isWalk = true;

            isStanding = false;
            isRun = false;
            isJump = false;

            playerMovement.SetMovementProperties(isJump, isRun, isWalk, isStanding);
        }
        if (obj.performed)
        {
            inputMovement = obj.ReadValue<Vector2>();
            playerMovement.SetInputActions(inputMovement);
        }
        if (obj.canceled)
        {
            isJump = false;
            isWalk = false;
            isRun = false;

            isStanding = true;

            playerMovement.SetMovementProperties(isJump, isRun, isWalk, isStanding);
        }
    }

    private void OnDisable()
    {
        if (inputControlActions == null)
        {
            Debug.LogError($"InputControlActions in OnDisable is NULL!");
            return;
        }

        inputControlActions.Disable();

        playerActions.Movement.started -= Movement;
        playerActions.Movement.performed -= Movement;
        playerActions.Movement.canceled -= Movement;

        playerActions.Run.started -= Run;
        playerActions.Run.canceled -= Run;

        playerActions.Jump.started -= Jump;
        playerActions.Jump.performed -= Jump;
        playerActions.Jump.canceled -= Jump;

        playerActions.Kick.started -= Kick;

        lifeControlActions.Spawn.started -= Spawn;
        lifeControlActions.Death.started -= Death;
    }
}
