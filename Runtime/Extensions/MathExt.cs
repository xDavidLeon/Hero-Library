using UnityEngine;

namespace HeroLib
{
    public class Line
    {
        public Vector3 Start;
        public Vector3 End;

        public float Length()
        {
            return Vector3.Distance(Start, End);
        }
    }

    public static partial class ByteExt
    {
        public static float FloatInterpolate(this float current, float desired, float speed)
        {
            if (current == desired)
            {
                return desired;
            }
            else if (current < desired)
            {
                current += Time.deltaTime * speed;
                if (current > desired)
                    return desired;
            }
            else
            {
                if (current > desired)
                {
                    current -= Time.deltaTime * speed;
                    if (current < desired)
                        return desired;
                }
            }

            return current;
        }

        /// <summary>
        /// this < (parameter * parameter)
        /// </summary>
        /// <param name="sqrMagnitude"></param>
        /// <param name="magnitude"></param>
        /// <returns></returns>
        public static bool SqrComparison(this float sqrMagnitude, float magnitude)
        {
            return sqrMagnitude < (magnitude * magnitude);
        }

        /// <summary>
        /// Returns true if the Random test is succeed
        /// </summary>
        /// <param name="prob">Value to be evaluated between 0 and 1 </param>
        /// <returns></returns>
        public static bool RandomChance(float prob)
        {
            prob = Mathf.Clamp01(prob);
            if (Random.Range(0f, 1f) < prob)
                return true;
            return false;
        }

        //caclulate the rotational difference from A to B
        public static Quaternion SubtractRotation(this Quaternion A, Quaternion B)
        {
            Quaternion C = Quaternion.Inverse(A) * B;
            return C;
        }

        //Find the line of intersection between two planes.	The planes are defined by a normal and a point on that plane.
        //The outputs are a point on the line and a vector which indicates it's direction. If the planes are not parallel, 
        //the function outputs true, otherwise false.
        public static bool PlanePlaneIntersection(out Vector3 linePoint, out Vector3 lineVec, Vector3 plane1Normal,
            Vector3 plane1Position, Vector3 plane2Normal, Vector3 plane2Position)
        {
            linePoint = Vector3.zero;
            lineVec = Vector3.zero;

            //We can get the direction of the line of intersection of the two planes by calculating the 
            //cross product of the normals of the two planes. Note that this is just a direction and the line
            //is not fixed in space yet. We need a point for that to go with the line vector.
            lineVec = Vector3.Cross(plane1Normal, plane2Normal);

            //Next is to calculate a point on the line to fix it's position in space. This is done by finding a vector from
            //the plane2 location, moving parallel to it's plane, and intersecting plane1. To prevent rounding
            //errors, this vector also has to be perpendicular to lineDirection. To get this vector, calculate
            //the cross product of the normal of plane2 and the lineDirection.		
            Vector3 ldir = Vector3.Cross(plane2Normal, lineVec);

            float denominator = Vector3.Dot(plane1Normal, ldir);

            //Prevent divide by zero and rounding errors by requiring about 5 degrees angle between the planes.
            if (Mathf.Abs(denominator) > 0.006f)
            {
                Vector3 plane1ToPlane2 = plane1Position - plane2Position;
                float t = Vector3.Dot(plane1Normal, plane1ToPlane2) / denominator;
                linePoint = plane2Position + t * ldir;

                return true;
            }

            //output not valid
            else
            {
                return false;
            }
        }

        //Get the intersection between a line and a plane. 
        //If the line and plane are not parallel, the function outputs true, otherwise false.
        public static bool LinePlaneIntersection(out Vector3 intersection, Vector3 linePoint, Vector3 lineVec,
            Vector3 planeNormal, Vector3 planePoint)
        {
            float length;
            float dotNumerator;
            float dotDenominator;
            Vector3 vector;
            intersection = Vector3.zero;

            //calculate the distance between the linePoint and the line-plane intersection point
            dotNumerator = Vector3.Dot((planePoint - linePoint), planeNormal);
            dotDenominator = Vector3.Dot(lineVec, planeNormal);

            //line and plane are not parallel
            if (dotDenominator != 0.0f)
            {
                length = dotNumerator / dotDenominator;

                //create a vector from the linePoint to the intersection point
                vector = lineVec.SetVectorLength(length);

                //get the coordinates of the line-plane intersection point
                intersection = linePoint + vector;

                return true;
            }

            //output not valid
            else
            {
                return false;
            }
        }

