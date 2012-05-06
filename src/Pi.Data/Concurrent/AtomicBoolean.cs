// 
// AtomicBoolean.cs
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
	//Microsoft does still not support boolean in the Interlocked class...: http://connect.microsoft.com/VisualStudio/feedback/details/98293/interlocked-compareexchange-should-offer-an-overload-for-boolean
	public class AtomicBoolean
	{
		private const int TRUE = 1;
		private const int FALSE = 0;
		private int value;

		public AtomicBoolean(bool initialValue)
		{
			Set(initialValue);
		}

		public bool CompareExchange(bool newValue, bool comparand)
		{
			return Interlocked.CompareExchange(ref this.value, newValue ? TRUE : FALSE, comparand ? TRUE : FALSE) == TRUE;
		}

		public bool Exchange(bool newValue)
		{
			return Interlocked.Exchange(ref this.value, newValue ? TRUE : FALSE) == TRUE;
		}
		
		public bool Get() {
			return Thread.VolatileRead(ref value) == TRUE;
		}
		
		public void Set(bool newValue) {
			Thread.VolatileWrite(ref this.value, newValue ? TRUE : FALSE);
		}

	}
}

