using UnityEngine;

namespace Dream.Extension.Unity
{
    public static class ERectTransform
    {
        public static RectTransform AddChild(this RectTransform t, RectTransform child)
        {
            if (null == child)
                return null;

            if (child.parent == t)
                return child;

            child.SetParent(t, false);
            return child;
        }

        public static RectTransform RemoveChild(this RectTransform t, RectTransform child)
        {
            if (null == child)
                return null;

            if (child.parent != t)
                return child;

            child.parent = null;

            return child;
        }
    }
}


