// 
// ImmutableList.cs
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
using System.Collections.Generic;

namespace Pi.Data
{
	/*
	 * This class is an incomplete port of the List<T> class from the functionalJava library.
	 * */
	public abstract class ImmutableList<T> : IList<T>
	{
		private class _Nil<V> : ImmutableList<V> {
			public override V Head { get { throw new Exception("Head on empty list!"); }}
			public override ImmutableList<V> Tail { get { throw new Exception("Tail on empty list!"); }}
		}
		private class _Cons<V> : ImmutableList<V> {
			private readonly V head;
			private readonly ImmutableList<V> tail;
			public _Cons (V head, ImmutableList<V> tail) {
				this.tail = tail;
				this.head = head;
			}
			public override V Head { get { return head; }}
			public override ImmutableList<V> Tail { get { return tail; }}
		}
		
		
		public static ImmutableList<T> Nil() {
			return new _Nil<T>();
		}
		
		public static ImmutableList<T> List(params T[] items) {
			return List((IEnumerable<T>)items);
		}
		
		public static ImmutableList<T> List(IEnumerable<T> items) {
			ImmutableList<T> result = Nil();
			foreach(T item in items) {
				result = result.Cons(item);
			}
			return result;
		}
		
		public Func<T, ImmutableList<T>> Cons() {
			return (item) => (Cons(item));
		}
		public ImmutableList<T> Cons(T item) {
			return new _Cons<T>(item, this);
		}
		
		public bool IsEmpty { get { return (this is _Nil<T>); }}
		public bool IsNotEmpty { get { return (this is _Cons<T>); }}
		
		
		public abstract T Head {get;}
		public abstract ImmutableList<T> Tail {get;}
		
		
		
		public A FoldLeft<A>(Func<A, T, A> f, A initial) {
			A current = initial;
			for(ImmutableList<T> xs = this; !xs.IsEmpty; xs = xs.Tail) {
				current = f(current, xs.Head);
			}
			return current;
		}
		
		
		public T[] ToArray() {
			T[] arr = new T[Count];
			int i = 0;
			foreach(T item in this) {
				arr[i++] = item;
			}
			return arr;
		}
		
		public ImmutableList<T> Reverse() {
			return FoldLeft<ImmutableList<T>>(
				(accu, a) => accu.Cons(a),
				ImmutableList<T>.Nil());
		}
		
		#region IList[T] implementation
		int IList<T>.IndexOf (T item)
		{
			int i = 0;
			ImmutableList<T> xs = this;
			for(; !xs.Head.Equals(item); xs = xs.Tail) {
				i++;
			}
			if(xs.Head.Equals(item)) {
				return i;
			} else {
				return -1;
			}
		}

		public T this[int index] {
			get {
				ImmutableList<T> current = this;
				for(int i = index; i > 0; i--) {
					current = current.Tail;
				}
				return current.Head;
			}
			set {
				throw new NotSupportedException ();
			}
		}
		#endregion

		#region IEnumerable[T] implementation
		IEnumerator<T> IEnumerable<T>.GetEnumerator ()
		{
			ImmutableList<T> current = this;
			while(!current.IsEmpty) {
				yield return current.Head;
				current = current.Tail;
			}
		}
		#endregion

		#region IEnumerable implementation
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator ()
		{
			ImmutableList<T> current = this;
			while(!current.IsEmpty) {
				yield return current.Head;
				current = current.Tail;
			}
		}
		#endregion
		
		
		
		#region ICollection[T] implementation
		

		bool ICollection<T>.Contains (T item)
		{
			return ((IList<T>)this).IndexOf(item) != -1;
		}

		void ICollection<T>.CopyTo (T[] array, int arrayIndex)
		{
			if(array == null) {
				throw new ArgumentNullException();
			}
			if(arrayIndex < 0) {
				throw new ArgumentOutOfRangeException();
			}
			try {
				int i = arrayIndex;
				ImmutableList<T> current = this;
				while(!current.IsEmpty) {
					array[i] = current.Head;
					i++;
					current = current.Tail;
				}
			} catch (IndexOutOfRangeException) {
				throw new ArgumentException();
			}
		}

		public int Count {
			get {
				return FoldLeft<int>((accu, _) => (accu + 1), 0);
			}
		}

		bool ICollection<T>.IsReadOnly {
			get {
				return true;
			}
		}
		#endregion
		
		#region Unsupported methods
		//All methods below are not supported, but have to appear here to comply with the interfaces
		void ICollection<T>.Add (T item)
		{
			throw new NotSupportedException ();
		}

		void ICollection<T>.Clear ()
		{
			throw new NotSupportedException ();
		}
		
		bool ICollection<T>.Remove (T item)
		{
			throw new NotSupportedException ();
		}
		void IList<T>.Insert (int index, T item)
		{
			throw new NotSupportedException ();
		}

		void IList<T>.RemoveAt (int index)
		{
			throw new NotSupportedException ();
		}
		#endregion
	}
}

