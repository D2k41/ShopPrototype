using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float RunThreshold = 0.8f;
    public float Direction;
    public bool Running = false;
    public bool Walking = false;

    private float horizontalInput;
    private float verticalInput;

    private PlayerStats playerStats;
    private Rigidbody2D rb;
    private Animator animator;

    private void Awake()
    {
        GameController.Instance.PlayerMovement = this;
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerStats = GameController.Instance.PlayerStats;
    }

    private void Update()
    {
        // Get input for movement
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
    }

    private void FixedUpdate()
    {
        // Calculate movement direction
        Vector2 movement = new Vector2(horizontalInput, verticalInput).normalized;

        if (horizontalInput != 0 || verticalInput != 0)
        {
            // Move the player
            Move(movement);
        }

        UpdateAnimation(movement);
        RegulateRunWalk();
    }

    private void Move(Vector2 direction)
    {
        Vector2 newDirection = rb.position + direction * playerStats.MovementSpeed * Time.deltaTime;
        rb.MovePosition(newDirection);
    }

    private void UpdateAnimation(Vector2 direction)
    {
        // Calculate the angle of movement relative to the up direction
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Normalize the angle to be in the range of 0 to 360 degrees
        if (angle < 0)
        {
            angle += 360f;
        }

        Direction = angle;

        // Update the "Direction" parameter of the animator
        if (animator != null)
        {
            if (horizontalInput != 0 || verticalInput != 0)
            {
                RegulateDirection(Direction);
            }
            animator.SetBool("Walk", Walking);
            animator.SetBool("Run", Running);
        }
    }

    private void RegulateRunWalk()
    {
        if (horizontalInput != 0 || verticalInput != 0)
        {
            Walking = true;
            Running = Mathf.Abs(horizontalInput) > 0.8f || Mathf.Abs(verticalInput) > RunThreshold;
        }
        else
        {
            Walking = false;
        }
    }

    private void RegulateDirection(float angle)
    {
        if ((angle >= 0 && angle < 45) || (angle >= 315 && angle < 360))
        {
            animator.SetBool("Right", true);
            animator.SetBool("Left", false);
            animator.SetBool("Up", false);
            animator.SetBool("Down", false);
        }
        else if (angle >= 45 && angle < 135)
        {
            animator.SetBool("Up", true);
            animator.SetBool("Left", false);
            animator.SetBool("Down", false);
            animator.SetBool("Right", false);
        }
        else if (angle >= 135 && angle < 225)
        {
            animator.SetBool("Left", true);
            animator.SetBool("Up", false);
            animator.SetBool("Down", false);
            animator.SetBool("Right", false);
        }
        else if (angle >= 225 && angle < 315)
        {
            animator.SetBool("Down", true);
            animator.SetBool("Left", false);
            animator.SetBool("Up", false);
            animator.SetBool("Right", false);
        }
    }
}
