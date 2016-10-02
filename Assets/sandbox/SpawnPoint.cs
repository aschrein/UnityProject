using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
	public GameObject enemy_unit_prefab;
	public GameObject target;
	public float spawn_cooldown;
	float timer = 0.0f;
	// Use this for initialization
	void Start()
	{

	}
	void spawn()
	{
		var new_unit = Instantiate( enemy_unit_prefab ,
			new Vector3( transform.position.x , 0.0f , transform.position.z ) ,
			Quaternion.identity , transform );
		new_unit.GetComponent<EnemyUnit>().target = target;
	}
	// Update is called once per frame
	void Update()
	{
		timer -= Time.deltaTime;
		if( timer <= 0.0f )
		{
			timer = spawn_cooldown;
			spawn();
		}
	}
}
