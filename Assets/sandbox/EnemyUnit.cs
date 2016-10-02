using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyUnit : MonoBehaviour
{
	public GameObject target;
	void Start()
	{
		NavMeshAgent agent = GetComponent<NavMeshAgent>();
		agent.destination = target.transform.position;
	}
	void Update()
	{
		/*var dr = target.transform.position - transform.position;
		if( dr.magnitude < 1.0f )
		{
			Destroy( gameObject );
			return;
		}
		dr = dr.normalized;
		transform.position += dr * 20.0f * Time.deltaTime;*/
	}
}
