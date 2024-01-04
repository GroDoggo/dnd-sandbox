using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class targetPositionHighlight : MonoBehaviour
{
    private GameObject gameBehavior;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            gameBehavior = GameObject.Find("GameManager");
            gameBehavior.GetComponent<GameBehavior>().DestroyTargetPositionHighlight();
        }
    }
}
