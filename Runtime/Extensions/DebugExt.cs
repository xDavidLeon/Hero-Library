using UnityEngine;
using System.Reflection;

/// <summary>
/// Debug Extension
/// 	- Static class that extends Unity's debugging functionallity.
/// 	- Attempts to mimic Unity's existing debugging behaviour for ease-of-use.
/// 	- Includes gizmo drawing methods for less memory-intensive debug visualization.
/// </summary>

namespace HeroLib

{
    public static class DebugExt
    {
        #region DebugDrawFunctions

        /// <summary>
        /// 	- Debugs a point.
        /// </summary>
        /// <param name='position'>
        /// 	- The point to debug.
        /// </param>
        /// <param name='color'>
        /// 	- The color of the point.
        /// </param>
        /// <param name='scale'>
        /// 	- The size of the point.
        /// </param>
        /// <param name='duration'>
        /// 	- How long to draw the point.
        /// </param>
        /// <param name='depthTest'>
        /// 	- Whether or not this point should be faded when behind other objects.
        /// </param>
        public static void DebugPoint(Vector3 position, Color color, float scale = 1.0f, float duration = 0,
            bool depthTest = true)
        {
            color = (color == default(Color)) ? Color.white : color;

            Debug.DrawRay(position + (Vector3.up * (scale * 0.5f)), -Vector3.up * scale, color, duration, depthTest);
            Debug.DrawRay(position + (Vector3.right * (scale * 0.5f)), -Vector3.right * scale, color, duration,
                depthTest);
            Debug.DrawRay(position + (Vector3.forward * (scale * 0.5f)), -Vector3.forward * scale, color, duration,
                depthTest);
        }

        /// <summary>
        /// 	- Debugs a point.
        /// </summary>
        /// <param name='position'>
        /// 	- The point to debug.
        /// </param>
        /// <param name='scale'>
        /// 	- The size of the point.
        /// </param>
        /// <param name='duration'>
        /// 	- How long to draw the point.
        /// </param>
        /// <param name='depthTest'>
        /// 	- Whether or not this point should be faded when behind other objects.
        /// </param>
        public static void DebugPoint(Vector3 position, float scale = 1.0f, float duration = 0, bool depthTest = true)
        {
            DebugPoint(position, Color.white, scale, duration, depthTest);
        }

        /// <summary>
        /// 	- Debugs an axis-aligned bounding box.
        /// </summary>
        /// <param name='bounds'>
        /// 	- The bounds to debug.
        /// </param>
        /// <param name='color'>
        /// 	- The color of the bounds.
        /// </param>
        /// <param name='duration'>
        /// 	- How long to draw the bounds.
        /// </param>
        /// <param name='depthTest'>
        /// 	- Whether or not the bounds should be faded when behind other objects.
        /// </param>
        public static void DebugBounds(Bounds bounds, Color color, float duration = 0, bool depthTest = true)
        {
            Vector3 center = bounds.center;

            float x = bounds.extents.x;
            float y = bounds.extents.y;
            float z = bounds.extents.z;

            Vector3 ruf = center + new Vector3(x, y, z);
            Vector3 rub = center + new Vector3(x, y, -z);
            Vector3 luf = center + new Vector3(-x, y, z);
            Vector3 lub = center + new Vector3(-x, y, -z);

            Vector3 rdf = center + new Vector3(x, -y, z);
            Vector3 rdb = center + new Vector3(x, -y, -z);
            Vector3 lfd = center + new Vector3(-x, -y, z);
            Vector3 lbd = center + new Vector3(-x, -y, -z);

            Debug.DrawLine(ruf, luf, color, duration, depthTest);
            Debug.DrawLine(ruf, rub, color, duration, depthTest);
            Debug.DrawLine(luf, lub, color, duration, depthTest);
            Debug.DrawLine(rub, lub, color, duration, depthTest);

            Debug.DrawLine(ruf, rdf, color, duration, depthTest);
            Debug.DrawLine(rub, rdb, color, duration, depthTest);
            Debug.DrawLine(luf, lfd, color, duration, depthTest);
            Debug.DrawLine(lub, lbd, color, duration, depthTest);

            Debug.DrawLine(rdf, lfd, color, duration, depthTest);
            Debug.DrawLine(rdf, rdb, color, duration, depthTest);
            Debug.DrawLine(lfd, lbd, color, duration, depthTest);
            Debug.DrawLine(lbd, rdb, color, duration, depthTest);
        }

        /// <summary>
        /// 	- Debugs an axis-aligned bounding box.
        /// </summary>
        /// <param name='bounds'>
        /// 	- The bounds to debug.
        /// </param>
        /// <param name='duration'>
        /// 	- How long to draw the bounds.
        /// </param>
        /// <param name='depthTest'>
        /// 	- Whether or not the bounds should be faded when behind other objects.
        /// </param>
        public static void DebugBounds(Bounds bounds, float duration = 0, bool depthTest = true)
        {
            DebugBounds(bounds, Color.white, duration, depthTest);
        }

        /// <summary>
        /// 	- Debugs an axis-aligned bounding box.
        /// </summary>
        /// <param name='bounds'>
        /// 	- The bounds to debug.
        /// </param>
        /// <param name='color'>
        /// 	- The color of the bounds.
        /// </param>
        /// <param name='duration'>
        /// 	- How long to draw the bounds.
        /// </param>
        /// <param name='depthTest'>
        /// 	- Whether or not the bounds should be faded when behind other objects.
        /// </param>
        public static void DebugBoundsOriented(Bounds bounds, Quaternion rotation, Color color, float duration = 0,
            bool depthTest = true)
        {
            Vector3 center = bounds.center;

            float x = bounds.extents.x;
            float y = bounds.extents.y;
            float z = bounds.extents.z;

            Vector3 ruf = center + rotation * new Vector3(x, y, z);
            Vector3 rub = center + rotation * new Vector3(x, y, -z);
            Vector3 luf = center + rotation * new Vector3(-x, y, z);
            Vector3 lub = center + rotation * new Vector3(-x, y, -z);

            Vector3 rdf = center + rotation * new Vector3(x, -y, z);
            Vector3 rdb = center + rotation * new Vector3(x, -y, -z);
            Vector3 lfd = center + rotation * new Vector3(-x, -y, z);
            Vector3 lbd = center + rotation * new Vector3(-x, -y, -z);

            Debug.DrawLine(ruf, luf, color, duration, depthTest);
            Debug.DrawLine(ruf, rub, color, duration, depthTest);
            Debug.DrawLine(luf, lub, color, duration, depthTest);
            Debug.DrawLine(rub, lub, color, duration, depthTest);

            Debug.DrawLine(ruf, rdf, color, duration, depthTest);
            Debug.DrawLine(rub, rdb, color, duration, depthTest);
            Debug.DrawLine(luf, lfd, color, duration, depthTest);
            Debug.DrawLine(lub, lbd, color, duration, depthTest);

            Debug.DrawLine(rdf, lfd, color, duration, depthTest);
            Debug.DrawLine(rdf, rdb, color, duration, depthTest);
            Debug.DrawLine(lfd, lbd, color, duration, depthTest);
            Debug.DrawLine(lbd, rdb, color, duration, depthTest);
        }

        /// <summary>
        /// 	- Debugs a local cube.
        /// </summary>
        /// <param name='transform'>
        /// 	- The transform that the cube will be local to.
        /// </param>
        /// <param name='size'>
        /// 	- The size of the cube.
        /// </param>
        /// <param name='color'>
        /// 	- Color of the cube.
        /// </param>
        /// <param name='center'>
        /// 	- The position (relative to transform) where the cube will be debugged.
        /// </param>
        /// <param name='duration'>
        /// 	- How long to draw the cube.
        /// </param>
        /// <param name='depthTest'>
        /// 	- Whether or not the cube should be faded when behind other objects.
        /// </param>
        public static void DebugLocalCube(Transform transform, Vector3 size, Color color,
            Vector3 center = default(Vector3), float duration = 0, bool depthTest = true)
        {
            Vector3 lbb = transform.TransformPoint(center + ((-size) * 0.5f));
            Vector3 rbb = transform.TransformPoint(center + (new Vector3(size.x, -size.y, -size.z) * 0.5f));

            Vector3 lbf = transform.TransformPoint(center + (new Vector3(size.x, -size.y, size.z) * 0.5f));
            Vector3 rbf = transform.TransformPoint(center + (new Vector3(-size.x, -size.y, size.z) * 0.5f));

            Vector3 lub = transform.TransformPoint(center + (new Vector3(-size.x, size.y, -size.z) * 0.5f));
            Vector3 rub = transform.TransformPoint(center + (new Vector3(size.x, size.y, -size.z) * 0.5f));

            Vector3 luf = transform.TransformPoint(center + ((size) * 0.5f));
            Vector3 ruf = transform.TransformPoint(center + (new Vector3(-size.x, size.y, size.z) * 0.5f));

            Debug.DrawLine(lbb, rbb, color, duration, depthTest);
            Debug.DrawLine(rbb, lbf, color, duration, depthTest);
            Debug.DrawLine(lbf, rbf, color, duration, depthTest);
            Debug.DrawLine(rbf, lbb, color, duration, depthTest);

            Debug.DrawLine(lub, rub, color, duration, depthTest);
            Debug.DrawLine(rub, luf, color, duration, depthTest);
            Debug.DrawLine(luf, ruf, color, duration, depthTest);
            Debug.DrawLine(ruf, lub, color, duration, depthTest);

            Debug.DrawLine(lbb, lub, color, duration, depthTest);
            Debug.DrawLine(rbb, rub, color, duration, depthTest);
            Debug.DrawLine(lbf, luf, color, duration, depthTest);
            Debug.DrawLine(rbf, ruf, color, duration, depthTest);
        }

