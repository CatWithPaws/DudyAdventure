using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(Camera))]
public class CameraFollow : MonoBehaviour
{
    //need to restrict for camera's move
    [SerializeField] private bool RestrictMode = true;

    [Header("CameraFollow")]
    [SerializeField] float _dampTime = 0.15f;
    private Vector3 _velocity = Vector3.zero;
    [SerializeField] private Transform _target;
    [Header("Camera Move Area")]
    [SerializeField] Vector2 restrictBottomLeftSide;
    [SerializeField] Vector2 restrictTopRightSide;
    private Camera _camera;

    private void Start()
    {
        _camera = GetComponent<Camera>();
        if(_target == null)
		{
            _target = FindObjectOfType<Player>().transform;
		}
        transform.position = new Vector3(_target.position.x, transform.position.y, transform.position.z);
    }
    void LateUpdate()
    {
        if (_target)
        {
            //Calculate where camera have to move
            Vector3 cameraPoint = _camera.transform.position;
            Vector3 targetPoint = _target.position;

            //Make smooth transition from current camera position to target position
            Vector3 willCameraPosition = Vector3.LerpUnclamped(cameraPoint, new Vector3(targetPoint.x,targetPoint.y,cameraPoint.z), Time.deltaTime * 5);

            if (RestrictMode == true)
            {
                //Clamping area to restrict camera move on no playable places
                willCameraPosition.x = Mathf.Clamp(willCameraPosition.x, restrictBottomLeftSide.x, restrictTopRightSide.x);
                willCameraPosition.y = Mathf.Clamp(willCameraPosition.y, restrictBottomLeftSide.y, restrictTopRightSide.y);
            }
            //Set camera position
            _camera.transform.position = willCameraPosition;

            

        }

    }
    
    public void SetRestrictBotLeftSide()
	{
        restrictBottomLeftSide = transform.position;
	}
    public void SetRestrictTopRightSide()
    {
        restrictTopRightSide = transform.position;
    }
}
