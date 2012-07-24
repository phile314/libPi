// 
// GLGraphicsContext.cs
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
using System.IO;
using System.Collections.Generic;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OGL = OpenTK.Graphics.OpenGL.GL;

namespace Pi.ThreeD.GL
{
	public class GLGraphicsContext
	{
		private bool isDisposed;
		private LinkedList<System.Drawing.Rectangle> viewports = new LinkedList<System.Drawing.Rectangle>();
		private LinkedList<WeakReference> toDispose = new System.Collections.Generic.LinkedList<WeakReference>();
		private Stack<TextureUnit> freeUnits = new Stack<TextureUnit>();
		private readonly bool debug;
		private readonly float maxAnisotropy;
		private float defaultAnisotropy;
		
		internal GLGraphicsContext ()
		{
			Initialize();
		}
		
		internal GLGraphicsContext (IGraphicsContext tkContext)
			: this(tkContext, false)
		{}
		
		internal GLGraphicsContext (IGraphicsContext tkContext, bool debug)
		{
			this.debug = debug;
			Initialize();
			OGL.GetFloat((GetPName)ExtTextureFilterAnisotropic.MaxTextureMaxAnisotropyExt, out maxAnisotropy);
			defaultAnisotropy = maxAnisotropy;
		}
		
		private void Initialize() {
			viewports.AddFirst(new System.Drawing.Rectangle(0, 0, 0, 0));
			
			int textureUnitCount;
			OGL.GetInteger(GetPName.MaxCombinedTextureImageUnits, out textureUnitCount);
			for(int i = textureUnitCount - 1; i >= 0; i--) {
				freeUnits.Push(GLHelpers.IdToTextureUnit(i));
			}
				
			OGL.ClearColor(0.0f, 0.0f, 0.0f, 0.0f);
			OGL.Enable(EnableCap.DepthTest);
			OGL.Enable(EnableCap.Texture2D);
			OGL.Enable(EnableCap.CullFace);
		}
		
		public GLGenericVertexBuffer<T> NewVertexBuffer<T> (VertexAttribPointerType glType, int size)
			where T : struct
		{
			return AddToDisposables(new GLGenericVertexBuffer<T>(BufferTarget.ArrayBuffer, glType, size));
		}
		
		public GLIndicesBuffer NewIndicesBuffer() {
			return AddToDisposables(new GLIndicesBuffer());
		}
		
		/// <summary>
		/// Creates a new texture and uploads a empty image.
		/// </summary>
		public GLTexture NewEmptyTexture(TextureMinFilter minFilter, TextureMagFilter magFilter, TextureWrapMode wrapS, TextureWrapMode wrapT,
			int width, int height,
			PixelFormat pixelFormat, PixelType pixelType) {
			return NewEmptyTexture(minFilter, magFilter, wrapS, wrapT,
				width, height,
				pixelFormat, pixelType, PixelInternalFormat.Four);
		}
		
		/// <summary>
		/// Creates a new texture and uploads a empty image.
		/// </summary>
		public GLTexture NewEmptyTexture(TextureMinFilter minFilter, TextureMagFilter magFilter, TextureWrapMode wrapS, TextureWrapMode wrapT,
			int width, int height,
			PixelFormat pixelFormat, PixelType pixelType, PixelInternalFormat internalFormat) {
			return NewEmptyTexture(minFilter, magFilter, wrapS, wrapT,
				width, height,
				pixelFormat, pixelType, PixelInternalFormat.Four, DefaultAnisotropy);
		}
		public GLTexture NewEmptyTexture(TextureMinFilter minFilter, TextureMagFilter magFilter, TextureWrapMode wrapS, TextureWrapMode wrapT,
			int width, int height,
			PixelFormat pixelFormat, PixelType pixelType, PixelInternalFormat internalFormat, float anisotropy) {
			GLTexture tex = NewTexture(minFilter, magFilter, wrapS, wrapT, anisotropy);
			byte[] empty = new byte[width * height * 4];
			tex.UploadImage(empty, width, height, internalFormat, pixelFormat, pixelType);
			return tex;
		}
		
		public GLTexture NewTexture (TextureMinFilter minFilter, TextureMagFilter magFilter, TextureWrapMode wrapS, TextureWrapMode wrapT) {
			return NewTexture (minFilter, magFilter, wrapS, wrapT, defaultAnisotropy);
		}
		
		public GLTexture NewTexture (TextureMinFilter minFilter, TextureMagFilter magFilter, TextureWrapMode wrapS, TextureWrapMode wrapT, float anisotropy)
		{
			TextureUnit texUnit = freeUnits.Pop();
			return AddToDisposables(new GLTexture(texUnit, (_ => freeUnits.Push(texUnit)),
				minFilter, magFilter, wrapS, wrapT, anisotropy));
		}
		
		public GLProgram NewProgram(String vertexShader, String fragmentShader) {
			return AddToDisposables(new GLProgram(vertexShader, fragmentShader));
		}
		
		public GLProgram NewProgramFromFiles(String vertexShaderFile, String fragmentShaderFile) {
			return NewProgram(File.ReadAllText(vertexShaderFile), File.ReadAllText(fragmentShaderFile));
		}
		
		public GLRenderBuffer NewRenderBuffer(int width, int height, RenderbufferStorage storage) {
			return AddToDisposables(new GLRenderBuffer(width, height, storage, this));
		}
		
