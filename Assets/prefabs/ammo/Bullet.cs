using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
	public GameObject target;
	public Vector3 dir;
	float lifetime = 10.0f;
	bool attached = false;
	// Use this for initialization
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
		if( attached )
		{
			return;
		}
		lifetime -= Time.deltaTime;
		if( lifetime <= 0.0f || target == null )
		{
			Destroy( gameObject );
			return;
		}

		var dr = target.transform.position + Vector3.up * 10.0f - transform.position;
		if( dr.magnitude < 2.0f )
		{
			//Destroy( gameObject );
			transform.SetParent( target.transform );
			target.GetComponent<EnemyUnit>().makeDamage( 50.0f );
			attached = true;
			return;
		}
		//dr = dr.normalized;
		transform.position += dir * SceneMeta.singleton.bullet_speed * Time.deltaTime;
	}
}
