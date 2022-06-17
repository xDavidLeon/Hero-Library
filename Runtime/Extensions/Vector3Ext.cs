using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace HeroLib
{
    /// <summary>
    /// Extension for the standard Vector3 that allows us to add functions
    /// </summary>
    public static class Vector3Ext
    {
        /// <summary>
        /// Static constructor
        /// </summary>
        static Vector3Ext()
        {
            // Force the culture on the thread. This will help ensure we don't have issues
            // as we serialize and deserialize data
            //System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US");
        }

        /// <summary>
        /// Used when we need to return an empty vector
        /// </summary>
        public static Vector3 Null = new Vector3(float.MinValue, float.MinValue, float.MinValue);

        /// <summary>
        /// Used to determine the angle of two vectors in relation to 
        /// the bone. This is important since the 'forward' direction of the
        /// bone can change
        /// </summary>
        /// <param name="rBone"></param>
        /// <param name="rStart"></param>
        /// <param name="rEnd"></param>
        /// <returns></returns>
        public static float SignedAngle(Vector3 rFrom, Vector3 rTo, Vector3 rAxis)
        {
            if (rTo == rFrom)
            {
                return 0f;
            }

            Vector3 lCross = Vector3.Cross(rFrom, rTo);
            float lDot = Vector3.Dot(rFrom, rTo);
            float lSign = (Vector3.Dot(rAxis, lCross) < 0 ? -1 : 1);

            return lSign * Mathf.Atan2(lCross.magnitude, lDot) * Mathf.Rad2Deg;
        }

        /// <summary>
        /// Determines the signed angle required to go from one vector to another.
        /// </summary>
        /// <param name="rThis">The source vector</param>
        /// <param name="rTarget">The destination vector</param>
        /// <returns></returns>
        public static float SignedAngle(Vector3 rFrom, Vector3 rTo)
        {
            if (rTo == rFrom)
            {
                return 0f;
            }

            Vector3 lCross = Vector3.Cross(rFrom, rTo);
            float lSign = (lCross.y < 0 ? -1 : 1);

            float lDot = Vector3.Dot(rFrom, rTo);

            return lSign * Mathf.Atan2(lCross.magnitude, lDot) * Mathf.Rad2Deg;
        }

        /// <summary>
        /// Determines the signed angle required to go from one vector to another.
        /// </summary>
        /// <param name="rThis">The source vector</param>
        /// <param name="rTarget">The destination vector</param>
        /// <returns></returns>
        public static float AngleTo(this Vector3 rFrom, Vector3 rTo)
        {
            return Vector3Ext.SignedAngle(rFrom, rTo);
        }

        /// <summary>
        /// Extract out the yaw and pitch required to get us from one vector direction to another. Note
        /// that we may need to account for the object's rotation.
        /// </summary>
        /// <param name="rFrom"></param>
        /// <param name="rTo"></param>
        /// <param name="rYaw"></param>
        /// <param name="rPitch"></param>
        public static void DecomposeYawPitch(Transform rOwner, Vector3 rFrom, Vector3 rTo, ref float rYaw,
            ref float rPitch)
        {
            // Determine the rotations required to view the target
            Vector3 lDelta = rTo - rFrom;
            rPitch =
                (-Mathf.Atan2(lDelta.y, Mathf.Sqrt((lDelta.x * lDelta.x) + (lDelta.z * lDelta.z))) * Mathf.Rad2Deg) +
                rOwner.rotation.eulerAngles.x;
            rYaw = (-Mathf.Atan2(lDelta.z, lDelta.x) * Mathf.Rad2Deg) + 90f - rOwner.rotation.eulerAngles.y;
        }

        /// <summary>
        /// Search the dictionary based on value and return the key
        /// </summary>
        /// <typeparam name="TKey">Key type</typeparam>
        /// <typeparam name="TValue">Value type</typeparam>
        /// <param name="rDictionary">Object the extension is tied to</param>
        /// <param name="rValue">Value that we are searching for</param>
        /// <returns>Returns the first key associated with the value</returns>
        public static float HorizontalMagnitude(this Vector3 rVector)
        {
            return Mathf.Sqrt((rVector.x * rVector.x) + (rVector.z * rVector.z));
        }

        /// <summary>
        /// Search the dictionary based on value and return the key
        /// </summary>
        /// <typeparam name="TKey">Key type</typeparam>
        /// <typeparam name="TValue">Value type</typeparam>
        /// <param name="rDictionary">Object the extension is tied to</param>
        /// <param name="rValue">Value that we are searching for</param>
        /// <returns>Returns the first key associated with the value</returns>
        public static float HorizontalSqrMagnitude(this Vector3 rVector)
        {
            return (rVector.x * rVector.x) + (rVector.z * rVector.z);
        }

        /// <summary>
        /// Gets the angle required to reach the target direction vector
        /// </summary>
        /// <returns>The signed horizontal angle (in degrees).</returns>
        /// <param name="rFrom">Starting direction vector</param>
        /// <param name="rTo">Resulting direction vector</param>
        /// <remarks>
        /// In unity:
        /// Rotation angle is posative when going clockwise
        /// Rotation angle is negative when going counter-clockwise
        /// 
        /// When using trig functions:
        /// Rotation angle is negative when going clockwise
        /// Rotation angle is posative when going counter-clockwise
        /// 
        ///   0 angle is to the right (+x)
        /// -90 angle is to the back (-z)
        /// 180 angle is to the left (-x)
        ///  90 angle is to the forward (+z)
        /// </remarks>
        public static float HorizontalAngleTo(this Vector3 rFrom, Vector3 rTo)
        {
            float lAngle = Mathf.Atan2(Vector3.Dot(Vector3.up, Vector3.Cross(rFrom, rTo)), Vector3.Dot(rFrom, rTo));
            lAngle *= Mathf.Rad2Deg;

            if (Mathf.Abs(lAngle) < 0.0001)
            {
                lAngle = 0f;
            }

            return lAngle;
        }

        /// <summary>
        /// Gets the angle required to reach the target direction vector
        /// </summary>
        /// <returns>The signed horizontal angle (in degrees).</returns>
        /// <param name="rFrom">Starting direction vector</param>
        /// <param name="rTo">Resulting direction vector</param>
        /// <remarks>
        /// In unity:
        /// Rotation angle is posative when going clockwise
        /// Rotation angle is negative when going counter-clockwise
        /// 
        /// When using trig functions:
        /// Rotation angle is negative when going clockwise
        /// Rotation angle is posative when going counter-clockwise
        /// 
        ///   0 angle is to the right (+x)
        /// -90 angle is to the back (-z)
        /// 180 angle is to the left (-x)
        ///  90 angle is to the forward (+z)
        /// </remarks>
        public static float VerticalAngleTo(this Vector3 rFrom, Vector3 rTo)
        {
            float lAngle = Mathf.Atan2(Vector3.Dot(Vector3.right, Vector3.Cross(rFrom, rTo)), Vector3.Dot(rFrom, rTo));
            lAngle *= Mathf.Rad2Deg;

            if (Mathf.Abs(lAngle) < 0.0001)
            {
                lAngle = 0f;
            }

            return lAngle;
        }

        /// <summary>
        /// Gets the angle required to reach the target direction vector
        /// </summary>
        /// <returns>The signed horizontal angle (in degrees).</returns>
        /// <param name="rFrom">Starting direction vector</param>
        /// <param name="rTo">Resulting direction vector</param>
        /// <remarks>
        /// In unity:
        /// Rotation angle is posative when going clockwise
        /// Rotation angle is negative when going counter-clockwise
        /// 
        /// When using trig functions:
        /// Rotation angle is negative when going clockwise
        /// Rotation angle is posative when going counter-clockwise
        /// 
        ///   0 angle is to the right (+x)
        /// -90 angle is to the back (-z)
        /// 180 angle is to the left (-x)
        ///  90 angle is to the forward (+z)
        /// </remarks>
        public static float HorizontalAngleTo(this Vector3 rFrom, Vector3 rTo, Vector3 rUp)
        {
            float lAngle = Mathf.Atan2(Vector3.Dot(rUp, Vector3.Cross(rFrom, rTo)), Vector3.Dot(rFrom, rTo));
            lAngle *= Mathf.Rad2Deg;

            if (Mathf.Abs(lAngle) < 0.0001)
            {
                lAngle = 0f;
            }

            return lAngle;
        }

        /// <summary>
        /// Gets the angle required to reach this direction vector
        /// </summary>
        /// <returns>The signed horizontal angle (in degrees).</returns>
        /// <param name="rTo">Resulting direction vector</param>
        /// <param name="rFrom">Starting direction vector</param>
        public static float HorizontalAngleFrom(this Vector3 rTo, Vector3 rFrom)
        {
            float lAngle = Mathf.Atan2(Vector3.Dot(Vector3.up, Vector3.Cross(rFrom, rTo)), Vector3.Dot(rFrom, rTo));
            lAngle *= Mathf.Rad2Deg;

            if (Mathf.Abs(lAngle) < 0.0001)
            {
                lAngle = 0f;
            }

            return lAngle;
        }

        /// <summary>
        /// Find the distance to the specified vector given a specific amount of vertical tolerance 
        /// to remove. In this way, we can get rid of any reasonable height differences.
        /// </summary>
        /// <param name="rFrom">Current position</param>
        /// <param name="rTo">Position we're measuring to</param>
        /// <param name="rYTolerance">Amount to reduce the Y diffence by</param>
        /// <returns></returns>
        public static float DistanceTo(this Vector3 rFrom, Vector3 rTo, float rYTolerance)
        {
            float lDiffY = rTo.y - rFrom.y;
            if (lDiffY > 0)
            {
                lDiffY = Mathf.Max(lDiffY - rYTolerance, 0f);
            }
            else if (lDiffY < 0)
            {
                lDiffY = Mathf.Min(lDiffY + rYTolerance, 0f);
            }

            rTo.y = rFrom.y + lDiffY;
            return Vector3.Distance(rFrom, rTo);
        }

        /// <summary>
        /// Returns a normalized vector that represents the direction required to
        /// get from the 'From' position to the 'To' position..
        /// </summary>
        /// <param name="rFrom">Originating position</param>
        /// <param name="rTo">Target position we're heading to</param>
        /// <returns>Normalized vector direction</returns>
        public static Vector3 DirectionTo(this Vector3 rFrom, Vector3 rTo)
        {
            Vector3 lDifference = rTo - rFrom;
            return lDifference.normalized;
        }

        /// <summary>
        /// Normalizes the rotational values from -180 to 180. This is important for things
        /// like averaging where 0-to-360 gives different values than -180-to-180. For example:
        // (-10 + 10) / 2 = 0 = what we're expecting
        // (350 + 10) / 2 = 180 = not what we're expecting 
        /// </summary>
        /// <param name="rSum"></param>
        /// <returns></returns>
        public static Vector3 NormalizeRotations(this Vector3 rThis)
        {
            Vector3 lResult = rThis;

            rThis.x = (rThis.x < -180 ? rThis.x + 360f : (rThis.x > 180f ? rThis.x - 360f : rThis.x));
            rThis.y = (rThis.y < -180 ? rThis.y + 360f : (rThis.y > 180f ? rThis.y - 360f : rThis.y));
            rThis.z = (rThis.z < -180 ? rThis.z + 360f : (rThis.z > 180f ? rThis.z - 360f : rThis.z));

            return lResult;
        }

        /// <summary>
        /// Add angular rotations (pitch, yaw, and roll). We add them from a range
        /// of (-180 to 180). This way a rotation of -10 + 10 will cancel each other out.
        /// </summary>
        /// <param name="rFrom">Originating position</param>
        /// <param name="rTo">Target position we're heading to</param>
        /// <returns>Normalized vector direction</returns>
        public static Vector3 AddRotation(this Vector3 rFrom, Vector3 rTo)
        {
            Vector3 lResult = rFrom;

            //rFrom.x = (rFrom.x < -180 ? rFrom.x + 360f : (rFrom.x > 180f ? rFrom.x - 360f : rFrom.x));
            //rFrom.y = (rFrom.y < -180 ? rFrom.y + 360f : (rFrom.y > 180f ? rFrom.y - 360f : rFrom.y));
            //rFrom.z = (rFrom.z < -180 ? rFrom.z + 360f : (rFrom.z > 180f ? rFrom.z - 360f : rFrom.z));

            //rTo.x = (rTo.x < -180 ? rTo.x + 360f : (rTo.x > 180f ? rTo.x - 360f : rTo.x));
            //rTo.y = (rTo.y < -180 ? rTo.y + 360f : (rTo.y > 180f ? rTo.y - 360f : rTo.y));
            //rTo.z = (rTo.z < -180 ? rTo.z + 360f : (rTo.z > 180f ? rTo.z - 360f : rTo.z));

            lResult = lResult + rTo;

            return lResult;
        }

        /// <summary>
        /// Add angular rotations (pitch, yaw, and roll). We add them from a range
        /// of (-180 to 180). This way a rotation of -10 + 10 will cancel each other out.
        /// </summary>
        /// <param name="rFrom">Originating position</param>
        /// <param name="rTo">Target position we're heading to</param>
        /// <returns>Normalized vector direction</returns>
        public static Vector3 AddRotation(this Vector3 rFrom, float rX, float rY, float rZ)
        {
            Vector3 lResult = rFrom;

            //rFrom.x = (rFrom.x < -180 ? rFrom.x + 360f : (rFrom.x > 180f ? rFrom.x - 360f : rFrom.x));
            //rFrom.y = (rFrom.y < -180 ? rFrom.y + 360f : (rFrom.y > 180f ? rFrom.y - 360f : rFrom.y));
            //rFrom.z = (rFrom.z < -180 ? rFrom.z + 360f : (rFrom.z > 180f ? rFrom.z - 360f : rFrom.z));

            //rX = (rX < -180 ? rX + 360f : (rX > 180f ? rX - 360f : rX));
            //rY = (rY < -180 ? rY + 360f : (rY > 180f ? rY - 360f : rY));
            //rZ = (rZ < -180 ? rZ + 360f : (rZ > 180f ? rZ - 360f : rZ));

            lResult.x = lResult.x + rX;
            lResult.y = lResult.y + rY;
            lResult.z = lResult.z + rZ;

            return lResult;
        }

        /// <summary>
        /// Find the two vectors that are orthogonal to the normal. These vectors
        /// can be used to define the plane the original vector is the normal of.        /// 
        /// </summary>
        /// <param name="rNormal"></param>
        /// <param name="rOrthoUp"></param>
        /// <param name="rOrthoRight"></param>
        public static void FindOrthogonals(Vector3 rNormal, ref Vector3 rOrthoUp, ref Vector3 rOrthoRight)
        {
            rNormal.Normalize();

            rOrthoRight = Quaternion.AngleAxis(90, Vector3.right) * rNormal;
            if (Mathf.Abs(Vector3.Dot(rNormal, rOrthoRight)) > 0.6f)
            {
                rOrthoRight = Quaternion.AngleAxis(90, Vector3.up) * rNormal;
            }

            rOrthoRight.Normalize();

            rOrthoRight = Vector3.Cross(rNormal, rOrthoRight).normalized;
            rOrthoUp = Vector3.Cross(rNormal, rOrthoRight).normalized;
        }

        //Convert a plane defined by 3 points to a plane defined by a vector and a point. 
        //The plane point is the middle of the triangle defined by the 3 points.
        /// <summary>
        /// 
        /// </summary>
        /// <param name="rVertexA"></param>
        /// <param name="rVertexB"></param>
        /// <param name="rVertexC"></param>
        /// <returns></returns>
        public static Vector3 PlaneNormal(Vector3 rVertexA, Vector3 rVertexB, Vector3 rVertexC)
        {
            // Make two vectors from the 3 input points, originating from point A
            Vector3 lAB = rVertexB - rVertexA;
            Vector3 lAC = rVertexC - rVertexA;

            //Calculate the normal
            return Vector3.Cross(lAC, lAB).normalized;
        }

        /// <summary>
        /// Convert a plane defined by 3 points to a plane defined by a vector and a point. 
        /// The plane point is the middle of the triangle defined by the 3 points.
        /// </summary>
        /// <param name="planeNormal"></param>
        /// <param name="planePoint"></param>
        /// <param name="pointA"></param>
        /// <param name="pointB"></param>
        /// <param name="pointC"></param>
        public static void PlaneFrom3Points(out Vector3 planeNormal, out Vector3 planePoint, Vector3 pointA,
            Vector3 pointB, Vector3 pointC)
        {
            planeNormal = Vector3.zero;
            planePoint = Vector3.zero;

            //Make two vectors from the 3 input points, originating from point A
            Vector3 AB = pointB - pointA;
            Vector3 AC = pointC - pointA;

            //Calculate the normal
            planeNormal = Vector3.Normalize(Vector3.Cross(AB, AC));

            //Get the points in the middle AB and AC
            Vector3 middleAB = pointA + (AB / 2.0f);
            Vector3 middleAC = pointA + (AC / 2.0f);

            //Get vectors from the middle of AB and AC to the point which is not on that line.
            Vector3 middleABtoC = pointC - middleAB;
            Vector3 middleACtoB = pointB - middleAC;

            //Calculate the intersection between the two lines. This will be the center 
            //of the triangle defined by the 3 points.
            //We could use LineLineIntersection instead of ClosestPointsOnTwoLines but due to rounding errors 
            //this sometimes doesn't work.
            Vector3 temp;
            ClosestPointsOnTwoLines(out planePoint, out temp, middleAB, middleABtoC, middleAC, middleACtoB);
        }

        //Two non-parallel lines which may or may not touch each other have a point on each line which are closest
        //to each other. This function finds those two points. If the lines are not parallel, the function 
        //outputs true, otherwise false.
        public static bool ClosestPointsOnTwoLines(out Vector3 closestPointLine1, out Vector3 closestPointLine2,
            Vector3 linePoint1, Vector3 lineVec1, Vector3 linePoint2, Vector3 lineVec2)
        {
            closestPointLine1 = Vector3.zero;
            closestPointLine2 = Vector3.zero;

            float a = Vector3.Dot(lineVec1, lineVec1);
            float b = Vector3.Dot(lineVec1, lineVec2);
            float e = Vector3.Dot(lineVec2, lineVec2);

            float d = a * e - b * b;

            //lines are not parallel
            if (d != 0.0f)
            {
                Vector3 r = linePoint1 - linePoint2;
                float c = Vector3.Dot(lineVec1, r);
                float f = Vector3.Dot(lineVec2, r);

                float s = (b * f - c * e) / d;
                float t = (a * f - c * b) / d;

                closestPointLine1 = linePoint1 + lineVec1 * s;
                closestPointLine2 = linePoint2 + lineVec2 * t;

                return true;
            }

            else
            {
                return false;
            }
        }

        /// <summary>
        /// Linearly moves the value to the target.
        /// </summary>
        /// <param name="current"></param>
        /// <param name="target"></param>
        /// <param name="currentVelocity"></param>
        /// <param name="smoothTime"></param>
        /// <param name="maxSpeed"></param>
        /// <param name="deltaTime"></param>
        /// <returns></returns>
        public static Vector3 MoveTo(Vector3 rValue, Vector3 rTarget, float rVelocity, float rDeltaTime)
        {
            if (rValue == rTarget)
            {
                return rTarget;
            }

            Vector3 lStep = (rTarget - rValue).normalized * rVelocity;
            Vector3 lNewValue = rValue + (lStep * rDeltaTime);

            if (lNewValue.sqrMagnitude > rTarget.sqrMagnitude)
            {
                return rTarget;
            }

            return lNewValue;
        }

        /// <summary>
        /// Parses out the vector values given a string
        /// </summary>
        /// <param name="rThis">Vector we are filling</param>
        /// <param name="rString">String containing the vector values. In the form of "(0,0)"</param>
        public static Vector2 FromString(this Vector2 rThis, string rString)
        {
            string[] lTemp = rString.Substring(1, rString.Length - 2).Split(',');
            if (lTemp.Length != 2)
            {
                return rThis;
            }

            rThis.x = float.Parse(lTemp[0]);
            rThis.y = float.Parse(lTemp[1]);
            return rThis;
        }

        /// <summary>
        /// Parses out the vector values given a string
        /// </summary>
        /// <param name="rThis">Vector we are filling</param>
        /// <param name="rString">String containing the vector values. In the form of "(0,0,0)"</param>
        public static Vector3 FromString(this Vector3 rThis, string rString)
        {
            string[] lTemp = rString.Substring(1, rString.Length - 2).Split(',');
            if (lTemp.Length != 3)
            {
                return rThis;
            }

            rThis.x = float.Parse(lTemp[0]);
            rThis.y = float.Parse(lTemp[1]);
            rThis.z = float.Parse(lTemp[2]);
            return rThis;
        }

        /// <summary>
        /// Parses out the vector values given a string
        /// </summary>
        /// <param name="rThis">Vector we are filling</param>
        /// <param name="rString">String containing the vector values. In the form of "(0,0,0)"</param>
        public static Vector4 FromString(this Vector4 rThis, string rString)
        {
            string[] lTemp = rString.Substring(1, rString.Length - 2).Split(',');
            if (lTemp.Length != 4)
            {
                return rThis;
            }

            rThis.x = float.Parse(lTemp[0]);
            rThis.y = float.Parse(lTemp[1]);
            rThis.z = float.Parse(lTemp[2]);
            rThis.w = float.Parse(lTemp[3]);
            return rThis;
        }

        public static Vector3 ProjectToXZPlane(Vector3 rThis, bool normalize = true)
        {
            rThis.y = 0f;
            if (normalize)
                rThis.Normalize();
            return rThis;
        }

        public static Vector3 Scale(this Vector3 vector, float x, float y, float z)
        {
            return Vector3.Scale(vector, new Vector3(x, y, z));
        }

        public static float SqrMagnitude2D(this Vector3 rThis)
        {
            return rThis.x * rThis.x + rThis.y * rThis.y;
        }

        public static bool SqrMagnitudeLessThan(this Vector3 rThis, float magnitude)
        {
            return rThis.sqrMagnitude < magnitude * magnitude;
        }

        /// <summary>
        /// Vector dot product
        /// </summary>
        public static float Dot(this Vector3 rThis, Vector3 rTarget)
        {
            return (rThis.x * rTarget.x) + (rThis.y * rTarget.y) + (rThis.z * rTarget.z);
        }

        /// <summary>
        /// Returns the smooth and eased value over time (0 to 1)
        /// </summary>
        /// <param name="rStart"></param>
        /// <param name="rEnd"></param>
        /// <param name="rTime"></param>
        /// <returns></returns>
        public static Vector3 SmoothStep(Vector3 rStart, Vector3 rEnd, float rTime)
        {
            if (rTime <= 0f)
            {
                return rStart;
            }

            if (rTime >= 1f)
            {
                return rEnd;
            }

            rTime = rTime * rTime * rTime * (rTime * (6f * rTime - 15f) + 10f);

            Vector3 lDelta = rEnd - rStart;
            float lDistance = lDelta.magnitude * rTime;

            return rStart + (lDelta.normalized * lDistance);
        }

        public static bool CloserThanDistance(Vector3 p1, Vector3 p2, float d)
        {
            float dist = (p1 - p2).sqrMagnitude;
            return dist < d * d;
        }

        public static Vector3 GetXZDirection(this Vector3 v, bool normalized = true)
        {
            Vector3 r = Vector3.Scale(v, new Vector3(1, 0, 1));
            if (normalized) r.Normalize();

            return r;
        }

        public static Vector3 GetXZDirection(Vector3 a, Vector3 b, bool normalized = true)
        {
            Vector3 dir = (b - a);
            dir.y = 0f;
            if (normalized)
                dir.Normalize();
            return dir;
        }

        public static bool CloserThanDistance2D(Vector3 p1, Vector3 p2, float d)
        {
            Vector3 p1_2D = p1;
            p1_2D.y = 0;
            Vector3 p2_2D = p2;
            p2_2D.y = 0;

            return CloserThanDistance(p1_2D, p2_2D, d);
        }

        public static Vector3 RoundPos(Vector3 pos)
        {
            Vector3 roundedPos = pos;
            roundedPos.x = Mathf.Round(roundedPos.x);
            roundedPos.y = Mathf.Round(roundedPos.y);
            roundedPos.z = Mathf.Round(roundedPos.z);

            return roundedPos;
        }

        public static Vector3 Randomize(this Vector3 v, Vector3 factor)
        {
            return v.Randomize(factor.x, factor.y, factor.z);
        }

        public static Vector3 Randomize(this Vector3 v, float xFactor, float yFactor, float zFactor)
        {
            v.x += Random.Range(-xFactor, xFactor);
            v.y += Random.Range(-yFactor, yFactor);
            v.x += Random.Range(-zFactor, zFactor);
            return v;
        }

        public static void RotateVector(ref Vector3 v, float angles, Vector3 axis)
        {
            v = Quaternion.AngleAxis(angles, axis) * v;
        }

        public static Vector3 RotateVector(this Vector3 v, float angles, Vector3 axis)
        {
            return Quaternion.AngleAxis(angles, axis) * v;
        }


        public static Vector3 ReplaceX(this Vector3 lhs, float val)
        {
            lhs.x = val;
            return lhs;
        }

        public static Vector3 ReplaceY(this Vector3 lhs, float val)
        {
            lhs.y = val;
            return lhs;
        }

        public static Vector3 ReplaceZ(this Vector3 lhs, float val)
        {
            lhs.z = val;
            return lhs;
        }

        //increase or decrease the length of vector by size
        public static Vector3 AddVectorLength(this Vector3 vector, float size)
        {
            //get the vector length
            float magnitude = Vector3.Magnitude(vector);

            //change the length
            magnitude += size;

            //normalize the vector
            Vector3 vectorNormalized = Vector3.Normalize(vector);

            //scale the vector
            return Vector3.Scale(vectorNormalized, new Vector3(magnitude, magnitude, magnitude));
        }

        //create a vector of direction "vector" with length "size"
        public static Vector3 SetVectorLength(this Vector3 vector, float size)
        {
            //normalize the vector
            Vector3 vectorNormalized = Vector3.Normalize(vector);

            //scale the vector
            return vectorNormalized *= size;
        }

        public static bool IsPointWithinCollider(this Vector3 point, Collider c)
        {
            return Vector3Ext.CloserThanDistance(c.ClosestPoint(point), point, Mathf.Epsilon);
        }

        static public float GetDistanceSummatory(this Vector3[] corners)
        {
            float length = 0f;

            for (int i = 1; i < corners.Length; i++)
            {
                if (corners[i] == Vector3.zero)
                    break;

                length += (corners[i - 1] - corners[i]).magnitude;
            }

            return length;
        }

        static public bool IsPointOnScreen(this Vector3 point, Camera cam)
        {
            Vector3 p = cam.WorldToViewportPoint(point);
            if (p.z < 0) return false;
            if (p.x < 0 || p.x > 1) return false;
            if (p.y < 0 || p.y > 1) return false;
            return true;
        }

        static public Vector3 GetLastPathCorner(this Vector3[] values)
        {
            if (values.Length == 0)
            {
                Debug.LogError(
                    "Vector3Ext GetLastPathCorner Error, Trying to get the last value of a Array with length 0.");
                return Vector3.zero;
            }
            else if (values.Length == 1)
            {
                return values[0];
            }

            int nextIndex = 0;
            for (int i = 0; i < values.Length; i++)
            {
                nextIndex = i + 1;
                if (nextIndex == values.Length)
                    return values[i];
                else
                {
                    if (values[nextIndex] == Vector3.zero)
                        return values[i];
                }
            }

            return values[values.Length - 1];
        }

        static public void ResetArray(this Vector3[] values)
        {
            for (int i = 0; i < values.Length; i++)
            {
                values[i] = Vector3.zero;
            }
        }


        //This function calculates a signed (+ or - sign instead of being ambiguous) dot product. It is basically used
        //to figure out whether a vector is positioned to the left or right of another vector. The way this is done is
        //by calculating a vector perpendicular to one of the vectors and using that as a reference. This is because
        //the result of a dot product only has signed information when an angle is transitioning between more or less
        //then 90 degrees.
        public static float SignedDotProduct(Vector3 vectorA, Vector3 vectorB, Vector3 normal)
        {
            Vector3 perpVector;
            float dot;

            //Use the geometry object normal and one of the input vectors to calculate the perpendicular vector
            perpVector = Vector3.Cross(normal, vectorA);

            //Now calculate the dot product between the perpendicular vector (perpVector) and the other input vector
            dot = Vector3.Dot(perpVector, vectorB);

            return dot;
        }

        public static float SignedVectorAngle(Vector3 referenceVector, Vector3 otherVector, Vector3 normal)
        {
            Vector3 perpVector;
            float angle;

            //Use the geometry object normal and one of the input vectors to calculate the perpendicular vector
            perpVector = Vector3.Cross(normal, referenceVector);

            //Now calculate the dot product between the perpendicular vector (perpVector) and the other input vector
            angle = Vector3.Angle(referenceVector, otherVector);
            angle *= Mathf.Sign(Vector3.Dot(perpVector, otherVector));

            return angle;
        }

        //Calculate the angle between a vector and a plane. The plane is made by a normal vector.
        //Output is in radians.
        public static float AngleVectorPlane(Vector3 vector, Vector3 normal)
        {
            float dot;
            float angle;

            //calculate the the dot product between the two input vectors. This gives the cosine between the two vectors
            dot = Vector3.Dot(vector, normal);

            //this is in radians
            angle = (float)Math.Acos(dot);

            return 1.570796326794897f - angle; //90 degrees - angle
        }

        //Calculate the dot product as an angle
        public static float DotProductAngle(Vector3 vec1, Vector3 vec2)
        {
            double dot;
            double angle;

            //get the dot product
            dot = Vector3.Dot(vec1, vec2);

            //Clamp to prevent NaN error. Shouldn't need this in the first place, but there could be a rounding error issue.
            if (dot < -1.0f)
            {
                dot = -1.0f;
            }

            if (dot > 1.0f)
            {
                dot = 1.0f;
            }

            //Calculate the angle. The output is in radians
            //This step can be skipped for optimization...
            angle = Math.Acos(dot);

            return (float)angle;
        }

        public static void ProjectPointToPlane(Vector3 point, Plane plane, ref Vector3 projection)
        {
            float distToPlane = plane.GetDistanceToPoint(point);
            projection = point - plane.normal * distToPlane;
        }

        public static void ProjectPointToPlane(Vector3 point, Plane plane, ref Vector3 projection,
            ref float distToPlane)
        {
            distToPlane = plane.GetDistanceToPoint(point);
            projection = point - plane.normal * distToPlane;
        }

        // The angle between dirA and dirB around axis
        public static float AngleAroundAxis(Vector3 dirA, Vector3 dirB, Vector3 axis)
        {
            // Project A and B onto the plane orthogonal target axis
            dirA = dirA - Vector3.Project(dirA, axis);
            dirB = dirB - Vector3.Project(dirB, axis);

            // Find (positive) angle between A and B
            float angle = Vector3.Angle(dirA, dirB);

            // Return angle multiplied with 1 or -1
            return angle * (Vector3.Dot(axis, Vector3.Cross(dirA, dirB)) < 0 ? -1 : 1);
        }

        public static bool IsPosInsideFrustrum(this Vector3 pos, Camera cam)
        {
            Vector3 screenPos = cam.WorldToScreenPoint(pos);

            if (screenPos.z > 0f && screenPos.x > 0f && screenPos.x < Screen.width && screenPos.y > 0f &&
                screenPos.y < Screen.height)
            {
                //Point inside Screen
                float dist = Vector3.Distance(cam.transform.position, pos);
                if (dist > cam.nearClipPlane && dist < cam.farClipPlane)
                {
                    //Point inside frustrum
                    return true;
                }
            }

            return false;
        }

        #region Vector2

        /// <summary>
        /// Same as .x
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static float Min(this Vector2 v)
        {
            return v.x;
        }

        /// <summary>
        /// Same as .y
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static float Max(this Vector2 v)
        {
            return v.y;
        }

        #endregion

        #region CARDINALS

        public enum CARDINAL_DIRECTION
        {
            NONE,
            N,
            NE,
            E,
            SE,
            S,
            SW,
            W,
            NW
        };

        public static CARDINAL_DIRECTION MirrorCardinalDirection(CARDINAL_DIRECTION dir)
        {
            switch (dir)
            {
                case CARDINAL_DIRECTION.N:
                    return CARDINAL_DIRECTION.S;
                case CARDINAL_DIRECTION.NE:
                    return CARDINAL_DIRECTION.SW;
                case CARDINAL_DIRECTION.E:
                    return CARDINAL_DIRECTION.W;
                case CARDINAL_DIRECTION.SE:
                    return CARDINAL_DIRECTION.NW;
                case CARDINAL_DIRECTION.S:
                    return CARDINAL_DIRECTION.N;
                case CARDINAL_DIRECTION.SW:
                    return CARDINAL_DIRECTION.NE;
                case CARDINAL_DIRECTION.W:
                    return CARDINAL_DIRECTION.E;
                case CARDINAL_DIRECTION.NW:
                    return CARDINAL_DIRECTION.SE;
            }

            return CARDINAL_DIRECTION.NONE;
        }

        public static Vector3 CardinalToWorldDirection(CARDINAL_DIRECTION dir)
        {
            switch (dir)
            {
                case CARDINAL_DIRECTION.N:
                    return new Vector3(0, 0, 1);
                case CARDINAL_DIRECTION.NE:
                    return new Vector3(0.5f, 0, 0.5f);
                case CARDINAL_DIRECTION.E:
                    return new Vector3(1.0f, 0, 0.0f);
                case CARDINAL_DIRECTION.SE:
                    return new Vector3(0.5f, 0, -0.5f);
                case CARDINAL_DIRECTION.S:
                    return new Vector3(0.0f, 0, -1.0f);
                case CARDINAL_DIRECTION.SW:
                    return new Vector3(-0.5f, 0, -0.5f);
                case CARDINAL_DIRECTION.W:
                    return new Vector3(-1.0f, 0, 0.0f);
                case CARDINAL_DIRECTION.NW:
                    return new Vector3(-0.5f, 0, 0.5f);
            }

            return new Vector3(0, 0, 0);
        }

        public static CARDINAL_DIRECTION CardinalDirection(this Vector3 dirXZ, Vector3 north, Vector3 east)
        {
            float dotforward = Vector3.Dot(dirXZ, north);
            float dotright = Vector3.Dot(dirXZ, east);

            float absForward = Mathf.Abs(dotforward);
            float absRight = Mathf.Abs(dotright);

            if (dirXZ.magnitude <= Mathf.Epsilon) return CARDINAL_DIRECTION.NONE;

            if (dotforward >= 0.0f) // North
            {
                if (dotright >= 0.0f) // East
                {
                    if (absForward >= 0.5f && absRight >= 0.5f) return CARDINAL_DIRECTION.NE;
                    else if (absForward > absRight) return CARDINAL_DIRECTION.N;
                    else return CARDINAL_DIRECTION.E;
                }
                else // West
                {
                    if (absForward >= 0.5f && absRight >= 0.5f) return CARDINAL_DIRECTION.NW;
                    else if (absForward > absRight) return CARDINAL_DIRECTION.N;
                    else return CARDINAL_DIRECTION.W;
                }
            }
            else // South
            {
                if (dotright >= 0.0f) // East
                {
                    if (absForward >= 0.5f && absRight >= 0.5f) return CARDINAL_DIRECTION.SE;
                    else if (absForward > absRight) return CARDINAL_DIRECTION.S;
                    else return CARDINAL_DIRECTION.E;
                }
                else // West
                {
                    if (absForward >= 0.5f && absRight >= 0.5f) return CARDINAL_DIRECTION.SW;
                    else if (absForward > absRight) return CARDINAL_DIRECTION.S;
                    else return CARDINAL_DIRECTION.W;
                }
            }
        }

        public static bool IsCardinalNorth(CARDINAL_DIRECTION direction)
        {
            return direction == Vector3Ext.CARDINAL_DIRECTION.N || direction == Vector3Ext.CARDINAL_DIRECTION.NE ||
                   direction == Vector3Ext.CARDINAL_DIRECTION.NW;
        }

        public static bool IsCardinalSouth(CARDINAL_DIRECTION direction)
        {
            return direction == Vector3Ext.CARDINAL_DIRECTION.S || direction == Vector3Ext.CARDINAL_DIRECTION.SE ||
                   direction == Vector3Ext.CARDINAL_DIRECTION.SW;
        }

        public static bool IsCardinalEast(CARDINAL_DIRECTION direction)
        {
            return direction == Vector3Ext.CARDINAL_DIRECTION.E || direction == Vector3Ext.CARDINAL_DIRECTION.NE ||
                   direction == Vector3Ext.CARDINAL_DIRECTION.SE;
        }

        public static bool IsCardinalWest(CARDINAL_DIRECTION direction)
        {
            return direction == Vector3Ext.CARDINAL_DIRECTION.W || direction == Vector3Ext.CARDINAL_DIRECTION.NW ||
                   direction == Vector3Ext.CARDINAL_DIRECTION.SW;
        }

        #endregion
    }
}