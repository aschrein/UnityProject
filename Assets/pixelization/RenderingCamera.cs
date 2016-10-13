using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderingCamera : MonoBehaviour
{
	void Start()
	{
		
	}
	public void resetTargets()
	{
	}
	public void setTargets( RenderTexture framebuffer , RenderTexture depthbuffer )
	{
		GetComponent<Camera>().depthTextureMode = DepthTextureMode.Depth;
		GetComponent<Camera>().SetTargetBuffers( framebuffer.colorBuffer , depthbuffer.depthBuffer );
	}
	// Update is called once per frame
	void Update()
	{

	}
}
