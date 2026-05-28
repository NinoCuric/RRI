using UnityEngine;

public class PlayerInteractable : MonoBehaviour
{
    private bool canInteract = false;
    private ElevatorMovement currentObject;

    void Update()
    {
        if (canInteract && Input.GetKeyDown(KeyCode.E))
        {
            StartCoroutine(currentObject.Activate());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        ElevatorMovement interactable = other.GetComponentInParent<ElevatorMovement>();
        if (other.CompareTag("Elevator"))
        {
            canInteract = true;
            currentObject = interactable;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        ElevatorMovement interactable = other.GetComponentInParent<ElevatorMovement>();
        if (other.CompareTag("Elevator"))
        {
            canInteract = false;
            currentObject = null;
        }
    }
}