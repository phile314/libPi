using System;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Audio;
using OpenTK.Audio.OpenAL;
using OpenTK.Input;
using System.Collections.Concurrent;
using System.Drawing;
using System.Threading;
using System.IO;
using System.Collections.Generic;

using Pi.ThreeD.GL;
using Pi.Data;

namespace CliTest
{
	public class FboTestWindow : GameWindow
	{	
		private readonly Bitmap sourceImage;
		
		private GLGraphicsContext context;
		private GLProgram program;
		private GLGenericVertexBuffer<Vector3> positions;
		private GLGenericVertexBuffer<Vector2> textureCoords;
		private GLIndicesBuffer indices;
		private GLTexture texture0, texture1;
		private ImmutableList<Tuple<Object, String>> paramSpec;
		
		private Matrix4 modelViewMatrix, projectionMatrix;
		private GLFrameBuffer fb;
		private GLRenderBuffer rc, rd;
		bool isFirst = true;
		
		
		public FboTestWindow ()
			: base(720, 720, GraphicsMode.Default, "Virtual Pan&Zoom", GameWindowFlags.Default, DisplayDevice.Default, 4, 0, GraphicsContextFlags.Debug)
		{
			this.VSync = VSyncMode.On;
			
			this.sourceImage = new Bitmap(Path.Combine("..", "..", "in.jpg"));
			
			this.modelViewMatrix = Matrix4.CreateTranslation(0, 0, -4f);
			this.projectionMatrix = Matrix4.CreateOrthographic(2f, 2f, 0.1f, 10f);
		}
		
		
		
		
		protected override void OnLoad (EventArgs e)
		{
			base.OnLoad (e);
			Context.ErrorChecking = true;
			
			String version = GL.GetString(StringName.Version);
			Console.WriteLine("GL version: " + version);
			
			context = GLGraphics.NewContext(Context, true);
			
			program = context.NewProgramFromFiles("Render.vert", "Render.frag");
			
			texture0 = context.NewTexture(TextureMinFilter.Nearest, TextureMagFilter.Nearest, TextureWrapMode.Clamp, TextureWrapMode.Clamp);
			texture0.UploadImage(sourceImage);
			sourceImage.Dispose();
			
			texture1 = context.NewTexture(TextureMinFilter.Nearest, TextureMagFilter.Nearest, TextureWrapMode.Clamp, TextureWrapMode.Clamp);
			texture1.UploadImage(new byte[4 * 4 * 4], 4, 4, PixelInternalFormat.Four, PixelFormat.Rgba, PixelType.UnsignedByte);
			
			CreateAndFillBuffers();
			paramSpec = ImmutableList<Tuple<Object, String>>.List(new Tuple<Object, String>[] {
				Tuple.Create<object,String>(positions, "a_position"),
				Tuple.Create<object,String>(textureCoords, "a_texPos"),
				Tuple.Create<object,String>(texture0, "u_texture0"),
				Tuple.Create<object,String>(texture1, "u_texture1")});
			
			rc = context.NewRenderBuffer(1000, 1000, RenderbufferStorage.Rgb8);
			rd = context.NewRenderBuffer(1000, 1000, RenderbufferStorage.DepthComponent24);
			
			
			
			fb = context.NewFrameBuffer(rc, rd, FramebufferTarget.Framebuffer);
			
			
		}
		
		private void CreateAndFillBuffers() {
			Vector3[] aPositions = new Vector3[] {
				new Vector3(-1, -1, 0),
				new Vector3(1, -1, 0),
				new Vector3(-1, 1, 0),
				new Vector3(1, 1, 0)};
			Vector2[] aTexCoords = new Vector2[] {
				new Vector2(0, 1),
				new Vector2(1, 1),
				new Vector2(0, 0),
				new Vector2(1, 0)};
			positions = context.NewVertexBuffer<Vector3>(VertexAttribPointerType.Float, 3);
			positions.UploadVertices(aPositions);
			textureCoords = context.NewVertexBuffer<Vector2>(VertexAttribPointerType.Float, 2);
			textureCoords.UploadVertices(aTexCoords);
			indices = context.NewIndicesBuffer();
			indices.UploadVertices(new uint[] {0, 1, 2, 3});
		}
		
		
		protected override void OnUnload (EventArgs e)
		{
			base.OnUnload (e);
			
			context.Dispose();
		}
		
		protected override void OnResize (EventArgs e)
		{
			base.OnResize (e);
			context.SetViewport(ClientRectangle);
		}
		
		
		protected override void OnRenderFrame (FrameEventArgs e)
		{
			
			base.OnRenderFrame (e);
			
			if(isFirst) {
			
				fb.Activate();
				
				context.Clear();
				
				Matrix4 mvp = modelViewMatrix * projectionMatrix;
				//with DrawArray
				//context.RunProgram(program, paramSpec.Cons(Tuple.Create<object,String>(mvp, "u_mvpMatrix")), BeginMode.TriangleStrip);
				//with DrawElements
				context.RunProgram(program, paramSpec.Cons(Tuple.Create<object,String>(mvp, "u_mvpMatrix")), indices, BeginMode.TriangleStrip);
				
				byte[] img = new byte[1000 * 1000 * 4];
				context.ReadPixels(0, 0, 1000, 1000, PixelFormat.Bgra, PixelType.UnsignedByte,
					img);
				
				fb.Deactivate();
				
				Exit();
				
			}
			isFirst = false;
			
		}
		
	}
}