using UnityEngine;

public class physicsmover : MonoBehaviour
{
    public float moveSpeed;

    private Vector2 input;
    private Rigidbody _rb;

    private int moveType = 1;

    private void Start()
    {
        _rb = this.GetComponent<Rigidbody>();
    }

    void Update()
    {
        UpdateInput();
    }

    private void FixedUpdate()
    {
        switch (moveType)
        {
            case 1:
                MoveWithBySettingPosition();
                break;
            case 2:
                MoveWithMovePosition();
                break;
            case 3:
                MoveWithAddForce();
                break;
            case 4:
                MoveWithAddTorque();
                break;
        }
    }

    private void UpdateInput()
    {
        input = Vector2.zero;
        if (Input.GetKey(KeyCode.W))
            input.y += 1;
        if (Input.GetKey(KeyCode.S))
            input.y -= 1;
        if (Input.GetKey(KeyCode.D))
            input.x += 1;
        if (Input.GetKey(KeyCode.A))
            input.x -= 1;


        if (Input.GetKeyDown(KeyCode.Alpha1)) moveType = 1;
        if (Input.GetKeyDown(KeyCode.Alpha2)) moveType = 2;
        if (Input.GetKeyDown(KeyCode.Alpha3)) moveType = 3;
        if (Input.GetKeyDown(KeyCode.Alpha4)) moveType = 4;
    }

    private void MoveWithBySettingPosition()
    {
        this.transform.position += new Vector3(moveSpeed * input.x, 0, moveSpeed * input.y);
    }

    private void MoveWithMovePosition()
    {
        _rb.MovePosition(this.transform.position + new Vector3(moveSpeed * input.x, 0, moveSpeed * input.y));
    }

    private void MoveWithAddForce()
    {
        _rb.AddForce(new Vector3(moveSpeed * input.x, 0, moveSpeed * input.y));
    }

    private void MoveWithAddTorque()
    {
        _rb.AddTorque(moveSpeed * input.y, 0, moveSpeed * -input.x);
    }
}
