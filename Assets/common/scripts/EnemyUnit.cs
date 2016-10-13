using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyUnit : MonoBehaviour
{
	public PathNode target;
	public float health = 550.0f;
	void Start()
	{
		NavMeshAgent agent = GetComponent<NavMeshAgent>();
		agent.destination = target.transform.position;
		//MiniMap.singleton.addUnit( gameObject );
	}
	void OnDestroy()
	{
		//MiniMap.singleton.removeUnit( gameObject );
	}
	public void makeDamage( float damage )
	{
		health -= damage;
		if( health <= 0.0f )
		{
			Destroy( gameObject );
		}
	}
	void Update()
	{
		if( target == null )
		{
			return;
		}
		var dr = target.transform.position - transform.position;
		if( dr.magnitude < SceneMeta.singleton.waypoint_radius )
		{
			if( target.next == null || target.next.Capacity == 0 )
			{
				Destroy( gameObject );
				return;
			}
			int next_count = target.next.Capacity;
			int next_index = Mathf.FloorToInt( next_count * Random.Range( 0.0f , 1.0f - 1.0e-7f ) );
			target = target.next.ToArray()[ next_index ];
			NavMeshAgent agent = GetComponent<NavMeshAgent>();
			agent.destination = target.transform.position;
		}
	}
}