        //Calculate the intersection point of two lines. Returns true if lines intersect, otherwise false.
        //Note that in 3d, two lines do not intersect most of the time. So if the two lines are not in the 
        //same plane, use ClosestPointsOnTwoLines() instead.
        public static bool LineLineIntersection(out Vector3 intersection, Vector3 linePoint1, Vector3 lineVec1,
            Vector3 linePoint2, Vector3 lineVec2)
        {
            intersection = Vector3.zero;

            Vector3 lineVec3 = linePoint2 - linePoint1;
            Vector3 crossVec1and2 = Vector3.Cross(lineVec1, lineVec2);
            Vector3 crossVec3and2 = Vector3.Cross(lineVec3, lineVec2);

            float planarFactor = Vector3.Dot(lineVec3, crossVec1and2);

            //Lines are not coplanar. Take into account rounding errors.
            if ((planarFactor >= 0.00001f) || (planarFactor <= -0.00001f))
            {
                return false;
            }

            //Note: sqrMagnitude does x*x+y*y+z*z on the input vector.
            float s = Vector3.Dot(crossVec3and2, crossVec1and2) / crossVec1and2.sqrMagnitude;

            if ((s >= 0.0f) && (s <= 1.0f))
            {
                intersection = linePoint1 + (lineVec1 * s);
                return true;
            }

            else
            {
                return false;
            }
        }

        public static Vector3 ClampPointToLine(Vector3 point, Vector3 p0, Vector3 p1)
        {
            Vector3 clampedPoint = Vector3.zero;
            float minX, minY, minZ, maxX, maxY, maxZ;

            if (p0.x <= p1.x)
            {
                minX = p0.x;
                maxX = p1.x;
            }
            else
            {
                minX = p1.x;
                maxX = p0.x;
            }

            if (p0.y <= p1.y)
            {
                minY = p0.y;
                maxY = p1.y;
            }
            else
            {
                minY = p1.y;
                maxY = p0.y;
            }

            if (p0.z <= p1.z)
            {
                minZ = p0.z;
                maxZ = p1.z;
            }
            else
            {
                minZ = p1.z;
                maxZ = p0.z;
            }

            clampedPoint.x = (point.x < minX) ? minX : (point.x > maxX) ? maxX : point.x;
            clampedPoint.y = (point.y < minY) ? minY : (point.y > maxY) ? maxY : point.y;
            clampedPoint.z = (point.z < minZ) ? minZ : (point.z > maxZ) ? maxZ : point.z;

            return clampedPoint;
        }

