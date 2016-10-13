using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteInEditMode]
public class CameraPostProcess : MonoBehaviour
{
	public float intensity;
	public Camera rendering_camera;
	public Material material;
	public RenderTexture framebuffer;
	public RenderTexture depthbuffer;
	public RenderTexture finaltarget;
	// Use this for initialization
	void Start()
	{
		buildRenderTargets();
	}
	void OnRenderImage( RenderTexture source , RenderTexture destination )
	{

		//material.SetFloat( "_bwBlend" , intensity );

		material.SetTexture( "depth_buffer" , depthbuffer );
		finaltarget.DiscardContents();
		Graphics.Blit( framebuffer , finaltarget , material );
		//source.
		//destination.();
		Graphics.Blit( finaltarget , destination );
	}
	void buildRenderTargets()
	{
		var camera = GetComponent<Camera>();
		int k = 1;
		material.SetFloat( "frame_width" , camera.pixelWidth / k );
		material.SetFloat( "frame_height" , camera.pixelHeight / k );
		rendering_camera.pixelRect = new Rect( camera.pixelRect );
		rendering_camera.GetComponent<RenderingCamera>().resetTargets();
		framebuffer.Release();
		framebuffer.height = camera.pixelHeight / k;
		framebuffer.width = camera.pixelWidth / k;
		framebuffer.Create();
		depthbuffer.Release();
		depthbuffer.height = camera.pixelHeight / k;
		depthbuffer.width = camera.pixelWidth / k;
		depthbuffer.Create();
		finaltarget.Release();
		finaltarget.height = camera.pixelHeight / k;
		finaltarget.width = camera.pixelWidth / k;
		finaltarget.Create();
		rendering_camera.GetComponent<RenderingCamera>().setTargets( framebuffer , depthbuffer );
	}
	Rect old_rect;
	void Update()
	{
		
		var camera = GetComponent<Camera>();
		if( old_rect.width != camera.pixelRect.width || old_rect.height != camera.pixelRect.height )
		{

			buildRenderTargets();
			old_rect = new Rect( camera.pixelRect );
		}
	}
}
