using System.Collections.Generic;
using UnityEngine;

namespace HeroLib
{
    /// <summary>
    /// Extension for the standard Transform that allows us to add functions
    /// </summary>
    public static partial class TransformExt
    {
        /// <summary>
        /// Recursively searches for a bone given the name and returns it if found
        /// </summary>
        /// <param name="rParent">Parent to search through</param>
        /// <param name="rBoneName">Bone to find</param>
        /// <returns>Transform of the bone or null</returns>
        public static Transform FindTransform(this Transform rThis, HumanBodyBones rBone)
        {
            Animator lAnimator = rThis.gameObject.GetComponent<Animator>();
            if (lAnimator != null) { return lAnimator.GetBoneTransform(rBone); }

            return null;
        }

        /// <summary>
        /// Recursively searches for a bone given the name and returns it if found
        /// </summary>
        /// <param name="rParent">Parent to search through</param>
        /// <param name="rBoneName">Bone to find</param>
        /// <returns>Transform of the bone or null</returns>
        public static Transform FindTransform(this Transform rThis, string rName)
        {
            return FindChildTransform(rThis, rName);
        }

        /// <summary>
        /// Recursively search for a bone that matches the specifie name
        /// </summary>
        /// <param name="rParent">Parent to search through</param>
        /// <param name="rBoneName">Bone to find</param>
        /// <returns></returns>
        public static Transform FindChildTransform(Transform rParent, string rName)
        {
            string lParentName = rParent.name;

            // We found it. Get out fast
            if (lParentName == rName) { return rParent; }

            // Handle the case where the bone name is nested in a namespace
            int lIndex = lParentName.IndexOf(':');
            if (lIndex >= 0)
            {
                lParentName = lParentName.Substring(lIndex + 1);
                if (lParentName == rName) { return rParent; }
            }

            // Since we didn't find it, check the children
            for (int i = 0; i < rParent.transform.childCount; i++)
            {
                Transform lTransform = FindChildTransform(rParent.transform.GetChild(i), rName);
                if (lTransform != null) { return lTransform; }
            }

            // Return nothing
            return null;
        }

        /// <summary>
        /// Retrieves the chain of transforms that start at the name and goes down the first child
        /// </summary>
        /// <param name="rParent"></param>
        /// <param name="rName"></param>
        /// <param name="rList"></param>
        public static void FindTransformChain(Transform rParent, string rName, ref List<Transform> rList)
        {
            Transform lTransform = rParent.FindTransform(rName);

            rList.Clear();
            while (lTransform != null)
            {
                rList.Add(lTransform);
                if (lTransform.childCount > 0)
                {
                    lTransform = lTransform.GetChild(0);
                }
                else
                {
                    lTransform = null;
                }
            }
        }

        public static Quaternion TransformRotation(this Transform transform, Quaternion rotation)
        {
            return rotation * transform.rotation;
        }

		/// <summary>
		/// Use in cases where GetComponent() had to be used because it was not manually assigned.
		/// </summary>
		public static T GetComponent<T>(this Transform transform, bool warningInfo) where T : Component
		{
			T comp = transform.GetComponent<T>();

			if (warningInfo)
				Debug.LogWarning("Auto-assigning " + comp.GetType().ToString() + " in " + transform.name + "! Try to assign it manually!", comp.gameObject);

			return comp;
		}

		/// <summary>
		/// Use in cases where GetComponent() had to be used because it was not manually assigned.
		/// </summary>
		public static T GetComponentInChildren<T>(this Transform transform, bool includeInactive, bool warningInfo) where T : Component
		{
			T comp = transform.GetComponentInChildren<T>(includeInactive);

			if (warningInfo)
				Debug.LogWarning("Auto-assigning " + comp.GetType().ToString() + " in " + transform.name + "! Try to assign it manually!", comp.gameObject);

			return comp;
		}

		public static T[] GetComponentsInChildren<T>(this Transform transform, bool includeInactive, bool warningInfo) where T : Component
		{
			T[] comp = transform.GetComponentsInChildren<T>(includeInactive);

			if (warningInfo)
				Debug.LogWarning("Auto-assigning " + comp.GetType().ToString() + " in " + transform.name + "! Try to assign it manually!", transform);

			return comp;
		}
		
		/// <summary>
		/// Determines if the "descendant" transform is a child (or grand child)
		/// of the "parent" transform.
		/// </summary>
		/// <param name="rParent"></param>
		/// <param name="rTest"></param>
		/// <returns></returns>
		public static bool IsDescendant(Transform rParent, Transform rDescendant)
		{
			if (rParent == null)
			{
				return false;
			}

			Transform lDescendantParent = rDescendant;
			while (lDescendantParent != null)
			{
				if (lDescendantParent == rParent)
				{
					return true;
				}

				lDescendantParent = lDescendantParent.parent;
			}

			return false;
		}
	}
}
