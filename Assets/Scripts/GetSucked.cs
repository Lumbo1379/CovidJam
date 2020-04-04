using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetSucked : MonoBehaviour
{
    public Transform Target { get; set; }
    public float RangeToScaleDown { get; set; }

    [Range(0, 10)] public float ShrinkSpeed;
    [Range(0, 10)] public float BalloonSpeed;
    [Range(0, 5)] public float ElectricityAmount;

    private Rigidbody _rb;
    private Vector3 _originalScale;
    private float _t;
    private Material _electricityMaterialInstance;

    private void Awake()
    {
        _originalScale = transform.localScale;
        _electricityMaterialInstance = GetComponent<Renderer>().materials[1];
        _electricityMaterialInstance.SetFloat("Vector1_4C910460", 0);
    }

    private void OnEnable()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.useGravity = false;
        _rb.velocity = Vector3.zero;
        _t = 0;
        _electricityMaterialInstance.SetFloat("Vector1_4C910460", ElectricityAmount);
    }

    private void Update()
    {
        _t += Time.deltaTime;

        if (_t < 1)
        {
            if (!_rb.useGravity)
            {
                var distance = Vector3.Distance(transform.position, Target.position);
                float scaleMultiplier = distance / RangeToScaleDown;
                scaleMultiplier = Mathf.Clamp(scaleMultiplier, 0, 1);
                scaleMultiplier = Mathf.Lerp(_originalScale.x, scaleMultiplier, _t * ShrinkSpeed);
                transform.localScale = _originalScale * scaleMultiplier;
            }
            else
            {
                float scaleMultiplier = Mathf.Lerp(transform.localScale.x, _originalScale.x, _t * BalloonSpeed);
                transform.localScale = _originalScale * scaleMultiplier;

                if (Math.Abs(transform.localScale.x - _originalScale.x) < 0.01)
                    enabled = false;
            }
        }
    }

    public void Release()
    {
        _rb.useGravity = true;
        _t = 0;
        _electricityMaterialInstance.SetFloat("Vector1_4C910460", 0);
    }
}
