using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
	public GameObject target;
	public Vector3 dir;
	float lifetime = 10.0f;
	// Use this for initialization
	void Start()
	{

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

		var dr = target.transform.position - transform.position;
		if( dr.magnitude < 5.0f )
		{
			Destroy( gameObject );
			return;
		}
		//dr = dr.normalized;
		transform.position += dir * 20.0f * Time.deltaTime;
	}
}
