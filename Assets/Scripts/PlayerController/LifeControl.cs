using System.Collections;
using UnityEngine;

public class LifeControl : MonoBehaviour
{
    private Animator animator;
    public Animator CharacterAnimator { get { if (animator == null) animator = GetComponent<Animator>();  return animator; } }

    private PlayerMovement playerMovement;
    public PlayerMovement PlayerMovement { get { if (playerMovement == null) playerMovement = GetComponent<PlayerMovement>(); 
            return playerMovement; } }

    private CharacterController characterController;
    public CharacterController CharacterController { get { if(characterController == null)
                characterController = GetComponent<CharacterController>(); return characterController; } }


    private bool isAlive = true;

    private bool canBePressedDeath;
    public bool CanBePressedDeath
    {
        get { return canBePressedDeath; }
        set { canBePressedDeath = value; }
    }

    private bool canBePressedSpawn;
    public bool CanBePressedSpawn
    {
        get { return canBePressedSpawn; }
        set { canBePressedSpawn = value; }
    }

    private bool canBePressedJump;
    public bool CanBePressedJump
    {
        get { return canBePressedJump; }
        set { canBePressedJump = value; }
    }

    private bool canBePressedKick;
    public bool CanBePressedKick
    {
        get { return canBePressedKick; }
        set { canBePressedKick = value; }
    }

    private bool isDeath;

    private bool isSpawn;

    private float waitingAnimation = 1.5f;

    private float gravity = -9.81f;


    private void Start()
    {
        CanBePressedDeath = true;

        CanBePressedSpawn = false;

        CanBePressedJump = true;

        CanBePressedKick = true;
    }

    private void Update()
    {
        if(isAlive)
        {
            if (isDeath)
            {
                isSpawn = false;
                CharacterAnimator.SetTrigger("Death");

                PlayerMovement.enabled = false;

                isAlive = false;

                CanBePressedDeath = false;
                CanBePressedSpawn = true;

                CanBePressedJump = false;
                CanBePressedKick = false;
            }
        }
        
        if (!isAlive)
        {
            CharacterController.Move(Vector3.up * gravity * Time.deltaTime);

            if (isSpawn)
            {
                isSpawn = false;
                CanBePressedSpawn = false;
                isDeath = false;
                CharacterAnimator.SetTrigger("Spawn");
                
                StartCoroutine(StopMovement());
            }
        }
    }

    private IEnumerator StopMovement()
    {
        yield return new WaitForSeconds(waitingAnimation);

        PlayerMovement.enabled = true;

        isAlive = true;

        CanBePressedDeath = true;

        CanBePressedJump = true;

        CanBePressedKick = true;
    }

    public void SetLifeControlProp(bool death, bool spawn)
    {
        isDeath = death;
        isSpawn = spawn;
    }
}
