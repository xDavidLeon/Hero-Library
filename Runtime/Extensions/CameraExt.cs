using UnityEngine;

namespace HeroLib
{
    /// <summary>
    /// Provides functions for specialized GameObject solutions
    /// </summary>
    public static class CameraExt
    {
        private static float _screenWidthHalf;
        private static float _screenHeightHalf;

        public static float AngleCameraToTarget(this Transform cam, Transform target)
        {
            Vector3 cameraForwardXZ = cam.forward;
            Vector3 camToObj = (target.position - cam.position).normalized;
            return Vector3.Angle(cameraForwardXZ, camToObj);
        }

        public static Vector2 WorldToScreenPosScaled(this Camera camera, Vector3 position, out bool inFrontOfCamera,
            float scaleFactor = 1.0f)
        {
            UnityEngine.Profiling.Profiler.BeginSample("WorldToScreenPosScaled");

            Vector3 screenCoords = camera.WorldToScreenPoint(position);
            _screenWidthHalf = Screen.width * 0.5f;
            _screenHeightHalf = Screen.height * 0.5f;
            screenCoords.x -= _screenWidthHalf;
            screenCoords.y -= _screenHeightHalf;

            inFrontOfCamera = screenCoords.z >= 0;

            screenCoords.x = screenCoords.x / scaleFactor;
            screenCoords.y = screenCoords.y / scaleFactor;

            UnityEngine.Profiling.Profiler.EndSample();

            return screenCoords;
        }

        /// <summary>
        /// Returns a canvas position given a WorldPosition. If the worldpos is out of canvas this funcion will return a 2D position mapped to an ellipse centered on the screen. 
        /// </summary>
        /// <param name="camera">current camera</param>
        /// <param name="position">world position</param>
        /// <param name="distFromEdgeFactor">Margin dist from screen edges</param>
        /// <param name="IsInsideEllipse">Is this world position inside the canvas?</param>
        /// <param name="pAngle">Angle from center of the canvas to the mapped pos. (0, 1 axis)</param>
        /// <param name="ellipseFactor">Ellipse form. 2 -> Normal Ellipse. WHen this number is greatter the ellipse will be more similiar to a square.</param>
        /// <returns></returns>
        public static Vector2 WorldToScreenPosEllipse(this Camera camera, Vector3 position, float distFromEdgeFactor,
            out bool IsInsideEllipse, out float pAngle, float scaleFactor = 1.0f, float ellipseFactor = 2f)
        {
            UnityEngine.Profiling.Profiler.BeginSample("WorldToScreenPosEllipse");

            Vector2 screenPos;

            //Coordinates Conversion: WorldScreenPoint has the (0,0) point at top left of screen, and we need it on the center
            //Example: on 1080x900 screen center is (540, 450). So, applying the corrdinate conversion... (540 - (1080/2), 450 - (900/2)) = (0,0)
            Vector3 screenCoords = camera.WorldToScreenPoint(position);
            _screenWidthHalf = Screen.width * 0.5f;
            _screenHeightHalf = Screen.height * 0.5f;
            screenCoords.x -= _screenWidthHalf;
            screenCoords.y -= _screenHeightHalf;

            //If target is behind the front of the camera (z is negative)... we need to invert coordinates and angle
            Vector2 toTarget = screenCoords;
            toTarget *= Mathf.Sign(screenCoords.z);
            toTarget.Normalize();

            float angle = Vector2.Angle(Vector2.up, toTarget);

            angle *= Mathf.Sign(screenCoords.z);

            pAngle = Vector2.Angle(Vector2.right, toTarget);
            if (toTarget.y < 0)
                pAngle = 360f - pAngle;

            ///Get ellipse Radius https://stackoverflow.com/questions/33050006/radius-of-an-ellipse-at-a-given-angle-with-given-length-and-width
            float a = _screenWidthHalf * distFromEdgeFactor;
            float b = _screenHeightHalf * distFromEdgeFactor;
            double c = Mathf.Abs(b * Mathf.Cos(pAngle * Mathf.Deg2Rad));
            double s = Mathf.Abs(a * Mathf.Sin(pAngle * Mathf.Deg2Rad));

            double cosPow = 1f;
            double sinPow = 1f;
            for (int i = 0; i < ellipseFactor; i++)
            {
                cosPow *= c;
                sinPow *= s;
            }

            double distFromCenter = (a * b) / System.Math.Pow(cosPow + sinPow, 1 / ellipseFactor);

            //if the target is on front of the camera and the newPos (elipse pos) is inside the elipse, we want to get the main pos
            if (screenCoords.z > 0f &&
                (ellipseFactor <= 0 || screenCoords.SqrMagnitude2D() < distFromCenter * distFromCenter))
            {
                //originalPosition
                screenPos = screenCoords;
                IsInsideEllipse = true;
            }
            else
            {
                //Elipse position
                screenPos = toTarget * (float)distFromCenter;
                IsInsideEllipse = false;
            }

            screenPos.x = screenPos.x / scaleFactor;
            screenPos.y = screenPos.y / scaleFactor;

            UnityEngine.Profiling.Profiler.EndSample();

            return screenPos;
        }

