using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMap : MonoBehaviour
{
	public static MiniMap singleton;
	List<GameObject> units = new List<GameObject>();
	Dictionary<GameObject , GameObject> units_images = new Dictionary<GameObject , GameObject>();
	public GameObject unit_image_prefab;
	public void addUnit( GameObject go )
	{
		
		units.Add( go );
		units_images.Add( go , Instantiate( unit_image_prefab , transform ) );
	}
	public void removeUnit( GameObject go )
	{
		units.Remove( go );
		GameObject image;
		units_images.TryGetValue( go , out image );
		Destroy( image );
		units_images.Remove( go );
	}
	void Start()
	{
		singleton = this;
		
	}
	void Update()
	{
		var world_mat = transform.localToWorldMatrix;
		foreach( var unit in units_images.Keys )
		{
			Vector3 pos = unit.transform.position;
			GameObject image;
			units_images.TryGetValue( unit , out image );
			image.transform.localScale.Set( 0.1f , 0.1f , 0.1f );

			image.transform.localPosition = pos / 32.0f;
		}
	}
}
