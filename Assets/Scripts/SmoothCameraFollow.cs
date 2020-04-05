using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Camera))]
public class SmoothCameraFollow : MonoBehaviour
{

    [SerializeField] float _dampTime = 0.15f;
    private Vector3 _velocity = Vector3.zero;
    [SerializeField] private Transform _target;
    private Camera _camera;

    private void Start()
    {
        _camera = GetComponent<Camera>();
    }
    void LateUpdate()
    {
        if (_target)
        {
            Vector3 cameraPoint = _camera.transform.position;
            Vector3 targetPoint = _target.position;

            Vector3 willCameraPosition = Vector3.LerpUnclamped(cameraPoint, new Vector3(targetPoint.x,targetPoint.y,cameraPoint.z), Time.deltaTime * 5);
            _camera.transform.position = willCameraPosition;

            

        }

    }
}