        /// <summary>
        /// A squircle is a square (or rectangle) which corners are rounded by a sphere. The radius of the spehere is determined by "circleRadiusFactor". This factor is a % of the width of the screen. 
        /// https://en.wikipedia.org/wiki/Squircle
        /// </summary>
        public static Vector2 WorldToScreenPosSquircle(this Camera camera, Vector3 position, float distFromEdgeFactor,
            out bool IsInsideSquircle, out float angle, out float perimeterAngle, float scaleFactor = 1.0f,
            float circleRadiusFactor = 0f, bool centerOnTopLeft = false)
        {
            UnityEngine.Profiling.Profiler.BeginSample("WorldToScreenPosSquircle");

            Vector2 screenPos = Vector2.zero;

            //Coordinates Conversion: WorldScreenPoint has the (0,0) point at top left of screen, and we need it on the center
            //Example: on 1080x900 screen center is (540, 450). So, applying the corrdinate conversion... (540 - (1080/2), 450 - (900/2)) = (0,0)
            Vector3 screenCoords = camera.WorldToScreenPoint(position);
            _screenWidthHalf = Screen.width * 0.5f;
            _screenHeightHalf = Screen.height * 0.5f;
            screenCoords.x -= centerOnTopLeft ? 0 : _screenWidthHalf;
            screenCoords.y -= centerOnTopLeft ? 0 : _screenHeightHalf;

            //toTarget is a vector that goes from Vector2.zero to screenCoords.
            Vector2 toTarget = screenCoords;
            toTarget *= Mathf.Sign(screenCoords
                .z); //If target is behind the front of the camera (z is negative)... we need to invert coordinates and angle
            toTarget.Normalize();

            angle = Vector2.Angle(Vector2.right, toTarget);
            if (toTarget.y < 0)
                angle = 360f - angle;

            //t1 & t2 will determine de "time factor" of toTarget where collides with an edge. 
            float t1 = 0f, t2 = 0f, u = 0f;

            //circleCenter is the position of the current circle of our squircle. It depends on the quadrant that toTarget is located to. 
            Vector2 circleCenter = Vector2.zero;
            float circleRadius = circleRadiusFactor * _screenWidthHalf;
            perimeterAngle = 0f;

            //first, we want to know in which edge of the screen the UI will be placed. 
            //As we do this we also check if the UI is inside one of the cercles that defines de roundness of the squirecle corners
            if (angle >= 0f && angle < 90f) //first quadrant (origin = Vector2.zero)
            {
                CheckTopEdge(Vector2.zero, toTarget, out t1, out u);
                CheckRightEdge(Vector2.zero, toTarget, out t2, out u);

                circleCenter.x = _screenWidthHalf - circleRadius;
                circleCenter.y = _screenHeightHalf - circleRadius;

                perimeterAngle = t1 < t2 ? 180f : 90f;
            }
            else if (angle >= 90f && angle < 180f) //second quadrant (origin = Vector2.zero)
            {
                CheckTopEdge(Vector2.zero, toTarget, out t1, out u);
                CheckLeftEdge(Vector2.zero, toTarget, out t2, out u);

                circleCenter.x = -_screenWidthHalf + circleRadius;
                circleCenter.y = _screenHeightHalf - circleRadius;

                perimeterAngle = t1 < t2 ? 180f : 270f;
            }
            else if (angle >= 180 && angle < 270f) //third quadrant (origin = Vector2.zero)
            {
                CheckDownEdge(Vector2.zero, toTarget, out t1, out u);
                CheckLeftEdge(Vector2.zero, toTarget, out t2, out u);

                circleCenter.x = -_screenWidthHalf + circleRadius;
                circleCenter.y = -_screenHeightHalf + circleRadius;

                perimeterAngle = t1 < t2 ? 0f : 270f;
            }
            else if (angle >= 270f && angle < 360f) //fourth quadrant (origin = Vector2.zero)
            {
                CheckDownEdge(Vector2.zero, toTarget, out t1, out u);
                CheckRightEdge(Vector2.zero, toTarget, out t2, out u);

                circleCenter.x = _screenWidthHalf - circleRadius;
                circleCenter.y = -_screenHeightHalf + circleRadius;

                perimeterAngle = t1 < t2 ? 0f : 90f;
            }

            //As the collision test will give always 2 results (collision with the Horizontal and Vertical edges), we get the position of the closest one. (the furthest one will we always outside the screen)
            screenPos = t1 < t2 ? toTarget * t1 : toTarget * t2;

            //if screenPos is in a corner-circle (using abs in order to transform the coordinates to the first cuadrant (only one check instead of 4)
            if (Mathf.Abs(screenPos.x) > _screenWidthHalf - circleRadius &&
                Mathf.Abs(screenPos.y) > _screenHeightHalf - circleRadius)
            {
                //get the posiiton in the circle perimeter and get the rotation angle
                Vector2 circleCenterToPerimeter = (screenPos - circleCenter).normalized;
                perimeterAngle = Vector2.Angle(circleCenterToPerimeter, Vector2.right);
                if (Vector2.Dot(circleCenterToPerimeter, Vector2.up) < 0f)
                    perimeterAngle = 360f - perimeterAngle;

                perimeterAngle += 90f;

                circleCenterToPerimeter = circleCenter + (circleCenterToPerimeter * circleRadius);
                screenPos = circleCenterToPerimeter;
            }

            IsInsideSquircle = false;

            screenPos.x = screenPos.x / scaleFactor;
            screenPos.y = screenPos.y / scaleFactor;

            UnityEngine.Profiling.Profiler.EndSample();

            return screenPos;
        }


