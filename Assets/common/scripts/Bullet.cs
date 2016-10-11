using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
	public Vector3 dir;
	float lifetime = 3.0f;
	bool attached = false;
	void Start()
	{

	}
	void OnTriggerEnter( Collider col )
	{
		if( col.gameObject.GetComponent< Tower >() )
		{
			return;
		}
		
		var eu = col.GetComponent<EnemyUnit>();
		if( eu )
		{
			transform.SetParent( col.transform );
			eu.makeDamage( 50.0f );
		}
		attached = true;
	}
	// Update is called once per frame
	void Update()
	{
		lifetime -= Time.deltaTime;
		if( lifetime <= 0.0f )
		{
			Destroy( gameObject );
			return;
		}
		if( attached )
		{
			return;
		}
		
		transform.position += dir * SceneMeta.singleton.bullet_speed * Time.deltaTime;
	}
}
