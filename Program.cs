using Benchmark;
using BenchmarkDotNet.Running;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace ConsoleClassic1
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BenchmarkRunner.Run<ConcurrentQueueBench>();
            //BenchmarkRunner.Run<ConcurrentQueueBench2>();

            Console.WriteLine("Press any key for exit");
            Console.ReadKey();
        }

        public static void Main0(string[] args)
        {
            var ready = 0;

            for (var n = 0; n < 4; n++)
            {
                Task.Factory.StartNew(() =>
                {
                    while (Volatile.Read(ref ready) != 5)
                    {
                    }

                    Console.WriteLine($"I am done, thread:{Thread.CurrentThread.ManagedThreadId}");

                }, CancellationToken.None, TaskCreationOptions.LongRunning, TaskScheduler.Default);
            }

            Console.WriteLine("Press 5 to stop threads");

            while (true)
            {
                if (Console.ReadLine() == "5")
                {
                    Volatile.Write(ref ready, 5);
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("Try again.");
                }
            }
        }

        public static void Main1(string[] args)
        {
            //Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.High;
            var sw = Stopwatch.StartNew();
            int threadsCompleteCount = 0;

            Console.WriteLine("Environment.ProcessorCount: " + Environment.ProcessorCount);

            int createdThreadsCount = Environment.ProcessorCount * 2;

            Barrier barrier = new Barrier(createdThreadsCount + 1);

            for (var n = 0; n < createdThreadsCount; n++)
            {
                Thread.Sleep(100);

                Task.Factory.StartNew(() =>
                {
                    barrier.SignalAndWait();

                    Console.WriteLine($"I am done, time:{Stopwatch.GetTimestamp()}");

                    Thread.Sleep(1000);

                    Console.WriteLine($"I am done, time:{sw.ElapsedMilliseconds}, thread:{Thread.CurrentThread.ManagedThreadId}");

                    Interlocked.Increment(ref threadsCompleteCount);
                }, CancellationToken.None, TaskCreationOptions.LongRunning, TaskScheduler.Default);
            }

            Console.WriteLine("Main Thread, wait for barrier");

            barrier.SignalAndWait();

            Console.WriteLine("Main Thread, wait for threadsCompleteCount");

            var spinner = new SpinWait();
            while (Volatile.Read(ref threadsCompleteCount) != createdThreadsCount)
            {
                spinner.SpinOnce();
            }

            Console.WriteLine("Main Thread complete");
        }
    }
}
