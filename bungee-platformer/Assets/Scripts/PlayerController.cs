using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float jumpForce;
    public float gravityModifier;
    public float walkAcceleration = 10.0f;
    public bool isOnGround = true;
    public float xInput;
    public float yInput;
    public float xMouse;
    public float yMouse;

    private Rigidbody _playerRb;
    private Camera _camera;
    // Start is called before the first frame update
    void Start()
    {
        _playerRb = GetComponent<Rigidbody>();
        _camera = GetComponent<Camera>();
        Physics.gravity *= gravityModifier;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isOnGround == true)
        {
            isOnGround = false;
            _playerRb.AddForce(Vector3.up * jumpForce);
        }
        ProcessInputs();
        Look();
        Move();
        Turn();
    }

    private void OnCollisionEnter()
    {
        isOnGround = true;
    }

    private void ProcessInputs()
    {
        xInput = Input.GetAxis("Vertical");
        yInput = Input.GetAxis("Horizontal");
        xMouse = Input.GetAxis("Mouse X");
        yMouse = Input.GetAxis("Mouse Y");
    }

    private void Move()
    {
        if (isOnGround == true && _playerRb.velocity.magnitude < 20.0f)
        {
            _playerRb.AddForce(new Vector3(xInput, 0f, yInput) * walkAcceleration);
        }
    }

    private void Look()
    {
        if (yMouse > 0)
        {
            transform.Rotate(Vector3.left);
        }
        if (yMouse < 0)
        {
            transform.Rotate(Vector3.right);
        }
        if (yMouse == 0)
        {
            transform.Rotate(Vector3.zero);
        }
    }

    private void Turn()
    {
        if (xMouse > 0)
        {
            _playerRb.transform.Rotate(Vector3.up);
        }
        if (xMouse < 0)
        {
            _playerRb.transform.Rotate(Vector3.down);
        }
        if (xMouse == 0)
        {
            _playerRb.transform.Rotate(Vector3.zero);
        }
    }
}
