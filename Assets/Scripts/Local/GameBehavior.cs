using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameBehavior : MonoBehaviour
{
    // Serialized Fields
    [Header("Main components")]
    public GameObject player;
    public Transform spawPoint;
    public Camera playerCamera;

    // Private Fields
    private GameObject playerInstance;
    private bool cameraFollowPlayer = true;

    //debug variables
    [Header("Camera Follow Offset")]
    public int yOffset = 10;
    public int zOffset = -8;
    public int xOffset = 0;

    // Start is called before the first frame update
    void Start()
    {
        playerInstance = Instantiate(player, spawPoint.position, spawPoint.rotation);
    }

    // Update is called once per frame
    void Update()
    {
        if (cameraFollowPlayer)
        {
            CameraFollow();
        }
    }

    /// <summary>
    /// The cameraFollow method adjusts the position of the playerCamera to follow the playerInstance.
    /// It sets the position of the playerCamera to be offset from the playerInstance by the values of xOffset, yOffset, and zOffset.
    /// It then makes the playerCamera look at the playerInstance.
    /// </summary>
    private void CameraFollow()
    {
        playerCamera.transform.position = new Vector3(playerInstance.transform.position.x + xOffset, playerInstance.transform.position.y + yOffset, playerInstance.transform.position.z + zOffset);
        playerCamera.transform.LookAt(playerInstance.transform);
    }

    public void OnPathSelection()
    {
        // Cast a ray from the camera to the mouse position
        Ray ray = playerCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            Vector3 target = hit.point;
            playerInstance.GetComponent<PlayerNavigation>().setTargetPosition(target);

        }
    }

    public void OnMoveCamera(InputValue value)
    {
        Vector3 v = value.Get<Vector3>();
        Debug.Log(v);
    }

}