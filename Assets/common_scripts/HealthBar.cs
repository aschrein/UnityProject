using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
	// Use this for initialization
	GameObject health_cube;
	void Start()
	{
		health_cube = new GameObject();
		var mf = health_cube.AddComponent<MeshFilter>();
		Mesh plane = new Mesh();
		
		plane.SetVertices( new List<Vector3>(
			new Vector3[]{
				new Vector3( -100.0f , -100.0f ) ,
				new Vector3( 100.0f , -100.0f ) ,
				new Vector3( 100.0f , 100.0f ) ,
				new Vector3( -100.0f , 100.0f ) 
			}
		) );
		plane.SetIndices( new int[]
		{
			0 , 1 , 2 , 0 , 2 , 3
		} , MeshTopology.Triangles , 0 );
		plane.RecalculateBounds();
		mf.mesh = plane;
		var mr = health_cube.AddComponent<MeshRenderer>();
		foreach( var mat in Material.FindObjectsOfType<Material>() )
		{
			if( mat.name == "HealthBarMaterial" )
			{
				mr.material = mat;
				break;
			}
		}
		health_cube.transform.SetParent( gameObject.transform );
	}

	void OnGUI()
	{
		var view_coord = Camera.current.WorldToScreenPoint( transform.position );
		
		GUI.color = new Color( 1.0f , 0.0f , 0.0f );
		GUI.Box( new Rect( view_coord.x , Camera.current.pixelHeight - view_coord.y , 200 , 100 ) , "" );
		GUI.color = new Color( 1.0f , 1.0f , 1.0f );
		/*if( GUI.Button( new Rect( 100 , 100 , 200 , 100 ) , new GUIContent( "test button" ) ) )
		{
			System.Console.Out.Write( "button pressed" );
		}*/
	}
	// Update is called once per frame
	void Update()
	{

	}
}
