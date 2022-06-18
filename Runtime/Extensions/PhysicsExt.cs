using UnityEngine;

namespace HeroLib
{
	/// <summary>
	/// Provides extensions to Physics
	/// </summary>
	public static partial class PhysicsExt
	{
		public static bool VisibleFrom(this Collider me, Vector3 position)
		{
			LayerMask l1 = me.gameObject.layer;
			me.gameObject.layer = 2;	//IgnoreRaycast

			bool result = Physics.Linecast(position, me.bounds.center);

			me.gameObject.layer = l1;

			return !result;
		}

		public static bool VisibleFrom(this Collider me, Vector3 position, LayerMask layerMask)
		{
			LayerMask l1 = me.gameObject.layer;
			me.gameObject.layer = 2;    //IgnoreRaycast

			bool result = Physics.Linecast(position, me.bounds.center, layerMask);

			me.gameObject.layer = l1;

			return !result;
		}

		public static bool VisibleFrom(this Collider me, Vector3 position, out RaycastHit hitInfo)
		{
			LayerMask l1 = me.gameObject.layer;
			me.gameObject.layer = 2;    //IgnoreRaycast

			bool result = Physics.Linecast(position, me.bounds.center, out hitInfo);

			me.gameObject.layer = l1;

			return !result;
		}

		public static bool VisibleFrom(this Collider me, Vector3 position, LayerMask layerMask, QueryTriggerInteraction queryTriggerInteraction)
		{
			LayerMask l1 = me.gameObject.layer;
			me.gameObject.layer = 2;    //IgnoreRaycast

			bool result = Physics.Linecast(position, me.bounds.center, layerMask, queryTriggerInteraction);

			me.gameObject.layer = l1;

			return !result;
		}

		public static bool VisibleFrom(this Collider me, Vector3 position, out RaycastHit hitInfo, LayerMask layerMask)
		{
			LayerMask l1 = me.gameObject.layer;
			me.gameObject.layer = 2;    //IgnoreRaycast

			bool result = Physics.Linecast(position, me.bounds.center, out hitInfo, layerMask);

			me.gameObject.layer = l1;

			return !result;
		}

		public static bool VisibleFrom(this Collider me, Vector3 position, out RaycastHit hitInfo, LayerMask layerMask, QueryTriggerInteraction queryTriggerInteraction)
		{
			LayerMask l1 = me.gameObject.layer;
			me.gameObject.layer = 2;    //IgnoreRaycast

			bool result = Physics.Linecast(position, me.bounds.center, out hitInfo, layerMask, queryTriggerInteraction);

			me.gameObject.layer = l1;

			return !result;
		}

		public static bool VisibleFrom(this Transform me, Vector3 position)
		{
			LayerMask l1 = me.gameObject.layer;
			me.gameObject.layer = 2; //IgnoreRaycast

			bool result = Physics.Linecast(position, me.position);

			me.gameObject.layer = l1;

			return !result;
		}

		public static bool VisibleFrom(this Transform me, Vector3 position, LayerMask layerMask)
		{
			LayerMask l1 = me.gameObject.layer;
			me.gameObject.layer = 2;    //IgnoreRaycast

			bool result = Physics.Linecast(position, me.position, layerMask);

			me.gameObject.layer = l1;

			return !result;
		}

		public static bool VisibleFrom(this Transform me, Vector3 position, out RaycastHit hitInfo)
		{
			LayerMask l1 = me.gameObject.layer;
			me.gameObject.layer = 2;    //IgnoreRaycast

			bool result = Physics.Linecast(position, me.position, out hitInfo);

			me.gameObject.layer = l1;

#if UNITY_EDITOR
			Debug.DrawLine(position, me.position, Color.magenta);
#endif
			return !result;
		}

		/// <summary>
		/// Expensive VisibleFrom withh all hits
		/// </summary>
		/// <param name="me"></param>
		/// <param name="position"></param>
		/// <param name="hitInfo"></param>
		/// <returns></returns>
		public static bool VisibleFrom(this Transform me, Vector3 position, out RaycastHit[] hitInfo)
		{
			LayerMask l1 = me.gameObject.layer;
			me.gameObject.layer = 2;    //IgnoreRaycast

			Vector3 vec = me.position - position;
			hitInfo = Physics.RaycastAll(position, vec, vec.magnitude);

			me.gameObject.layer = l1;

			return hitInfo.Length == 0;
		}

		public static bool VisibleFrom(this Transform me, Vector3 position, LayerMask layerMask, QueryTriggerInteraction queryTriggerInteraction)
		{
			LayerMask l1 = me.gameObject.layer;
			me.gameObject.layer = 2;    //IgnoreRaycast

			bool result = Physics.Linecast(position, me.position, layerMask, queryTriggerInteraction);

			me.gameObject.layer = l1;

			return !result;
		}

		public static bool VisibleFrom(this Transform me, Vector3 position, out RaycastHit hitInfo, LayerMask layerMask)
		{
			LayerMask l1 = me.gameObject.layer;
			me.gameObject.layer = 2;    //IgnoreRaycast

			bool result = Physics.Linecast(position, me.position, out hitInfo, layerMask);

			me.gameObject.layer = l1;

			return !result;
		}

		public static bool VisibleFrom(this Transform me, Vector3 position, out RaycastHit hitInfo, LayerMask layerMask, QueryTriggerInteraction queryTriggerInteraction)
		{
			LayerMask l1 = me.gameObject.layer;
			me.gameObject.layer = 2;    //IgnoreRaycast

			bool result = Physics.Linecast(position, me.position, out hitInfo, layerMask, queryTriggerInteraction);

			me.gameObject.layer = l1;

			return !result;
		}
	}
}
