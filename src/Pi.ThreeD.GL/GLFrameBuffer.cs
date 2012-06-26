// 
// GLFrameBuffer.cs
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
	public class GLFrameBuffer : IDisposable
	{
		private bool isDisposed;
		private readonly int fboId;
		private readonly FramebufferTarget fbTarget;
		private readonly GLRenderBuffer colorBuffer;
		private readonly GLRenderBuffer depthBuffer;
		private readonly GLGraphicsContext context;
		private readonly bool disposeRenderBuffers;
		
		private GLFrameBuffer(GLGraphicsContext context) {
			this.context = context;
			
			OGL.GenFramebuffers(1, out fboId);
			context.CheckForErrorsIfDebugging();
		}
		
		internal GLFrameBuffer (GLRenderBuffer colorBuffer, GLRenderBuffer depthBuffer, FramebufferTarget fbTarget, bool disposeRenderBuffers, GLGraphicsContext context)
			: this(context)
		{
			this.disposeRenderBuffers = disposeRenderBuffers;
			this.colorBuffer = colorBuffer;
			this.depthBuffer = depthBuffer;
			this.fbTarget = fbTarget;
			
			OGL.BindFramebuffer(fbTarget, fboId);
			
			OGL.FramebufferRenderbuffer(fbTarget, FramebufferAttachment.ColorAttachment0,
				RenderbufferTarget.Renderbuffer, colorBuffer.Id);
			OGL.FramebufferRenderbuffer(fbTarget, FramebufferAttachment.DepthAttachment,
				RenderbufferTarget.Renderbuffer, depthBuffer.Id);
			
			FramebufferErrorCode err = OGL.CheckFramebufferStatus(fbTarget);
			switch(err) {
			case FramebufferErrorCode.FramebufferComplete:
				break;
			default:
				this.Dispose();
				throw new Exception(String.Format("Invalid frame buffer status: {0}", err));
			}
			
			OGL.BindFramebuffer(fbTarget, 0);
			
			context.CheckForErrorsIfDebugging();
			
		}
		
		/// <summary>
		/// Activates this fbo replacing the default fbo.
		/// </summary>
		public void Activate() {
			OGL.BindFramebuffer(fbTarget, fboId);
			
			context.PushViewport();
			context.SetViewport(0, 0, colorBuffer.Width, colorBuffer.Height);
		}
		
		/// <summary>
		/// Disables this fbo and uses the default fbo again.
		/// </summary>
		public void Deactivate() {
			context.PopViewport();
			OGL.BindFramebuffer(fbTarget, 0);
		}
		
		
		public void Dispose ()
		{
			Dispose (true);
		}
		
		private void Dispose (bool disposing) {
			if(!isDisposed) {
				if(disposing) {
					OGL.FramebufferRenderbuffer(fbTarget, FramebufferAttachment.ColorAttachment0,
						RenderbufferTarget.Renderbuffer, 0);
					OGL.FramebufferRenderbuffer(fbTarget, FramebufferAttachment.DepthAttachment,
						RenderbufferTarget.Renderbuffer, 0);
					int id = fboId;
					OGL.DeleteFramebuffers(1, ref id);
					
					if(disposeRenderBuffers) {
						depthBuffer.Dispose();
						colorBuffer.Dispose();
					}
				}
				
				isDisposed = true;
			}
		}
	}
}

