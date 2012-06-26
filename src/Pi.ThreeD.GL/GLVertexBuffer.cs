// 
// GLVertexBuffer.cs
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
using OpenTK.Graphics.OpenGL;
using OGL = OpenTK.Graphics.OpenGL.GL;

namespace Pi.ThreeD.GL
{
	public abstract class GLVertexBuffer : IDisposable
	{
		private bool isDisposed;
		protected int bufferId, length, stride, size;
		protected BufferTarget target;
		protected VertexAttribPointerType glType;
		
		protected int attribLoc = -1;
		
		internal GLVertexBuffer (BufferTarget target, VertexAttribPointerType glType, int stride, int size)
		{
			this.stride = stride;
			this.size = size;
			this.glType = glType;
			this.target = target;
			OGL.GenBuffers(1, out bufferId);
		}
		
		internal void BindAndEnable(int attribLoc) {
			if(this.attribLoc != -1) throw new Exception();
			this.attribLoc = attribLoc;
			OGL.BindBuffer(target, bufferId);
			OGL.EnableVertexAttribArray(attribLoc);
			OGL.VertexAttribPointer(attribLoc, size, glType, false, stride, 0);
		}
		
		internal virtual void Disable() {
			if(attribLoc == -1) throw new Exception();
			OGL.DisableVertexAttribArray(attribLoc);
			OGL.BindBuffer(target, 0);
			this.attribLoc = -1;
		}
		
		internal int Length { get { return length; } }
		
		public void Dispose() {
			Dispose(true);
		}
		
		protected void Dispose(bool disposing) {
			if(!isDisposed) {
				if(disposing) {
					OGL.DeleteBuffers(1, ref bufferId);
				}
				isDisposed = true;
			}
		}
	}
}

