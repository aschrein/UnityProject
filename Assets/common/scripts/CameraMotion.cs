using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMotion : MonoBehaviour
{
	void Start()
	{

	}
	Vector3 last_mouse_pos;
	bool drag = false;
	bool was_touchinf = false;
	void placeTower(Vector3 world_pos )
	{
	}
	void Update()
	{
		/*if( Input.touchCount != 0 )
		{
			Touch tch = Input.GetTouch( 0 );
			transform.position += new Vector3(
				tch.deltaPosition.x , 0.0f , tch.deltaPosition.y
				) * Time.deltaTime * 100.0f;
			last_mouse_pos = tch.position;
			drag = false;
			if( tch.deltaPosition.magnitude > 0.0f )
			{
				drag = true;
			}
			was_touchinf = true;
		} else if( !drag && was_touchinf )
		{
			RaycastHit cast = new RaycastHit();
			Camera cam = GetComponent<Camera>();
			if( Physics.Raycast( cam.ScreenPointToRay( last_mouse_pos ) , out cast , 10000.0f ) )
			{
				var world_pos = cast.point;
				placeTower( world_pos );
			}
			was_touchinf = false;
		}*/
		Camera cam = GetComponent<Camera>();
		if( Input.GetMouseButtonDown( 0 ) )
		{
			drag = false;
			//var ray = cam.ScreenPointToRay( Input.mousePosition );
			last_mouse_pos = Input.mousePosition;// ray.origin + ray.direction * ray.origin.y / ray.direction.y;
		} else if( Input.GetMouseButton( 0 ) )
		{
			/*var dp = Input.mousePosition - last_mouse_pos;
			if( dp.magnitude > 0.0f )
			{
				drag = true;
			}
			transform.position += new Vector3(
				dp.x , 0.0f , dp.y
				) * Time.deltaTime * 100.0f;
			last_mouse_pos = Input.mousePosition;*/
			var ray = cam.ScreenPointToRay( last_mouse_pos );
			var col_pos0 = ray.origin + ray.direction * ray.origin.y / ray.direction.y;
			ray = cam.ScreenPointToRay( Input.mousePosition );
			var col_pos1 = ray.origin + ray.direction * ray.origin.y / ray.direction.y;
			var dr = col_pos1 - col_pos0;
			dr.y = 0;
			last_mouse_pos = Input.mousePosition;
			cam.transform.position += dr;
		}
		/*if( !drag && Input.GetMouseButtonUp( 0 ) )
		{
			RaycastHit cast = new RaycastHit();
			
			if( Physics.Raycast( cam.ScreenPointToRay( Input.mousePosition ) , out cast , 10000.0f ) )
			{
				var world_pos = cast.point;
				placeTower( world_pos );
			}
		}*/
	}
}