        public static void DebugLocalCube(CustomTransform transform, Vector3 size, Color color,
            Vector3 center = default(Vector3), float duration = 0, bool depthTest = true)
        {
            Vector3 lbb = transform.TransformPoint(center + ((-size) * 0.5f));
            Vector3 rbb = transform.TransformPoint(center + (new Vector3(size.x, -size.y, -size.z) * 0.5f));

            Vector3 lbf = transform.TransformPoint(center + (new Vector3(size.x, -size.y, size.z) * 0.5f));
            Vector3 rbf = transform.TransformPoint(center + (new Vector3(-size.x, -size.y, size.z) * 0.5f));

            Vector3 lub = transform.TransformPoint(center + (new Vector3(-size.x, size.y, -size.z) * 0.5f));
            Vector3 rub = transform.TransformPoint(center + (new Vector3(size.x, size.y, -size.z) * 0.5f));

            Vector3 luf = transform.TransformPoint(center + ((size) * 0.5f));
            Vector3 ruf = transform.TransformPoint(center + (new Vector3(-size.x, size.y, size.z) * 0.5f));

            Debug.DrawLine(lbb, rbb, color, duration, depthTest);
            Debug.DrawLine(rbb, lbf, color, duration, depthTest);
            Debug.DrawLine(lbf, rbf, color, duration, depthTest);
            Debug.DrawLine(rbf, lbb, color, duration, depthTest);

            Debug.DrawLine(lub, rub, color, duration, depthTest);
            Debug.DrawLine(rub, luf, color, duration, depthTest);
            Debug.DrawLine(luf, ruf, color, duration, depthTest);
            Debug.DrawLine(ruf, lub, color, duration, depthTest);

            Debug.DrawLine(lbb, lub, color, duration, depthTest);
            Debug.DrawLine(rbb, rub, color, duration, depthTest);
            Debug.DrawLine(lbf, luf, color, duration, depthTest);
            Debug.DrawLine(rbf, ruf, color, duration, depthTest);
        }

        /// <summary>
        /// 	- Debugs a local cube.
        /// </summary>
        /// <param name='transform'>
        /// 	- The transform that the cube will be local to.
        /// </param>
        /// <param name='size'>
        /// 	- The size of the cube.
        /// </param>
        /// <param name='center'>
        /// 	- The position (relative to transform) where the cube will be debugged.
        /// </param>
        /// <param name='duration'>
        /// 	- How long to draw the cube.
        /// </param>
        /// <param name='depthTest'>
        /// 	- Whether or not the cube should be faded when behind other objects.
        /// </param>
        public static void DebugLocalCube(Transform transform, Vector3 size, Vector3 center = default(Vector3),
            float duration = 0, bool depthTest = true)
        {
            DebugLocalCube(transform, size, Color.white, center, duration, depthTest);
        }

        /// <summary>
        /// 	- Debugs a local cube.
        /// </summary>
        /// <param name='space'>
        /// 	- The space the cube will be local to.
        /// </param>
        /// <param name='size'>
        ///		- The size of the cube.
        /// </param>
        /// <param name='color'>
        /// 	- Color of the cube.
        /// </param>
        /// <param name='center'>
        /// 	- The position (relative to transform) where the cube will be debugged.
        /// </param>
        /// <param name='duration'>
        /// 	- How long to draw the cube.
        /// </param>
        /// <param name='depthTest'>
        /// 	- Whether or not the cube should be faded when behind other objects.
        /// </param>
        public static void DebugLocalCube(Matrix4x4 space, Vector3 size, Color color, Vector3 center = default(Vector3),
            float duration = 0, bool depthTest = true)
        {
            color = (color == default(Color)) ? Color.white : color;

            Vector3 lbb = space.MultiplyPoint3x4(center + ((-size) * 0.5f));
            Vector3 rbb = space.MultiplyPoint3x4(center + (new Vector3(size.x, -size.y, -size.z) * 0.5f));

            Vector3 lbf = space.MultiplyPoint3x4(center + (new Vector3(size.x, -size.y, size.z) * 0.5f));
            Vector3 rbf = space.MultiplyPoint3x4(center + (new Vector3(-size.x, -size.y, size.z) * 0.5f));

            Vector3 lub = space.MultiplyPoint3x4(center + (new Vector3(-size.x, size.y, -size.z) * 0.5f));
            Vector3 rub = space.MultiplyPoint3x4(center + (new Vector3(size.x, size.y, -size.z) * 0.5f));

            Vector3 luf = space.MultiplyPoint3x4(center + ((size) * 0.5f));
            Vector3 ruf = space.MultiplyPoint3x4(center + (new Vector3(-size.x, size.y, size.z) * 0.5f));

            Debug.DrawLine(lbb, rbb, color, duration, depthTest);
            Debug.DrawLine(rbb, lbf, color, duration, depthTest);
            Debug.DrawLine(lbf, rbf, color, duration, depthTest);
            Debug.DrawLine(rbf, lbb, color, duration, depthTest);

            Debug.DrawLine(lub, rub, color, duration, depthTest);
            Debug.DrawLine(rub, luf, color, duration, depthTest);
            Debug.DrawLine(luf, ruf, color, duration, depthTest);
            Debug.DrawLine(ruf, lub, color, duration, depthTest);

            Debug.DrawLine(lbb, lub, color, duration, depthTest);
            Debug.DrawLine(rbb, rub, color, duration, depthTest);
            Debug.DrawLine(lbf, luf, color, duration, depthTest);
            Debug.DrawLine(rbf, ruf, color, duration, depthTest);
        }

        /// <summary>
        /// 	- Debugs a local cube.
        /// </summary>
        /// <param name='space'>
        /// 	- The space the cube will be local to.
        /// </param>
        /// <param name='size'>
        ///		- The size of the cube.
        /// </param>
        /// <param name='center'>
        /// 	- The position (relative to transform) where the cube will be debugged.
        /// </param>
        /// <param name='duration'>
        /// 	- How long to draw the cube.
        /// </param>
        /// <param name='depthTest'>
        /// 	- Whether or not the cube should be faded when behind other objects.
        /// </param>
        public static void DebugLocalCube(Matrix4x4 space, Vector3 size, Vector3 center = default(Vector3),
            float duration = 0, bool depthTest = true)
        {
            DebugLocalCube(space, size, Color.white, center, duration, depthTest);
        }

        /// <summary>
        /// 	- Debugs a circle.
        /// </summary>
        /// <param name='position'>
        /// 	- Where the center of the circle will be positioned.
        /// </param>
        /// <param name='up'>
        /// 	- The direction perpendicular to the surface of the circle.
        /// </param>
        /// <param name='color'>
        /// 	- The color of the circle.
        /// </param>
        /// <param name='radius'>
        /// 	- The radius of the circle.
        /// </param>
        /// <param name='duration'>
        /// 	- How long to draw the circle.
        /// </param>
        /// <param name='depthTest'>
        /// 	- Whether or not the circle should be faded when behind other objects.
        /// </param>
        public static void DebugCircle(Vector3 position, Vector3 up, Color color, float radius = 1.0f,
            float duration = 0, bool depthTest = true)
        {
            Vector3 _up = up.normalized * radius;
            Vector3 _forward = Vector3.Slerp(_up, -_up, 0.5f);
            Vector3 _right = Vector3.Cross(_up, _forward).normalized * radius;

            Matrix4x4 matrix = new Matrix4x4();

            matrix[0] = _right.x;
            matrix[1] = _right.y;
            matrix[2] = _right.z;

            matrix[4] = _up.x;
            matrix[5] = _up.y;
            matrix[6] = _up.z;

            matrix[8] = _forward.x;
            matrix[9] = _forward.y;
            matrix[10] = _forward.z;

            Vector3 _lastPoint = position + matrix.MultiplyPoint3x4(new Vector3(Mathf.Cos(0), 0, Mathf.Sin(0)));
            Vector3 _nextPoint = Vector3.zero;

            color = (color == default(Color)) ? Color.white : color;

            for (var i = 0; i < 91; i++)
            {
                _nextPoint.x = Mathf.Cos((i * 4) * Mathf.Deg2Rad);
                _nextPoint.z = Mathf.Sin((i * 4) * Mathf.Deg2Rad);
                _nextPoint.y = 0;

                _nextPoint = position + matrix.MultiplyPoint3x4(_nextPoint);

                Debug.DrawLine(_lastPoint, _nextPoint, color, duration, depthTest);
                _lastPoint = _nextPoint;
            }
        }

