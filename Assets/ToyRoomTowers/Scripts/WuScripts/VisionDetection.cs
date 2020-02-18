using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class VisionDetection : MonoBehaviour
{
    int layer_mask;
    public TowerBehaviour owner;

    public void OnEnable()
    {
        owner = GetComponentInParent<TowerBehaviour>();
    }

    // Start is called before the first frame update
    void Start()
    {
        layer_mask = (1 << LayerMask.NameToLayer("Enemies"));
    }

    // Update is called once per frame
    void Update()
    {
        DetectionRange();
    }

    void DetectionRange()
    {
        Collider[] colliders = Physics.OverlapSphere(owner.transform.position, 6f, layer_mask);

        var ownerList = owner.targets;

        foreach (Collider detected in colliders)
        {
            if (!ownerList.Contains(detected.gameObject))
            {
                ownerList.Add(detected.gameObject);

            }

        }

        Debug.Log("array is " + colliders.Length + " list is " + ownerList.Count);

        for (int i = 0; i <= colliders.Length; i++)
        {
            if (colliders.Length != ownerList.Count)
            {
                if (ownerList.Count == 1)
                {
                    owner.GetComponent<TowerBehaviour>().CurrentTarget = null;
                    ownerList.RemoveAt(i);
                }
                else
                {
                    ownerList.RemoveAt(i);
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        owner.targets.Remove(other.gameObject);
    }
}
