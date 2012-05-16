// 
// TextRenderer.cs
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
using System.Drawing;

namespace Pi.ThreeD.GL.Renderers
{
	public class TextRenderer : IDisposable
	{
		private bool isDisposed;
		
		private readonly GLTexture texture;
		private readonly TextureRenderer texRenderer;
		private string text;
		private Bitmap textBitmap;
		
		private Brush brush = new SolidBrush(Color.White);
		private Font font = new Font("Arial", 1);
		
		public TextRenderer (GLGraphicsContext context, int height, int width, String text)
		{
			this.text = text;
			
			texture = context.NewEmptyTexture(OpenTK.Graphics.OpenGL.TextureMinFilter.Linear, OpenTK.Graphics.OpenGL.TextureMagFilter.Linear,
				OpenTK.Graphics.OpenGL.TextureWrapMode.ClampToBorder, OpenTK.Graphics.OpenGL.TextureWrapMode.ClampToBorder,
				width, height,
				OpenTK.Graphics.OpenGL.PixelFormat.Rgba, OpenTK.Graphics.OpenGL.PixelType.UnsignedByte);
			
			texRenderer = new TextureRenderer(context, texture);
			
			Resize (width, height);
		}
		
		public void Resize(int width, int height) {
			
			if(textBitmap != null)
				textBitmap.Dispose();
			
			textBitmap = new Bitmap(width, height);
			RenderToTexture();
		}
		
		public void SetText(String text) {
			this.text = text;
			RenderToTexture();
		}
		
		private void RenderToTexture() {
			using (Graphics gfx = Graphics.FromImage(textBitmap))
			{
				gfx.Clear(Color.Transparent);
				gfx.DrawString(text, font, brush, Point.Empty);
			}

			texture.UploadImage(textBitmap);
		}
		
		public void Render(Matrix4 mvpMatrix) {
			texRenderer.Render(mvpMatrix);
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
					texRenderer.Dispose();
					texture.Dispose();
					textBitmap.Dispose();
				}
				isDisposed = true;
			}
		}
	}
}

