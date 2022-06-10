using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float movePower;
    private Vector3 velocity;
    public Vector3 currentDirection;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        velocity = new Vector3();

        Move();
        RotateDirection();
    }

    // ˆÚ“®ˆ—
    private void Move()
    {
        Vector3 moveVector = new Vector3(Input.GetAxisRaw("Horizontal"),0, Input.GetAxisRaw("Vertical"));
        Vector3 inputDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        bool isInput;

        velocity += movePower * Time.deltaTime * moveVector.normalized;

        transform.position += velocity;

        isInput = inputDirection.magnitude > 0.5f;
        if(isInput)currentDirection = inputDirection.normalized;
    }

    private void RotateDirection()
    {
        transform.rotation = Quaternion.LookRotation(currentDirection);
    }
}
