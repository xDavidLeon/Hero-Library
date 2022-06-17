using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace HeroLib
{
    public static class UIExt
    {
        //Get the screen size of an object in pixels, given its distance and diameter.
        // diameter = target.collider.bounds.extents.magnitude;
        public static float DistanceAndDiameterToPixelSize(float distance, float diameter)
        {
            float pixelSize = (diameter * Mathf.Rad2Deg * Screen.height) / (distance * Camera.main.fieldOfView);
            return pixelSize;
        }

        //Get the distance of an object, given its screen size in pixels and diameter.
        public static float PixelSizeAndDiameterToDistance(float pixelSize, float diameter)
        {

            float distance = (diameter * Mathf.Rad2Deg * Screen.height) / (pixelSize * Camera.main.fieldOfView);
            return distance;
        }

        //Get the diameter of an object, given its screen size in pixels and distance.
        public static float PixelSizeAndDistanceToDiameter(float pixelSize, float distance)
        {

            float diameter = (pixelSize * distance * Camera.main.fieldOfView) / (Mathf.Rad2Deg * Screen.height);
            return diameter;
        }

        public static Rect ToScreenSpace(this Bounds bounds, Camera camera)
        {
            var origin = camera.WorldToScreenPoint(new Vector3(bounds.min.x, bounds.max.y, 0.0f));
            var extents = camera.WorldToScreenPoint(new Vector3(bounds.max.x, bounds.min.y, 0.0f));

            return new Rect(origin.x, Screen.height - origin.y, extents.x - origin.x, origin.y - extents.y);
        }

        public static Rect GUIRectWithObject(GameObject go)
        {
            Vector3 cen = go.GetComponent<Renderer>().bounds.center;
            Vector3 ext = go.GetComponent<Renderer>().bounds.extents;
            Vector2[] extentPoints = new Vector2[8]
             {
               WorldToGUIPoint(new Vector3(cen.x-ext.x, cen.y-ext.y, cen.z-ext.z)),
               WorldToGUIPoint(new Vector3(cen.x+ext.x, cen.y-ext.y, cen.z-ext.z)),
               WorldToGUIPoint(new Vector3(cen.x-ext.x, cen.y-ext.y, cen.z+ext.z)),
               WorldToGUIPoint(new Vector3(cen.x+ext.x, cen.y-ext.y, cen.z+ext.z)),
               WorldToGUIPoint(new Vector3(cen.x-ext.x, cen.y+ext.y, cen.z-ext.z)),
               WorldToGUIPoint(new Vector3(cen.x+ext.x, cen.y+ext.y, cen.z-ext.z)),
               WorldToGUIPoint(new Vector3(cen.x-ext.x, cen.y+ext.y, cen.z+ext.z)),
               WorldToGUIPoint(new Vector3(cen.x+ext.x, cen.y+ext.y, cen.z+ext.z))
             };
            Vector2 min = extentPoints[0];
            Vector2 max = extentPoints[0];
            for (int i = 0; i < extentPoints.Length; i++)
            {
                min = Vector2.Min(min, extentPoints[i]);
                max = Vector2.Max(max, extentPoints[i]);
            }
            return new Rect(min.x, min.y, max.x - min.x, max.y - min.y);
        }

        public static Vector2 WorldToGUIPoint(Vector3 world)
        {
            Vector2 screenPoint = Camera.main.WorldToScreenPoint(world);
            screenPoint.y = (float)Screen.height - screenPoint.y;
            return screenPoint;
        }

        public static void SimulateButtonClick(Button button, EventSystem e)
        {
            ExecuteEvents.Execute(button.gameObject, new PointerEventData(e), ExecuteEvents.pointerClickHandler);
        }

        public static void SimulateButtonDeselect(Button button, EventSystem e)
        {
            ExecuteEvents.Execute(button.gameObject, new PointerEventData(e), ExecuteEvents.pointerExitHandler);
        }

        public static void PlaceUIChildrenVertical(this RectTransform r, bool checkInteractable = true)
        {
            int n = r.childCount;
            float sizeY = 1.0f / n;
            int v = 0;
            for (int i = 0; i < n; i++)
            {
                Transform child = r.GetChild(i);
                if (child.gameObject.activeInHierarchy == false) continue;
                if (child.GetComponent<Selectable>() != null &&
                    (checkInteractable && child.GetComponent<Selectable>().IsInteractable() == false)) continue;

                RectTransform rt = child.GetComponent<RectTransform>();
                rt.anchorMin = new Vector2(rt.anchorMin.x, 1.0f - (v + 1.0f) * sizeY);
                rt.anchorMax = new Vector2(rt.anchorMax.x, 1.0f - (v + 0.0f) * sizeY);
                rt.localPosition = Vector3.zero;
                rt.anchoredPosition = Vector2.zero;
                v++;
            }
        }

        public static void PlaceUIChildrenHorizontal(this RectTransform r)
        {
            int n = r.childCount;
            float sizeX = 1.0f / n;
            int v = 0;

            for (int i = 0; i < n; i++)
            {
                Transform child = r.GetChild(i);
                if (child.gameObject.activeInHierarchy == false) continue;
                if (child.GetComponent<Selectable>() != null &&
                    child.GetComponent<Selectable>().IsInteractable() == false) continue;

                RectTransform rt = child.GetComponent<RectTransform>();
                rt.anchorMin = new Vector2(v * sizeX, rt.anchorMin.y);
                rt.anchorMax = new Vector2((v + 1.0f) * sizeX, rt.anchorMax.y);
                rt.localPosition = Vector3.zero;
                rt.anchoredPosition = Vector2.zero;
            }
        }
        
        //public static IEnumerator HoldingButton(PlayerAction button, System.Func<bool> condition, System.Action callbackSucced, System.Action callbackFail, System.Action<float> callbackUpdate, float time)
        //{
        //    float holdingTime = 0;
        //    while (button.IsPressed && holdingTime < time && condition())
        //    {
        //        holdingTime += Time.unscaledDeltaTime;
        //        if (callbackUpdate != null) callbackUpdate(holdingTime / time);
        //        yield return null;
        //    }

        //    if (holdingTime >= time)
        //    {
        //        if (callbackSucced != null) callbackSucced();
        //    }
        //    else
        //    {
        //        if (callbackFail != null)
        //            callbackFail();
        //    }
        //}

        //public static IEnumerator HoldingButton(PlayerAction button, System.Func<bool> condition, System.Action callbackSucced, System.Action<float> callbackFail, System.Action<float> callbackUpdate, float time)
        //{
        //    float holdingTime = 0;
        //    while (button.IsPressed && holdingTime < time && condition())
        //    {
        //        holdingTime += Time.unscaledDeltaTime;
        //        if (callbackUpdate != null) callbackUpdate(holdingTime / time);
        //        yield return null;
        //    }

        //    if (holdingTime >= time)
        //    {
        //        if (callbackSucced != null) callbackSucced();
        //    }
        //    else
        //    {
        //        if (callbackFail != null)
        //            callbackFail(holdingTime / time);
        //    }
        //}
    }
}
