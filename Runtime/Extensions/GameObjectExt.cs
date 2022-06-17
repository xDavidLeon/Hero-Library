using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;

namespace HeroLib
{
    /// <summary>
    /// Provides functions for specialized GameObject solutions
    /// </summary>
    public static class GameObjectExt
    {
        public static void SafeRelease<T>(ref T obj) where T : UnityEngine.Object
        {
            if (obj != null)
            {
                GameObject.DestroyImmediate(obj);
                obj = null;
            }
        }

        public static GameObject FindAndReplace(GameObject prefab)
        {
            GameObject g = GameObject.Find(prefab.name);
            if (g) GameObject.Destroy(g);
            return GameObject.Instantiate(prefab);
        }

        public static GameObject InstantiateClone(this GameObject gameObject, string new_name = null)
        {
            GameObject c = GameObject.Instantiate(gameObject) as GameObject;
            if (new_name != null) c.name = new_name;
            else c.name = gameObject.name;
            return c;
        }

        //	public static GameObject InstantiateConnectedClone(this GameObject gameObject, string new_name = null)
        //	{
        //		GameObject c = PrefabUtility.InstantiatePrefab (gameObject) as GameObject;
        //		if (new_name != null) c.name = new_name;
        //		else c.name = gameObject.name;
        //		return c;
        //	}

        public static Rect GetScreenRect(this RectTransform r)
        {
            return new Rect(r.position.x - r.sizeDelta.x / 2, r.position.y - r.sizeDelta.y / 2, r.sizeDelta.x,
                r.sizeDelta.y);
        }

        public static void SetLayerRecursively(this GameObject gameObject, int layer)
        {
            gameObject.layer = layer;
            foreach (Transform t in gameObject.transform)
                t.gameObject.SetLayerRecursively(layer);
        }

        public static void SetMaterialRecursively(this GameObject gameObject, Material[] m)
        {
            if (gameObject.GetComponent<Renderer>()) gameObject.GetComponent<Renderer>().sharedMaterials = m;
            foreach (Transform t in gameObject.transform)
                t.gameObject.SetMaterialRecursively(m);
        }

        public static void SetStaticRecursively(this GameObject gameObject, bool isObjectStatic)
        {
            gameObject.isStatic = isObjectStatic;
            foreach (Transform t in gameObject.transform)
                t.gameObject.SetStaticRecursively(isObjectStatic);
        }

        public static void SetCollisionRecursively(this GameObject gameObject, bool tf)
        {
            Collider[] colliders = gameObject.GetComponentsInChildren<Collider>();
            foreach (Collider collider in colliders)
                collider.enabled = tf;
        }

        public static void SetVisualRecursively(this GameObject gameObject, bool tf)
        {
            Renderer[] renderers = gameObject.GetComponentsInChildren<Renderer>();
            foreach (Renderer renderer in renderers)
                renderer.enabled = tf;
        }

        public static void SetSharedMaterialRecursively(this GameObject gameObject, Material m)
        {
            Renderer[] renderers = gameObject.GetComponentsInChildren<Renderer>();
            foreach (Renderer renderer in renderers)
                renderer.sharedMaterial = m;
        }

        public static T[] GetComponentsInChildrenWithTag<T>(this GameObject gameObject, string tag)
            where T : Component
        {
            List<T> results = new List<T>();

            if (gameObject.CompareTag(tag))
                results.Add(gameObject.GetComponent<T>());

            foreach (Transform t in gameObject.transform)
                results.AddRange(t.gameObject.GetComponentsInChildrenWithTag<T>(tag));

            return results.ToArray();
        }

        public static T GetComponentInParents<T>(this GameObject gameObject)
            where T : Component
        {
            for (Transform t = gameObject.transform; t != null; t = t.parent)
            {
                T result = t.GetComponent<T>();
                if (result != null)
                    return result;
            }

            return null;
        }

        public static T[] GetComponentsInParents<T>(this GameObject gameObject)
            where T : Component
        {
            List<T> results = new List<T>();
            for (Transform t = gameObject.transform; t != null; t = t.parent)
            {
                T result = t.GetComponent<T>();
                if (result != null)
                    results.Add(result);
            }

            return results.ToArray();
        }

        public static int GetCollisionMask(this GameObject gameObject, int layer = -1)
        {
            if (layer == -1)
                layer = gameObject.layer;

            int mask = 0;
            for (int i = 0; i < 32; i++)
                mask |= (Physics.GetIgnoreLayerCollision(layer, i) ? 0 : 1) << i;

            return mask;
        }

