using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
	public GameObject enemy_unit_prefab;
	public PathNode origin_path_node;
	public float spawn_cooldown;
	void Start()
	{
		StartCoroutine( spawn() );
	}
	IEnumerator spawn()
	{
		while( true )
		{
			var new_unit = Instantiate( enemy_unit_prefab ,
				new Vector3( transform.position.x , 0.0f , transform.position.z ) ,
				Quaternion.identity , transform );
			new_unit.GetComponent<EnemyUnit>().target = origin_path_node;
			yield return new WaitForSeconds( spawn_cooldown );
		}
		yield return null;
	}
	void Update()
	{
	}
}
