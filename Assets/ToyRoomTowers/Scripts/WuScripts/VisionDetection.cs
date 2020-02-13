using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisionDetection : MonoBehaviour
{
    public TowerBehaviour owner;


    public void OnEnable()
    {
        owner = GetComponentInParent<TowerBehaviour>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (!owner.targets.Contains(other.gameObject))
        {
            owner.targets.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        owner.targets.Remove(other.gameObject);
    }
}
