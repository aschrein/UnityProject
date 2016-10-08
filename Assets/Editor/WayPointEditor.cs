using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[System.Serializable]
[CustomEditor(typeof(PathNode))]
public class WayPointEditor : Editor
{
	static PathNode last_selection;
	static bool pick_next_active;
	static int pick_slot;
	public override void OnInspectorGUI()
	{
		var path_node = ( PathNode )target;
		if( pick_next_active && path_node != last_selection )
		{
			last_selection.next[ pick_slot ] = path_node;
			path_node.income.Add( last_selection );
			pick_next_active = false;
		}
		int div_index = -1;
		int rem_index = -1;
		for( int i = 0; i < path_node.next.Count; i++ )
		{
			GUILayout.BeginHorizontal();
			path_node.next[ i ] = ( PathNode )EditorGUILayout.ObjectField( path_node.next[ i ] , typeof( PathNode ) );
			if( path_node.next[ i ] )
			{
				path_node.next[ i ].income.Add( path_node );
			}
			if( GUILayout.Button( "divide" ) )
			{
				div_index = i;
			}
			if( GUILayout.Button( "remove" ) )
			{
				rem_index = i;
			}
			if( GUILayout.Button( "pick" ) )
			{
				last_selection = path_node;
				pick_next_active = true;
				pick_slot = i;
			}
			GUILayout.EndHorizontal();
		}
		if( rem_index > -1 )
		{
			path_node.next.RemoveAt( rem_index );
			SceneView.RepaintAll();
		}
		if( div_index > -1 )
		{
			var new_node = new GameObject( "wp" );
			new_node.transform.SetParent( path_node.transform.parent );
			var node_c = new_node.AddComponent<PathNode>();
			node_c.next.Add( path_node.next[ div_index ] );
			new_node.transform.position = ( path_node.transform.position + path_node.next[ div_index ].transform.position ) * 0.5f;
			path_node.next[ div_index ] = node_c;
			SceneView.RepaintAll();
		}
		if( GUILayout.Button( "add field" ) )
		{
			path_node.next.Add( null );
		}
		if( GUILayout.Button( "remove" ) )
		{
			foreach( var outcome in path_node.next )
			{
				outcome.income.Remove( path_node );
			}
			foreach( var income in path_node.income )
			{
				income.next.Remove( path_node );

				foreach( var outcome in path_node.next )
				{
					income.next.Add( outcome );
				}
			}
			DestroyImmediate( path_node.gameObject );
			SceneView.RepaintAll();
		}
	}
}
