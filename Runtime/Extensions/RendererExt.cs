using UnityEngine;

namespace HeroLib
{
    /// <summary>
    /// Provides functions for specialized Renderer solutions
    /// </summary>
    public static partial class RendererExt
    {
        public static bool IsVisibleFrom(this Renderer renderer, Camera camera)
        {
            if (camera == null)
            {
                Debug.LogWarning("Camera is null", renderer);
                return false;
            }
            Plane[] planes = GeometryUtility.CalculateFrustumPlanes(camera);
            return GeometryUtility.TestPlanesAABB(planes, renderer.bounds);
        }
    }
}