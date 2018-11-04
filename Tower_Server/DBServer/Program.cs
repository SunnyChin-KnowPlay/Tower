using System;
using System.Runtime.InteropServices;
using System.Threading;
using CoreFramework.Thread;
using DBServer.Logic;
using DBServer.MongoDb;
using Microsoft.Extensions.Configuration;

namespace DBServer
{
    class Program
    {

        //定义处理程序委托
        delegate bool ConsoleCtrlDelegate(int dwCtrlType);

        [DllImport("kernel32.dll")]
        private static extern bool SetConsoleCtrlHandler(ConsoleCtrlDelegate HandlerRoutine, bool Add);

        /// <summary>
        /// 主线程
        /// </summary>
        private static ThreadManager mainThread = null;

        /// <summary>
        /// 控制器
        /// </summary>
        private static Controller controller = null;

        /// <summary>
        /// 控制台关闭事件
        /// </summary>
        const int CTRL_CLOSE_EVENT = 2;

        //public static bool HandlerRoutine(int CtrlType)
        //{
        //    switch (CtrlType)
        //    {
        //        case 0:
        //            Console.WriteLine("0工具被强制关闭"); //Ctrl+C关闭
        //            break;
        //        case 2:
        //            Console.WriteLine("2工具被强制关闭");//按控制台关闭按钮关闭
        //            break;
        //    }
        //    Console.ReadLine();
        //    return false;
        //}

        static void Main(string[] args)
        {

            ConsoleCtrlDelegate ctrlDelegate = new ConsoleCtrlDelegate(HandlerRoutine);
            if (SetConsoleCtrlHandler(ctrlDelegate, true))
            {
                Account account = new Account();

                account.AccountName = "sunny";
                account.Password = "111";

                AccountHelper accountHelper = new AccountHelper();
                accountHelper.Insert(account);

                mainThread = new ThreadManager(MainThread, new CycExecution(DateTime.Now, new TimeSpan(0, 0, 0, 0, 30)));
                mainThread.Start();

                controller = new Controller();
                controller.Start();

                while (true)
                {
                    Thread.Sleep(100);
                }
            }
        }

        /// <summary>       
        /// 事件回调
        /// </summary>       
        /// <param name="CtrlType"></param>   
        /// <returns></returns>  
        static bool HandlerRoutine(int CtrlType)
        {
            switch (CtrlType)
            {
                case CTRL_CLOSE_EVENT:       //用户要关闭Console了
                    Console.WriteLine();
                    Console.WriteLine("任务还没有完成，确认要退出吗？（Y/N）");
                    ConsoleKeyInfo ki = Console.ReadKey();

                    if (null != mainThread)
                    {
                        mainThread.Stop();
                    }

                    return ki.Key == ConsoleKey.Y;
                default:
                    return false;
            }
        }

        private static void MainThread(object obj)
        {
            if (null != controller)
            {
                controller.Update();
            }
        }
    }
}
