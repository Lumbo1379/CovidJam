using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreSuckables : MonoBehaviour
{
    [Range(0, 5)] public float MinBoundSizeToStore;
    public VacuumSuck Vacuum;
    public LayerMask CollectMask;

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (Input.GetMouseButton(0))
    //    {
    //        if (((1 << other.gameObject.layer) & CollectMask) != 0)
    //        {
    //            var getSucked = other.gameObject.GetComponent<GetSucked>();

    //            float boundSize = getSucked.ChildRenderer == null
    //                ? other.gameObject.GetComponent<Renderer>().bounds.size.y
    //                : getSucked.ChildRenderer.GetComponent<Renderer>().bounds.size.y;

    //            Debug.Log(boundSize);

    //            if (boundSize <= MinBoundSizeToStore)
    //                Vacuum.StoreObject(other.gameObject);
    //        }
    //    }
    //}

    private void OnTriggerStay(Collider other)
    {
        if (Input.GetMouseButton(0))
        {
            if (((1 << other.gameObject.layer) & CollectMask) != 0)
            {
                var getSucked = other.gameObject.GetComponent<GetSucked>();

                float boundSize = getSucked.ChildRenderer == null
                    ? other.gameObject.GetComponent<Renderer>().bounds.size.y
                    : getSucked.ChildRenderer.GetComponent<Renderer>().bounds.size.y;

                Debug.Log(boundSize);

                if (boundSize <= MinBoundSizeToStore)
                    Vacuum.StoreObject(other.gameObject);
            }
        }
    }
}
