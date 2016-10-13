using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealBox : MonoBehaviour
{
	
	void Start()
	{

	}
	void OnTriggerEnter( Collider col )
	{
		var eu = col.GetComponent<EnemyUnit>();
		if( eu )
		{
			Instantiate( SceneMeta.singleton.healeffect_prefab , col.transform.position + Vector3.up * 0.3f , Quaternion.identity , col.transform );
			//Destroy( gameObject );
		}
	}
	void Update()
	{

	}
}
