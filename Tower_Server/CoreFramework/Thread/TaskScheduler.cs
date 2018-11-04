using System.Collections.Generic;

namespace CoreFramework.Thread
{
    /// <summary>
    /// 任务管理中心
    /// 使用它可以管理一个或则多个同时运行的任务
    /// </summary>
    public static class TaskScheduler
    {
        private static List<ThreadManager> taskScheduler;

        public static int Count
        {
            get { return taskScheduler.Count; }
        }

        static TaskScheduler()
        {
            taskScheduler = new List<ThreadManager>();
        }

        /// <summary>
        /// 查找任务
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static ThreadManager Find(string name)
        {
            return taskScheduler.Find(task => task.Name == name);
        }

        public static IEnumerator<ThreadManager> GetEnumerator()
        {
            return taskScheduler.GetEnumerator();
        }

        /// <summary>
        /// 终止任务
        /// </summary>
        public static void TerminateAllTask()
        {
            lock (taskScheduler)
            {
                taskScheduler.ForEach(task => task.Close());
                taskScheduler.Clear();
                taskScheduler.TrimExcess();
            }
        }

        internal static void Register(ThreadManager task)
        {
            lock (taskScheduler)
            {
                taskScheduler.Add(task);
            }
        }

        internal static void Unregister(ThreadManager task)
        {
            lock (taskScheduler)
            {
                taskScheduler.Remove(task);
            }
        }
    }
}