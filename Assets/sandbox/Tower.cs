using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Tower : MonoBehaviour
{

	public GameObject spawn_point;
	public GameObject bullet_prefab;
	public float spawn_cooldown = 1.0f;
	float timer = 0.0f;
	Vector3 start_pos, end_pos;
	bool shooting = false;
	GameObject closest_child = null;
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
		timer -= Time.deltaTime;
		if( !shooting && timer <= 0.0f )
		{
			closest_child = null;
			float closest_dist = 1.0e7f;
			timer = spawn_cooldown;
			for( int i = 0; i < spawn_point.transform.childCount; i++ )
			{
				var child = spawn_point.transform.GetChild( i );
				float dist = ( child.transform.position - transform.position ).sqrMagnitude;
				if( dist < closest_dist )
				{
					closest_dist = dist;
					closest_child = child.gameObject;
				}
			}
			if( closest_child )
			{
				var na = closest_child.GetComponent<NavMeshAgent>();
				start_pos = closest_child.transform.position;
				shooting = true;
			}
		} else if( shooting )
		{
			end_pos = closest_child.transform.position;
			var vel = ( end_pos - start_pos ) / Time.deltaTime;
			var unit_speed = vel.magnitude;
			var dr = transform.position - end_pos;
			var bullet_speed = 20.0f;
			var a = bullet_speed * bullet_speed - unit_speed * unit_speed;
			var b = 2 * Vector3.Dot( vel , dr );
			var c = -dr.sqrMagnitude;
			var d = Mathf.Sqrt( b * b - 4 * a * c );
			var t0 = ( -b + d ) / 2 / a;
			var e_point = end_pos + vel * t0;
			var dir = ( e_point - transform.position ).normalized;
			var bullet = Instantiate( bullet_prefab , new Vector3(
					transform.position.x , 10.0f , transform.position.z
					) , Quaternion.identity , transform );
			bullet.GetComponent<Bullet>().target = closest_child;
			bullet.GetComponent<Bullet>().dir = dir;
			closest_child = null;
			shooting = false;
		}
	}
}
