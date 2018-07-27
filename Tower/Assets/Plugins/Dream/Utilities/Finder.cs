using UnityEngine;
using UnityEngine.SceneManagement;

namespace Dream.Utilities
{
    public static class Finder
    {
        /// <summary>
        /// 通过名字在本场景中寻找对象
        /// </summary>
        /// <param name="behaviour"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static GameObject FindObjectInScene(Scene scene, string name)
        {
            var gameObjects = scene.GetRootGameObjects();

            for (int i = 0; i < gameObjects.Length; i++)
            {
                var go = gameObjects[i];
                if (go != null && go.name == name)
                    return go;
            }
            return null;
        }

        /// <summary>
        /// 通过名字在本场景中寻找对象
        /// </summary>
        /// <param name="behaviour"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static GameObject FindObjectInScene(this Behaviour behaviour, string name)
        {
            var scene = behaviour.gameObject.scene;
            return FindObjectInScene(scene, name);
        }

        /// <summary>
        /// 通过名字或路径在场景中寻找对象并找到其中的组件
        /// 如果组件不存在，则会向里面添加一个对应的组件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="behaviour"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static T FindComponentInScene<T>(Scene scene, string name) where T : Behaviour
        {
            var gameObjects = scene.GetRootGameObjects();

            GameObject go = null;

            for (int i = 0; i < gameObjects.Length; i++)
            {
                var temp = gameObjects[i];
                if (temp != null && temp.name == name)
                {
                    go = temp;
                    break;
                }
            }

            if (go == null)
                return null;

            T beh = null;
            beh = go.GetComponent<T>();
            if (beh == null)
            {
                beh = go.AddComponent<T>();
            }

            return beh;
        }

        /// <summary>
        /// 通过名字或路径在场景中寻找对象并找到其中的组件
        /// 如果组件不存在，则会向里面添加一个对应的组件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="behaviour"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static T FindComponentInScene<T>(this Behaviour behaviour, string name) where T : Behaviour
        {
            var scene = behaviour.gameObject.scene;
            return FindComponentInScene<T>(scene, name);
        }

        public static string CalculatePath(this Transform transform)
        {
            string path = "";
            return CalculatePath(transform, path);
        }

        private static string CalculatePath(Transform transform, string path)
        {
            string fullPath;
            if (string.IsNullOrEmpty(path))
            {
                fullPath = transform.name;
            }
            else
            {
                fullPath = string.Format("{0}/{1}", transform.name, path);
            }

            if (transform.parent != null)
            {
                return CalculatePath(transform.parent, fullPath);
            }
            else
            {
                return fullPath;
            }
        }
    }
}