        /// <summary>
        /// 	- Debugs a circle.
        /// </summary>
        /// <param name='position'>
        /// 	- Where the center of the circle will be positioned.
        /// </param>
        /// <param name='color'>
        /// 	- The color of the circle.
        /// </param>
        /// <param name='radius'>
        /// 	- The radius of the circle.
        /// </param>
        /// <param name='duration'>
        /// 	- How long to draw the circle.
        /// </param>
        /// <param name='depthTest'>
        /// 	- Whether or not the circle should be faded when behind other objects.
        /// </param>
        public static void DebugCircle(Vector3 position, Color color, float radius = 1.0f, float duration = 0,
            bool depthTest = true)
        {
            DebugCircle(position, Vector3.up, color, radius, duration, depthTest);
        }

        /// <summary>
        /// 	- Debugs a circle.
        /// </summary>
        /// <param name='position'>
        /// 	- Where the center of the circle will be positioned.
        /// </param>
        /// <param name='up'>
        /// 	- The direction perpendicular to the surface of the circle.
        /// </param>
        /// <param name='radius'>
        /// 	- The radius of the circle.
        /// </param>
        /// <param name='duration'>
        /// 	- How long to draw the circle.
        /// </param>
        /// <param name='depthTest'>
        /// 	- Whether or not the circle should be faded when behind other objects.
        /// </param>
        public static void DebugCircle(Vector3 position, Vector3 up, float radius = 1.0f, float duration = 0,
            bool depthTest = true)
        {
            DebugCircle(position, up, Color.white, radius, duration, depthTest);
        }

        /// <summary>
        /// 	- Debugs a circle.
        /// </summary>
        /// <param name='position'>
        /// 	- Where the center of the circle will be positioned.
        /// </param>
        /// <param name='radius'>
        /// 	- The radius of the circle.
        /// </param>
        /// <param name='duration'>
        /// 	- How long to draw the circle.
        /// </param>
        /// <param name='depthTest'>
        /// 	- Whether or not the circle should be faded when behind other objects.
        /// </param>
        public static void DebugCircle(Vector3 position, float radius = 1.0f, float duration = 0, bool depthTest = true)
        {
            DebugCircle(position, Vector3.up, Color.white, radius, duration, depthTest);
        }

        public static void DebugCircleSimple(Vector3 center, float radius, Color color)
        {
            Vector3 prevPos = center + new Vector3(radius, 0, 0);
            for (int i = 0; i < 30; i++)
            {
                float angle = (float)(i + 1) / 30.0f * Mathf.PI * 2.0f;
                Vector3 newPos = center + new Vector3(Mathf.Cos(angle) * radius, 0, Mathf.Sin(angle) * radius);
                Debug.DrawLine(prevPos, newPos, color);
                prevPos = newPos;
            }
        }

        /// <summary>
        /// 	- Debugs a wire sphere.
        /// </summary>
        /// <param name='position'>
        /// 	- The position of the center of the sphere.
        /// </param>
        /// <param name='color'>
        /// 	- The color of the sphere.
        /// </param>
        /// <param name='radius'>
        /// 	- The radius of the sphere.
        /// </param>
        /// <param name='duration'>
        /// 	- How long to draw the sphere.
        /// </param>
        /// <param name='depthTest'>
        /// 	- Whether or not the sphere should be faded when behind other objects.
        /// </param>
        public static void DebugWireSphere(Vector3 position, Color color, float radius = 1.0f, float duration = 0,
            bool depthTest = true)
        {
            float angle = 10.0f;

            Vector3 x = new Vector3(position.x, position.y + radius * Mathf.Sin(0), position.z + radius * Mathf.Cos(0));
            Vector3 y = new Vector3(position.x + radius * Mathf.Cos(0), position.y, position.z + radius * Mathf.Sin(0));
            Vector3 z = new Vector3(position.x + radius * Mathf.Cos(0), position.y + radius * Mathf.Sin(0), position.z);

            Vector3 new_x;
            Vector3 new_y;
            Vector3 new_z;

            for (int i = 1; i < 37; i++)
            {
                new_x = new Vector3(position.x, position.y + radius * Mathf.Sin(angle * i * Mathf.Deg2Rad),
                    position.z + radius * Mathf.Cos(angle * i * Mathf.Deg2Rad));
                new_y = new Vector3(position.x + radius * Mathf.Cos(angle * i * Mathf.Deg2Rad), position.y,
                    position.z + radius * Mathf.Sin(angle * i * Mathf.Deg2Rad));
                new_z = new Vector3(position.x + radius * Mathf.Cos(angle * i * Mathf.Deg2Rad),
                    position.y + radius * Mathf.Sin(angle * i * Mathf.Deg2Rad), position.z);

                Debug.DrawLine(x, new_x, color, duration, depthTest);
                Debug.DrawLine(y, new_y, color, duration, depthTest);
                Debug.DrawLine(z, new_z, color, duration, depthTest);

                x = new_x;
                y = new_y;
                z = new_z;
            }
        }

        /// <summary>
        /// 	- Debugs a wire sphere.
        /// </summary>
        /// <param name='position'>
        /// 	- The position of the center of the sphere.
        /// </param>
        /// <param name='radius'>
        /// 	- The radius of the sphere.
        /// </param>
        /// <param name='duration'>
        /// 	- How long to draw the sphere.
        /// </param>
        /// <param name='depthTest'>
        /// 	- Whether or not the sphere should be faded when behind other objects.
        /// </param>
        public static void DebugWireSphere(Vector3 position, float radius = 1.0f, float duration = 0,
            bool depthTest = true)
        {
            DebugWireSphere(position, Color.white, radius, duration, depthTest);
        }

        /// <summary>
        /// 	- Debugs a cylinder.
        /// </summary>
        /// <param name='start'>
        /// 	- The position of one end of the cylinder.
        /// </param>
        /// <param name='end'>
        /// 	- The position of the other end of the cylinder.
        /// </param>
        /// <param name='color'>
        /// 	- The color of the cylinder.
        /// </param>
        /// <param name='radius'>
        /// 	- The radius of the cylinder.
        /// </param>
        /// <param name='duration'>
        /// 	- How long to draw the cylinder.
        /// </param>
        /// <param name='depthTest'>
        /// 	- Whether or not the cylinder should be faded when behind other objects.
        /// </param>
        public static void DebugCylinder(Vector3 start, Vector3 end, Color color, float radius = 1, float duration = 0,
            bool depthTest = true)
        {
            Vector3 up = (end - start).normalized * radius;
            Vector3 forward = Vector3.Slerp(up, -up, 0.5f);
            Vector3 right = Vector3.Cross(up, forward).normalized * radius;

            //Radial circles
            DebugExt.DebugCircle(start, up, color, radius, duration, depthTest);
            DebugExt.DebugCircle(end, -up, color, radius, duration, depthTest);
            DebugExt.DebugCircle((start + end) * 0.5f, up, color, radius, duration, depthTest);

            //Side lines
            Debug.DrawLine(start + right, end + right, color, duration, depthTest);
            Debug.DrawLine(start - right, end - right, color, duration, depthTest);

            Debug.DrawLine(start + forward, end + forward, color, duration, depthTest);
            Debug.DrawLine(start - forward, end - forward, color, duration, depthTest);

            //Start endcap
            Debug.DrawLine(start - right, start + right, color, duration, depthTest);
            Debug.DrawLine(start - forward, start + forward, color, duration, depthTest);

            //End endcap
            Debug.DrawLine(end - right, end + right, color, duration, depthTest);
            Debug.DrawLine(end - forward, end + forward, color, duration, depthTest);
        }

        /// <summary>
        /// 	- Debugs a cylinder.
        /// </summary>
        /// <param name='start'>
        /// 	- The position of one end of the cylinder.
        /// </param>
        /// <param name='end'>
        /// 	- The position of the other end of the cylinder.
        /// </param>
        /// <param name='radius'>
        /// 	- The radius of the cylinder.
        /// </param>
        /// <param name='duration'>
        /// 	- How long to draw the cylinder.
        /// </param>
        /// <param name='depthTest'>
        /// 	- Whether or not the cylinder should be faded when behind other objects.
        /// </param>
        public static void DebugCylinder(Vector3 start, Vector3 end, float radius = 1, float duration = 0,
            bool depthTest = true)
        {
            DebugCylinder(start, end, Color.white, radius, duration, depthTest);
        }

