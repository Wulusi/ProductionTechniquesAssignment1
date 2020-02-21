
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        GameHub.GameManager.CurrentHealth--;

        other.gameObject.SetActive(false);
    }
}
