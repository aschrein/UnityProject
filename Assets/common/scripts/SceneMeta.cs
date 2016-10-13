using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SceneMeta : MonoBehaviour
{
	public SpawnPoint spawn_point;
	public PathNode path_origin;
	public GameObject bullet_prefab;
	public float bullet_speed;
	public float tower_cooldown;
	static public SceneMeta singleton;
	public GameObject hiteffect_prefab;
	public GameObject healeffect_prefab;
	public float waypoint_radius = 1.0f;
	void Start()
	{
		singleton = this;
	}
}
