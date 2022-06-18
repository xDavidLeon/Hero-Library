using UnityEngine;
using UnityEngine.AI;

namespace HeroLib
{
    /// <summary>
    /// Provides extensions to Navigation systems
    /// </summary>
    public static partial class NavExt
    {
        private static Vector3[] tempCorners = new Vector3[128];
        private static NavMeshPath navMeshPath = new NavMeshPath();

        /// <summary>
        /// WARNING! Returns 0 if path is not Complete.
        /// </summary>
        public static float GetPathLengthNonAlloc(Vector3 origin, Vector3 destiny, LayerMask scenarioMask,
            int walkableAreaMask, bool debug = false)
        {
            origin = GetGroundPos(origin, scenarioMask, 0.1f, 0.5f);
            destiny = GetGroundPos(destiny, scenarioMask, 0.1f, 0.5f);
            if (origin != Vector3.zero && destiny != Vector3.zero)
            {
                NavMesh.CalculatePath(origin, destiny, walkableAreaMask, navMeshPath);
                if (navMeshPath.status == NavMeshPathStatus.PathComplete)
                {
                    tempCorners.ResetArray();
                    navMeshPath.GetCornersNonAlloc(tempCorners);

                    if (debug)
                        navMeshPath.DebugPath(Color.green, 0.1f);

                    return tempCorners.GetDistanceSummatory();
                }
            }

            if (debug)
                navMeshPath.DebugPath(Color.red, 0.1f);

            return 0f;
        }

        //Raycasts "pos" using Vector3.down*maxDist
        public static Vector3 GetGroundPos(Vector3 pos, LayerMask collisionMask, float initialVOffset = 0.1f,
            float maxDist = 5f)
        {
            ProfilingUtils.BeginSample("NavExt::GetGroundPos()");

            if (collisionMask == 0)
            {
                collisionMask = 1; //Layer Default
            }

            Vector3 groundPos = Vector3.zero;
            RaycastExt.RayBetweenPoints(pos + Vector3.up * initialVOffset, pos + Vector3.down * maxDist, collisionMask,
                ref groundPos);

            ProfilingUtils.EndSample("NavExt::GetGroundPos()");
            return groundPos;
        }

        ///<summary>
        /// Llena points de puntos generados según algoritmo de compás con pos como centro (el número de puntos es points.Length)
        /// Si groundPoints es true, se hará ray tracing para que los puntos estén en el suelo. Los que no lo estén serán Vector3.zero
        ///</summary>
        public static void GeneratePointsAroundPosCompass(Vector3 pivot, ref Vector3[] points, LayerMask collisionMask,
            float minRadius = 1f, float maxRadius = 5f, bool groundPoints = false, float yOffset = 1.0f)
        {
            float numOfPoints = points.Length;

            if (numOfPoints == 0) return;

            if (collisionMask == 0)
            {
                collisionMask = LayerMask.NameToLayer("Default");
            }

            if (groundPoints) pivot += Vector3.up * yOffset;
            Vector3 dir = Vector3.right;
            Vector3 deltaRot = Vector3.zero;
            deltaRot.y = 360f / numOfPoints;
            Vector3 testPos = Vector3.zero;

            //Rotar vector un random para que cada vez lo haga distinto
            Vector3Ext.RotateVector(ref dir, UnityEngine.Random.Range(0f, 360f), Vector3.up);

            //obtener posiciones alrededor, en el mismo plano.
            for (int i = 0; i < numOfPoints; i++)
            {
                testPos = pivot + dir * UnityEngine.Random.Range(minRadius, maxRadius);

                if (groundPoints)
                {
                    Vector3 endPos = testPos + Vector3.down * yOffset * 2f;
                    //Mirar si un rayo lanzado desde ahi llega al suelo
                    if (RaycastExt.RayBetweenPoints(testPos, endPos, collisionMask, ref points[i]))
                    {
                        //Si sí, la colisión ya se ha asignado por referencia. Seguimos.
                    }
                    else
                    {
                        points[i] = Vector3.zero;
                    }
                }
                else
                {
                    points[i] = testPos;
                }

                //Rotar vector
                dir = Quaternion.Euler(deltaRot) * dir;
            }
        }

        /// <summary>
        /// Returns true if _position is Walkable
        /// </summary>
        /// <param name="newTarget"></param>
        /// <returns></returns>
        public static bool CheckWalkablePos(ref Vector3 _position, float maxDistance = 1f)
        {
            NavMeshHit hit;
            if (NavMesh.SamplePosition(_position, out hit, maxDistance, NavMesh.AllAreas))
            {
                _position = hit.position;
                return true;
            }

            return false;
        }

        public static Vector3 GetLastCorner(this NavMeshPath path)
        {
            if (path.corners.Length == 0)
            {
                Debug.LogError("MiscExt: GetLastCorner() Error, NavMeshPath with corners.Length == 0.");
                return Vector3.zero;
            }

            return path.corners[path.corners.Length - 1];
        }

        /// <summary>
        /// Sum of distance between all corners
        /// </summary>
        /// <param name="path"></param>
        /// <param name="sqrMagnitude"></param>
        /// <param name="printInfo"></param>
        /// <returns></returns>
        static public float GetLength(this NavMeshPath path)
        {
            return path.corners.GetDistanceSummatory();
        }

        /// <summary>
        /// Returns the distance between the first and the last corners
        /// </summary>
        /// <param name="path"></param>
        /// <param name="sqrMagnitude"></param>
        /// <returns></returns>
        static public float GetDistance(this NavMeshPath path)
        {
            Vector3[] corners = path.corners;

            if (corners.Length == 0 || corners.Length == 1)
                return 0f;

            return (corners[0] + corners[corners.Length - 1]).sqrMagnitude;
        }

        static public void DebugPath(this NavMeshPath path, Color color, float duration = 0f)
        {
            Vector3[] corners = path.corners;
            Vector3 upOffset = Vector3.up * .0f;

            if (corners.Length > 1)
            {
                for (int i = 0; i < corners.Length - 1; i++)
                {
                    Debug.DrawLine(corners[i] + upOffset, corners[i + 1] + upOffset, color, duration);
                }
            }
        }

        static public void DebugPath(this Vector3[] corners, Color color, float duration = 0f)
        {
            Vector3 upOffset = Vector3.up * .0f;

            if (corners.Length > 1)
            {
                for (int i = 0; i < corners.Length - 1; i++)
                {
                    if (i + 1 < corners.Length)
                    {
                        if (corners[i + 1] == Vector3.zero)
                            continue;
                    }

                    Debug.DrawLine(corners[i] + upOffset, corners[i + 1] + upOffset, color, duration);
                }
            }
        }
    }
}