        public static Line DistBetweenLines(Line l1, Line l2)
        {
            Vector3 p1, p2, p3, p4, d1, d2;
            p1 = l1.Start;
            p2 = l1.End;
            p3 = l2.Start;
            p4 = l2.End;
            d1 = p2 - p1;
            d2 = p4 - p3;
            float eq1nCoeff = (d1.x * d2.x) + (d1.y * d2.y) + (d1.z * d2.z);
            float eq1mCoeff = (-(Mathf.Pow(d1.x, 2.0f)) - (Mathf.Pow(d1.y, 2.0f)) - (Mathf.Pow(d1.z, 2.0f)));
            float eq1Const = ((d1.x * p3.x) - (d1.x * p1.x) + (d1.y * p3.y) - (d1.y * p1.y) + (d1.z * p3.z) -
                              (d1.z * p1.z));
            float eq2nCoeff = ((Mathf.Pow(d2.x, 2)) + (Mathf.Pow(d2.y, 2)) + (Mathf.Pow(d2.z, 2)));
            float eq2mCoeff = -(d1.x * d2.x) - (d1.y * d2.y) - (d1.z * d2.z);
            float eq2Const = ((d2.x * p3.x) - (d2.x * p1.x) + (d2.y * p3.y) - (d2.y * p2.y) + (d2.z * p3.z) -
                              (d2.z * p1.z));
            float[,] M = new float[,] { { eq1nCoeff, eq1mCoeff, -eq1Const }, { eq2nCoeff, eq2mCoeff, -eq2Const } };
            int rowCount = M.GetUpperBound(0) + 1;
            // pivoting
            for (int col = 0; col + 1 < rowCount; col++)
                if (M[col, col] == 0)
                    // check for zero coefficients
                {
                    // find non-zero coefficient
                    int swapRow = col + 1;
                    for (; swapRow < rowCount; swapRow++)
                        if (M[swapRow, col] != 0)
                            break;

                    if (M[swapRow, col] != 0) // found a non-zero coefficient?
                    {
                        // yes, then swap it with the above
                        float[] tmp = new float[rowCount + 1];
                        for (int i = 0; i < rowCount + 1; i++)
                        {
                            tmp[i] = M[swapRow, i];
                            M[swapRow, i] = M[col, i];
                            M[col, i] = tmp[i];
                        }
                    }
                    else return null; // no, then the matrix has no unique solution
                }

            // elimination
            for (int sourceRow = 0; sourceRow + 1 < rowCount; sourceRow++)
            {
                for (int destRow = sourceRow + 1; destRow < rowCount; destRow++)
                {
                    float df = M[sourceRow, sourceRow];
                    float sf = M[destRow, sourceRow];
                    for (int i = 0; i < rowCount + 1; i++)
                        M[destRow, i] = M[destRow, i] * df - M[sourceRow, i] * sf;
                }
            }

            // back-insertion
            for (int row = rowCount - 1; row >= 0; row--)
            {
                float f = M[row, row];
                if (f == 0) return null;

                for (int i = 0; i < rowCount + 1; i++) M[row, i] /= f;
                for (int destRow = 0; destRow < row; destRow++)
                {
                    M[destRow, rowCount] -= M[destRow, row] * M[row, rowCount];
                    M[destRow, row] = 0;
                }
            }

            float n = M[0, 2];
            float m = M[1, 2];
            Vector3 i1 = new Vector3(p1.x + (m * d1.x), p1.y + (m * d1.y), p1.z + (m * d1.z));
            Vector3 i2 = new Vector3(p3.x + (n * d2.x), p3.y + (n * d2.y), p3.z + (n * d2.z));
            Vector3 i1Clamped = ClampPointToLine(i1, l1.Start, l1.End);
            Vector3 i2Clamped = ClampPointToLine(i2, l2.Start, l2.End);
            return new Line { Start = i1Clamped, End = i2Clamped };
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
                closestPointLine1 = linePoint1;
                closestPointLine2 = linePoint1;
                return false;
            }
        }

        //This function returns a point which is a projection from a point to a line.
        //The line is regarded infinite. If the line is finite, use ProjectPointOnLineSegment() instead.
        public static Vector3 ProjectPointOnLine(Vector3 linePoint, Vector3 lineVec, Vector3 point)
        {
            //get vector from point on line to point in space
            Vector3 linePointToPoint = point - linePoint;

            float t = Vector3.Dot(linePointToPoint, lineVec);

            return linePoint + lineVec * t;
        }

        //This function returns a point which is a projection from a point to a line segment.
        //If the projected point lies outside of the line segment, the projected point will 
        //be clamped to the appropriate line edge.
        //If the line is infinite instead of a segment, use ProjectPointOnLine() instead.
        public static Vector3 ProjectPointOnLineSegment(Vector3 linePoint1, Vector3 linePoint2, Vector3 point)
        {
            Vector3 vector = linePoint2 - linePoint1;

            Vector3 projectedPoint = ProjectPointOnLine(linePoint1, vector.normalized, point);

            int side = PointOnWhichSideOfLineSegment(linePoint1, linePoint2, projectedPoint);

            //The projected point is on the line segment
            if (side == 0)
            {
                return projectedPoint;
            }

            if (side == 1)
            {
                return linePoint1;
            }

            if (side == 2)
            {
                return linePoint2;
            }

            //output is invalid
            return Vector3.zero;
        }

        //This function returns a point which is a projection from a point to a plane.
        public static Vector3 ProjectPointOnPlane(Vector3 planeNormal, Vector3 planePoint, Vector3 point)
        {
            float distance;
            Vector3 translationVector;

            //First calculate the distance from the point to the plane:
            distance = SignedDistancePlanePoint(planeNormal, planePoint, point);

            //Reverse the sign of the distance
            distance *= -1;

            //Get a translation vector
            translationVector = planeNormal.SetVectorLength(distance);

            //Translate the point to form a projection
            return point + translationVector;
        }

        //Projects a vector onto a plane. The output is not normalized.
        public static Vector3 ProjectVectorOnPlane(Vector3 planeNormal, Vector3 vector)
        {
            return vector - (Vector3.Dot(vector, planeNormal) * planeNormal);
        }

