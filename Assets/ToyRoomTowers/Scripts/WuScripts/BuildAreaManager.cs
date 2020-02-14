using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildAreaManager : MonoBehaviour
{
    public List<GameObject> buildableTiles = new List<GameObject>();

    public Material highlightMaterial;

    public string BuildableTileMask;

    // Start is called before the first frame update
    void Start()
    {
        DisableSprites();
        StartCoroutine(_detectRay());
    }

    void DisableSprites()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            buildableTiles.Add(transform.GetChild(i).gameObject);
            transform.GetChild(i).gameObject.GetComponent<TileDisabler>().hoverstate = TileDisabler.Hoverstate.inactive;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public IEnumerator _detectRay()
    {
        while (true)
        {
            detectRay();
            yield return null;
        }
    }

    void detectRay()
    {
        Ray ray;
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        LayerMask layerMask = (1 << LayerMask.NameToLayer(BuildableTileMask) | ~1 << LayerMask.NameToLayer("Towers"));

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
        {
            //if (buildableTiles.Contains(hit.collider.gameObject))
            //print(hit.collider.name);
            if (hit.collider.GetComponent<TileDisabler>())
            {
                hit.collider.gameObject.GetComponent<TileDisabler>().hoverstate = TileDisabler.Hoverstate.active;
                hit.collider.gameObject.GetComponent<MeshRenderer>().material = highlightMaterial;
            }
        }
        else
        {
            disableOverlay();
            //hit.collider.gameObject.GetComponent<TileDisabler>().hoverstate = TileDisabler.Hoverstate.inactive;
        }
    }

    void disableOverlay()
    {
        for (int i = 0; i < buildableTiles.Count; i++)
        {
            buildableTiles[i].GetComponent<TileDisabler>().hoverstate = TileDisabler.Hoverstate.inactive;
        }
    }
}
