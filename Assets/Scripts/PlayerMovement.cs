using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.U2D;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMovement : MonoBehaviour
{
    // Singleton instance of the player.
    public static PlayerMovement instance = null;

    // Handles all player movement
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Camera cam;

    private Vector3 PlayerMovementInput;

    private float speed = 6f;
    private float jumpSpeed = 4;

    bool isGrounded = true;
    public bool hasWings = false;
    public bool hasHorns = false;
    bool facingRight = true;

    bool isEngaged = false;

    public SpriteRenderer playerSprite;

    public Sprite defaultSprite;
    public Sprite wingsSprite;
    public Sprite hornsSprite;
    public Sprite jumpSprite;
    public Sprite flyingSprite;
    public Sprite hornsJumpSprite;
    public Sprite attackSprite;

    //public DialogueHandler dialogueHandler; why is dis here.

    private void Awake()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        PlayerMovementInput = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
        Walk();
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
    }

    private void Walk()
    {
        Vector3 MoveVector = transform.TransformDirection(PlayerMovementInput) * speed;
        rb.velocity = new Vector3(MoveVector.x, rb.velocity.y, MoveVector.z);

        if (MoveVector.x < 0 && facingRight)
        {
            playerSprite.flipX = false;
            facingRight = false;
        }
        else if (MoveVector.x > 0 && !facingRight)
        {
            playerSprite.flipX = true;
            facingRight = true;
        }
        else
        {
            //
        }
    }

    private void Jump()
    {
        if (isGrounded || hasWings)
        {
            rb.velocity += Vector3.up * jumpSpeed;
        }

    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            if (!hasHorns)
            {
                playerSprite.sprite = defaultSprite;
            }
            else if (hasWings && hasHorns)
            {
                playerSprite.sprite = wingsSprite;
            }
            else{
                playerSprite.sprite = hornsSprite;
            }
        }
    }

    public void freezePlayer()
    {
        speed = 0;
        jumpSpeed = 0;
    }

    public void unfreezePlayer()
    {
        speed = 6f;
        jumpSpeed = 5;
    }
    public void giveHorns()
    {
        hasHorns = true;
        playerSprite.sprite = hornsSprite;
    }
    public void giveWings()
    {
        hasWings = true;
        playerSprite.sprite = wingsSprite;
    }

    public void attackSprites()
    {
        playerSprite.flipX = true;
        if (playerSprite.sprite == attackSprite)
        {
            playerSprite.sprite = hornsSprite;
        }
        else
        {
            playerSprite.sprite = attackSprite;
        }
    }

    public void cameraShift()
    {
        if (isEngaged)
        {
            //TODO: shift camera for battle
        }
        else
        {
            //cam.Reset(); //resets camera after battle
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
            if(!hasHorns)
            {
                playerSprite.sprite = jumpSprite;
            }
            else if(hasWings && hasHorns)
            {
                playerSprite.sprite = flyingSprite;
            }
            else
            {
                playerSprite.sprite = hornsJumpSprite;
            }
                
        }
    }
}
