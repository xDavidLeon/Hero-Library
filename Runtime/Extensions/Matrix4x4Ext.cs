using UnityEngine;

namespace HeroLib
{
    /// <summary>
    /// Extension for the standard Vector3 that allows us to add functions
    /// </summary>
    public static partial class Matrix4x4Ext
    {
        /// <summary>
        /// Return the position of the matrix
        /// </summary>
        public static Vector3 Position(this Matrix4x4 rMatrix)
        {
            return rMatrix.GetColumn(3);
        }

        /// <summary>
        /// Return the rotation of the matrix
        /// </summary>
        public static Quaternion Rotation(this Matrix4x4 rMatrix)
        {
            return Quaternion.LookRotation(rMatrix.GetColumn(2), rMatrix.GetColumn(1));
        }

        /// <summary>
        /// Return the scale of the matrix
        /// </summary>
        public static Vector3 Scale(this Matrix4x4 rMatrix)
        {
            return new Vector3(rMatrix.GetColumn(0).magnitude, rMatrix.GetColumn(1).magnitude,
                rMatrix.GetColumn(2).magnitude);
        }
        
        //Gets a quaternion from a matrix
        public static Quaternion QuaternionFromMatrix(this Matrix4x4 m)
        {
            return Quaternion.LookRotation(m.GetColumn(2), m.GetColumn(1));
        }

        //Gets a position from a matrix
        public static Vector3 PositionFromMatrix(this Matrix4x4 m)
        {
            Vector4 vector4Position = m.GetColumn(3);
            return new Vector3(vector4Position.x, vector4Position.y, vector4Position.z);
        }

    }
}