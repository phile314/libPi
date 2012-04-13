// 
// GLAbstractShader.cs
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
using OGL = OpenTK.Graphics.OpenGL.GL;

namespace Pi.ThreeD.GL.Internal
{
	internal abstract class GLAbstractShader : IDisposable
	{
		private bool isDisposed;
		private int shaderId;
		public GLAbstractShader (OpenTK.Graphics.OpenGL.ShaderType shaderType, String shader)
		{
			shaderId = OGL.CreateShader(shaderType);
			LoadShader(shader);
		}
		
		private void LoadShader(String shader) {
			OGL.ShaderSource(shaderId, shader);
			OGL.CompileShader(shaderId);
			int success;
			OGL.GetShader(shaderId, OpenTK.Graphics.OpenGL.ShaderParameter.CompileStatus, out success);
			if(success == 0) {
				String message;
				OGL.GetShaderInfoLog(shaderId, out message);
				throw new Exception(String.Format("Error while compiling the shader: {0}", message));
			}
		}
		
		public int Id { get { return shaderId; } }
		
		public void Dispose() {
			Dispose(true);
			GC.SuppressFinalize(this);
		}
		
		protected void Dispose(bool disposing) {
			if(!isDisposed) {
				if(disposing) {
					
					OGL.DeleteShader(shaderId);
				}
				isDisposed = true;
			}
		}
		
		
	}
}

