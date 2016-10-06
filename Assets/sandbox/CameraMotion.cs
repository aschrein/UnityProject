using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMotion : MonoBehaviour
{
	void Start()
	{

	}
	public GameObject tower_prefab;
	public GameObject spawn_point;
	public GameObject bullet_prefab;
	Vector3 last_mouse_pos;
	bool drag = false;
	bool was_touchinf = false;
	void placeTower(Vector3 world_pos )
	{
		
		var tower = Instantiate( tower_prefab , world_pos , Quaternion.identity );
		tower.GetComponent<Tower>().spawn_cooldown = 1.0f;
		tower.GetComponent<Tower>().spawn_point = spawn_point;
		tower.GetComponent<Tower>().bullet_prefab = bullet_prefab;
	}
	void Update()
	{
		if( Input.touchCount != 0 )
		{
			Touch tch = Input.GetTouch( 0 );
			transform.position += new Vector3(
				tch.deltaPosition.x , 0.0f , tch.deltaPosition.y
				);
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
		}
		if( Input.GetMouseButtonDown( 0 ) )
		{
			drag = false;
			last_mouse_pos = Input.mousePosition;
		} else if( Input.GetMouseButton( 0 ) )
		{
			var dp = Input.mousePosition - last_mouse_pos;
			if( dp.magnitude > 0.0f )
			{
				drag = true;
			}
			transform.position += new Vector3(
				dp.x , 0.0f , dp.y
				);
			last_mouse_pos = Input.mousePosition;
		}
		if( !drag && Input.GetMouseButtonUp( 0 ) )
		{
			RaycastHit cast = new RaycastHit();
			Camera cam = GetComponent<Camera>();
			if( Physics.Raycast( cam.ScreenPointToRay( Input.mousePosition ) , out cast , 10000.0f ) )
			{
				var world_pos = cast.point;
				placeTower( world_pos );
			}
		}
	}
}