        //Get the shortest distance between a point and a plane. The output is signed so it holds information
        //as to which side of the plane normal the point is.
        public static float SignedDistancePlanePoint(Vector3 planeNormal, Vector3 planePoint, Vector3 point)
        {
            return Vector3.Dot(planeNormal, (point - planePoint));
        }


        //Convert a plane defined by 3 points to a plane defined by a vector and a point. 
        //The plane point is the middle of the triangle defined by the 3 points.
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

        //This is an alternative for Quaternion.LookRotation. Instead of aligning the forward and up vector of the game 
        //object with the input vectors, a custom direction can be used instead of the fixed forward and up vectors.
        //alignWithVector and alignWithNormal are in world space.
        //customForward and customUp are in object space.
        //Usage: use alignWithVector and alignWithNormal as if you are using the default LookRotation function.
        //Set customForward and customUp to the vectors you wish to use instead of the default forward and up vectors.
        public static void LookRotationExtended(ref GameObject gameObjectInOut, Vector3 alignWithVector,
            Vector3 alignWithNormal, Vector3 customForward, Vector3 customUp)
        {
            //Set the rotation of the destination
            Quaternion rotationA = Quaternion.LookRotation(alignWithVector, alignWithNormal);

            //Set the rotation of the custom normal and up vectors. 
            //When using the default LookRotation function, this would be hard coded to the forward and up vector.
            Quaternion rotationB = Quaternion.LookRotation(customForward, customUp);

            //Calculate the rotation
            gameObjectInOut.transform.rotation = rotationA * Quaternion.Inverse(rotationB);
        }

        //With this function you can align a triangle of an object with any transform.
        //Usage: gameObjectInOut is the game object you want to transform.
        //alignWithVector, alignWithNormal, and alignWithPosition is the transform with which the triangle of the object should be aligned with.
        //triangleForward, triangleNormal, and trianglePosition is the transform of the triangle from the object.
        //alignWithVector, alignWithNormal, and alignWithPosition are in world space.
        //triangleForward, triangleNormal, and trianglePosition are in object space.
        //trianglePosition is the mesh position of the triangle. The effect of the scale of the object is handled automatically.
        //trianglePosition can be set at any position, it does not have to be at a vertex or in the middle of the triangle.
        public static void PreciseAlign(ref GameObject gameObjectInOut, Vector3 alignWithVector,
            Vector3 alignWithNormal, Vector3 alignWithPosition, Vector3 triangleForward, Vector3 triangleNormal,
            Vector3 trianglePosition)
        {
            //Set the rotation.
            LookRotationExtended(ref gameObjectInOut, alignWithVector, alignWithNormal, triangleForward,
                triangleNormal);

            //Get the world space position of trianglePosition
            Vector3 trianglePositionWorld = gameObjectInOut.transform.TransformPoint(trianglePosition);

            //Get a vector from trianglePosition to alignWithPosition
            Vector3 translateVector = alignWithPosition - trianglePositionWorld;

            //Now transform the object so the triangle lines up correctly.
            gameObjectInOut.transform.Translate(translateVector, Space.World);
        }


        //Convert a position, direction, and normal vector to a transform
        public static void VectorsToTransform(ref GameObject gameObjectInOut, Vector3 positionVector,
            Vector3 directionVector,
            Vector3 normalVector)
        {
            gameObjectInOut.transform.position = positionVector;
            gameObjectInOut.transform.rotation = Quaternion.LookRotation(directionVector, normalVector);
        }

        //This function finds out on which side of a line segment the point is located.
        //The point is assumed to be on a line created by linePoint1 and linePoint2. If the point is not on
        //the line segment, project it on the line using ProjectPointOnLine() first.
        //Returns 0 if point is on the line segment.
        //Returns 1 if point is outside of the line segment and located on the side of linePoint1.
        //Returns 2 if point is outside of the line segment and located on the side of linePoint2.
        public static int PointOnWhichSideOfLineSegment(Vector3 linePoint1, Vector3 linePoint2, Vector3 point)
        {
            Vector3 lineVec = linePoint2 - linePoint1;
            Vector3 pointVec = point - linePoint1;

            float dot = Vector3.Dot(pointVec, lineVec);

            //point is on side of linePoint2, compared to linePoint1
            if (dot > 0)
            {
                //point is on the line segment
                if (pointVec.magnitude <= lineVec.magnitude)
                {
                    return 0;
                }

                //point is not on the line segment and it is on the side of linePoint2
                else
                {
                    return 2;
                }
            }

            //Point is not on side of linePoint2, compared to linePoint1.
            //Point is not on the line segment and it is on the side of linePoint1.
            else
            {
                return 1;
            }
        }


