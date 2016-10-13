using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEditor;
public class PathNode : MonoBehaviour
{
	public List<PathNode> next = new List<PathNode>();
	void Start()
	{

	}
	void Update()
	{


	}
	void OnDrawGizmos()
	{
		Vector3 position = transform.position;
		Gizmos.color = new Color( 1.0f , 0.0f , 0.0f );
		Gizmos.DrawSphere( position , 0.5f );
		foreach( var target in next )
		{
			if( target )
			{
				Gizmos.DrawLine( position , target.transform.position );
			}
		}
	}
	/*[DrawGizmo( GizmoType.Selected | GizmoType.Active )]
	static void DrawGizmoForMyScript( PathNode node , GizmoType gizmoType )
	{
		Vector3 position = node.transform.position;
		Gizmos.color = new Color( 1.0f , 0.0f , 0.0f );
		foreach( var target in node.next )
		{
			
			Gizmos.DrawLine( position , target.transform.position );
		}
	}*/
}