        /// <summary>
        /// Gets a GameObject component or adds it if it doesn't exist.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="child"></param>
        /// <returns></returns>
        public static T GetOrAddComponent<T>(this Component child, bool warningInfo = false) where T : Component
        {
            T result = child.GetComponent<T>();
            if (result == null)
            {
                result = child.gameObject.AddComponent<T>();
                if (warningInfo)
                    Debug.LogWarning(
                        "Auto-assigning " + typeof(T).ToString() + " in " + child.name + "! Try to assign it manually!",
                        child.gameObject);
            }

            return result;
        }

        public static void FindChildRecursive(this Transform tr, string name, ref Transform tout)
        {
            if (tout != null) return;
            if (tr == null) return;
            if (tr.name == name)
            {
                tout = tr;
                return;
            }

            foreach (Transform t in tr) t.FindChildRecursive(name, ref tout);
        }

        public static T GetComponentInChildrenRecursive<T>(this Transform t) where T : Component
        {
            T component = t.GetComponent<T>();
            if (component != null) return component;

            foreach (Transform child in t)
            {
                component = child.GetComponentInChildrenRecursive<T>();
                if (component != null) return component;
            }

            return null;
        }

        /// <summary>
        /// Finds a gameObject with a name that contains a string recursively in its childrens
        /// </summary>
        /// <param name="containedString"> string to be contained on the name </param>
        /// <param name="node"> node to start the search on </param>
        /// <param name="tout"> collected transform reference as return value</param>
        public static void FindChildContainingRecursive(this Transform t, string containedString, ref Transform tout)
        {
            if (t.name.Contains(containedString))
            {
                tout = t;
                return;
            }

            foreach (Transform tt in t)
                tt.FindChildContainingRecursive(containedString, ref tout);
        }

        public static int GetChildId(this Transform t)
        {
            Transform parent = t.parent;
            if (parent == null) return 0;

            for (int i = 0; i < parent.childCount; i++)
            {
                if (parent.GetChild(i) == t) return i;
            }

            return 0;
        }

        /// <summary>
        /// Returns all monobehaviours (casted to T)
        /// </summary>
        /// <typeparam name="T">interface type</typeparam>
        /// <param name="gObj"></param>
        /// <returns></returns>
        public static T[] GetInterfaces<T>(this GameObject gObj)
        {
            if (!typeof(T).IsInterface) throw new SystemException("Specified type is not an interface!");
            var mObjs = gObj.GetComponents<MonoBehaviour>();

            return (from a in mObjs where a.GetType().GetInterfaces().Any(k => k == typeof(T)) select (T)(object)a)
                .ToArray();
        }

        /// <summary>
        /// Returns the first monobehaviour that is of the interface type (casted to T)
        /// </summary>
        /// <typeparam name="T">Interface type</typeparam>
        /// <param name="gObj"></param>
        /// <returns></returns>
        public static T GetInterface<T>(this GameObject gObj)
        {
            if (!typeof(T).IsInterface) throw new SystemException("Specified type is not an interface!");
            return gObj.GetInterfaces<T>().FirstOrDefault();
        }

        /// <summary>
        /// Returns the first instance of the monobehaviour that is of the interface type T (casted to T)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="gObj"></param>
        /// <returns></returns>
        public static T GetInterfaceInChildren<T>(this GameObject gObj)
        {
            if (!typeof(T).IsInterface) throw new SystemException("Specified type is not an interface!");
            return gObj.GetInterfacesInChildren<T>().FirstOrDefault();
        }

        /// <summary>
        /// Gets all monobehaviours in children that implement the interface of type T (casted to T)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="gObj"></param>
        /// <returns></returns>
        public static T[] GetInterfacesInChildren<T>(this GameObject gObj)
        {
            if (!typeof(T).IsInterface) throw new SystemException("Specified type is not an interface!");

            var mObjs = gObj.GetComponentsInChildren<MonoBehaviour>();

            return (from a in mObjs where a.GetType().GetInterfaces().Any(k => k == typeof(T)) select (T)(object)a)
                .ToArray();
        }

        public static T GetIComponent<T>(this GameObject t) where T : class
        {
            return t.GetComponent(typeof(T)) as T;
        }

        /// <summary>
        /// Get the component T, or add it to the GameObject if none exists
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetComponentOrAdd<T>(this GameObject obj) where T : Component
        {
            var t = obj.GetComponent<T>();

            if (t == null)
            {
                t = obj.AddComponent<T>();
            }

            return t;
        }

        /// <summary>
        /// Removed component of type T if it exists on the GameObject
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        public static void RemoveComponentIfExists<T>(this GameObject obj) where T : Component
        {
            var t = obj.GetComponent<T>();

            if (t != null)
            {
                UnityEngine.Object.DestroyImmediate(t);
            }
        }

