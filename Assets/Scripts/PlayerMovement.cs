using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.U2D;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMovement : MonoBehaviour
{
    // Handles all player movement
    [SerializeField] private Rigidbody rb;

    private Vector3 PlayerMovementInput;

    //private float gravity = 20f;
    private float speed = 6f;
    private float jumpSpeed = 5;

    public Vector3 movement;

    bool isGrounded = true;

    // Update is called once per frame
    void Update()
    {
        PlayerMovementInput = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
        Walk();
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Fly();
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

    private void Fly()
    {
        // TODO: if wings earned, jump infintiely. otherwise, jump depending on whether they are already on the ground
        if (isGrounded)
        {
            rb.velocity += Vector3.up * jumpSpeed;
        }

    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }


    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}
