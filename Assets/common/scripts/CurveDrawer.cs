using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurveDrawer : MonoBehaviour
{
	Mesh mesh;
	MeshFilter mf;
	// Use this for initialization
	void Start()
	{
		mf = gameObject.AddComponent<MeshFilter>();
		mesh = new Mesh();
		mf.mesh = mesh;
		var mr = gameObject.AddComponent<MeshRenderer>();
	}

	// Update is called once per frame
	void FixedUpdate()
	{
		var vertices = new List<Vector3>();
		var indices = new List<int>();
		float velocity = 5.0e-1f;
		float angle_agility = 1.0e-2f;
		int cur_target_index = 0;
		Vector3 cur_target = transform.GetChild( 1 ).position;
		Vector3 cur_pos = transform.GetChild( 0 ).position;
		vertices.Add( cur_pos );
		indices.Add( 0 );
		int index_counter = 1;
		Vector3 cur_dir = ( transform.GetChild( 1 ).position - transform.GetChild( 0 ).position ).normalized;
		while( true )
		{
			var dr = cur_target - cur_pos;
			if( dr.sqrMagnitude < 1.0f )
			{
				try
				{
					cur_target = transform.GetChild( ++cur_target_index ).transform.position;
				} catch( Exception e )
				{
					break;
				}
			}
			dr = dr.normalized;
			cur_dir = Vector3.Slerp( cur_dir , dr , angle_agility ).normalized;
			cur_pos += cur_dir * velocity;
			vertices.Add( cur_pos );
			indices.Add( index_counter++ );
		}
		mesh.SetVertices( vertices );
		mesh.SetIndices( indices.ToArray() , MeshTopology.LineStrip , 0 );
	}
}
