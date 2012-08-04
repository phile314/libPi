// 
// MutableTuple.cs
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

namespace Pi.Data
{
	public static class MutableTuple {
		public static MutableTuple<T1, T2> Create<T1, T2>(T1 item1, T2 item2) {
			return new MutableTuple<T1, T2>(item1, item2);
		}
		public static MutableTuple<T1, T2, T3> Create<T1, T2, T3>(T1 item1, T2 item2, T3 item3) {
			return new MutableTuple<T1, T2, T3>(item1, item2, item3);
		}
	}
	public class MutableTuple<T1, T2>
	{
		private T1 item1;
		private T2 item2;
		public MutableTuple (T1 item1, T2 item2)
		{
			this.item1 = item1;
			this.item2 = item2;
		}
		public T1 Item1 {
			get { return item1; }
			set { this.item1 = value; }
		}
		public T2 Item2 {
			get { return item2; }
			set { this.item2 = value; }
		}
	}
	public class MutableTuple<T1, T2, T3>
	{
		private T1 item1;
		private T2 item2;
		private T3 item3;
		public MutableTuple (T1 item1, T2 item2, T3 item3)
		{
			this.item1 = item1;
			this.item2 = item2;
			this.item3 = item3;
		}
		public T1 Item1 {
			get { return item1; }
			set { this.item1 = value; }
		}
		public T2 Item2 {
			get { return item2; }
			set { this.item2 = value; }
		}
		public T3 Item3 {
			get { return item3; }
			set { this.item3 = value; }
		}
	}
}