        //Returns the pixel distance from the mouse pointer to a line.
        //Alternative for HandleUtility.DistanceToLine(). Works both in Editor mode and Play mode.
        //Do not call this function from OnGUI() as the mouse position will be wrong.
        public static float MouseDistanceToLine(Vector3 linePoint1, Vector3 linePoint2)
        {
            Camera currentCamera;
            Vector3 mousePosition;

#if UNITY_EDITOR
            if (Camera.current != null)
            {
                currentCamera = Camera.current;
            }

            else
            {
                currentCamera = Camera.main;
            }

            //convert format because y is flipped
            mousePosition = new Vector3(Event.current.mousePosition.x,
                currentCamera.pixelHeight - Event.current.mousePosition.y, 0f);

#else
		currentCamera = Camera.main;
		mousePosition = Input.mousePosition;
#endif

            Vector3 screenPos1 = currentCamera.WorldToScreenPoint(linePoint1);
            Vector3 screenPos2 = currentCamera.WorldToScreenPoint(linePoint2);
            Vector3 projectedPoint = ProjectPointOnLineSegment(screenPos1, screenPos2, mousePosition);

            //set z to zero
            projectedPoint = new Vector3(projectedPoint.x, projectedPoint.y, 0f);

            Vector3 vector = projectedPoint - mousePosition;
            return vector.magnitude;
        }


        //Returns the pixel distance from the mouse pointer to a camera facing circle.
        //Alternative for HandleUtility.DistanceToCircle(). Works both in Editor mode and Play mode.
        //Do not call this function from OnGUI() as the mouse position will be wrong.
        //If you want the distance to a point instead of a circle, set the radius to 0.
        public static float MouseDistanceToCircle(Vector3 point, float radius)
        {
            Camera currentCamera;
            Vector3 mousePosition;

#if UNITY_EDITOR
            if (Camera.current != null)
            {
                currentCamera = Camera.current;
            }

            else
            {
                currentCamera = Camera.main;
            }

            //convert format because y is flipped
            mousePosition = new Vector3(Event.current.mousePosition.x,
                currentCamera.pixelHeight - Event.current.mousePosition.y, 0f);
#else
		currentCamera = Camera.main;
		mousePosition = Input.mousePosition;
#endif

            Vector3 screenPos = currentCamera.WorldToScreenPoint(point);

            //set z to zero
            screenPos = new Vector3(screenPos.x, screenPos.y, 0f);

            Vector3 vector = screenPos - mousePosition;
            float fullDistance = vector.magnitude;
            float circleDistance = fullDistance - radius;

            return circleDistance;
        }

        //Returns true if a line segment (made up of linePoint1 and linePoint2) is fully or partially in a rectangle
        //made up of RectA to RectD. The line segment is assumed to be on the same plane as the rectangle. If the line is 
        //not on the plane, use ProjectPointOnPlane() on linePoint1 and linePoint2 first.
        public static bool IsLineInRectangle(Vector3 linePoint1, Vector3 linePoint2, Vector3 rectA, Vector3 rectB,
            Vector3 rectC, Vector3 rectD)
        {
            bool pointAInside = false;
            bool pointBInside = false;

            pointAInside = IsPointInRectangle(linePoint1, rectA, rectC, rectB, rectD);

            if (!pointAInside)
            {
                pointBInside = IsPointInRectangle(linePoint2, rectA, rectC, rectB, rectD);
            }

            //none of the points are inside, so check if a line is crossing
            if (!pointAInside && !pointBInside)
            {
                bool lineACrossing = AreLineSegmentsCrossing(linePoint1, linePoint2, rectA, rectB);
                bool lineBCrossing = AreLineSegmentsCrossing(linePoint1, linePoint2, rectB, rectC);
                bool lineCCrossing = AreLineSegmentsCrossing(linePoint1, linePoint2, rectC, rectD);
                bool lineDCrossing = AreLineSegmentsCrossing(linePoint1, linePoint2, rectD, rectA);

                if (lineACrossing || lineBCrossing || lineCCrossing || lineDCrossing)
                {
                    return true;
                }

                else
                {
                    return false;
                }
            }

            else
            {
                return true;
            }
        }

