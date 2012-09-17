// 
// MutableParameterSet.cs
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
using Pi.Data;
using System.Collections.Generic;

namespace Pi.ThreeD.GL
{
	public class MutableParameterSet
	{
		public static MutableParameterSet FromTuples(params Tuple<String, Object>[] parameters) {
			return FromTuples ((IEnumerable<Tuple<String, Object>>)parameters);
		}
		public static MutableParameterSet FromTuples(IEnumerable<Tuple<String, Object>> parameters) {
			MutableParameterSet mps = new MutableParameterSet();
			foreach(var param in parameters) {
				mps.SetParameter(param.Item1, param.Item2);
			}
			return mps;
		}
		
		private List<MutableTuple<String, Object>> parameters = new List<MutableTuple<String, Object>>();
		private IDictionary<String,int> parameterPositions = new Dictionary<String,int>();
		public MutableParameterSet ()
		{}
		public void SetParameter(String name, Object value) {
			int pos;
			if(parameterPositions.TryGetValue(name, out pos)) {
				parameters[pos].Item2 = value;
			} else {
				parameters.Add(MutableTuple.Create(name, value));
				parameterPositions[name] = parameters.Count - 1;
			}
		}
		
		public IEnumerable<MutableTuple<String, Object>> Parameters {
			get { return parameters; }
		}
	}
}

