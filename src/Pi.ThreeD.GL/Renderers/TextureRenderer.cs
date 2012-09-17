// 
// ImageRenderer.cs
//  
// Author:
//       Philipp Hausmann <philipp_code@314.ch>
// 
// Copyright (c) 2012 Philipp Hausmann
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
using System;
using System.Threading;
using Pi.ThreeD.GL;
using OpenTK;
using Pi.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace Pi.ThreeD.GL.Renderers
{
	/// <summary>
	/// Renders an image to a opengl quad.
	/// </summary>
	public class TextureRenderer
	{
		private const String MVP_VERTEX_SHADER = @"
#version 120

attribute vec4 a_position;
attribute vec2 a_texPos;

uniform mat4 u_mvpMatrix;

varying vec2 v_texPos;

void main() {
	v_texPos = a_texPos;
	gl_Position = u_mvpMatrix * a_position;

}";
		
		private const String TEXTURE_SHADER = @"
#version 120

uniform sampler2D u_texture0;

varying vec2 v_texPos;

void main() {
	gl_FragColor = texture2D(u_texture0, v_texPos);

}";
		
		private readonly GLGraphicsContext context;
		private GLGenericVertexBuffer<Vector3> vertices;
		private GLGenericVertexBuffer<Vector2> texCoords;
		private GLProgram prog;
		private ImmutableList<Tuple<String, Object>> paramSpec;
		private byte[] newImage;
		private readonly int width, height;
		private bool isDisposed;

		public TextureRenderer (GLGraphicsContext context, GLTexture texture)
		{
			this.context = context;
			
			texture = context.NewTexture(OpenTK.Graphics.OpenGL.TextureMinFilter.Nearest, OpenTK.Graphics.OpenGL.TextureMagFilter.Nearest,
				OpenTK.Graphics.OpenGL.TextureWrapMode.Clamp, OpenTK.Graphics.OpenGL.TextureWrapMode.Clamp);
			
			
			vertices = context.NewVertexBuffer<Vector3>(OpenTK.Graphics.OpenGL.VertexAttribPointerType.Float,
				3);
			vertices.UploadVertices(new Vector3[] {
				new Vector3(-1, -1, 0),
				new Vector3(1, -1, 0),
				new Vector3(-1, 1, 0),
				new Vector3(1, 1, 0)});
			
			texCoords = context.NewVertexBuffer<Vector2>(OpenTK.Graphics.OpenGL.VertexAttribPointerType.Float,
				2);
			texCoords.UploadVertices(new Vector2[] {
				new Vector2(0, 0),
				new Vector2(1, 0),
				new Vector2(0, 1),
				new Vector2(1, 1)});
			
			prog = context.NewProgram(MVP_VERTEX_SHADER, TEXTURE_SHADER);
			
			paramSpec = ImmutableList<Tuple<String, Object>>.List(
				Tuple.Create<String, Object>("a_position", vertices),
				Tuple.Create<String, Object>("a_texPos", texCoords),
				Tuple.Create<String, Object>("u_texture0", texture));
		}

		
		public void Render (Matrix4 mvpMatrix)
		{
			prog.Run(paramSpec.Cons(Tuple.Create<String, Object>("u_mvpMatrix", mvpMatrix)),
				OpenTK.Graphics.OpenGL.BeginMode.TriangleStrip);
		}
		
		

		#region IDisposable implementation
		public void Dispose ()
		{
			Dispose (true);
		}
		#endregion
		
		private void Dispose(bool disposing) {
			if(!isDisposed) {
				if(disposing) {
					prog.Dispose();
					vertices.Dispose();
					texCoords.Dispose();
					
				}
				isDisposed = true;
			}
		}
	}
}