        //Returns true if "point" is in a rectangle mad up of RectA to RectD. The line point is assumed to be on the same 
        //plane as the rectangle. If the point is not on the plane, use ProjectPointOnPlane() first.
        public static bool IsPointInRectangle(Vector3 point, Vector3 rectA, Vector3 rectC, Vector3 rectB, Vector3 rectD)
        {
            Vector3 vector;
            Vector3 linePoint;

            //get the center of the rectangle
            vector = rectC - rectA;
            float size = -(vector.magnitude / 2f);
            vector = vector.AddVectorLength(size);
            Vector3 middle = rectA + vector;

            Vector3 xVector = rectB - rectA;
            float width = xVector.magnitude / 2f;

            Vector3 yVector = rectD - rectA;
            float height = yVector.magnitude / 2f;

            linePoint = ProjectPointOnLine(middle, xVector.normalized, point);
            vector = linePoint - point;
            float yDistance = vector.magnitude;

            linePoint = ProjectPointOnLine(middle, yVector.normalized, point);
            vector = linePoint - point;
            float xDistance = vector.magnitude;

            if ((xDistance <= width) && (yDistance <= height))
            {
                return true;
            }

            else
            {
                return false;
            }
        }

        //Returns true if line segment made up of pointA1 and pointA2 is crossing line segment made up of
        //pointB1 and pointB2. The two lines are assumed to be in the same plane.
        public static bool AreLineSegmentsCrossing(Vector3 pointA1, Vector3 pointA2, Vector3 pointB1, Vector3 pointB2)
        {
            Vector3 closestPointA;
            Vector3 closestPointB;
            int sideA;
            int sideB;

            Vector3 lineVecA = pointA2 - pointA1;
            Vector3 lineVecB = pointB2 - pointB1;

            bool valid = ClosestPointsOnTwoLines(out closestPointA, out closestPointB, pointA1, lineVecA.normalized,
                pointB1, lineVecB.normalized);

            //lines are not parallel
            if (valid)
            {
                sideA = PointOnWhichSideOfLineSegment(pointA1, pointA2, closestPointA);
                sideB = PointOnWhichSideOfLineSegment(pointB1, pointB2, closestPointB);

                if ((sideA == 0) && (sideB == 0))
                {
                    return true;
                }

                else
                {
                    return false;
                }
            }

            //lines are parallel
            else
            {
                return false;
            }
        }


        public static bool IsPosInsideCone(Vector3 conePosition, Vector3 coneFront, float coneLength, float coneAngle,
            Vector3 targetPosition)
        {
            Vector3 toCone = targetPosition - conePosition;
            float dist = toCone.magnitude;
            if (dist > coneLength)
            {
                return false;
            }

            return Vector3.Angle(toCone, coneFront) < (coneAngle / 2.0f);
            //toCone /= dist;
            //float cosAngle = Vector3.Dot(toCone, coneFront);
            //float cosAngle2 = Mathf.Cos(coneAngle);
            //if (cosAngle < cosAngle2)
            //{
            //    return false;
            //}
            //return true;
        }

        //Does exactly the same as the one above, but receives the cos of the angle, in case it's calculated already. Faster
        public static bool IsPosInsideConeCos(Vector3 conePosition, Vector3 coneFront, float coneLength,
            float coneAngleCos, Vector3 targetPosition)
        {
            Vector3 toTarget = targetPosition - conePosition;
            float dist = toTarget.magnitude;
            toTarget.Normalize();
            if (dist > coneLength)
            {
                return false;
            }

            return Vector3.Dot(toTarget, coneFront) > coneAngleCos;
        }

        /// <summary>
        /// if maxdist == 0 it won't be used
        /// </summary>
        public static bool IsPosInsideFrustrumSpherecast(Vector3 pos, Camera cam, float s_cast_rad, int mask,
            float maxdist = 0f)
        {
            //distance text
            if (maxdist != 0f)
            {
                if ((pos - cam.transform.position).sqrMagnitude > maxdist * maxdist) //sqr dist
                    return false;
            }

            //frustrum test
            if (pos.IsPosInsideFrustrum(cam))
            {
                //Sphere cast test
                if (!RaycastExt.SphereCastBetweenPoints(pos, cam.transform.position, s_cast_rad, mask))
                {
                    return true;
                }
            }

            return false;
        }
    }
}