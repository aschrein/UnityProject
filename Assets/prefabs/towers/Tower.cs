using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Tower : MonoBehaviour
{
	float timer = 0.0f;
	Vector3 unit_origin_pos;
	bool pos_measured = false;
	bool shooting = false;
	EnemyUnit target_unit = null;
	GameObject pivot, body , fire_point;
	HashSet<EnemyUnit > units_in_area = new HashSet<EnemyUnit>();
	void Start()
	{
		pivot = transform.Find( "pivot" ).gameObject;
		body = pivot.transform.Find( "body_node" ).gameObject;
		fire_point = body.transform.Find( "fire_point" ).gameObject;
	}
	void OnTriggerEnter( Collider collider )
	{
		var eu = collider.gameObject.GetComponent<EnemyUnit>();
		if( eu != null )
		{
			units_in_area.Add( eu );
		}
	}
	void OnTriggerExit( Collider collider )
	{
		var eu = collider.gameObject.GetComponent<EnemyUnit>();
		if( eu != null )
		{
			units_in_area.Remove( eu );
		}
	}
	void reload()
	{

	}
	void chooseNextUnit()
	{
		
		float closest_dist2 = 9000.0f;
		EnemyUnit closest_unit = null;
		foreach( var unit in units_in_area )
		{
			if( unit == null )
			{
				units_in_area.Remove( unit );
				continue;
			}
			var dist2 = ( transform.position - unit.transform.position ).sqrMagnitude;
			if( dist2 < closest_dist2 )
			{
				closest_dist2 = dist2;
				closest_unit = unit;
			}
		}
		if( closest_unit != null )
		{
			target_unit = closest_unit;
		}
	}
	void Update()
	{

		if( target_unit == null || !units_in_area.Contains( target_unit ) )
		{
			chooseNextUnit();
		} else
		{
			timer -= Time.deltaTime;
			if( timer <= 0.0f )
			{
				if( !pos_measured )
				{
					unit_origin_pos = target_unit.transform.position + Vector3.up * 10.0f;
					pos_measured = true;
					return;
				}
				var unit_end_pos = target_unit.transform.position + Vector3.up * 10.0f;
				var vel = ( unit_end_pos - unit_origin_pos ) / Time.deltaTime;
				var unit_speed = vel.magnitude;
				var dr = fire_point.transform.position - unit_end_pos;
				var bullet_speed = SceneMeta.singleton.bullet_speed;
				var a = bullet_speed * bullet_speed - unit_speed * unit_speed;
				var b = 2 * Vector3.Dot( vel , dr );
				var c = -dr.sqrMagnitude;
				var d = Mathf.Sqrt( b * b - 4 * a * c );
				var t0 = ( -b + d ) / 2 / a;
				var e_point = unit_end_pos + vel * t0;
				var dir = ( e_point - fire_point.transform.position ).normalized;

				pivot.transform.rotation = Quaternion.LookRotation( new Vector3( 0.0f , 1.0f , 0.0f ) ,
						new Vector3(
							-dir.x , 0.0f , -dir.z
							)
					);
				GetComponent<Animator>().Play( 0 );
				float angle = 90.0f - Vector3.Angle( new Vector3( dir.x , 0.0f , dir.z ) , dir ) * Mathf.Sign( dir.y );
				body.transform.localRotation = Quaternion.AngleAxis( angle , new Vector3( 1.0f , 0.0f , 0.0f ) );
				timer = SceneMeta.singleton.tower_cooldown;
				pos_measured = false;

				var bullet = Instantiate( SceneMeta.singleton.bullet_prefab ,
					fire_point.transform.position , Quaternion.LookRotation( dir ) , transform );
				bullet.GetComponent<Bullet>().target = target_unit.gameObject;
				bullet.GetComponent<Bullet>().dir = dir;

				
			}
		}

		/*timer -= Time.deltaTime;
		if( !shooting && timer <= 0.0f )
		{
			target_unit = null;
			float closest_dist = 9000.0f;
			timer = SceneMeta.singleton.tower_cooldown;
			for( int i = 0; i < SceneMeta.singleton.spawn_point.transform.childCount; i++ )
			{
				var child = SceneMeta.singleton.spawn_point.transform.GetChild( i );
				float dist = ( child.transform.position - transform.position ).sqrMagnitude;
				if( dist < closest_dist )
				{
					closest_dist = dist;
					target_unit = child.gameObject;
				}
			}
			if( target_unit )
			{
				var na = target_unit.GetComponent<NavMeshAgent>();
				unit_origin_pos = target_unit.transform.position + Vector3.up * 10.0f;
				shooting = true;
				var dr = unit_origin_pos - transform.position;
			}
		} else if( shooting )
		{
			if( target_unit == null )
			{
				target_unit = null;
				shooting = false;
			}
			unit_end_pos = target_unit.transform.position + Vector3.up * 10.0f;
			var vel = ( unit_end_pos - unit_origin_pos ) / Time.deltaTime;
			var unit_speed = vel.magnitude;
			var dr = fire_point.transform.position - unit_end_pos;
			var bullet_speed = SceneMeta.singleton.bullet_speed;
			var a = bullet_speed * bullet_speed - unit_speed * unit_speed;
			var b = 2 * Vector3.Dot( vel , dr );
			var c = -dr.sqrMagnitude;
			var d = Mathf.Sqrt( b * b - 4 * a * c );
			var t0 = ( -b + d ) / 2 / a;
			var e_point = unit_end_pos + vel * t0;
			var dir = ( e_point - fire_point.transform.position ).normalized;
			var bullet = Instantiate( SceneMeta.singleton.bullet_prefab ,
				fire_point.transform.position , Quaternion.LookRotation( dir ) , transform );
			bullet.GetComponent<Bullet>().target = target_unit;
			bullet.GetComponent<Bullet>().dir = dir;

			pivot.transform.rotation = Quaternion.LookRotation( new Vector3( 0.0f , 1.0f , 0.0f ) ,
					new Vector3(
						-dir.x , 0.0f , -dir.z
						)
				);
			//Vector3 dir = pivot.transform.TransformDirection( new Vector3( 0.0f , 0.0f , 1.0f ) );
			GetComponent<Animator>().Play( 0 );
			float angle = 90.0f - Vector3.Angle( new Vector3( dir.x , 0.0f , dir.z ) , dir ) * Mathf.Sign( dir.y );
			body.transform.localRotation = Quaternion.AngleAxis( angle , new Vector3( 1.0f , 0.0f , 0.0f ) ) ;

			target_unit = null;
			shooting = false;
		}*/
		}
	void OnDrawGizmos()
	{
		Vector3 position = transform.position;
		Gizmos.color = new Color( 1.0f , 0.0f , 0.0f );
		if( target_unit )
		{
			Gizmos.DrawLine( position , target_unit.transform.position );
		}
		Gizmos.color = new Color( 1.0f , 1.0f , 0.0f );
		Gizmos.DrawWireSphere( position , GetComponent<SphereCollider>().radius );
	}
}
