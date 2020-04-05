using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatalogueContents : MonoBehaviour
{
    public List<GameObject> ObjectsInHouse { get; set; }

    public LayerMask ObjectsInHouseMask;

    private void Start()
    {
        ObjectsInHouse = new List<GameObject>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & ObjectsInHouseMask) != 0)
            ObjectsInHouse.Add(other.gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        if (((1 << other.gameObject.layer) & ObjectsInHouseMask) != 0)
            ObjectsInHouse.Remove(other.gameObject);
    }

    public void RemoveSuckedObjectInside(GameObject obj)
    {
        ObjectsInHouse.Remove(obj);
    }
}
