using UnityEngine;
using UnityEngine.UIElements;

public class PerspectiveSwitch : MonoBehaviour
{
    private bool CameraToggled = false;
    private Transform CameraPivot;
    private GameObject Player;


    [SerializeField] private GameObject Gun;
    [SerializeField] private GameObject Gun3rdPerson;
    [Space]
    [SerializeField] private Vector3 FirstPerson;
    [SerializeField] private Vector3 ThirdPerson;

    private void Start()
    {
        CameraPivot = transform.parent;
        Player = transform.parent.parent.gameObject;

        Gun3rdPerson.SetActive(false);

        FirstPerson = transform.localPosition;
        ThirdPerson = new Vector3(transform.localPosition.x + 1f, transform.localPosition.y, transform.localPosition.z - 2f);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            CameraToggled = !CameraToggled;

            if (CameraToggled)
            {
                transform.localPosition = ThirdPerson;
                Gun.SetActive(false);
                Gun3rdPerson.SetActive(true);
            }
            else
            {
                transform.localPosition = FirstPerson;
                Gun.SetActive(true);
                Gun3rdPerson.SetActive(false);
            }
        }

        if (CameraToggled)
        {
            float z = CameraPivot.rotation.eulerAngles.x;
            float y = Player.transform.rotation.eulerAngles.y;

            Gun3rdPerson.transform.rotation = Quaternion.Euler(0f, y + 90f, z);
        }
    }
}
