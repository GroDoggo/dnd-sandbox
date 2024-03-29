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
    public GameObject HighlightPositionOsbject;
    public Transform EntitiesParent;

    // Private Fields
    private GameObject playerInstance;
    private bool cameraFollowPlayer = true;
    private Vector3 cameraInputMovement = Vector3.zero;
    private GameObject highlightPositionInstance;
    

    //debug variables
    [Header("Camera Follow Offset")]
    public int yOffset = 10;
    public int zOffset = -8;
    public int xOffset = 0;
    [Header("Camera Speed")]
    public float cameraTranslationSpeed = 0.01f;
    public float cameraRotationSpeed = 0.01f;
    [Header("Debug Ray Cast")]
    public GameObject rayGameObject;
    private GameObject rayInstance;


    // Start is called before the first frame update
    void Start()
    {
        playerInstance = Instantiate(player, spawPoint.position, spawPoint.rotation, EntitiesParent);
    }

    // Update is called once per frame
    void Update()
    {
        if (cameraFollowPlayer)
        {
            CameraFollow();
        } else
        {
            playerCamera.transform.position = playerCamera.transform.position + cameraInputMovement * cameraTranslationSpeed;
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

            /* DEBUG */
            if (rayInstance != null)
                Destroy(rayInstance);

            rayInstance = Instantiate(rayGameObject, Vector3.zero, Quaternion.identity);
            rayInstance.GetComponent<LineRenderer>().SetPosition(0, ray.origin);
            rayInstance.GetComponent<LineRenderer>().SetPosition(1, hit.point);

            //Debug.Log("Hit: " + hit.collider.gameObject.name);
            /* END DEBUG */

            //if the tag of the object hit is "Terrain", move the player to the position of the hit
            if (hit.collider.tag == "Terrain")
            {
                Vector3 target = hit.point;
                playerInstance.GetComponent<PlayerNavigation>().setTargetPosition(target);
                DestroyTargetPositionHighlight();
                highlightPositionInstance = Instantiate(HighlightPositionOsbject, target + new Vector3(0.0f, 0.1f, 0.0f), Quaternion.identity, EntitiesParent);
            }

            //if the tag of the object hit is "Player", activate camera follow
            if (hit.collider.tag == "Player")
            {
                cameraFollowPlayer = true;
            }
        }
    }

    public void OnMoveCamera(InputValue value)
    {
        Vector3 v = value.Get<Vector3>();
        cameraFollowPlayer = false;
        cameraInputMovement = v;
    }

    public void DestroyTargetPositionHighlight()
    {
        if (highlightPositionInstance != null) Destroy(highlightPositionInstance);
    }

}