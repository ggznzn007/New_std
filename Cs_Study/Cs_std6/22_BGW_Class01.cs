using System;
using System.ComponentModel;

namespace MultiThrdApp
{
    class Program
    {
        static void Main(string[] args)
        {
            new Program().Execute();
            Console.Read();
        }

        private BackgroundWorker worker;

        public void Execute()
        {
            // 쓰레드풀에서 작업쓰레드 시작
            worker = new BackgroundWorker();
            worker.DoWork += new DoWorkEventHandler(worker_DoWork);
            worker.RunWorkerAsync();
        }

        // 작업쓰레드가 실행할 Task 메서드
        void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            //긴 처리 가정
            Console.WriteLine("Long running task");
        }
    }
}