        /// <summary>
        /// Removed components of type T if it exists on the GameObject
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        public static void RemoveComponentsIfExists<T>(this GameObject obj) where T : Component
        {
            var t = obj.GetComponents<T>();

            for (var i = 0; i < t.Length; i++)
            {
                UnityEngine.Object.DestroyImmediate(t[i]);
            }
        }

        /// <summary>
        /// Set enabled property MonoBehaviour of type T if it exists
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="enable"></param>
        /// <returns>True if the component exists</returns>
        public static bool EnableComponentIfExists<T>(this GameObject obj, bool enable = true) where T : MonoBehaviour
        {
            if (obj == null) return false;
            var t = obj.GetComponent<T>();

            if (t == null)
            {
                return false;
            }

            t.enabled = enable;

            return true;
        }

        /// <summary>
        /// Set the layer of a gameobject and all child objects instead of filtered GameObjects
        /// </summary>
        /// <param name="o"></param>
        /// <param name="layer"></param>
        public static void SetLayerRecursive(this GameObject o, int layer, GameObject[] filteredGameObjects)
        {
            SetLayerInternal(o.transform, layer, filteredGameObjects);
        }

        private static void SetLayerInternal(Transform t, int layer, GameObject[] filteredGameObjects)
        {
            if (!filteredGameObjects.Contains(t.gameObject))
            {
                t.gameObject.layer = layer;
            }

            foreach (Transform o in t)
            {
                SetLayerInternal(o, layer, filteredGameObjects);
            }
        }

        /// <summary>
        /// Set the layer of a gameobject and all child objects
        /// </summary>
        /// <param name="o"></param>
        /// <param name="layer"></param>
        public static void SetLayerRecursive(this GameObject o, int layer)
        {
            SetLayerInternal(o.transform, layer);
        }

        private static void SetLayerInternal(Transform t, int layer)
        {
            t.gameObject.layer = layer;

            foreach (Transform o in t)
            {
                SetLayerInternal(o, layer);
            }
        }

        //// Already done by GetComponentsInChildren
        //public static void GetComponentsInChildrenRecursive<T>(this Transform t, ref List<T> tout) where T : Component
        //{
        //    if (tout == null) tout = new List<T>();
        //    T c = t.GetComponent<T>();
        //    if (c != null) tout.Add(c);

        //    foreach (Transform child in t) child.GetComponentsInChildrenRecursive<T>(ref tout);
        //}

        //	public static Transform FindChildRecursive(this Transform tr, string name)
        //	{
        //		if (tr.name == name) return tr;
        //
        //		foreach (Transform t in tr) return t.FindChildRecursive(name);
        ////
        ////
        ////		if (tout != null) return;
        ////		
        ////		if (tr.name == name) 
        ////		{
        ////			tout = tr;
        ////			return;
        ////		}
        ////		
        ////		foreach (Transform t in tr) t.FindChildRecursive(name, ref tout);
        //	}

        public static T GetComponentInChildren<T>(this Component g, GetComponentSafety safety) where T : Component
        {
            var c = g.GetComponentInChildren<T>();
#if !UNITY_EDITOR
        Debug.LogWarning("Safe GetComponent being used. Slowness may ensue");
#endif
            if ((safety & GetComponentSafety.NoNullExpected) == GetComponentSafety.NoNullExpected)
            {
                if (c == null)
                    Debug.LogError(
                        "No Component of type " + (typeof(T)).ToString() + " on Game Object " + g.gameObject.name,
                        g.gameObject);
            }

            if ((safety & GetComponentSafety.SingleResultExpected) == GetComponentSafety.SingleResultExpected)
            {
                if (g.GetComponents<T>().Length > 1)
                    Debug.LogError(
                        "More than one component of type " + (typeof(T)).ToString() + " on Game Object " +
                        g.gameObject.name, g.gameObject);
            }

            return c;
        }

        public static T GetComponent<T>(this Component g, GetComponentSafety safety) where T : Component
        {
            var c = g.GetComponent<T>();
#if !UNITY_EDITOR
        Debug.LogWarning("Safe GetComponent being used. Slowness may ensue");
#endif
            if ((safety & GetComponentSafety.NoNullExpected) == GetComponentSafety.NoNullExpected)
            {
                if (c == null)
                    Debug.LogError(
                        "No Component of type " + (typeof(T)).ToString() + " on Game Object " + g.gameObject.name,
                        g.gameObject);
            }

            if ((safety & GetComponentSafety.SingleResultExpected) == GetComponentSafety.SingleResultExpected)
            {
                if (g.GetComponents<T>().Length > 1)
                    Debug.LogError(
                        "More than one component of type " + (typeof(T)).ToString() + " on Game Object " +
                        g.gameObject.name, g.gameObject);
            }

            return c;
        }

        public enum GetComponentSafety
        {
            None = 0,
            NoNullExpected = 1,
            SingleResultExpected = 2
        }
    }
}