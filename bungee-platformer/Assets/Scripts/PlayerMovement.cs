using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Transform debugHitPointTransform;

    public CharacterController controller;
    public Camera camera;
    public float speed = 12f;
    public float gravity = -9.81f;
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    public float jumpHeight = 3f;


    Vector3 velocity;
    bool isGrounded;

    private Vector3 hookshotPosition;
    private Vector3 PlayerMomentum;
    private State state = State.Normal;
    private enum State
    {
        Normal,
        HookshotFlyingPlayer
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            default:
            case State.Normal:
                HandleMovement();
                HandleHookshotStart();
                break;
            case State.HookshotFlyingPlayer:
                HandleHookshotMovement();
                break;
        }
    }

    private void HandleMovement()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
        }

        float x = Input.GetAxis("Horizontal");
        float z = -Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        controller.Move(move * speed * Time.deltaTime);

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
    }

    private void HandleHookshotStart()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (Physics.Raycast(camera.transform.position, camera.transform.forward, out RaycastHit raycastHit)) {
                debugHitPointTransform.position = raycastHit.point;
                hookshotPosition = raycastHit.point;
                state = State.HookshotFlyingPlayer;
            }
        }
    }

    private void ResetGravityEffect()
    {
        velocity = Vector3.zero;
    }

    private void HandleHookshotMovement()
    {
        Vector3 hookshotDir = (hookshotPosition - transform.position).normalized;

        float hookshotSpeedMin = 10f;
        float hookshotSpeedMax = 40f;
        float hookshotSpeed = Mathf.Clamp(Vector3.Distance(transform.position, hookshotPosition), hookshotSpeedMin, hookshotSpeedMax);
        float hookshotSpeedMultiplier = 2f;

        controller.Move(hookshotDir * hookshotSpeed * hookshotSpeedMultiplier * Time.deltaTime);

        float reachedHookshotPositionDistance = 1.5f;

        if (Vector3.Distance(transform.position, hookshotPosition) < reachedHookshotPositionDistance)
        {
            state = State.Normal;
            ResetGravityEffect();
        }

        if (TestInputDownHookshot())
        {
            state = State.Normal;
            ResetGravityEffect();
        }
    }

    private bool TestInputDownHookshot()
    {
        return Input.GetKeyDown(KeyCode.E);
    }
}
