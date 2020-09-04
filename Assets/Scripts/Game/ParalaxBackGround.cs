using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ParalaxBackGround : MonoBehaviour
{
    [SerializeField] private GameObject[] BGFragments;
	[SerializeField] private float parallaxSpeed = 1;
	private float cameraLastPosX;
	private Transform CameraTransform;
	private Transform PlayerTransform;
	private void Start()
	{
		CameraTransform = Camera.main.gameObject.transform;
		PlayerTransform = Player.Instance.gameObject.transform;
	}
	private void Update()
	{
		//transform.position = new Vector3(CameraTransform.position.x, transform.position.y, transform.position.z);
		if (Player.Instance.CanBGMove() || cameraLastPosX == CameraTransform.position.x) return;
		foreach(GameObject bgFragment in BGFragments)
		{
			bgFragment.transform.Translate(new Vector3(-Player.Instance.directionByX * Time.fixedDeltaTime *parallaxSpeed , 0, 0));
		}
		cameraLastPosX = CameraTransform.position.x;
	}
}