        /// <summary>
        /// 	- Debugs a cone.
        /// </summary>
        /// <param name='position'>
        /// 	- The position for the tip of the cone.
        /// </param>
        /// <param name='direction'>
        /// 	- The direction for the cone gets wider in.
        /// </param>
        /// <param name='angle'>
        /// 	- The angle of the cone.
        /// </param>
        /// <param name='color'>
        /// 	- The color of the cone.
        /// </param>
        /// <param name='duration'>
        /// 	- How long to draw the cone.
        /// </param>
        /// <param name='depthTest'>
        /// 	- Whether or not the cone should be faded when behind other objects.
        /// </param>
        public static void DebugCone(Vector3 position, Vector3 direction, Color color, float angle = 45,
            float duration = 0, bool depthTest = true)
        {
            float length = direction.magnitude;

            Vector3 _forward = direction;
            Vector3 _up = Vector3.Slerp(_forward, -_forward, 0.5f);
            Vector3 _right = Vector3.Cross(_forward, _up).normalized * length;

            direction = direction.normalized;

            Vector3 slerpedVector = Vector3.Slerp(_forward, _up, angle / 90.0f);

            float dist;
            var farPlane = new Plane(-direction, position + _forward);
            var distRay = new Ray(position, slerpedVector);

            farPlane.Raycast(distRay, out dist);

            Debug.DrawRay(position, slerpedVector.normalized * dist, color);
            Debug.DrawRay(position, Vector3.Slerp(_forward, -_up, angle / 90.0f).normalized * dist, color, duration,
                depthTest);
            Debug.DrawRay(position, Vector3.Slerp(_forward, _right, angle / 90.0f).normalized * dist, color, duration,
                depthTest);
            Debug.DrawRay(position, Vector3.Slerp(_forward, -_right, angle / 90.0f).normalized * dist, color, duration,
                depthTest);

            DebugExt.DebugCircle(position + _forward, direction, color,
                (_forward - (slerpedVector.normalized * dist)).magnitude, duration, depthTest);
            DebugExt.DebugCircle(position + (_forward * 0.5f), direction, color,
                ((_forward * 0.5f) - (slerpedVector.normalized * (dist * 0.5f))).magnitude, duration, depthTest);
        }

        /// <summary>
        /// 	- Debugs a cone.
        /// </summary>
        /// <param name='position'>
        /// 	- The position for the tip of the cone.
        /// </param>
        /// <param name='direction'>
        /// 	- The direction for the cone gets wider in.
        /// </param>
        /// <param name='angle'>
        /// 	- The angle of the cone.
        /// </param>
        /// <param name='duration'>
        /// 	- How long to draw the cone.
        /// </param>
        /// <param name='depthTest'>
        /// 	- Whether or not the cone should be faded when behind other objects.
        /// </param>
        public static void DebugCone(Vector3 position, Vector3 direction, float angle = 45, float duration = 0,
            bool depthTest = true)
        {
            DebugCone(position, direction, Color.white, angle, duration, depthTest);
        }

        /// <summary>
        /// 	- Debugs a cone.
        /// </summary>
        /// <param name='position'>
        /// 	- The position for the tip of the cone.
        /// </param>
        /// <param name='angle'>
        /// 	- The angle of the cone.
        /// </param>
        /// <param name='color'>
        /// 	- The color of the cone.
        /// </param>
        /// <param name='duration'>
        /// 	- How long to draw the cone.
        /// </param>
        /// <param name='depthTest'>
        /// 	- Whether or not the cone should be faded when behind other objects.
        /// </param>
        public static void DebugCone(Vector3 position, Color color, float angle = 45, float duration = 0,
            bool depthTest = true)
        {
            DebugCone(position, Vector3.up, color, angle, duration, depthTest);
        }

        /// <summary>
        /// 	- Debugs a cone.
        /// </summary>
        /// <param name='position'>
        /// 	- The position for the tip of the cone.
        /// </param>
        /// <param name='angle'>
        /// 	- The angle of the cone.
        /// </param>
        /// <param name='duration'>
        /// 	- How long to draw the cone.
        /// </param>
        /// <param name='depthTest'>
        /// 	- Whether or not the cone should be faded when behind other objects.
        /// </param>
        public static void DebugCone(Vector3 position, float angle = 45, float duration = 0, bool depthTest = true)
        {
            DebugCone(position, Vector3.up, Color.white, angle, duration, depthTest);
        }

        /// <summary>
        /// 	- Debugs an arrow.
        /// </summary>
        /// <param name='position'>
        /// 	- The start position of the arrow.
        /// </param>
        /// <param name='direction'>
        /// 	- The direction the arrow will point in.
        /// </param>
        /// <param name='color'>
        /// 	- The color of the arrow.
        /// </param>
        /// <param name='duration'>
        /// 	- How long to draw the arrow.
        /// </param>
        /// <param name='depthTest'>
        /// 	- Whether or not the arrow should be faded when behind other objects. 
        /// </param>
        public static void DebugArrow(Vector3 position, Vector3 direction, Color color, float duration = 0,
            bool depthTest = true, float headLength = 0.1f)
        {
            Debug.DrawRay(position, direction, color, duration, depthTest);
            DebugExt.DebugCone(position + direction, -direction * headLength, color, 15, duration, depthTest);
        }

        /// <summary>
        /// 	- Debugs an arrow.
        /// </summary>
        /// <param name='position'>
        /// 	- The start position of the arrow.
        /// </param>
        /// <param name='direction'>
        /// 	- The direction the arrow will point in.
        /// </param>
        /// <param name='duration'>
        /// 	- How long to draw the arrow.
        /// </param>
        /// <param name='depthTest'>
        /// 	- Whether or not the arrow should be faded when behind other objects. 
        /// </param>
        public static void DebugArrow(Vector3 position, Vector3 direction, float duration = 0, bool depthTest = true)
        {
            DebugArrow(position, direction, Color.white, duration, depthTest);
        }

        public static void DebugArrowByPositions(Vector3 origin, Vector3 destination, Color color, float duration = 0,
            bool depthTest = true)
        {
            DebugArrow(origin, destination - origin, color, duration, depthTest);
        }

        /// <summary>
        /// 	- Debugs a capsule.
        /// </summary>
        /// <param name='start'>
        /// 	- The position of one end of the capsule.
        /// </param>
        /// <param name='end'>
        /// 	- The position of the other end of the capsule.
        /// </param>
        /// <param name='color'>
        /// 	- The color of the capsule.
        /// </param>
        /// <param name='radius'>
        /// 	- The radius of the capsule.
        /// </param>
        /// <param name='duration'>
        /// 	- How long to draw the capsule.
        /// </param>
        /// <param name='depthTest'>
        /// 	- Whether or not the capsule should be faded when behind other objects.
        /// </param>
        public static void DebugCapsule(Vector3 start, Vector3 end, Color color, float radius = 1, float duration = 0,
            bool depthTest = true)
        {
            Vector3 up = (end - start).normalized * radius;
            Vector3 forward = Vector3.Slerp(up, -up, 0.5f);
            Vector3 right = Vector3.Cross(up, forward).normalized * radius;

            float height = (start - end).magnitude;
            float sideLength = Mathf.Max(0, (height * 0.5f) - radius);
            Vector3 middle = (end + start) * 0.5f;

            start = middle + ((start - middle).normalized * sideLength);
            end = middle + ((end - middle).normalized * sideLength);

            //Radial circles
            DebugExt.DebugCircle(start, up, color, radius, duration, depthTest);
            DebugExt.DebugCircle(end, -up, color, radius, duration, depthTest);

            //Side lines
            Debug.DrawLine(start + right, end + right, color, duration, depthTest);
            Debug.DrawLine(start - right, end - right, color, duration, depthTest);

            Debug.DrawLine(start + forward, end + forward, color, duration, depthTest);
            Debug.DrawLine(start - forward, end - forward, color, duration, depthTest);

            for (int i = 1; i < 26; i++)
            {
                //Start endcap
                Debug.DrawLine(Vector3.Slerp(right, -up, i / 25.0f) + start,
                    Vector3.Slerp(right, -up, (i - 1) / 25.0f) + start, color, duration, depthTest);
                Debug.DrawLine(Vector3.Slerp(-right, -up, i / 25.0f) + start,
                    Vector3.Slerp(-right, -up, (i - 1) / 25.0f) + start, color, duration, depthTest);
                Debug.DrawLine(Vector3.Slerp(forward, -up, i / 25.0f) + start,
                    Vector3.Slerp(forward, -up, (i - 1) / 25.0f) + start, color, duration, depthTest);
                Debug.DrawLine(Vector3.Slerp(-forward, -up, i / 25.0f) + start,
                    Vector3.Slerp(-forward, -up, (i - 1) / 25.0f) + start, color, duration, depthTest);

                //End endcap
                Debug.DrawLine(Vector3.Slerp(right, up, i / 25.0f) + end,
                    Vector3.Slerp(right, up, (i - 1) / 25.0f) + end, color, duration, depthTest);
                Debug.DrawLine(Vector3.Slerp(-right, up, i / 25.0f) + end,
                    Vector3.Slerp(-right, up, (i - 1) / 25.0f) + end, color, duration, depthTest);
                Debug.DrawLine(Vector3.Slerp(forward, up, i / 25.0f) + end,
                    Vector3.Slerp(forward, up, (i - 1) / 25.0f) + end, color, duration, depthTest);
                Debug.DrawLine(Vector3.Slerp(-forward, up, i / 25.0f) + end,
                    Vector3.Slerp(-forward, up, (i - 1) / 25.0f) + end, color, duration, depthTest);
            }
        }

