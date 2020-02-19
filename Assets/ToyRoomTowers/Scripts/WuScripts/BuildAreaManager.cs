using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildAreaManager : MonoBehaviour
{
    public List<GameObject> buildableTiles = new List<GameObject>();
    public GameObject selectedTile;
    public Material highlightMaterial;
    public string BuildableTileMask;
    public bool noSelection;

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
            noSelection = true;
        }
    }

    public IEnumerator _detectRay()
    {
        while (noSelection)
        {
            detectRay();
            yield return null;
        }

        while (true)
        {
            ResetSelection();
            yield return null;
        }
    }

    public void SelectedTile(GameObject _selectedTile)
    {
        selectedTile = _selectedTile;
        if (selectedTile != null)
        {
            selectedTile.GetComponent<MeshRenderer>().material = highlightMaterial;
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

            print("ray hit " + hit.collider.name);
            if (hit.collider.GetComponent<TileDisabler>())
            {
                hit.collider.gameObject.GetComponent<TileDisabler>().hoverstate = TileDisabler.Hoverstate.active;
                hit.collider.gameObject.GetComponent<MeshRenderer>().material = highlightMaterial;
            }
        }
        else
        {
            disableOverlay();
        }
    }

    void disableOverlay()
    {
        for (int i = 0; i < buildableTiles.Count; i++)
        {
            buildableTiles[i].GetComponent<TileDisabler>().hoverstate = TileDisabler.Hoverstate.inactive;
        }
    }

    public void ResetSelection()
    {
        if (Input.GetMouseButtonDown(1))
        {
            DisableSprites();
            StopCoroutine(_detectRay());
            StartCoroutine(_detectRay());
            SelectedTile(null);
        }
    }
}
