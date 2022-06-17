using UnityEngine;

namespace HeroLib
{
    [System.Serializable]
    public class CustomTransform
    {
        public Vector3 position;
        public Quaternion rotation;
        public Vector3 scale;

        public CustomTransform()
        {
            position = Vector3.zero;
            rotation = Quaternion.identity;
            scale = Vector3.one;
        }

        public CustomTransform(Vector3 newPosition, Quaternion newRotation, Vector3 newScale)
        {
            position = newPosition;
            rotation = newRotation;
            scale = newScale;
        }

        public void SetValues(Vector3 newPosition, Quaternion newRotation, Vector3 newScale)
        {
            position = newPosition;
            rotation = newRotation;
            scale = newScale;
        }

        public Vector3 TransformPoint(Vector3 point)
        {
            return rotation * Vector3.Scale(point, scale) + position;
        }

        public Vector3 InverseTransformPoint(Vector3 point)
        {
            return Vector3.Scale(new Vector3(1 / scale.x, 1 / scale.y, 1 / scale.z),
                Quaternion.Inverse(rotation) * (point - position));
        }

        public Quaternion TransformRotation(Quaternion rot)
        {
            return rot * rotation;
        }

        public Quaternion InverseTransformRotation(Quaternion rot)
        {
            return Quaternion.Inverse(rotation) * rot;
        }

        public Vector3 InverseTransformDirection(Vector3 dir)
        {
            return Quaternion.Inverse(rotation) * dir;
        }

        public Vector3 TransformDirection(Vector3 dir)
        {
            return rotation * dir;
        }
    }
}