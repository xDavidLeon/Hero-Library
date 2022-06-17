using UnityEngine;

namespace HeroLib
{
    public static class CapsuleExt {

        /// <summary>
        /// Get the center of the upper sphere of the capsule in world coordinates
        /// </summary>
        /// <param name="offset">a positive offset to be added in the direction of the capsule direction</param>
	    public static Vector3 UppderCenter(this CapsuleCollider capsule, float offset = 0f)
        {
            Vector3 axis = capsule.GetCapsuleDir();
            return capsule.bounds.center + (axis * ((capsule.height * 0.5f) - capsule.radius + offset));
        }

        /// <summary>
        /// Get the center of the lower sphere of the capsule in world coordinates
        /// </summary>
        /// <param name="offset">a positive offset to be added in the direction of the capsule direction</param>
        public static Vector3 LowerCenter(this CapsuleCollider capsule, float offset = 0f)
        {
            Vector3 axis = capsule.GetCapsuleDir();
            return capsule.bounds.center + (-axis * ((capsule.height * 0.5f) - capsule.radius - offset));
        }

        /// <summary>
        /// Get the top point of the capsule in world cordinates
        /// </summary>
        /// <param name="offset">a positive offset to be added in the direction of the capsule direction</param>
        public static Vector3 Top(this CapsuleCollider capsule, float offset = 0f)
        {
            Vector3 axis = capsule.GetCapsuleDir();
            return capsule.bounds.center + (axis * ((capsule.height * 0.5f) + offset));
        }

        /// <summary>
        /// Get the bottom point of the capsule  in world cordinates
        /// </summary>
        /// <param name="offset">a positive offset to be added in the direction of the capsule direction</param>
        public static Vector3 Bottom(this CapsuleCollider capsule, float offset = 0f)
        {
            Vector3 axis = capsule.GetCapsuleDir();
            return capsule.bounds.center + (-axis * ((capsule.height * 0.5f) - offset));
        }

        /// <summary>
        /// Get the center of the capsule in world coordinates
        /// </summary>
        /// <param name="offset">a positive offset to be added in the direction of the capsule direction</param>
        public static Vector3 CenterWorld(this CapsuleCollider capsule, float offset = 0f)
        {
            return capsule.bounds.center;
        }

        public static Vector3 GetCapsuleDir(this CapsuleCollider capsule)
        {
            //capsule.direction
            //x = 0
            //y = 1
            //z = 2
            switch (capsule.direction)
            {
                case 0:
                    return capsule.transform.right;
                case 1:
                    return capsule.transform.up;
                case 2:
                    return capsule.transform.forward;
                default:
                    Debug.LogError("Wrong capsule direction", capsule.gameObject);
                    break;
            }

            return capsule.transform.up;
        }
    }
}
