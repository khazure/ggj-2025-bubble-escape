using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffsetFromTarget : MonoBehaviour
{
    [SerializeField] private Transform target;

    [SerializeField] private Vector3 offsetFromTarget;
    [SerializeField] private Vector3 axis;
    
    // Update is called once per frame
    void Update()
    {
        transform.position = target.transform.position + offsetFromTarget;
        float angle = Quaternion.Angle(transform.rotation, target.rotation);
        transform.RotateAround(target.position, axis, angle);
        transform.forward = target.transform.position - transform.position;
    }

    private void LateUpdate()
    {
        transform.forward = (target.transform.position - transform.position) * Time.deltaTime;
    }
}
