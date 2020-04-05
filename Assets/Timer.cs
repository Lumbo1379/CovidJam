using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public int StartingTime;
    public BoxCollider ColliderTemplate;
    public LayerMask ObjectsInHouseMask;

    private int _time;
    private TextMeshProUGUI _text;

    private void Start()
    {
        _text = GetComponent<TextMeshProUGUI>();
        _time = StartingTime;

        _text.text = _time.ToString();
        InvokeRepeating("DecreaseTime", 1f, 1f);
    }

    private void DecreaseTime()
    {
        _time--;

        if (_time > 0)
            _text.text = _time.ToString();
    }

    private void CalculateScore()
    {
        var objectsInHouse = ScanObjectsInsideHouse();
        int x = 1;
    }

    private Collider[] ScanObjectsInsideHouse()
    {
         return Physics.OverlapBox(ColliderTemplate.center, ColliderTemplate.size / 2f, Quaternion.identity, ObjectsInHouseMask);
    }
}
