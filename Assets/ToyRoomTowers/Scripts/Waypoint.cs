using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Waypoint : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        GameHub.GameManager.CurrentHealth--;

        other.gameObject.SetActive(false);
    }
}
