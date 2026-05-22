using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.XR;


public class PlayerMovementController : MonoBehaviour
{
    private Vector3 Velocity;
    private Vector3 PlayerMovementInput;
    private Vector2 PlayerMouseInput;
    private float xRot;
    private bool Sprinting = false;
    private bool Crouching = false;
    private float Stamina;
    private bool StaminaRecharging = false;
    private float MaxStamina = 100f;
    private float StaminaSecond; 

    //[SerializeField] private Transform PlayerCamera;
    [SerializeField] private Transform CameraPivot;
    [SerializeField] private CharacterController Controller;
    [Space]
    [SerializeField] private float Speed = 1.5f;
    [SerializeField] private float Jumpforce = 5f;
    [SerializeField] private float Sensitivity = 3;
    [SerializeField] private float Gravity = -9.81f;
    [SerializeField] private float SprintTime = 5f;

    void Start()
    {
        Stamina = MaxStamina;
        StaminaSecond = MaxStamina / SprintTime;
    }

    void Update()
    {
        PlayerMovementInput = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
        PlayerMouseInput = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            SprintStop(); 
        }


        UseStamina();

        MovePlayer();
        MovePlayerCamera();
    }

    private void MovePlayer()
    {
        Vector3 MoveVector = transform.TransformDirection(PlayerMovementInput);

        if(Controller.isGrounded)
        {
            Velocity.y = -1f;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                Velocity.y = Jumpforce;
            }

            if (Input.GetKeyDown(KeyCode.LeftShift) && !StaminaRecharging)
            {
                Speed *= 2f;
                Sprinting = !Sprinting;
            }
        }
        else
        {
            Velocity.y -= Gravity * -2f * Time.deltaTime;
        }

        Crouch();

        Controller.Move(MoveVector * Speed * Time.deltaTime);
        Controller.Move(Velocity * Time.deltaTime);
    }

    private void MovePlayerCamera()
    {
        xRot -= PlayerMouseInput.y * Sensitivity;
        xRot = Mathf.Clamp(xRot, -60f, 60f);
        transform.Rotate(0f, PlayerMouseInput.x * Sensitivity, 0f);
        CameraPivot.localRotation = Quaternion.Euler(xRot, 0f, 0f);
        //PlayerCamera.transform.localRotation = Quaternion.Euler(xRot, 0f, 0f);
    }

    private void SprintStop()
    {
        if (Sprinting)
        {
            Speed /= 2f;
            Sprinting = !Sprinting;
        }
    }

    private void UseStamina()
    {
        if (StaminaRecharging)
        {
            Stamina += StaminaSecond * Time.deltaTime;
            if (Stamina >= MaxStamina)
            {
                StaminaRecharging = false;
                Stamina = MaxStamina;
            }
        }
        else if (!Sprinting)
        {
            if (Stamina < MaxStamina)
            {
                Stamina += StaminaSecond * Time.deltaTime;
            }
            else if (Stamina > MaxStamina)
            {
                Stamina = MaxStamina;
            }
        }
        else if (Sprinting)
        {
            if (Stamina > 0f)
            {
                Stamina -= StaminaSecond * Time.deltaTime;
            }
            else
            {
                Stamina = 0f;
                SprintStop();
                StaminaRecharging = true;
            }
        }
    }


private void Crouch()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl) && !Crouching)
        {
            Crouching = !Crouching;

            SprintStop();
            Speed /= 2f;
            Controller.height /= 2f;
            Controller.center += new Vector3(0, Controller.height / 2f, 0);
        }
        else if (Input.GetKeyUp(KeyCode.LeftControl) && Crouching)
        {
            Crouching = !Crouching;
            Speed *= 2f;
            Controller.center -= new Vector3(0, Controller.height / 2f, 0);
            Controller.height *= 2f;
        }
       
    }

}
