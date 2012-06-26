// 
// GLRenderBuffer.cs
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

using OpenTK;
using OpenTK.Graphics.OpenGL;
using OGL = OpenTK.Graphics.OpenGL.GL;

namespace Pi.ThreeD.GL
{
	public class GLRenderBuffer : IDisposable
	{
		private readonly int renderBufferId;
		private readonly int width, height;
		private bool isDisposed;
		internal GLRenderBuffer (int width, int height, RenderbufferStorage storage, GLGraphicsContext context)
		{
			this.width = width;
			this.height = height;
			
			
			OGL.GenRenderbuffers(1, out renderBufferId);
			context.CheckForErrorsIfDebugging();
			
			OGL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, renderBufferId);
			OGL.RenderbufferStorage(RenderbufferTarget.Renderbuffer,
				storage, width, height);
			OGL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, 0);
			
			context.CheckForErrorsIfDebugging();
		}
		
		
		public int Id {
			get { return renderBufferId; }
		}
		
		public int Width {
			get { return width; }
		}
		
		public int Height {
			get { return height; }
		}
		public void Dispose ()
		{
			Dispose (true);
		}
		
		private void Dispose(bool disposing) {
			if(!isDisposed) {
				if(disposing) {
					int id = renderBufferId;
					OGL.DeleteRenderbuffers(1, ref id);
				}
				isDisposed = true;
			}
		}
	}
}

