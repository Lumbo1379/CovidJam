using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting;
using UnityEngine;
using UnityEngine.Rendering;

public class VacuumSuck : MonoBehaviour
{
    [Range(0, 100)] public float AttractPower;
    [Range(0, 100)] public float Range;
    [Range(0, 3)] public float AttractArea;
    [Range(0, 5000)] public float ShootForce;
    [Range(0, 1)] public float ShootCooldown;
    [Range(0, 100)] public float MaxItemsStored;
    public Transform Meter;
    public Transform SuckOrigin;
    public LayerMask SuckMask;
    public CatalogueContents CatalogueContents;

    private Camera _camera;
    private List<GameObject> _currentAttractedObjects;
    private List<GameObject> _attractedObjectsInFrame;
    private List<GameObject> _storedObjects;
    private bool _shootBlocked;

    private void Start()
    {
        _camera = Camera.main;
        _currentAttractedObjects = new List<GameObject>();
        _attractedObjectsInFrame = new List<GameObject>();
        _storedObjects = new List<GameObject>();
        _shootBlocked = false;
    }

    private void FixedUpdate()
    {
        if (Input.GetMouseButton(0))
            DrawRay();
    }

    private void Update()
    {
        if (Input.GetMouseButtonUp(0))
            ReleaseObjects();

        if (Input.GetMouseButton(1))
        {
            if (!_shootBlocked)
                ShootStoredObject();
        }

        Meter.localScale = new Vector3(Meter.localScale.x, _storedObjects.Count / MaxItemsStored, Meter.localScale.z);
    }

    private void DrawRay()
    {
        RaycastHit hit;
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, Range))
        {
            foreach (var collision in ScanForColliders(hit.point))
            {
                if (((1 << collision.gameObject.layer) & SuckMask) != 0)
                {
                    _attractedObjectsInFrame.Add(collision.gameObject);

                    AttractObject(hit.point, collision.gameObject, hit.distance);

                    var getSucked = collision.gameObject.GetComponent<GetSucked>();
                    if (!getSucked.isActiveAndEnabled)
                    {
                        _currentAttractedObjects.Add(collision.gameObject);

                        getSucked.Target = SuckOrigin;
                        getSucked.RangeToScaleDown = Range;
                        getSucked.enabled = true;
                    }
                }
            }

            CheckIfObjectsLeftAttractRadius();
        }
    }

    private void AttractObject(Vector3 hitPosition, GameObject obj, float distanceAway)
    {
        var fallOff = (Range - distanceAway) / Range;
        var rb = obj.GetComponent<Rigidbody>();
        var direction = SuckOrigin.position - obj.transform.position;

        rb.AddForce(direction * AttractPower * fallOff * Time.deltaTime);
    }

    private void CheckIfObjectsLeftAttractRadius()
    {
        foreach (var obj in _currentAttractedObjects.ToList()) // Create copy as modifying when iterating on it
        {
            if (!_attractedObjectsInFrame.Contains(obj))
            {
                obj.GetComponent<GetSucked>().Release();
                _currentAttractedObjects.Remove(obj);
            }
        }

        _attractedObjectsInFrame.Clear();
    }

    private void ReleaseObjects()
    {
        foreach (var currentAttractedObject in _currentAttractedObjects)
            currentAttractedObject.GetComponent<GetSucked>().Release();

        _currentAttractedObjects.Clear();
    }

    public void StoreObject(GameObject obj)
    {
        if (_storedObjects.Count >= MaxItemsStored) return;

        obj.SetActive(false);

        _storedObjects.Add(obj);
        _currentAttractedObjects.Remove(obj);
        CatalogueContents.RemoveSuckedObjectInside(obj);
    }

    private void ShootStoredObject()
    {
        if (_storedObjects.Count > 0)
        {
            _shootBlocked = true;
            Invoke("UnblockShoot", ShootCooldown);

            var obj = _storedObjects.Dequeue();
            obj.SetActive(true);

            obj.transform.position = SuckOrigin.transform.position;
            obj.GetComponent<GetSucked>().Release();
            obj.GetComponent<Rigidbody>().AddForce(SuckOrigin.transform.forward * ShootForce);
        }
    }

    private void UnblockShoot()
    {
        _shootBlocked = false;
    }

    private Collider[] ScanForColliders(Vector3 hit)
    {
        return Physics.OverlapSphere(hit, AttractArea);
    }
}
