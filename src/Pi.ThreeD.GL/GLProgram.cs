// 
// GLProgram.cs
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
using Pi.ThreeD.GL.Internal;
using OpenTK.Graphics.OpenGL;
using OGL = OpenTK.Graphics.OpenGL.GL;
using log4net;
using System.Collections.Generic;

namespace Pi.ThreeD.GL
{
	public class GLProgram : IDisposable
	{
		private readonly static ILog log = LogManager.GetLogger(typeof(GLProgram));
		
		private bool isDisposed;
		private GLVertexShader vertexShader;
		private GLFragmentShader fragmentShader;
		private int programId;
		
		private IDictionary<String,int> uniformLocations = new Dictionary<String,int>();
		private IDictionary<String,int> attributeLocations = new Dictionary<String,int>();
		
		internal GLProgram (String vertexShader, String fragmentShader)
		{
			this.vertexShader = new GLVertexShader(vertexShader);
			this.fragmentShader = new GLFragmentShader(fragmentShader);
			
			programId = OGL.CreateProgram();
			OGL.AttachShader(programId, this.vertexShader.Id);
			OGL.AttachShader(programId, this.fragmentShader.Id);
			
			Link();
		}
		
		private void Link() {
			OGL.LinkProgram(programId);
			int success;
			OGL.GetProgram(programId, ProgramParameter.LinkStatus, out success);
			if(success == 0) {
				String message;
				OGL.GetProgramInfoLog(programId, out message);
				throw new Exception(String.Format("Error while linking shader: {0}", message));
			}
			
			OGL.ValidateProgram(programId);
			OGL.GetProgram(programId, ProgramParameter.ValidateStatus, out success);
			if(success == 0) {
				String message;
				OGL.GetProgramInfoLog(programId, out message);
				throw new Exception(String.Format("Error while validating shader: {0}", message));
			}
			
		}
		
		internal void Use() {
			OGL.UseProgram(programId);
		}
		
		internal int GetUniformLocation(String uniformName) {
			int loc;
			if(uniformLocations.ContainsKey(uniformName)) {
				loc = uniformLocations[uniformName];
			} else {
				loc = OGL.GetUniformLocation(programId, uniformName);
				uniformLocations[uniformName] = loc;
			}
			if(loc == -1) log.Debug(String.Format("No uniform with name {0} found.", uniformName));
			return loc;
		}
		
		internal int GetAttributeLocation(String attributeName) {
			int loc;
			if(attributeLocations.ContainsKey(attributeName)) {
				loc = attributeLocations[attributeName];
			} else {
				loc = OGL.GetAttribLocation(programId, attributeName);
				attributeLocations[attributeName] = loc;
			}
			if(loc == -1) log.Debug(String.Format("No attribute with name {0} found.", attributeName));
			return loc;
		}
		
		
		public void Dispose() {
			Dispose(true);
		}
		
		protected void Dispose(bool disposing) {
			if(!isDisposed) {
				if(disposing) {
					OGL.DetachShader(programId, vertexShader.Id);
					vertexShader.Dispose();
					OGL.DetachShader(programId, fragmentShader.Id);
					fragmentShader.Dispose();
					OGL.DeleteProgram(programId);
				}
				vertexShader = null;
				fragmentShader = null;
				isDisposed = true;
			}
		}
	}
}

