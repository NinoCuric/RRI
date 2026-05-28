using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class ElevatorMovement : MonoBehaviour
{
    public GameObject elevatorEnd;
    public float elevatorSpeed = 0.5f;

    private Vector3 startPos;
    private Vector3 endPos;
    private bool elevatorUp = false;

    void Start()
    {
        startPos = transform.position;
        endPos = elevatorEnd.transform.position;
    }

    public IEnumerator Activate()
    {
        float t = elevatorUp ? 1f : 0f;
        while (t <= 1f && t >= 0f)
        {
            if (elevatorUp)
                t = t - Time.deltaTime * elevatorSpeed;
            else
                t = t + Time.deltaTime * elevatorSpeed;
            transform.position = Vector3.Lerp(startPos, endPos, t);
            yield return null;
        }
        elevatorUp = !elevatorUp;

    }
}