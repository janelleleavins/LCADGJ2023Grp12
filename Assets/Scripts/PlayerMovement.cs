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

    private Vector3 PlayerMovementInput;

    private float speed = 6f;
    private float jumpSpeed = 4;

    bool isGrounded = true;
    bool hasWings = true;
    bool hasHorns = false;
    bool facingRight = true;

    public SpriteRenderer playerSprite;

    public Sprite defaultSprite;
    public Sprite wingsSprite;
    public Sprite hornsWingsSprite;
    public Sprite jumpSprite;
    public Sprite flyingSprite;
    public Sprite hornsFlyingSprite;

    public DialogueHandler dialogueHandler;
    private int heightLimit = 4;

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

        if (Input.GetAxis("Horizontal") < 0 && facingRight)
        {
            
            playerSprite.flipX = false;
            facingRight = false;

        }
        else if (Input.GetAxis("Horizontal") > 0 && !facingRight)
        {
            playerSprite.flipX = true;
            facingRight = true;
        }
    }

    private void Jump()
    {
        if (isGrounded)
        {
            rb.velocity += Vector3.up * jumpSpeed;
        }
        else if (hasWings & transform.position.y < heightLimit)
        {
            rb.velocity += Vector3.up * jumpSpeed; //this wonky afffff
        }

    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            if (!hasWings)
            {
                playerSprite.sprite = defaultSprite;
            }
            else if (hasWings && hasHorns)
            {
                playerSprite.sprite = hornsWingsSprite;
            }
            else{
                playerSprite.sprite = wingsSprite;
            }
        }
    }

    void freezePlayer()
    {
        speed = 0;
        jumpSpeed = 0;
    }

    public void unfreezePlayer()
    {
        speed = 6f;
        jumpSpeed = 5;
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
            if(!hasWings)
            {
                playerSprite.sprite = jumpSprite;
            }
            else if(hasWings && hasHorns)
            {
                playerSprite.sprite = hornsFlyingSprite;
            }
            else
            {
                playerSprite.sprite = flyingSprite;
            }
                
        }
    }
}
