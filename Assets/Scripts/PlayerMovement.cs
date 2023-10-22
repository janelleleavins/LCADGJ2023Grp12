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

    //private float gravity = 20f;
    private float speed = 6f;
    private float jumpSpeed = 4;

    bool isGrounded = true;
    bool hasWings = true;

    public GameManager simonSays;
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

        //TODO: if flips, call turn
    }

    private void Turn()
    {
        // flips model depending on the way they move ?
    }

    private void Jump()
    {
        if (isGrounded)
        {
            rb.velocity += Vector3.up * jumpSpeed;
        }
        else if (hasWings & transform.position.y < heightLimit)
        {
            //Debug.Log(transform.position.y + " " + heightLimit);
            rb.velocity += Vector3.up * jumpSpeed;
        }

    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }

        if (collision.gameObject == simonSays.neighborModel1)
        {
            simonSays.MakeRoundOne();
            freezePlayer();
        }
        else if (collision.gameObject == simonSays.neighborModel2)
        {
            simonSays.MakeRoundTwo();
            freezePlayer();
        }
        else if (collision.gameObject == simonSays.neighborModel3)
        {
            simonSays.MakeRoundThree();
            freezePlayer();
        }
        else if (collision.gameObject.CompareTag("Drop"))
        {
            transform.position = new Vector3(0, 1, 0); //CHANGE
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
        }
    }
}
