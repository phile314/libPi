// 
// GLTexture.cs
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
using System.Drawing;
using System.Drawing.Imaging;
using OGL = OpenTK.Graphics.OpenGL.GL;
using OpenTK.Graphics.OpenGL;

namespace Pi.ThreeD.GL
{
	public class GLTexture : IDisposable
	{
		private bool isDisposed;
		private int textureId;
		
		private TextureTarget target = TextureTarget.Texture2D;
		private TextureMinFilter minFilter;
		private TextureMagFilter magFilter;
		private TextureWrapMode wrapS, wrapT;
		private TextureUnit unit;
		private Action<GLTexture> disposing;
		
		private OpenTK.Graphics.OpenGL.PixelFormat pixelFormat;
		private OpenTK.Graphics.OpenGL.PixelType pixelType;
		
		internal GLTexture (TextureUnit unit, Action<GLTexture> disposing, TextureMinFilter minFilter, TextureMagFilter magFilter, TextureWrapMode wrapS, TextureWrapMode wrapT)
		{
			this.minFilter = minFilter;
			this.magFilter = magFilter;
			this.wrapS = wrapS;
			this.wrapT = wrapT;
			this.unit = unit;
			this.disposing = disposing;
			
			OGL.ActiveTexture(unit);
			OGL.GenTextures(1, out textureId);
			OGL.BindTexture(target, textureId);
		}
		
		public void UploadImage(Bitmap image) {
			UploadImage (image, PixelInternalFormat.Four);
		}
		
		public void UploadImage(Bitmap image, PixelInternalFormat internalFormat) {
			this.pixelFormat = OpenTK.Graphics.OpenGL.PixelFormat.Bgr;
			this.pixelType = OpenTK.Graphics.OpenGL.PixelType.UnsignedByte;
			PrepareForUpload();
			BitmapData data = image.LockBits(new Rectangle(0, 0, image.Width, image.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
			OGL.TexImage2D(target, 0, internalFormat, image.Width, image.Height, 0, pixelFormat, pixelType, data.Scan0);
			image.UnlockBits(data);
		}
		
		/// <summary>
		/// Uploads a image to a texture unit. If you update an existing image, UploadSubImage should be used as it is faster.
		/// </summary>
		/// <param name='image'>
		/// The image.
		/// </param>
		/// <param name='imageWidth'>
		/// Image width.
		/// </param>
		/// <param name='imageHeight'>
		/// Image height.
		/// </param>
		/// <param name='internalFormat'>
		/// Internal format.
		/// </param>
		/// <param name='pixelFormat'>
		/// Pixel format.
		/// </param>
		/// <param name='pixelType'>
		/// Pixel type.
		/// </param>
		public void UploadImage<T>(T[] image, int imageWidth, int imageHeight, OpenTK.Graphics.OpenGL.PixelInternalFormat internalFormat,
			OpenTK.Graphics.OpenGL.PixelFormat pixelFormat,
			OpenTK.Graphics.OpenGL.PixelType pixelType)
		where T : struct {
			this.pixelFormat = pixelFormat;
			this.pixelType = pixelType;
			PrepareForUpload();
			OGL.TexImage2D<T>(target, 0, internalFormat, imageWidth, imageHeight, 0, pixelFormat, pixelType, image);
		}
		/// <summary>
		/// Uploads a image to a texture unit. If you update an existing image, UploadSubImage should be used as it is faster.
		/// </summary>
		/// <param name='image'>
		/// The image.
		/// </param>
		/// <param name='imageWidth'>
		/// Image width.
		/// </param>
		/// <param name='imageHeight'>
		/// Image height.
		/// </param>
		/// <param name='internalFormat'>
		/// Internal format.
		/// </param>
		/// <param name='pixelFormat'>
		/// Pixel format.
		/// </param>
		/// <param name='pixelType'>
		/// Pixel type.
		/// </param>
		public void UploadImage(IntPtr image, int imageWidth, int imageHeight, OpenTK.Graphics.OpenGL.PixelInternalFormat internalFormat,
			OpenTK.Graphics.OpenGL.PixelFormat pixelFormat,
			OpenTK.Graphics.OpenGL.PixelType pixelType) {
			this.pixelFormat = pixelFormat;
			this.pixelType = pixelType;
			PrepareForUpload();
			OGL.TexImage2D(target, 0, internalFormat, imageWidth, imageHeight, 0, pixelFormat, pixelType, image);
		}
		
		/// <summary>
		/// Updates a part of an existing texture with a new image.
		/// </summary>
		/// <param name='subImage'>
		/// The new sub image.
		/// </param>
		/// <param name='offsetX'>
		/// Offset x.
		/// </param>
		/// <param name='offsetY'>
		/// Offset y.
		/// </param>
		/// <param name='imageWidth'>
		/// Image width.
		/// </param>
		/// <param name='imageHeight'>
		/// Image height.
		/// </param>
		public void UploadSubImage<T>(T[] subImage, int offsetX, int offsetY, int imageWidth, int imageHeight)
		where T : struct {
			PrepareForUpload();
			OGL.TexSubImage2D(target, 0, offsetX, offsetY, imageWidth, imageHeight, pixelFormat, pixelType, subImage);
		}
		/// <summary>
		/// Updates a part of an existing texture with a new image.
		/// </summary>
		/// <param name='subImage'>
		/// The new sub image.
		/// </param>
		/// <param name='offsetX'>
		/// Offset x.
		/// </param>
		/// <param name='offsetY'>
		/// Offset y.
		/// </param>
		/// <param name='imageWidth'>
		/// Image width.
		/// </param>
		/// <param name='imageHeight'>
		/// Image height.
		/// </param>
		public void UploadSubImage(IntPtr subImage, int offsetX, int offsetY, int imageWidth, int imageHeight) {
			PrepareForUpload();
			OGL.TexSubImage2D(target, 0, offsetX, offsetY, imageWidth, imageHeight, pixelFormat, pixelType, subImage);
		}
		
		private void PrepareForUpload() {
			OGL.ActiveTexture(unit);
			OGL.TexParameter(target, TextureParameterName.TextureMinFilter, (int)minFilter);
			OGL.TexParameter(target, TextureParameterName.TextureMagFilter, (int)magFilter);
			OGL.TexParameter(target, TextureParameterName.TextureWrapS, (int)wrapS);
			OGL.TexParameter(target, TextureParameterName.TextureWrapT, (int)wrapT);
		}
		
		internal TextureUnit Unit {
			get { return unit; }
		}
			
		
		public void Dispose() {
			Dispose(true);
		}
		
		protected void Dispose(bool disposing) {
			if(!isDisposed) {
				if(disposing) {
					this.disposing(this);
					OGL.DeleteTexture(textureId);
				}
				isDisposed = true;
			}
		}
	}
}

