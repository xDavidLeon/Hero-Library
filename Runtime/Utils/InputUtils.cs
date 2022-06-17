using System;
using HeroLib;
using UnityEngine;

namespace HeroLib
{
    public static class InputUtils
    {
        public static FunctionUpdater CreateMouseDraggingAction(Action<Vector3> onMouseDragging)
        {
            return CreateMouseDraggingAction(0, onMouseDragging);
        }

        public static FunctionUpdater CreateMouseDraggingAction(int mouseButton, Action<Vector3> onMouseDragging)
        {
            bool dragging = false;
            return FunctionUpdater.Create(() =>
            {
                if (Input.GetMouseButtonDown(mouseButton))
                {
                    dragging = true;
                }

                if (Input.GetMouseButtonUp(mouseButton))
                {
                    dragging = false;
                }

                if (dragging)
                {
                    onMouseDragging(UIUtils.GetMouseWorldPosition());
                }

                return false;
            });
        }

        public static FunctionUpdater CreateMouseClickFromToAction(Action<Vector3, Vector3> onMouseClickFromTo,
            Action<Vector3, Vector3> onWaitingForToPosition)
        {
            return CreateMouseClickFromToAction(0, 1, onMouseClickFromTo, onWaitingForToPosition);
        }

        public static FunctionUpdater CreateMouseClickFromToAction(int mouseButton, int cancelMouseButton,
            Action<Vector3, Vector3> onMouseClickFromTo, Action<Vector3, Vector3> onWaitingForToPosition)
        {
            int state = 0;
            Vector3 from = Vector3.zero;
            return FunctionUpdater.Create(() =>
            {
                if (state == 1)
                {
                    if (onWaitingForToPosition != null) onWaitingForToPosition(from, UIUtils.GetMouseWorldPosition());
                }

                if (state == 1 && Input.GetMouseButtonDown(cancelMouseButton))
                {
                    // Cancel
                    state = 0;
                }

                if (Input.GetMouseButtonDown(mouseButton) && !UIUtils.IsPointerOverUI())
                {
                    if (state == 0)
                    {
                        state = 1;
                        from = UIUtils.GetMouseWorldPosition();
                    }
                    else
                    {
                        state = 0;
                        onMouseClickFromTo(from, UIUtils.GetMouseWorldPosition());
                    }
                }

                return false;
            });
        }

        public static FunctionUpdater CreateMouseClickAction(Action<Vector3> onMouseClick)
        {
            return CreateMouseClickAction(0, onMouseClick);
        }

        public static FunctionUpdater CreateMouseClickAction(int mouseButton, Action<Vector3> onMouseClick)
        {
            return FunctionUpdater.Create(() =>
            {
                if (Input.GetMouseButtonDown(mouseButton))
                {
                    onMouseClick(UIUtils.GetWorldPositionFromUI());
                }

                return false;
            });
        }

        public static FunctionUpdater CreateKeyCodeAction(KeyCode keyCode, Action onKeyDown)
        {
            return FunctionUpdater.Create(() =>
            {
                if (Input.GetKeyDown(keyCode))
                {
                    onKeyDown();
                }

                return false;
            });
        }
    }
}