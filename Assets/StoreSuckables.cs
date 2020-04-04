using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreSuckables : MonoBehaviour
{
    public VacuumSuck Vacuum;
    public LayerMask CollectMask;

    private void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & CollectMask) != 0)
        {
            Vacuum.StoreObject(other.gameObject);
            other.gameObject.SetActive(false);
        }
    }
}