        /// <summary>
        /// 	- Debugs a capsule.
        /// </summary>
        /// <param name='start'>
        /// 	- The position of one end of the capsule.
        /// </param>
        /// <param name='end'>
        /// 	- The position of the other end of the capsule.
        /// </param>
        /// <param name='radius'>
        /// 	- The radius of the capsule.
        /// </param>
        /// <param name='duration'>
        /// 	- How long to draw the capsule.
        /// </param>
        /// <param name='depthTest'>
        /// 	- Whether or not the capsule should be faded when behind other objects.
        /// </param>
        public static void DebugCapsule(Vector3 start, Vector3 end, float radius = 1, float duration = 0,
            bool depthTest = true)
        {
            DebugCapsule(start, end, Color.white, radius, duration, depthTest);
        }

        public static void DebugDrawMarker(Vector3 position, float size, Color color, float duration,
            bool depthTest = true)
        {
            Vector3 line1PosA = position + Vector3.up * size * 0.5f;
            Vector3 line1PosB = position - Vector3.up * size * 0.5f;

            Vector3 line2PosA = position + Vector3.right * size * 0.5f;
            Vector3 line2PosB = position - Vector3.right * size * 0.5f;

            Vector3 line3PosA = position + Vector3.forward * size * 0.5f;
            Vector3 line3PosB = position - Vector3.forward * size * 0.5f;

            Debug.DrawLine(line1PosA, line1PosB, color, duration, depthTest);
            Debug.DrawLine(line2PosA, line2PosB, color, duration, depthTest);
            Debug.DrawLine(line3PosA, line3PosB, color, duration, depthTest);
        }

        public static void DebugDrawPlane(Vector3 position, Vector3 normal, float size, Color color, float duration,
            bool depthTest = true)
        {
            Vector3 v3;

            if (normal.normalized != Vector3.forward)
                v3 = Vector3.Cross(normal, Vector3.forward).normalized * normal.magnitude;
            else
                v3 = Vector3.Cross(normal, Vector3.up).normalized * normal.magnitude;
            ;

            Vector3 corner0 = position + v3 * size;
            Vector3 corner2 = position - v3 * size;

            Quaternion q = Quaternion.AngleAxis(90.0f, normal);
            v3 = q * v3;
            Vector3 corner1 = position + v3 * size;
            Vector3 corner3 = position - v3 * size;

            Debug.DrawLine(corner0, corner2, color, duration, depthTest);
            Debug.DrawLine(corner1, corner3, color, duration, depthTest);
            Debug.DrawLine(corner0, corner1, color, duration, depthTest);
            Debug.DrawLine(corner1, corner2, color, duration, depthTest);
            Debug.DrawLine(corner2, corner3, color, duration, depthTest);
            Debug.DrawLine(corner3, corner0, color, duration, depthTest);
            Debug.DrawRay(position, normal * size, color, duration, depthTest);
        }

        public static void DebugDrawVector(Vector3 position, Vector3 direction, float raySize, float markerSize,
            Color color, float duration, bool depthTest = true)
        {
            Debug.DrawRay(position, direction * raySize, color, duration, false);
            DebugDrawMarker(position + direction * raySize, markerSize, color, duration, false);
        }

        public static void DebugDrawTriangle(Vector3 a, Vector3 b, Vector3 c, Color color)
        {
            Debug.DrawLine(a, b, color);
            Debug.DrawLine(b, c, color);
            Debug.DrawLine(c, a, color);
        }

