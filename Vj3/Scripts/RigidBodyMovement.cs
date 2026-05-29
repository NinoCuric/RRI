using UnityEngine;

public class RigidBodyMovement : MonoBehaviour
{
    private Vector3 PlayerMovementInput;
    private Vector2 PlayerMouseInput;
    private float xRot;
    private bool onSpeedPad = false;
    private bool onJumpPad = false;


    [SerializeField] private LayerMask FloorMask;
    [SerializeField] private Transform FeetTransform;
    [SerializeField] private Transform PlayerCamera;
    [SerializeField] private Rigidbody PlayerBody;
    [Space]
    [SerializeField] private float Speed;
    [SerializeField] private float Sensitivty;
    [SerializeField] private float Jumpforce;

    void Update()
    {
        PlayerMovementInput = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
        PlayerMouseInput = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        MovePlayer();
        MovePlayerCamera();

    }

    private void MovePlayer()
    {
        float speedFinal;
        speedFinal = onSpeedPad ? Speed * 3f : Speed;

        Vector3 MoveVector = transform.TransformDirection(PlayerMovementInput) * speedFinal;
        PlayerBody.linearVelocity = new Vector3(MoveVector.x, PlayerBody.linearVelocity.y, MoveVector.z);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (Physics.CheckSphere(FeetTransform.position, 0.1f, FloorMask))
            {
                float JumpforceFinal;
                JumpforceFinal = onJumpPad ? Jumpforce * 3f : Jumpforce;
                PlayerBody.AddForce(Vector3.up * JumpforceFinal, ForceMode.Impulse);
            }
        }
    }

    private void MovePlayerCamera()
    {
        xRot -= PlayerMouseInput.y * Sensitivty;
        xRot = Mathf.Clamp(xRot, -60f, 60f);
        transform.Rotate(0f, PlayerMouseInput.x * Sensitivty, 0f);
        PlayerCamera.transform.localRotation = Quaternion.Euler(xRot, 0f, 0f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Highway"))  
        {
            onSpeedPad = true;
        }
        if (other.CompareTag("HighJumpPlatform"))  
            {
            onJumpPad = true;
        }
        if (other.CompareTag("Coin"))
        {
            PlayerBody.AddForce(Vector3.up * 10f, ForceMode.Impulse);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Highway"))
        {
            onSpeedPad = false;
        }
        if (other.CompareTag("HighJumpPlatform"))
        {
            onJumpPad = false;
        }
    }

}
