using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[System.Serializable]
[CustomEditor(typeof(PathNode))]
public class WayPointEditor : Editor
{
	static PathNode last_selection;
	static bool pick_next_active = false , create_next_active = false;
	static int pick_slot;
	void OnEnable()
	{
		SceneView.onSceneGUIDelegate += SceneGUI;
	}
	void SceneGUI( SceneView sceneView )
	{
		if( create_next_active && Event.current.type == EventType.MouseDown && Event.current.button == 0 )
		{
			RaycastHit hit;
			Vector2 mp = Event.current.mousePosition;
			mp.y = SceneView.currentDrawingSceneView.camera.pixelHeight - mp.y;
			Ray ray = SceneView.currentDrawingSceneView.camera.ScreenPointToRay( mp );
			if( Physics.Raycast( ray , out hit ) )
			{
				Vector3 pos = hit.point;
				var new_node = new GameObject( "wp" );
				new_node.transform.SetParent( last_selection.transform.parent );
				var node_c = new_node.AddComponent<PathNode>();
				new_node.transform.position = pos;
				connect( last_selection , node_c );
				SceneView.RepaintAll();
			}
			create_next_active = false;
		}
	}
	static void connect( PathNode from , PathNode to , int slot = -1 )
	{
		if( slot > -1 )
		{
			from.next[ slot ] = to;
		} else
		{
			from.next.Add( to );
		}
	}
	static void disconnect( PathNode from , PathNode to )
	{
		from.next.Remove( to );
	}
	public override void OnInspectorGUI()
	{
		var path_node = ( PathNode )target;
		if( pick_next_active && path_node != last_selection )
		{
			connect( last_selection , path_node );
			SceneView.RepaintAll();
			pick_next_active = false;
		}
		int div_index = -1;
		int rem_index = -1;
		GUILayout.Label( "out:" );
		for( int i = 0; i < path_node.next.Count; i++ )
		{
			GUILayout.BeginHorizontal();
			path_node.next[ i ] = ( PathNode )EditorGUILayout.ObjectField( path_node.next[ i ] , typeof( PathNode ) );
			if( GUILayout.Button( "divide" ) )
			{
				div_index = i;
			}
			if( GUILayout.Button( "remove" ) )
			{
				rem_index = i;
			}
			GUILayout.EndHorizontal();
		}
		if( rem_index > -1 )
		{
			if( path_node.next[ rem_index ] )
			{
				disconnect( path_node , path_node.next[ rem_index ] );
			}
			path_node.next.RemoveAt( rem_index );
			SceneView.RepaintAll();
		}
		if( div_index > -1 )
		{
			var new_node = new GameObject( "wp" );
			new_node.transform.SetParent( path_node.transform.parent );
			var node_c = new_node.AddComponent<PathNode>();
			
			//node_c.next.Add( path_node.next[ div_index ] );
			new_node.transform.position = ( path_node.transform.position + path_node.next[ div_index ].transform.position ) * 0.5f;
			//path_node.next[ div_index ] = node_c;
			connect( node_c , path_node.next[ div_index ] );
			disconnect( path_node , path_node.next[ div_index ] );
			connect( path_node , node_c );
			SceneView.RepaintAll();
		}
		if( GUILayout.Button( "pick" ) )
		{
			last_selection = path_node;
			pick_next_active = true;
		}
		if( GUILayout.Button( "new" ) )
		{
			create_next_active = true;
			last_selection = path_node;
		}
		if( GUILayout.Button( "add slot" ) )
		{
			path_node.next.Add( null );
		}
		if( GUILayout.Button( "remove this node" ) )
		{
			var outcome_copy = new HashSet<PathNode>( path_node.next );
			foreach( var outcome in outcome_copy )
			{
				disconnect( path_node , outcome );
			}
			DestroyImmediate( path_node.gameObject );
			SceneView.RepaintAll();
		}
	}

}
