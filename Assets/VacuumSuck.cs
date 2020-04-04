using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting;
using UnityEngine;

public class VacuumSuck : MonoBehaviour
{
    [Range(0, 100)] public float AttractPower;
    [Range(0, 100)] public float Range;
    [Range(0, 3)] public float AttractArea;
    public Transform SuckOrigin;
    public LayerMask SuckMask;

    private Camera _camera;
    private List<GameObject> _currentAttractedObjects;
    private List<GameObject> _attractedObjectsInFrame;
    private List<GameObject> _storedObjects;

    private void Start()
    {
        _camera = Camera.main;
        _currentAttractedObjects = new List<GameObject>();
        _attractedObjectsInFrame = new List<GameObject>();
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
        obj.SetActive(false);

        _storedObjects.Add(obj);
        _currentAttractedObjects.Remove(obj); 
    }

    private Collider[] ScanForColliders(Vector3 hit)
    {
        return Physics.OverlapSphere(hit, AttractArea);
    }
}