        public static void DebugDrawArrow(Vector3 pos, Vector3 direction, Color color, float arrowHeadLength = 0.25f,
            float arrowHeadAngle = 20.0f)
        {
            Debug.DrawRay(pos, direction, color);

            Vector3 right = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 + arrowHeadAngle, 0) *
                            new Vector3(0, 0, 1);
            Vector3 left = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 - arrowHeadAngle, 0) *
                           new Vector3(0, 0, 1);
            Debug.DrawRay(pos + direction, right * arrowHeadLength, color);
            Debug.DrawRay(pos + direction, left * arrowHeadLength, color);
        }

        public static void DebugDrawArrowToPos(Vector3 pos1, Vector3 pos2, Color color, float arrowHeadLength = 0.25f,
            float arrowHeadAngle = 20.0f)
        {
            DebugDrawArrow(pos1, pos2 - pos1, color, arrowHeadLength, arrowHeadAngle);
        }

        public static void DebugDrawTriangle(Vector3 a, Vector3 b, Vector3 c, Color color, Transform t)
        {
            a = t.TransformPoint(a);
            b = t.TransformPoint(b);
            c = t.TransformPoint(c);

            Debug.DrawLine(a, b, color);
            Debug.DrawLine(b, c, color);
            Debug.DrawLine(c, a, color);
        }

        public static void DebugDrawMesh(Mesh mesh, Color color, Transform t)
        {
            for (int i = 0; i < mesh.triangles.Length; i += 3)
            {
                DebugDrawTriangle(mesh.vertices[mesh.triangles[i]], mesh.vertices[mesh.triangles[i + 1]],
                    mesh.vertices[mesh.triangles[i + 2]], color, t);
            }
        }

        //Para debugar donde se encuentra una posicion con un "asterisco"
        public static void DebugDrawAsterisk(Vector3 pos, Color color, float duration = 0f)
        {
            Debug.DrawRay(pos, Vector3.up, color, duration);
            Debug.DrawRay(pos - Vector3.right * 0.5f, Vector3.right, color, duration);
            Debug.DrawRay(pos - Vector3.forward * 0.5f, Vector3.forward, color, duration);
        }

        #endregion

        #region GizmoDrawFunctions

        public static void GizmoDrawCube(Vector3 position, Quaternion rotation, Vector3 scale)
        {
            Matrix4x4 cubeTransform = Matrix4x4.TRS(position, rotation, scale);
            Matrix4x4 oldGizmosMatrix = Gizmos.matrix;

            Gizmos.matrix *= cubeTransform;

            Gizmos.DrawCube(Vector3.zero, Vector3.one);

            Gizmos.matrix = oldGizmosMatrix;
        }

        public static void GizmoDrawWireCube(Vector3 position, Quaternion rotation, Vector3 scale)
        {
            Matrix4x4 cubeTransform = Matrix4x4.TRS(position, rotation, scale);
            Matrix4x4 oldGizmosMatrix = Gizmos.matrix;

            Gizmos.matrix *= cubeTransform;

            Gizmos.DrawWireCube(Vector3.zero, Vector3.one);

            Gizmos.matrix = oldGizmosMatrix;
        }

        /// <summary>
        /// 	- Draws a point.
        /// </summary>
        /// <param name='position'>
        /// 	- The point to draw.
        /// </param>
        ///  <param name='color'>
        /// 	- The color of the drawn point.
        /// </param>
        /// <param name='scale'>
        /// 	- The size of the drawn point.
        /// </param>
        public static void DrawPoint(Vector3 position, Color color, float scale = 1.0f)
        {
            Color oldColor = Gizmos.color;

            Gizmos.color = color;
            Gizmos.DrawRay(position + (Vector3.up * (scale * 0.5f)), -Vector3.up * scale);
            Gizmos.DrawRay(position + (Vector3.right * (scale * 0.5f)), -Vector3.right * scale);
            Gizmos.DrawRay(position + (Vector3.forward * (scale * 0.5f)), -Vector3.forward * scale);

            Gizmos.color = oldColor;
        }

        /// <summary>
        /// 	- Draws a point.
        /// </summary>
        /// <param name='position'>
        /// 	- The point to draw.
        /// </param>
        /// <param name='scale'>
        /// 	- The size of the drawn point.
        /// </param>
        public static void DrawPoint(Vector3 position, float scale = 1.0f)
        {
            DrawPoint(position, Color.white, scale);
        }

        /// <summary>
        /// 	- Draws an axis-aligned bounding box.
        /// </summary>
        /// <param name='bounds'>
        /// 	- The bounds to draw.
        /// </param>
        /// <param name='color'>
        /// 	- The color of the bounds.
        /// </param>
        public static void DrawBounds(Bounds bounds, Color color)
        {
            Vector3 center = bounds.center;

            float x = bounds.extents.x;
            float y = bounds.extents.y;
            float z = bounds.extents.z;

            Vector3 ruf = center + new Vector3(x, y, z);
            Vector3 rub = center + new Vector3(x, y, -z);
            Vector3 luf = center + new Vector3(-x, y, z);
            Vector3 lub = center + new Vector3(-x, y, -z);

            Vector3 rdf = center + new Vector3(x, -y, z);
            Vector3 rdb = center + new Vector3(x, -y, -z);
            Vector3 lfd = center + new Vector3(-x, -y, z);
            Vector3 lbd = center + new Vector3(-x, -y, -z);

            Color oldColor = Gizmos.color;
            Gizmos.color = color;

            Gizmos.DrawLine(ruf, luf);
            Gizmos.DrawLine(ruf, rub);
            Gizmos.DrawLine(luf, lub);
            Gizmos.DrawLine(rub, lub);

            Gizmos.DrawLine(ruf, rdf);
            Gizmos.DrawLine(rub, rdb);
            Gizmos.DrawLine(luf, lfd);
            Gizmos.DrawLine(lub, lbd);

            Gizmos.DrawLine(rdf, lfd);
            Gizmos.DrawLine(rdf, rdb);
            Gizmos.DrawLine(lfd, lbd);
            Gizmos.DrawLine(lbd, rdb);

            Gizmos.color = oldColor;
        }

        public static void DrawGrid(Vector3 startPosition, float rows, float columns, float tileSize, Color color)
        {
            Color oldColor = Gizmos.color;
            Gizmos.color = color;
            for (int i = 0; i <= rows; i++)
            {
                Gizmos.DrawLine(startPosition + new Vector3(0, 0.01f, i * tileSize),
                    startPosition + new Vector3(columns * tileSize, 0.01f, i * tileSize));
            }

            for (int j = 0; j <= columns; j++)
            {
                Gizmos.DrawLine(startPosition + new Vector3(j * tileSize, 0.01f, 0),
                    startPosition + new Vector3(j * tileSize, 0.01f, rows * tileSize));
            }

            Gizmos.color = oldColor;
        }

        /// <summary>
        /// 	- Draws an axis-aligned bounding box.
        /// </summary>
        /// <param name='bounds'>
        /// 	- The bounds to draw.
        /// </param>
        public static void DrawBounds(Bounds bounds)
        {
            DrawBounds(bounds, Color.white);
        }

        /// <summary>
        /// 	- Draws a local cube.
        /// </summary>
        /// <param name='transform'>
        /// 	- The transform the cube will be local to.
        /// </param>
        /// <param name='size'>
        /// 	- The local size of the cube.
        /// </param>
        /// <param name='center'>
        ///		- The local position of the cube.
        /// </param>
        /// <param name='color'>
        /// 	- The color of the cube.
        /// </param>
        public static void DrawLocalCube(Transform transform, Vector3 size, Color color,
            Vector3 center = default(Vector3))
        {
            Color oldColor = Gizmos.color;
            Gizmos.color = color;

            Vector3 lbb = transform.TransformPoint(center + ((-size) * 0.5f));
            Vector3 rbb = transform.TransformPoint(center + (new Vector3(size.x, -size.y, -size.z) * 0.5f));

            Vector3 lbf = transform.TransformPoint(center + (new Vector3(size.x, -size.y, size.z) * 0.5f));
            Vector3 rbf = transform.TransformPoint(center + (new Vector3(-size.x, -size.y, size.z) * 0.5f));

            Vector3 lub = transform.TransformPoint(center + (new Vector3(-size.x, size.y, -size.z) * 0.5f));
            Vector3 rub = transform.TransformPoint(center + (new Vector3(size.x, size.y, -size.z) * 0.5f));

            Vector3 luf = transform.TransformPoint(center + ((size) * 0.5f));
            Vector3 ruf = transform.TransformPoint(center + (new Vector3(-size.x, size.y, size.z) * 0.5f));

            Gizmos.DrawLine(lbb, rbb);
            Gizmos.DrawLine(rbb, lbf);
            Gizmos.DrawLine(lbf, rbf);
            Gizmos.DrawLine(rbf, lbb);

            Gizmos.DrawLine(lub, rub);
            Gizmos.DrawLine(rub, luf);
            Gizmos.DrawLine(luf, ruf);
            Gizmos.DrawLine(ruf, lub);

            Gizmos.DrawLine(lbb, lub);
            Gizmos.DrawLine(rbb, rub);
            Gizmos.DrawLine(lbf, luf);
            Gizmos.DrawLine(rbf, ruf);

            Gizmos.color = oldColor;
        }

        /// <summary>
        /// 	- Draws a local cube.
        /// </summary>
        /// <param name='transform'>
        /// 	- The transform the cube will be local to.
        /// </param>
        /// <param name='size'>
        /// 	- The local size of the cube.
        /// </param>
        /// <param name='center'>
        ///		- The local position of the cube.
        /// </param>	
        public static void DrawLocalCube(Transform transform, Vector3 size, Vector3 center = default(Vector3))
        {
            DrawLocalCube(transform, size, Color.white, center);
        }

        /// <summary>
        /// 	- Draws a local cube.
        /// </summary>
        /// <param name='space'>
        /// 	- The space the cube will be local to.
        /// </param>
        /// <param name='size'>
        /// 	- The local size of the cube.
        /// </param>
        /// <param name='center'>
        /// 	- The local position of the cube.
        /// </param>
        /// <param name='color'>
        /// 	- The color of the cube.
        /// </param>
        public static void DrawLocalCube(Matrix4x4 space, Vector3 size, Color color, Vector3 center = default(Vector3))
        {
            Color oldColor = Gizmos.color;
            Gizmos.color = color;

            Vector3 lbb = space.MultiplyPoint3x4(center + ((-size) * 0.5f));
            Vector3 rbb = space.MultiplyPoint3x4(center + (new Vector3(size.x, -size.y, -size.z) * 0.5f));

            Vector3 lbf = space.MultiplyPoint3x4(center + (new Vector3(size.x, -size.y, size.z) * 0.5f));
            Vector3 rbf = space.MultiplyPoint3x4(center + (new Vector3(-size.x, -size.y, size.z) * 0.5f));

            Vector3 lub = space.MultiplyPoint3x4(center + (new Vector3(-size.x, size.y, -size.z) * 0.5f));
            Vector3 rub = space.MultiplyPoint3x4(center + (new Vector3(size.x, size.y, -size.z) * 0.5f));

            Vector3 luf = space.MultiplyPoint3x4(center + ((size) * 0.5f));
            Vector3 ruf = space.MultiplyPoint3x4(center + (new Vector3(-size.x, size.y, size.z) * 0.5f));

            Gizmos.DrawLine(lbb, rbb);
            Gizmos.DrawLine(rbb, lbf);
            Gizmos.DrawLine(lbf, rbf);
            Gizmos.DrawLine(rbf, lbb);

            Gizmos.DrawLine(lub, rub);
            Gizmos.DrawLine(rub, luf);
            Gizmos.DrawLine(luf, ruf);
            Gizmos.DrawLine(ruf, lub);

            Gizmos.DrawLine(lbb, lub);
            Gizmos.DrawLine(rbb, rub);
            Gizmos.DrawLine(lbf, luf);
            Gizmos.DrawLine(rbf, ruf);

            Gizmos.color = oldColor;
        }

        /// <summary>
        /// 	- Draws a local cube.
        /// </summary>
        /// <param name='space'>
        /// 	- The space the cube will be local to.
        /// </param>
        /// <param name='size'>
        /// 	- The local size of the cube.
        /// </param>
        /// <param name='center'>
        /// 	- The local position of the cube.
        /// </param>
        public static void DrawLocalCube(Matrix4x4 space, Vector3 size, Vector3 center = default(Vector3))
        {
            DrawLocalCube(space, size, Color.white, center);
        }

        /// <summary>
        /// 	- Draws a circle.
        /// </summary>
        /// <param name='position'>
        /// 	- Where the center of the circle will be positioned.
        /// </param>
        /// <param name='up'>
        /// 	- The direction perpendicular to the surface of the circle.
        /// </param>
        /// <param name='color'>
        /// 	- The color of the circle.
        /// </param>
        /// <param name='radius'>
        /// 	- The radius of the circle.
        /// </param>
        public static void DrawCircle(Vector3 position, Vector3 up, Color color, float radius = 1.0f)
        {
            UnityEngine.Profiling.Profiler.BeginSample("DebugExt::DrawCircle");

            up = ((up == Vector3.zero) ? Vector3.up : up).normalized * radius;
            Vector3 _forward = Vector3.Slerp(up, -up, 0.5f);
            Vector3 _right = Vector3.Cross(up, _forward).normalized * radius;

            Matrix4x4 matrix = new Matrix4x4();

            matrix[0] = _right.x;
            matrix[1] = _right.y;
            matrix[2] = _right.z;

            matrix[4] = up.x;
            matrix[5] = up.y;
            matrix[6] = up.z;

            matrix[8] = _forward.x;
            matrix[9] = _forward.y;
            matrix[10] = _forward.z;

            Vector3 _lastPoint = position + matrix.MultiplyPoint3x4(new Vector3(Mathf.Cos(0), 0, Mathf.Sin(0)));
            Vector3 _nextPoint = Vector3.zero;

            Color oldColor = Gizmos.color;
            Gizmos.color = (color == default(Color)) ? Color.white : color;

            for (var i = 0; i < 91; i++)
            {
                _nextPoint.x = Mathf.Cos((i * 4) * Mathf.Deg2Rad);
                _nextPoint.z = Mathf.Sin((i * 4) * Mathf.Deg2Rad);
                _nextPoint.y = 0;

                _nextPoint = position + matrix.MultiplyPoint3x4(_nextPoint);

                Gizmos.DrawLine(_lastPoint, _nextPoint);
                _lastPoint = _nextPoint;
            }

            Gizmos.color = oldColor;

            UnityEngine.Profiling.Profiler.EndSample();
        }


        /// <summary>
        /// 	- Draws a circle.
        /// </summary>
        /// <param name='position'>
        /// 	- Where the center of the circle will be positioned.
        /// </param>
        /// <param name='color'>
        /// 	- The color of the circle.
        /// </param>
        /// <param name='radius'>
        /// 	- The radius of the circle.
        /// </param>
        public static void DrawCircle(Vector3 position, Color color, float radius = 1.0f)
        {
            DrawCircle(position, Vector3.up, color, radius);
        }

        /// <summary>
        /// 	- Draws a circle.
        /// </summary>
        /// <param name='position'>
        /// 	- Where the center of the circle will be positioned.
        /// </param>
        /// <param name='up'>
        /// 	- The direction perpendicular to the surface of the circle.
        /// </param>
        /// <param name='radius'>
        /// 	- The radius of the circle.
        /// </param>
        public static void DrawCircle(Vector3 position, Vector3 up, float radius = 1.0f)
        {
            DrawCircle(position, position, Color.white, radius);
        }

        /// <summary>
        /// 	- Draws a circle.
        /// </summary>
        /// <param name='position'>
        /// 	- Where the center of the circle will be positioned.
        /// </param>
        /// <param name='radius'>
        /// 	- The radius of the circle.
        /// </param>
        public static void DrawCircle(Vector3 position, float radius = 1.0f)
        {
            DrawCircle(position, Vector3.up, Color.white, radius);
        }

        //Wiresphere already exists

        /// <summary>
        /// 	- Draws a cylinder.
        /// </summary>
        /// <param name='start'>
        /// 	- The position of one end of the cylinder.
        /// </param>
        /// <param name='end'>
        /// 	- The position of the other end of the cylinder.
        /// </param>
        /// <param name='color'>
        /// 	- The color of the cylinder.
        /// </param>
        /// <param name='radius'>
        /// 	- The radius of the cylinder.
        /// </param>
        public static void DrawCylinder(Vector3 start, Vector3 end, Color color, float radius = 1.0f)
        {
            Vector3 up = (end - start).normalized * radius;
            Vector3 forward = Vector3.Slerp(up, -up, 0.5f);
            Vector3 right = Vector3.Cross(up, forward).normalized * radius;

            //Radial circles
            DebugExt.DrawCircle(start, up, color, radius);
            DebugExt.DrawCircle(end, -up, color, radius);
            DebugExt.DrawCircle((start + end) * 0.5f, up, color, radius);

            Color oldColor = Gizmos.color;
            Gizmos.color = color;

            //Side lines
            Gizmos.DrawLine(start + right, end + right);
            Gizmos.DrawLine(start - right, end - right);

            Gizmos.DrawLine(start + forward, end + forward);
            Gizmos.DrawLine(start - forward, end - forward);

            //Start endcap
            Gizmos.DrawLine(start - right, start + right);
            Gizmos.DrawLine(start - forward, start + forward);

            //End endcap
            Gizmos.DrawLine(end - right, end + right);
            Gizmos.DrawLine(end - forward, end + forward);

            Gizmos.color = oldColor;
        }

        /// <summary>
        /// 	- Draws a cylinder.
        /// </summary>
        /// <param name='start'>
        /// 	- The position of one end of the cylinder.
        /// </param>
        /// <param name='end'>
        /// 	- The position of the other end of the cylinder.
        /// </param>
        /// <param name='radius'>
        /// 	- The radius of the cylinder.
        /// </param>
        public static void DrawCylinder(Vector3 start, Vector3 end, float radius = 1.0f)
        {
            DrawCylinder(start, end, Color.white, radius);
        }

        /// <summary>
        /// 	- Draws a cone.
        /// </summary>
        /// <param name='position'>
        /// 	- The position for the tip of the cone.
        /// </param>
        /// <param name='direction'>
        /// 	- The direction for the cone to get wider in.
        /// </param>
        /// <param name='color'>
        /// 	- The color of the cone.
        /// </param>
        /// <param name='angle'>
        /// 	- The angle of the cone.
        /// </param>
        public static void DrawCone(Vector3 position, Vector3 direction, Color color, float angle = 45)
        {
            float length = direction.magnitude;

            Vector3 _forward = direction;
            Vector3 _up = Vector3.Slerp(_forward, -_forward, 0.5f);
            Vector3 _right = Vector3.Cross(_forward, _up).normalized * length;

            direction = direction.normalized;

            Vector3 slerpedVector = Vector3.Slerp(_forward, _up, angle / 90.0f);

            float dist;
            var farPlane = new Plane(-direction, position + _forward);
            var distRay = new Ray(position, slerpedVector);

            farPlane.Raycast(distRay, out dist);

            Color oldColor = Gizmos.color;
            Gizmos.color = color;

            Gizmos.DrawRay(position, slerpedVector.normalized * dist);
            Gizmos.DrawRay(position, Vector3.Slerp(_forward, -_up, angle / 90.0f).normalized * dist);
            Gizmos.DrawRay(position, Vector3.Slerp(_forward, _right, angle / 90.0f).normalized * dist);
            Gizmos.DrawRay(position, Vector3.Slerp(_forward, -_right, angle / 90.0f).normalized * dist);

            DebugExt.DrawCircle(position + _forward, direction, color,
                (_forward - (slerpedVector.normalized * dist)).magnitude);
            DebugExt.DrawCircle(position + (_forward * 0.5f), direction, color,
                ((_forward * 0.5f) - (slerpedVector.normalized * (dist * 0.5f))).magnitude);

            Gizmos.color = oldColor;
        }

        /// <summary>
        /// 	- Draws a cone.
        /// </summary>
        /// <param name='position'>
        /// 	- The position for the tip of the cone.
        /// </param>
        /// <param name='direction'>
        /// 	- The direction for the cone to get wider in.
        /// </param>
        /// <param name='angle'>
        /// 	- The angle of the cone.
        /// </param>
        public static void DrawCone(Vector3 position, Vector3 direction, float angle = 45)
        {
            DrawCone(position, direction, Color.white, angle);
        }

        /// <summary>
        /// 	- Draws a cone.
        /// </summary>
        /// <param name='position'>
        /// 	- The position for the tip of the cone.
        /// </param>
        /// <param name='color'>
        /// 	- The color of the cone.
        /// </param>
        /// <param name='angle'>
        /// 	- The angle of the cone.
        /// </param>
        public static void DrawCone(Vector3 position, Color color, float angle = 45)
        {
            DrawCone(position, Vector3.up, color, angle);
        }

        /// <summary>
        /// 	- Draws a cone.
        /// </summary>
        /// <param name='position'>
        /// 	- The position for the tip of the cone.
        /// </param>
        /// <param name='angle'>
        /// 	- The angle of the cone.
        /// </param>
        public static void DrawCone(Vector3 position, float angle = 45)
        {
            DrawCone(position, Vector3.up, Color.white, angle);
        }

        public static void DrawQuickArrowBetweenPoints(Vector3 start, Vector3 end, Color color,
            float arrowHeadLength = 0.25f, float arrowHeadAngle = 20.0f)
        {
            DrawQuickArrow(start, end - start, color, arrowHeadLength, arrowHeadAngle);
        }

        public static void DrawQuickArrow(Vector3 pos, Vector3 direction, float arrowHeadLength = 0.25f,
            float arrowHeadAngle = 20.0f)
        {
            if (direction == Vector3.zero) return;

            Gizmos.DrawRay(pos, direction);

            Vector3 right = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 + arrowHeadAngle, 0) *
                            new Vector3(0, 0, 1);
            Vector3 left = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 - arrowHeadAngle, 0) *
                           new Vector3(0, 0, 1);
            Gizmos.DrawRay(pos + direction, right * arrowHeadLength);
            Gizmos.DrawRay(pos + direction, left * arrowHeadLength);
        }

        public static void DrawQuickArrow(Vector3 pos, Vector3 direction, Color color, float arrowHeadLength = 0.25f,
            float arrowHeadAngle = 20.0f)
        {
            if (direction == Vector3.zero) return;

            UnityEngine.Profiling.Profiler.BeginSample("DebugExt::DrawQuickArrow");

            Gizmos.color = color;
            Gizmos.DrawRay(pos, direction);

            Vector3 right = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 + arrowHeadAngle, 0) *
                            new Vector3(0, 0, 1);
            Vector3 left = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 - arrowHeadAngle, 0) *
                           new Vector3(0, 0, 1);
            Gizmos.DrawRay(pos + direction, right * arrowHeadLength);
            Gizmos.DrawRay(pos + direction, left * arrowHeadLength);

            UnityEngine.Profiling.Profiler.EndSample();
        }

        public static void DrawCross(Vector3 pos, Color color, float size = 1f)
        {
            Color mainColor = Gizmos.color;
            Gizmos.color = color;

            Vector3 right = pos + Vector3.right * size;
            Vector3 left = pos + Vector3.left * size;
            Vector3 forward = pos + Vector3.forward * size;
            Vector3 backward = pos + Vector3.back * size;

            Gizmos.DrawLine(right, left);
            Gizmos.DrawLine(forward, backward);

            Gizmos.color = mainColor;
        }

        public static void DebugQuickArrowBetweenPoints(Vector3 start, Vector3 end, Color color,
            float arrowHeadLength = 0.25f, float arrowHeadAngle = 20.0f)
        {
            DebugQuickArrow(start, end - start, color, arrowHeadLength, arrowHeadAngle);
        }

        public static void DebugQuickArrow(Vector3 pos, Vector3 direction, float arrowHeadLength = 0.25f,
            float arrowHeadAngle = 20.0f)
        {
            if (direction == Vector3.zero) return;

            Debug.DrawRay(pos, direction);

            Vector3 right = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 + arrowHeadAngle, 0) *
                            new Vector3(0, 0, 1);
            Vector3 left = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 - arrowHeadAngle, 0) *
                           new Vector3(0, 0, 1);
            Debug.DrawRay(pos + direction, right * arrowHeadLength);
            Debug.DrawRay(pos + direction, left * arrowHeadLength);
        }

        public static void DebugQuickArrow(Vector3 pos, Vector3 direction, Color color, float arrowHeadLength = 0.25f,
            float arrowHeadAngle = 20.0f)
        {
            if (direction == Vector3.zero) return;

            Debug.DrawRay(pos, direction, color);

            Vector3 right = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 + arrowHeadAngle, 0) *
                            new Vector3(0, 0, 1);
            Vector3 left = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 - arrowHeadAngle, 0) *
                           new Vector3(0, 0, 1);
            Debug.DrawRay(pos + direction, right * arrowHeadLength, color);
            Debug.DrawRay(pos + direction, left * arrowHeadLength, color);
        }

        /// <summary>
        /// 	- Draws an arrow.
        /// </summary>
        /// <param name='position'>
        /// 	- The start position of the arrow.
        /// </param>
        /// <param name='direction'>
        /// 	- The direction the arrow will point in.
        /// </param>
        /// <param name='color'>
        /// 	- The color of the arrow.
        /// </param>
        public static void DrawArrow(Vector3 position, Vector3 direction, Color color)
        {
            if (direction == Vector3.zero) return;

            Color oldColor = Gizmos.color;
            Gizmos.color = color;

            Gizmos.DrawRay(position, direction);
            DebugExt.DrawCone(position + direction, -direction.normalized, color, 15);

            Gizmos.color = oldColor;
        }

        /// <summary>
        /// 	- Draws an arrow.
        /// </summary>
        /// <param name='position'>
        /// 	- The start position of the arrow.
        /// </param>
        /// <param name='direction'>
        /// 	- The direction the arrow will point in.
        /// </param>
        public static void DrawArrow(Vector3 position, Vector3 direction)
        {
            if (direction == Vector3.zero) return;

            DrawArrow(position, direction, Color.white);
        }

        public static void DrawArrowBetweenPoints(Vector3 start, Vector3 end, Color color)
        {
            DrawArrow(start, end - start, color);
        }

        public static void DrawArrowBetweenPoints(Vector3 start, Vector3 end)
        {
            DrawArrow(start, end - start);
        }

        /// <summary>
        /// 	- Draws a capsule.
        /// </summary>
        /// <param name='start'>
        /// 	- The position of one end of the capsule.
        /// </param>
        /// <param name='end'>
        /// 	- The position of the other end of the capsule.
        /// </param>
        /// <param name='color'>
        /// 	- The color of the capsule.
        /// </param>
        /// <param name='radius'>
        /// 	- The radius of the capsule.
        /// </param>
        public static void DrawCapsule(Vector3 start, Vector3 end, Color color, float radius = 1)
        {
            Vector3 up = (end - start).normalized * radius;
            Vector3 forward = Vector3.Slerp(up, -up, 0.5f);
            Vector3 right = Vector3.Cross(up, forward).normalized * radius;

            Color oldColor = Gizmos.color;
            Gizmos.color = color;

            float height = (start - end).magnitude;
            float sideLength = Mathf.Max(0, (height * 0.5f) - radius);
            Vector3 middle = (end + start) * 0.5f;

            start = middle + ((start - middle).normalized * sideLength);
            end = middle + ((end - middle).normalized * sideLength);

            //Radial circles
            DebugExt.DrawCircle(start, up, color, radius);
            DebugExt.DrawCircle(end, -up, color, radius);

            //Side lines
            Gizmos.DrawLine(start + right, end + right);
            Gizmos.DrawLine(start - right, end - right);

            Gizmos.DrawLine(start + forward, end + forward);
            Gizmos.DrawLine(start - forward, end - forward);

            for (int i = 1; i < 26; i++)
            {
                //Start endcap
                Gizmos.DrawLine(Vector3.Slerp(right, -up, i / 25.0f) + start,
                    Vector3.Slerp(right, -up, (i - 1) / 25.0f) + start);
                Gizmos.DrawLine(Vector3.Slerp(-right, -up, i / 25.0f) + start,
                    Vector3.Slerp(-right, -up, (i - 1) / 25.0f) + start);
                Gizmos.DrawLine(Vector3.Slerp(forward, -up, i / 25.0f) + start,
                    Vector3.Slerp(forward, -up, (i - 1) / 25.0f) + start);
                Gizmos.DrawLine(Vector3.Slerp(-forward, -up, i / 25.0f) + start,
                    Vector3.Slerp(-forward, -up, (i - 1) / 25.0f) + start);

                //End endcap
                Gizmos.DrawLine(Vector3.Slerp(right, up, i / 25.0f) + end,
                    Vector3.Slerp(right, up, (i - 1) / 25.0f) + end);
                Gizmos.DrawLine(Vector3.Slerp(-right, up, i / 25.0f) + end,
                    Vector3.Slerp(-right, up, (i - 1) / 25.0f) + end);
                Gizmos.DrawLine(Vector3.Slerp(forward, up, i / 25.0f) + end,
                    Vector3.Slerp(forward, up, (i - 1) / 25.0f) + end);
                Gizmos.DrawLine(Vector3.Slerp(-forward, up, i / 25.0f) + end,
                    Vector3.Slerp(-forward, up, (i - 1) / 25.0f) + end);
            }

            Gizmos.color = oldColor;
        }

        /// <summary>
        /// 	- Draws a capsule.
        /// </summary>
        /// <param name='start'>
        /// 	- The position of one end of the capsule.
        /// </param>
        /// <param name='end'>
        /// 	- The position of the other end of the capsule.
        /// </param>
        /// <param name='radius'>
        /// 	- The radius of the capsule.
        /// </param>
        public static void DrawCapsule(Vector3 start, Vector3 end, float radius = 1)
        {
            DrawCapsule(start, end, Color.white, radius);
        }

        #endregion

        #region DebugFunctions

        /// <summary>
        /// 	- Gets the methods of an object.
        /// </summary>
        /// <returns>
        /// 	- A list of methods accessible from this object.
        /// </returns>
        /// <param name='obj'>
        /// 	- The object to get the methods of.
        /// </param>
        /// <param name='includeInfo'>
        /// 	- Whether or not to include each method's method info in the list.
        /// </param>
        public static string MethodsOfObject(System.Object obj, bool includeInfo = false)
        {
            string methods = "";
            MethodInfo[] methodInfos = obj.GetType().GetMethods();
            for (int i = 0; i < methodInfos.Length; i++)
            {
                if (includeInfo)
                {
                    methods += methodInfos[i] + "\n";
                }

                else
                {
                    methods += methodInfos[i].Name + "\n";
                }
            }

            return (methods);
        }

        /// <summary>
        /// 	- Gets the methods of a type.
        /// </summary>
        /// <returns>
        /// 	- A list of methods accessible from this type.
        /// </returns>
        /// <param name='type'>
        /// 	- The type to get the methods of.
        /// </param>
        /// <param name='includeInfo'>
        /// 	- Whether or not to include each method's method info in the list.
        /// </param>
        public static string MethodsOfType(System.Type type, bool includeInfo = false)
        {
            string methods = "";
            MethodInfo[] methodInfos = type.GetMethods();
            for (var i = 0; i < methodInfos.Length; i++)
            {
                if (includeInfo)
                {
                    methods += methodInfos[i] + "\n";
                }

                else
                {
                    methods += methodInfos[i].Name + "\n";
                }
            }

            return (methods);
        }

        #endregion
    }
}