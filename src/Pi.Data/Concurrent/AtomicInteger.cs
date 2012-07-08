// 
// AtomicInteger.cs
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
using System.Threading;

namespace Pi.Data.Concurrent
{
	public class AtomicInteger
	{
		private volatile int value;

		public AtomicInteger(int initialValue)
		{
			Set(initialValue);
		}

		public int CompareExchange(int newValue, int comparand)
		{
			#pragma warning disable 0420
			return Interlocked.CompareExchange(ref this.value, newValue, comparand);
			#pragma warning restore 0420
		}
		
		/// <summary>
		/// Applies the given function to the current value. This operation is atomic.
		/// The function f MUST be pure, else the behaviour of this method is undefined.
		/// </summary>
		/// <param name='f'>
		/// The update function.
		/// </param>
		public void Update(Func<int, int> f) {
			int initial, newValue;
			do {
				initial = value;
				newValue = f(initial);
				
			} while (CompareExchange(newValue, initial) != initial);
		}

		public int Exchange(int newValue)
		{
			#pragma warning disable 0420
			return Interlocked.Exchange(ref this.value, newValue);
			#pragma warning restore 0420
		}
		
		public int Decrement() {
			#pragma warning disable 0420
			return Interlocked.Decrement(ref this.value);
			#pragma warning restore 0420
		}
		
		public int Increment() {
			#pragma warning disable 0420
			return Interlocked.Increment(ref this.value);
			#pragma warning restore 0420
		}
		
		public int Get() {
			return value;
		}
		
		public void Set(int newValue) {
			value = newValue;
		}

	}
}

