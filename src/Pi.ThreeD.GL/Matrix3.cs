#region --- License ---
/*
Copyright (c) 2006 - 2008 The Open Toolkit library.

Permission is hereby granted, free of charge, to any person obtaining a copy of
this software and associated documentation files (the "Software"), to deal in
the Software without restriction, including without limitation the rights to
use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies
of the Software, and to permit persons to whom the Software is furnished to do
so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
 */
#endregion

//this is a modified version of the Matrix4 class from OpenTK.

using System;
using System.Runtime.InteropServices;
using OpenTK;

namespace Pi.ThreeD.GL
{
    /// <summary>
    /// Represents a 3x3 Matrix.
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct Matrix3 : IEquatable<Matrix3>
    {
        #region Fields

        /// <summary>
        /// Top row of the matrix
        /// </summary>
        public Vector3 Row0;
        /// <summary>
        /// 2nd row of the matrix
        /// </summary>
        public Vector3 Row1;
        /// <summary>
        /// 3rd row of the matrix
        /// </summary>
        public Vector3 Row2;
 
        /// <summary>
        /// The identity matrix
        /// </summary>
        public static Matrix3 Identity = new Matrix3(Vector3.UnitX, Vector3.UnitY, Vector3.UnitZ);

        #endregion

        #region Constructors

        /// <summary>
        /// Constructs a new instance.
        /// </summary>
        /// <param name="row0">Top row of the matrix</param>
        /// <param name="row1">Second row of the matrix</param>
        /// <param name="row2">Third row of the matrix</param>
        public Matrix3(Vector3 row0, Vector3 row1, Vector3 row2)
        {
            Row0 = row0;
            Row1 = row1;
            Row2 = row2;
        }

        /// <summary>
        /// Constructs a new instance.
        /// </summary>
        /// <param name="m00">First item of the first row of the matrix.</param>
        /// <param name="m01">Second item of the first row of the matrix.</param>
        /// <param name="m02">Third item of the first row of the matrix.</param>
        /// <param name="m10">First item of the second row of the matrix.</param>
        /// <param name="m11">Second item of the second row of the matrix.</param>
        /// <param name="m12">Third item of the second row of the matrix.</param>
        /// <param name="m20">First item of the third row of the matrix.</param>
        /// <param name="m21">Second item of the third row of the matrix.</param>
        /// <param name="m22">Third item of the third row of the matrix.</param>
        public Matrix3(
            float m00, float m01, float m02,
            float m10, float m11, float m12,
            float m20, float m21, float m22)
        {
            Row0 = new Vector3(m00, m01, m02);
            Row1 = new Vector3(m10, m11, m12);
            Row2 = new Vector3(m20, m21, m22);
        }

        #endregion

        #region Public Members

        #region Properties

        /// <summary>
        /// Not implemented!
        /// 
        /// The determinant of this matrix
        /// </summary>
        public float Determinant
        {
            get
            {
				throw new NotImplementedException();
                /*return
                    Row0.X * Row1.Y * Row2.Z * Row3.W - Row0.X * Row1.Y * Row2.W * Row3.Z + Row0.X * Row1.Z * Row2.W * Row3.Y - Row0.X * Row1.Z * Row2.Y * Row3.W
                  + Row0.X * Row1.W * Row2.Y * Row3.Z - Row0.X * Row1.W * Row2.Z * Row3.Y - Row0.Y * Row1.Z * Row2.W * Row3.X + Row0.Y * Row1.Z * Row2.X * Row3.W
                  - Row0.Y * Row1.W * Row2.X * Row3.Z + Row0.Y * Row1.W * Row2.Z * Row3.X - Row0.Y * Row1.X * Row2.Z * Row3.W + Row0.Y * Row1.X * Row2.W * Row3.Z
                  + Row0.Z * Row1.W * Row2.X * Row3.Y - Row0.Z * Row1.W * Row2.Y * Row3.X + Row0.Z * Row1.X * Row2.Y * Row3.W - Row0.Z * Row1.X * Row2.W * Row3.Y
                  + Row0.Z * Row1.Y * Row2.W * Row3.X - Row0.Z * Row1.Y * Row2.X * Row3.W - Row0.W * Row1.X * Row2.Y * Row3.Z + Row0.W * Row1.X * Row2.Z * Row3.Y
                  - Row0.W * Row1.Y * Row2.Z * Row3.X + Row0.W * Row1.Y * Row2.X * Row3.Z - Row0.W * Row1.Z * Row2.X * Row3.Y + Row0.W * Row1.Z * Row2.Y * Row3.X;*/
            }
        }

        /// <summary>
        /// The first column of this matrix
        /// </summary>
        public Vector3 Column0
        {
            get { return new Vector3(Row0.X, Row1.X, Row2.X); }
        }

        /// <summary>
        /// The second column of this matrix
        /// </summary>
        public Vector3 Column1
        {
            get { return new Vector3(Row0.Y, Row1.Y, Row2.Y); }
        }

        /// <summary>
        /// The third column of this matrix
        /// </summary>
        public Vector3 Column2
        {
            get { return new Vector3(Row0.Z, Row1.Z, Row2.Z); }
        }

        /// <summary>
        /// Gets or sets the value at row 1, column 1 of this instance.
        /// </summary>
        public float M11 { get { return Row0.X; } set { Row0.X = value; } }

        /// <summary>
        /// Gets or sets the value at row 1, column 2 of this instance.
        /// </summary>
        public float M12 { get { return Row0.Y; } set { Row0.Y = value; } }

        /// <summary>
        /// Gets or sets the value at row 1, column 3 of this instance.
        /// </summary>
        public float M13 { get { return Row0.Z; } set { Row0.Z = value; } }

        /// <summary>
        /// Gets or sets the value at row 2, column 1 of this instance.
        /// </summary>
        public float M21 { get { return Row1.X; } set { Row1.X = value; } }

        /// <summary>
        /// Gets or sets the value at row 2, column 2 of this instance.
        /// </summary>
        public float M22 { get { return Row1.Y; } set { Row1.Y = value; } }

        /// <summary>
        /// Gets or sets the value at row 2, column 3 of this instance.
        /// </summary>
        public float M23 { get { return Row1.Z; } set { Row1.Z = value; } }

        /// <summary>
        /// Gets or sets the value at row 3, column 1 of this instance.
        /// </summary>
        public float M31 { get { return Row2.X; } set { Row2.X = value; } }

        /// <summary>
        /// Gets or sets the value at row 3, column 2 of this instance.
        /// </summary>
        public float M32 { get { return Row2.Y; } set { Row2.Y = value; } }

        /// <summary>
        /// Gets or sets the value at row 3, column 3 of this instance.
        /// </summary>
        public float M33 { get { return Row2.Z; } set { Row2.Z = value; } }

        #endregion

        #region Instance

        #region public void Invert()

        /// <summary>
        /// Converts this instance into its inverse.
        /// </summary>
        public void Invert()
        {
            this = Matrix3.Invert(this);
        }

        #endregion

        #region public void Transpose()

        /// <summary>
        /// Converts this instance into its transpose.
        /// </summary>
        public void Transpose()
        {
            this = Matrix3.Transpose(this);
        }

        #endregion

        #endregion

        #region Static
        
        #region CreateFromAxisAngle
        
        /// <summary>
        /// Build a rotation matrix from the specified axis/angle rotation.
        /// </summary>
        /// <param name="axis">The axis to rotate about.</param>
        /// <param name="angle">Angle in radians to rotate counter-clockwise (looking in the direction of the given axis).</param>
        /// <param name="result">A matrix instance.</param>
        public static void CreateFromAxisAngle(Vector3 axis, float angle, out Matrix3 result)
        {
            float cos = (float)System.Math.Cos(-angle);
            float sin = (float)System.Math.Sin(-angle);
            float t = 1.0f - cos;

            axis.Normalize();

            result = new Matrix3(t * axis.X * axis.X + cos, t * axis.X * axis.Y - sin * axis.Z, t * axis.X * axis.Z + sin * axis.Y,
                                 t * axis.X * axis.Y + sin * axis.Z, t * axis.Y * axis.Y + cos, t * axis.Y * axis.Z - sin * axis.X,
                                 t * axis.X * axis.Z - sin * axis.Y, t * axis.Y * axis.Z + sin * axis.X, t * axis.Z * axis.Z + cos);
        }
        
        /// <summary>
        /// Build a rotation matrix from the specified axis/angle rotation.
        /// </summary>
        /// <param name="axis">The axis to rotate about.</param>
        /// <param name="angle">Angle in radians to rotate counter-clockwise (looking in the direction of the given axis).</param>
        /// <returns>A matrix instance.</returns>
        public static Matrix3 CreateFromAxisAngle(Vector3 axis, float angle)
        {
            Matrix3 result;
            CreateFromAxisAngle(axis, angle, out result);
            return result;
        }
        
        #endregion

        #region CreateRotation[XYZ]

        /// <summary>
        /// Builds a rotation matrix for a rotation around the x-axis.
        /// </summary>
        /// <param name="angle">The counter-clockwise angle in radians.</param>
        /// <param name="result">The resulting Matrix3 instance.</param>
        public static void CreateRotationX(float angle, out Matrix3 result)
        {
            float cos = (float)System.Math.Cos(angle);
            float sin = (float)System.Math.Sin(angle);

            result.Row0 = Vector3.UnitX;
            result.Row1 = new Vector3(0.0f, cos, sin);
            result.Row2 = new Vector3(0.0f, -sin, cos);
        }

        /// <summary>
        /// Builds a rotation matrix for a rotation around the x-axis.
        /// </summary>
        /// <param name="angle">The counter-clockwise angle in radians.</param>
        /// <returns>The resulting Matrix3 instance.</returns>
        public static Matrix3 CreateRotationX(float angle)
        {
            Matrix3 result;
            CreateRotationX(angle, out result);
            return result;
        }

        /// <summary>
        /// Builds a rotation matrix for a rotation around the y-axis.
        /// </summary>
        /// <param name="angle">The counter-clockwise angle in radians.</param>
        /// <param name="result">The resulting Matrix3 instance.</param>
        public static void CreateRotationY(float angle, out Matrix3 result)
        {
            float cos = (float)System.Math.Cos(angle);
            float sin = (float)System.Math.Sin(angle);

            result.Row0 = new Vector3(cos, 0.0f, -sin);
            result.Row1 = Vector3.UnitY;
            result.Row2 = new Vector3(sin, 0.0f, cos);
        }

        /// <summary>
        /// Builds a rotation matrix for a rotation around the y-axis.
        /// </summary>
        /// <param name="angle">The counter-clockwise angle in radians.</param>
        /// <returns>The resulting Matrix3 instance.</returns>
        public static Matrix3 CreateRotationY(float angle)
        {
            Matrix3 result;
            CreateRotationY(angle, out result);
            return result;
        }

        /// <summary>
        /// Builds a rotation matrix for a rotation around the z-axis.
        /// </summary>
        /// <param name="angle">The counter-clockwise angle in radians.</param>
        /// <param name="result">The resulting Matrix3 instance.</param>
        public static void CreateRotationZ(float angle, out Matrix3 result)
        {
            float cos = (float)System.Math.Cos(angle);
            float sin = (float)System.Math.Sin(angle);

            result.Row0 = new Vector3(cos, sin, 0.0f);
            result.Row1 = new Vector3(-sin, cos, 0.0f);
            result.Row2 = Vector3.UnitZ;
        }

        /// <summary>
        /// Builds a rotation matrix for a rotation around the z-axis.
        /// </summary>
        /// <param name="angle">The counter-clockwise angle in radians.</param>
        /// <returns>The resulting Matrix3 instance.</returns>
        public static Matrix3 CreateRotationZ(float angle)
        {
            Matrix3 result;
            CreateRotationZ(angle, out result);
            return result;
        }

        #endregion

        #region CreateTranslation

        /// <summary>
        /// Creates a translation matrix.
        /// </summary>
        /// <param name="x">X translation.</param>
        /// <param name="y">Y translation.</param>
        /// <param name="result">The resulting Matrix3 instance.</param>
        public static void CreateTranslation(float x, float y, out Matrix3 result)
        {
            result = Identity;
            result.Row2 = new Vector3(x, y, 1);
        }

        /// <summary>
        /// Creates a translation matrix.
        /// </summary>
        /// <param name="vector">The translation vector.</param>
        /// <param name="result">The resulting Matrix3 instance.</param>
        public static void CreateTranslation(ref Vector2 vector, out Matrix3 result)
        {
            result = Identity;
            result.Row2 = new Vector3(vector.X, vector.Y, 1);
        }

        /// <summary>
        /// Creates a translation matrix.
        /// </summary>
        /// <param name="x">X translation.</param>
        /// <param name="y">Y translation.</param>
        /// <returns>The resulting Matrix3 instance.</returns>
        public static Matrix3 CreateTranslation(float x, float y)
        {
            Matrix3 result;
            CreateTranslation(x, y, out result);
            return result;
        }

        /// <summary>
        /// Creates a translation matrix.
        /// </summary>
        /// <param name="vector">The translation vector.</param>
        /// <returns>The resulting Matrix3 instance.</returns>
        public static Matrix3 CreateTranslation(Vector2 vector)
        {
            Matrix3 result;
            CreateTranslation(vector.X, vector.Y, out result);
            return result;
        }

        #endregion

        #region Scale Functions

        /// <summary>
        /// Build a scaling matrix
        /// </summary>
        /// <param name="scale">Single scale factor for x,y and z axes</param>
        /// <returns>A scaling matrix</returns>
        public static Matrix3 Scale(float scale)
        {
            return Scale(scale, scale, scale);
        }

        /// <summary>
        /// Build a scaling matrix
        /// </summary>
        /// <param name="scale">Scale factors for x,y and z axes</param>
        /// <returns>A scaling matrix</returns>
        public static Matrix3 Scale(Vector3 scale)
        {
            return Scale(scale.X, scale.Y, scale.Z);
        }
		
		/// <summary>
        /// Build a scaling matrix
        /// </summary>
        /// <param name="scale">Scale factors for x and y axes</param>
        /// <returns>A scaling matrix</returns>
        public static Matrix3 Scale(Vector2 scale)
        {
            return Scale(scale.X, scale.Y, 1);
        }

        /// <summary>
        /// Build a scaling matrix
        /// </summary>
        /// <param name="x">Scale factor for x-axis</param>
        /// <param name="y">Scale factor for y-axis</param>
        /// <param name="z">Scale factor for z-axis</param>
        /// <returns>A scaling matrix</returns>
        public static Matrix3 Scale(float x, float y, float z)
        {
            Matrix3 result;
            result.Row0 = Vector3.UnitX * x;
            result.Row1 = Vector3.UnitY * y;
            result.Row2 = Vector3.UnitZ * z;
            return result;
        }

        #endregion

        #region Rotation Functions

        /// <summary>
        /// Build a rotation matrix from a quaternion
        /// </summary>
        /// <param name="q">the quaternion</param>
        /// <returns>A rotation matrix</returns>
        public static Matrix3 Rotate(Quaternion q)
        {
            Vector3 axis;
            float angle;
            q.ToAxisAngle(out axis, out angle);
            return CreateFromAxisAngle(axis, angle);
        }

        #endregion

        #region Multiply Functions

        /// <summary>
        /// Multiplies two instances.
        /// </summary>
        /// <param name="left">The left operand of the multiplication.</param>
        /// <param name="right">The right operand of the multiplication.</param>
        /// <returns>A new instance that is the result of the multiplication</returns>
        public static Matrix3 Mult(Matrix3 left, Matrix3 right)
        {
            Matrix3 result;
            Mult(ref left, ref right, out result);
            return result;
        }

        /// <summary>
        /// Multiplies two instances.
        /// </summary>
        /// <param name="left">The left operand of the multiplication.</param>
        /// <param name="right">The right operand of the multiplication.</param>
        /// <param name="result">A new instance that is the result of the multiplication</param>
        public static void Mult(ref Matrix3 left, ref Matrix3 right, out Matrix3 result)
        {
            float lM11 = left.Row0.X, lM12 = left.Row0.Y, lM13 = left.Row0.Z,
                lM21 = left.Row1.X, lM22 = left.Row1.Y, lM23 = left.Row1.Z,
                lM31 = left.Row2.X, lM32 = left.Row2.Y, lM33 = left.Row2.Z,
                rM11 = right.Row0.X, rM12 = right.Row0.Y, rM13 = right.Row0.Z,
                rM21 = right.Row1.X, rM22 = right.Row1.Y, rM23 = right.Row1.Z,
                rM31 = right.Row2.X, rM32 = right.Row2.Y, rM33 = right.Row2.Z;

            result.Row0.X = (((lM11 * rM11) + (lM12 * rM21)) + (lM13 * rM31));
            result.Row0.Y = (((lM11 * rM12) + (lM12 * rM22)) + (lM13 * rM32));
            result.Row0.Z = (((lM11 * rM13) + (lM12 * rM23)) + (lM13 * rM33));
            result.Row1.X = (((lM21 * rM11) + (lM22 * rM21)) + (lM23 * rM31));
            result.Row1.Y = (((lM21 * rM12) + (lM22 * rM22)) + (lM23 * rM32));
            result.Row1.Z = (((lM21 * rM13) + (lM22 * rM23)) + (lM23 * rM33));
            result.Row2.X = (((lM31 * rM11) + (lM32 * rM21)) + (lM33 * rM31));
            result.Row2.Y = (((lM31 * rM12) + (lM32 * rM22)) + (lM33 * rM32));
            result.Row2.Z = (((lM31 * rM13) + (lM32 * rM23)) + (lM33 * rM33));
        }
		
		/// <summary>
        /// Multiplies a Matrix3 with a Vector3.
        /// </summary>
        /// <param name="left">The left operand of the multiplication.</param>
        /// <param name="right">The right operand of the multiplication.</param>
        /// <return name="result">A new Vector3 that is the result of the multiplication</return>
		public static Vector3 Mult(Matrix3 left, Vector3 right) {
			Vector3 result;
			Mult (ref left, ref right, out result);
			return result;
		}
		
		/// <summary>
        /// Multiplies a Matrix3 with a Vector3.
        /// </summary>
        /// <param name="left">The left operand of the multiplication.</param>
        /// <param name="right">The right operand of the multiplication.</param>
        /// <param name="result">A new Vector3 that is the result of the multiplication</param>
		public static void Mult(ref Matrix3 left, ref Vector3 right, out Vector3 result) {
			
			result.X = left.M11 * right.X + left.M21 * right.Y + left.M31 * right.Z;
			result.Y = left.M12 * right.X + left.M22 * right.Y + left.M32 * right.Z;
			result.Z = left.M13 * right.X + left.M23 * right.Y + left.M33 * right.Z;
		}

        #endregion

        #region Invert Functions

        /// <summary>
        /// Not implemented!
        /// 
        /// Calculate the inverse of the given matrix
        /// </summary>
        /// <param name="mat">The matrix to invert</param>
        /// <returns>The inverse of the given matrix if it has one, or the input if it is singular</returns>
        /// <exception cref="InvalidOperationException">Thrown if the Matrix4 is singular.</exception>
        public static Matrix3 Invert(Matrix3 mat)
        {
			//see Matrix4 for an implementation
			throw new NotImplementedException();
        }

        #endregion

        #region Transpose

        /// <summary>
        /// Calculate the transpose of the given matrix
        /// </summary>
        /// <param name="mat">The matrix to transpose</param>
        /// <returns>The transpose of the given matrix</returns>
        public static Matrix3 Transpose(Matrix3 mat)
        {
            return new Matrix3(mat.Column0, mat.Column1, mat.Column2);
        }


        /// <summary>
        /// Calculate the transpose of the given matrix
        /// </summary>
        /// <param name="mat">The matrix to transpose</param>
        /// <param name="result">The result of the calculation</param>
        public static void Transpose(ref Matrix3 mat, out Matrix3 result)
        {
            result.Row0 = mat.Column0;
            result.Row1 = mat.Column1;
            result.Row2 = mat.Column2;
        }

        #endregion

        #endregion

        #region Operators

        /// <summary>
        /// Matrix multiplication
        /// </summary>
        /// <param name="left">left-hand operand</param>
        /// <param name="right">right-hand operand</param>
        /// <returns>A new Matrix33 which holds the result of the multiplication</returns>
        public static Matrix3 operator *(Matrix3 left, Matrix3 right)
        {
            return Matrix3.Mult(left, right);
        }
		
		/// <summary>
        /// Matrix - Vector multiplication
        /// </summary>
        /// <param name="left">left-hand operand</param>
        /// <param name="right">right-hand operand</param>
        /// <returns>A new Vector3 which holds the result of the multiplication</returns>
		public static Vector3 operator *(Matrix3 left, Vector3 right)
		{
			return Matrix3.Mult(left, right);
		}

        /// <summary>
        /// Compares two instances for equality.
        /// </summary>
        /// <param name="left">The first instance.</param>
        /// <param name="right">The second instance.</param>
        /// <returns>True, if left equals right; false otherwise.</returns>
        public static bool operator ==(Matrix3 left, Matrix3 right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Compares two instances for inequality.
        /// </summary>
        /// <param name="left">The first instance.</param>
        /// <param name="right">The second instance.</param>
        /// <returns>True, if left does not equal right; false otherwise.</returns>
        public static bool operator !=(Matrix3 left, Matrix3 right)
        {
            return !left.Equals(right);
        }

        #endregion

        #region Overrides

        #region public override string ToString()

        /// <summary>
        /// Returns a System.String that represents the current Matrix33.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return String.Format("{0}\n{1}\n{2}", Row0, Row1, Row2);
        }

        #endregion

        #region public override int GetHashCode()

        /// <summary>
        /// Returns the hashcode for this instance.
        /// </summary>
        /// <returns>A System.Int32 containing the unique hashcode for this instance.</returns>
        public override int GetHashCode()
        {
            return Row0.GetHashCode() ^ Row1.GetHashCode() ^ Row2.GetHashCode();
        }

        #endregion

        #region public override bool Equals(object obj)

        /// <summary>
        /// Indicates whether this instance and a specified object are equal.
        /// </summary>
        /// <param name="obj">The object to compare tresult.</param>
        /// <returns>True if the instances are equal; false otherwise.</returns>
        public override bool Equals(object obj)
        {
            if (!(obj is Matrix3))
                return false;

            return this.Equals((Matrix4)obj);
        }

        #endregion

        #endregion

        #endregion

        #region IEquatable<Matrix4> Members

        /// <summary>Indicates whether the current matrix is equal to another matrix.</summary>
        /// <param name="other">An matrix to compare with this matrix.</param>
        /// <returns>true if the current matrix is equal to the matrix parameter; otherwise, false.</returns>
        public bool Equals(Matrix3 other)
        {
            return
                Row0 == other.Row0 &&
                Row1 == other.Row1 &&
                Row2 == other.Row2;
        }

        #endregion
    }
}