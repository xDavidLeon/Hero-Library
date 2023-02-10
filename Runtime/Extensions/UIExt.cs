using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace HeroLib
{
    public static partial class UIExt
    {
        private static readonly Vector3 Vector3zero = Vector3.zero;
        private static readonly Vector3 Vector3one = Vector3.one;
        private static readonly Vector3 Vector3yDown = new Vector3(0, -1);

        public const int sortingOrderDefault = 5000;

        // Get Sorting order to set SpriteRenderer sortingOrder, higher position = lower sortingOrder
        public static int GetSortingOrder(Vector3 position, int offset, int baseSortingOrder = sortingOrderDefault)
        {
            return (int)(baseSortingOrder - position.y) + offset;
        }

        // Get Main Canvas Transform
        private static Transform cachedCanvasTransform;

        public static Transform GetCanvasTransform()
        {
            if (cachedCanvasTransform == null)
            {
                Canvas canvas = MonoBehaviour.FindObjectOfType<Canvas>();
                if (canvas != null)
                {
                    cachedCanvasTransform = canvas.transform;
                }
            }

            return cachedCanvasTransform;
        }

        // Get Default Unity Font, used in text objects if no font given
        public static Font GetDefaultFont()
        {
            return Resources.GetBuiltinResource<Font>("Arial.ttf");
        }

        // Create a Sprite in the World, no parent
        public static GameObject CreateWorldSprite(string name, Sprite sprite, Vector3 position, Vector3 localScale,
            int sortingOrder, Color color)
        {
            return CreateWorldSprite(null, name, sprite, position, localScale, sortingOrder, color);
        }

        // Create a Sprite in the World
        public static GameObject CreateWorldSprite(Transform parent, string name, Sprite sprite, Vector3 localPosition,
            Vector3 localScale, int sortingOrder, Color color)
        {
            GameObject gameObject = new GameObject(name, typeof(SpriteRenderer));
            Transform transform = gameObject.transform;
            transform.SetParent(parent, false);
            transform.localPosition = localPosition;
            transform.localScale = localScale;
            SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = sprite;
            spriteRenderer.sortingOrder = sortingOrder;
            spriteRenderer.color = color;
            return gameObject;
        }

        // Create a Sprite in the World with Button_Sprite, no parent
        public static Button_Sprite CreateWorldSpriteButton(string name, Sprite sprite, Vector3 localPosition,
            Vector3 localScale, int sortingOrder, Color color)
        {
            return CreateWorldSpriteButton(null, name, sprite, localPosition, localScale, sortingOrder, color);
        }

        // Create a Sprite in the World with Button_Sprite
        public static Button_Sprite CreateWorldSpriteButton(Transform parent, string name, Sprite sprite,
            Vector3 localPosition, Vector3 localScale, int sortingOrder, Color color)
        {
            GameObject gameObject =
                CreateWorldSprite(parent, name, sprite, localPosition, localScale, sortingOrder, color);
            gameObject.AddComponent<BoxCollider2D>();
            Button_Sprite buttonSprite = gameObject.AddComponent<Button_Sprite>();
            return buttonSprite;
        }

        // Creates a Text Mesh in the World and constantly updates it
        public static FunctionUpdater CreateWorldTextUpdater(Func<string> GetTextFunc, Vector3 localPosition,
            Transform parent = null)
        {
            TextMesh textMesh = CreateWorldText(GetTextFunc(), parent, localPosition);
            return FunctionUpdater.Create(() =>
            {
                textMesh.text = GetTextFunc();
                return false;
            }, "WorldTextUpdater");
        }

        // Create Text in the World
        public static TextMesh CreateWorldText(string text, Transform parent = null,
            Vector3 localPosition = default(Vector3), int fontSize = 40, Color? color = null,
            TextAnchor textAnchor = TextAnchor.UpperLeft, TextAlignment textAlignment = TextAlignment.Left,
            int sortingOrder = sortingOrderDefault)
        {
            if (color == null) color = Color.white;
            return CreateWorldText(parent, text, localPosition, fontSize, (Color)color, textAnchor, textAlignment,
                sortingOrder);
        }

        // Create Text in the World
        public static TextMesh CreateWorldText(Transform parent, string text, Vector3 localPosition, int fontSize,
            Color color, TextAnchor textAnchor, TextAlignment textAlignment, int sortingOrder)
        {
            GameObject gameObject = new GameObject("World_Text", typeof(TextMesh));
            Transform transform = gameObject.transform;
            transform.SetParent(parent, false);
            transform.localPosition = localPosition;
            TextMesh textMesh = gameObject.GetComponent<TextMesh>();
            textMesh.anchor = textAnchor;
            textMesh.alignment = textAlignment;
            textMesh.text = text;
            textMesh.fontSize = fontSize;
            textMesh.color = color;
            textMesh.GetComponent<MeshRenderer>().sortingOrder = sortingOrder;
            return textMesh;
        }


        // Create a Text Popup in the World, no parent
        public static void CreateWorldTextPopup(string text, Vector3 localPosition)
        {
            CreateWorldTextPopup(null, text, localPosition, 20, Color.white, localPosition + new Vector3(0, 10), 1f);
        }

        // Create a Text Popup in the World
        public static void CreateWorldTextPopup(Transform parent, string text, Vector3 localPosition, int fontSize,
            Color color, Vector3 finalPopupPosition, float popupTime)
        {
            TextMesh textMesh = CreateWorldText(parent, text, localPosition, fontSize, color, TextAnchor.LowerLeft,
                TextAlignment.Left, sortingOrderDefault);
            Transform transform = textMesh.transform;
            Vector3 moveAmount = (finalPopupPosition - localPosition) / popupTime;
            FunctionUpdater.Create(delegate()
            {
                transform.position += moveAmount * Time.deltaTime;
                popupTime -= Time.deltaTime;
                if (popupTime <= 0f)
                {
                    UnityEngine.Object.Destroy(transform.gameObject);
                    return true;
                }
                else
                {
                    return false;
                }
            }, "WorldTextPopup");
        }

        // Create Text Updater in UI
        public static FunctionUpdater CreateUITextUpdater(Func<string> GetTextFunc, Vector2 anchoredPosition)
        {
            Text text = DrawTextUI(GetTextFunc(), anchoredPosition, 20, GetDefaultFont());
            return FunctionUpdater.Create(() =>
            {
                text.text = GetTextFunc();
                return false;
            }, "UITextUpdater");
        }


        // Draw a UI Sprite
        public static RectTransform DrawSprite(Color color, Transform parent, Vector2 pos, Vector2 size,
            string name = null)
        {
            RectTransform rectTransform = DrawSprite(null, color, parent, pos, size, name);
            return rectTransform;
        }

        // Draw a UI Sprite
        public static RectTransform DrawSprite(Sprite sprite, Transform parent, Vector2 pos, Vector2 size,
            string name = null)
        {
            RectTransform rectTransform = DrawSprite(sprite, Color.white, parent, pos, size, name);
            return rectTransform;
        }

        // Draw a UI Sprite
        public static RectTransform DrawSprite(Sprite sprite, Color color, Transform parent, Vector2 pos, Vector2 size,
            string name = null)
        {
            // Setup icon
            if (name == null || name == "") name = "Sprite";
            GameObject go = new GameObject(name, typeof(RectTransform), typeof(Image));
            RectTransform goRectTransform = go.GetComponent<RectTransform>();
            goRectTransform.SetParent(parent, false);
            goRectTransform.sizeDelta = size;
            goRectTransform.anchoredPosition = pos;

            Image image = go.GetComponent<Image>();
            image.sprite = sprite;
            image.color = color;

            return goRectTransform;
        }

        public static Text DrawTextUI(string textString, Vector2 anchoredPosition, int fontSize, Font font)
        {
            return DrawTextUI(textString, GetCanvasTransform(), anchoredPosition, fontSize, font);
        }

        public static Text DrawTextUI(string textString, Transform parent, Vector2 anchoredPosition, int fontSize,
            Font font)
        {
            GameObject textGo = new GameObject("Text", typeof(RectTransform), typeof(Text));
            textGo.transform.SetParent(parent, false);
            Transform textGoTrans = textGo.transform;
            textGoTrans.SetParent(parent, false);
            textGoTrans.localPosition = Vector3zero;
            textGoTrans.localScale = Vector3one;

            RectTransform textGoRectTransform = textGo.GetComponent<RectTransform>();
            textGoRectTransform.sizeDelta = new Vector2(0, 0);
            textGoRectTransform.anchoredPosition = anchoredPosition;

            Text text = textGo.GetComponent<Text>();
            text.text = textString;
            text.verticalOverflow = VerticalWrapMode.Overflow;
            text.horizontalOverflow = HorizontalWrapMode.Overflow;
            text.alignment = TextAnchor.MiddleLeft;
            if (font == null) font = GetDefaultFont();
            text.font = font;
            text.fontSize = fontSize;

            return text;
        }


        // Parse a float, return default if failed
        public static float Parse_Float(string txt, float _default)
        {
            float f;
            if (!float.TryParse(txt, out f))
            {
                f = _default;
            }

            return f;
        }

        // Parse a int, return default if failed
        public static int Parse_Int(string txt, int _default)
        {
            int i;
            if (!int.TryParse(txt, out i))
            {
                i = _default;
            }

            return i;
        }

        public static int Parse_Int(string txt)
        {
            return Parse_Int(txt, -1);
        }


        // Get Mouse Position in World with Z = 0f
        public static Vector3 GetMouseWorldPosition()
        {
            return GetMouseWorldPosition(Camera.main);
        }

        public static Vector3 GetMouseWorldPosition(Camera worldCamera)
        {
            return GetMouseWorldPosition(Input.mousePosition, worldCamera);
        }

        public static Vector3 GetMouseWorldPosition(Vector3 screenPosition, Camera worldCamera)
        {
            var vec = GetMouseWorldPositionWithZ(screenPosition, worldCamera);
            vec.z = 0f;
            return vec;
        }

        public static Vector3 GetMouseWorldPositionWithZ()
        {
            return GetMouseWorldPositionWithZ(Input.mousePosition, Camera.main);
        }

        public static Vector3 GetMouseWorldPositionWithZ(Camera worldCamera)
        {
            return GetMouseWorldPositionWithZ(Input.mousePosition, worldCamera);
        }

        public static Vector3 GetMouseWorldPositionWithZ(Vector3 screenPosition, Camera worldCamera)
        {
            Vector3 worldPosition = worldCamera.ScreenToWorldPoint(screenPosition);
            return worldPosition;
        }

        public static Vector3 GetMouseWorldPosition(Camera worldCamera, LayerMask mouseColliderLayerMask)
        {
            Ray ray = worldCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, mouseColliderLayerMask))
                return raycastHit.point;
            return Vector3.zero;
        }
        
        /// <summary>
        /// Use a plane to get world position
        /// </summary>
        /// <param name="screenPosition"></param>
        /// <param name="worldCamera"></param>
        /// <param name="planeDepth"></param>
        /// <returns></returns>
        public static Vector3 GetMouseWorldPosition(Vector3 screenPosition, Camera worldCamera, float planeDepth)
        {
            Plane plane = new Plane(Vector3.up, planeDepth);
            Ray ray = worldCamera.ScreenPointToRay(screenPosition);

            if (plane.Raycast(ray, out var distance))
                return ray.GetPoint(distance);

            return Vector3.zero;
        }

        // Is Mouse over a UI Element? Used for ignoring World clicks through UI
        public static bool IsPointerOverUI()
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return true;
            }
            else
            {
                PointerEventData pe = new PointerEventData(EventSystem.current);
                pe.position = Input.mousePosition;
                List<RaycastResult> hits = new List<RaycastResult>();
                EventSystem.current.RaycastAll(pe, hits);
                return hits.Count > 0;
            }
        }

        // Get UI Position from World Position
        public static Vector2 GetWorldUIPosition(Vector3 worldPosition, Transform parent, Camera uiCamera,
            Camera worldCamera)
        {
            Vector3 screenPosition = worldCamera.WorldToScreenPoint(worldPosition);
            Vector3 uiCameraWorldPosition = uiCamera.ScreenToWorldPoint(screenPosition);
            Vector3 localPos = parent.InverseTransformPoint(uiCameraWorldPosition);
            return new Vector2(localPos.x, localPos.y);
        }

        public static Vector3 GetWorldPositionFromUIZeroZ()
        {
            Vector3 vec = GetWorldPositionFromUI(Input.mousePosition, Camera.main);
            vec.z = 0f;
            return vec;
        }

        // Get World Position from UI Position
        public static Vector3 GetWorldPositionFromUI()
        {
            return GetWorldPositionFromUI(Input.mousePosition, Camera.main);
        }

        public static Vector3 GetWorldPositionFromUI(Camera worldCamera)
        {
            return GetWorldPositionFromUI(Input.mousePosition, worldCamera);
        }

        public static Vector3 GetWorldPositionFromUI(Vector3 screenPosition, Camera worldCamera)
        {
            Vector3 worldPosition = worldCamera.ScreenToWorldPoint(screenPosition);
            return worldPosition;
        }

        public static Vector3 GetWorldPositionFromUI_Perspective()
        {
            return GetWorldPositionFromUI_Perspective(Input.mousePosition, Camera.main);
        }

        public static Vector3 GetWorldPositionFromUI_Perspective(Camera worldCamera)
        {
            return GetWorldPositionFromUI_Perspective(Input.mousePosition, worldCamera);
        }

        public static Vector3 GetWorldPositionFromUI_Perspective(Vector3 screenPosition, Camera worldCamera)
        {
            Ray ray = worldCamera.ScreenPointToRay(screenPosition);
            Plane xy = new Plane(Vector3.forward, new Vector3(0, 0, 0f));
            float distance;
            xy.Raycast(ray, out distance);
            return ray.GetPoint(distance);
        }
        
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
