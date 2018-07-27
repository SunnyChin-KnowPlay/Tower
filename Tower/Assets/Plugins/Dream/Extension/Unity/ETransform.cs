﻿using UnityEngine;

namespace Dream.Extension.Unity
{
    public static class ETransform
    {
        public static Transform AddChild(this Transform t, Transform child)
        {
            if (child.parent == t)
                return child;

            child.parent = t;
            return child;
        }

        public static Transform RemoveChild(this Transform t, Transform child)
        {
            if (child.parent != t)
                return child;

            child.parent = null;
            return child;
        }
    }
}

