using System;
using System.Collections.Generic;
using System.Text;

namespace CoreFramework.Utilities
{
    public interface IControl
    {
        /// <summary>
        /// 唤醒，启动
        /// </summary>
        /// <returns></returns>
        bool Start();
        /// <summary>
        /// 关闭，结束
        /// </summary>
        /// <returns></returns>
        bool Shut();
        /// <summary>
        /// 迭代
        /// </summary>
        /// <returns></returns>
        bool Update();
    }
}