		public GLFrameBuffer NewFrameBuffer(GLRenderBuffer colorBuffer, GLRenderBuffer depthBuffer,
			FramebufferTarget fbTarget) {
			return AddToDisposables(new GLFrameBuffer(colorBuffer, depthBuffer, fbTarget,
				false, this));
		}
		
		
		public void CheckForErrors() {
			
			ErrorCode err = OGL.GetError();
			if(err != ErrorCode.NoError) {
				throw new Exception(String.Format("GL error: {0}", err));
				
			}
		}
		
		public void CheckForErrorsIfDebugging() {
			if(debug) {
				CheckForErrors();
			}
		}
		
		
		public void ReadPixels(int x, int y, int width, int height, PixelFormat pixelFormat, PixelType pixelType,
			IntPtr data) {
			OGL.ReadPixels(x, y, width, height, pixelFormat, pixelType,
				data);
		}
		
		public void ReadPixels<T>(int x, int y, int width, int height, PixelFormat pixelFormat, PixelType pixelType,
			T[] data)
			where T : struct
		{
			OGL.ReadPixels<T>(x, y, width, height, pixelFormat, pixelType,
				data);
		}
		
		public void Clear() {
			OGL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
		}
		
		public void PushViewport() {
			viewports.AddFirst(viewports.First.Value);
		}
		public void PopViewport() {
			viewports.RemoveFirst();
			OGL.Viewport(viewports.First.Value);
		}
		public void SetViewport(System.Drawing.Rectangle rect) {
			viewports.First.Value = rect;
			OGL.Viewport(rect);
		}
		public void SetViewport(int x, int y, int width, int height) {
			SetViewport(new System.Drawing.Rectangle(x, y, width, height));
		}
		
		public void RunProgram(GLProgram program, IEnumerable<Tuple<Object, String>> parameters,
			BeginMode drawMode) {
			
			program.Use();
			
			int bufferLength = PassParameters(program, parameters);
			if(bufferLength == -1) {
				throw new Exception("There are no buffers to draw!");
			}
			
			OGL.DrawArrays(drawMode, 0, bufferLength);
			
			Cleanup(parameters);
			
			CheckForErrorsIfDebugging();
		}
		
		public void RunProgram(GLProgram program, IEnumerable<Tuple<Object, String>> parameters,
			GLIndicesBuffer indicesBuffer,
			BeginMode drawMode) {
			
			program.Use();
			
			PassParameters(program, parameters);
			
			indicesBuffer.BindAndDraw(drawMode);
			indicesBuffer.Disable();			
			
			Cleanup(parameters);
			
			CheckForErrorsIfDebugging();
		}
		
		public float MaxAnisotropy {
			get { return maxAnisotropy; }
		}
		public float DefaultAnisotropy {
			get { return defaultAnisotropy; }
			set { defaultAnisotropy = value; }
		}
		
		private int PassParameters(GLProgram program, IEnumerable<Tuple<Object, String>> parameters) {
			int bufferLength = -1;
			foreach(Tuple<Object, String> param in parameters) {
				if(param.Item1 is GLVertexBuffer) {
					((GLVertexBuffer)param.Item1).BindAndEnable(program.GetAttributeLocation(param.Item2));
					int length = ((GLVertexBuffer)param.Item1).Length;
					if(bufferLength != length) {
						if(bufferLength == -1) {
							bufferLength = length;
						} else {
							throw new Exception("All attribute buffers must have the same length.");
						}
					}
				} else {
					PassUniform(param.Item1, program.GetUniformLocation(param.Item2));
				}
				CheckForErrorsIfDebugging();
			}
			return bufferLength;
		}
		
		private void Cleanup(IEnumerable<Tuple<Object, String>> parameters) {
			foreach(Tuple<Object, String> param in parameters) {
				if(param.Item1 is GLVertexBuffer) {
					((GLVertexBuffer)param.Item1).Disable();
				}
			}
		}
		
		private void PassUniform(Object data, int uniformLoc) {
			if(data is GLTexture) {
				OGL.Uniform1(uniformLoc, GLHelpers.TextureUnitToId(((GLTexture)data).Unit));
			} else if(data is Matrix3) {
				Matrix3 temp = (Matrix3)data;
				unsafe {
					OGL.UniformMatrix3(uniformLoc, 1, false, &temp.Row0.X);
				}
			} else if(data is Matrix4) {
				Matrix4 temp = (Matrix4)data;
				OGL.UniformMatrix4(uniformLoc, false, ref temp);
			} else if(data is Vector2) {
				Vector2 temp = (Vector2)data;
				OGL.Uniform2(uniformLoc, ref temp);
			} else if(data is Vector3) {
				Vector3 temp = (Vector3)data;
				OGL.Uniform3(uniformLoc, ref temp);
			} else if(data is Vector4) {
				Vector4 temp = (Vector4)data;
				OGL.Uniform4(uniformLoc, ref temp);
			} else if(data is float) {
				OGL.Uniform1(uniformLoc, (float)data);
			} else if(data is int) {
				OGL.Uniform1(uniformLoc, (int)data);
			} else {
				throw new NotSupportedException();
			}
		}
		
		private T AddToDisposables<T>(T item)
			where T : IDisposable
		{
			//it is important to add new objects at the head, to ensure that the dispose order
			//is the inverted creation order
			toDispose.AddFirst(new WeakReference(item));
			return item;
		}
		
		public void Dispose() {
			Dispose (true);
		}
		
		private void Dispose(bool disposing) {
			if(isDisposed) {
				if(disposing) {
					foreach(WeakReference w in toDispose) {
						IDisposable obj = (IDisposable)w.Target;
						if(obj != null) {
							obj.Dispose();
						}
					}
				}
				isDisposed = true;
			}
		}
	}
}