        //Clamp to edge code
        private static bool CheckTopEdge(Vector2 A, Vector2 B, out float t, out float u)
        {
            return LinesIntersect(A, B, new Vector2(-_screenWidthHalf, _screenHeightHalf),
                new Vector2(_screenWidthHalf, _screenHeightHalf), out t, out u);
        }

        private static bool CheckDownEdge(Vector2 A, Vector2 B, out float t, out float u)
        {
            return LinesIntersect(A, B, new Vector2(-_screenWidthHalf, -_screenHeightHalf),
                new Vector2(_screenWidthHalf, -_screenHeightHalf), out t, out u);
        }

        private static bool CheckLeftEdge(Vector2 A, Vector2 B, out float t, out float u)
        {
            return LinesIntersect(A, B, new Vector2(-_screenWidthHalf, -_screenHeightHalf),
                new Vector2(-_screenWidthHalf, _screenHeightHalf), out t, out u);
        }

        private static bool CheckRightEdge(Vector2 A, Vector2 B, out float t, out float u)
        {
            return LinesIntersect(A, B, new Vector2(_screenWidthHalf, -_screenHeightHalf),
                new Vector2(_screenWidthHalf, _screenHeightHalf), out t, out u);
        }

        // Determines if the lines AB and CD intersect.
        private static bool LinesIntersect(Vector2 A, Vector2 B, Vector2 C, Vector2 D, out float t, out float u)
        {
            Vector2 CmP = new Vector2(C.x - A.x, C.y - A.y);
            Vector2 r = new Vector2(B.x - A.x, B.y - A.y);
            Vector2 s = new Vector2(D.x - C.x, D.y - C.y);

            float CmPxr = CmP.x * r.y - CmP.y * r.x;
            float CmPxs = CmP.x * s.y - CmP.y * s.x;
            float rxs = r.x * s.y - r.y * s.x;

            t = 0f;
            u = 0;
            if (CmPxr == 0f)
            {
                // Lines are collinear, and so intersect if they have any overlap

                return ((C.x - A.x < 0f) != (C.x - B.x < 0f))
                       || ((C.y - A.y < 0f) != (C.y - B.y < 0f));
            }

            if (rxs == 0f)
                return false; // Lines are parallel.

            float rxsr = 1f / rxs;
            t = CmPxs * rxsr;
            u = CmPxr * rxsr;

            return (t >= 0f) && (u >= 0f);
        }
    }
}