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
	void Start()
	{
		singleton = this;
	}
}
