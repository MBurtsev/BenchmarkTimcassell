using BenchmarkDotNet.Attributes;
using Proto.Utilities.Benchmark;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace Benchmark
{
    [Config(typeof(BenchConfig))]
    public class ConcurrentQueueBench2
    {
        private const int COUNT = 100_000_000;
        private const int THREADS = 2;
        private BenchmarkThreadHelper _helper;
        private ConcurrentQueue<int> _queue;

        [IterationSetup(Target = nameof(Enqueue))]
        public void EnqueueSetup()
        {
            _helper = new BenchmarkThreadHelper();
            _queue = new ConcurrentQueue<int>();

            for (var n = 0; n < THREADS; n++)
            {
                _helper.AddAction(EnqueueWork);
            }
        }

        [Benchmark(OperationsPerInvoke = COUNT * THREADS)]
        public void Enqueue()
        {
            _helper.ExecuteAndWait();
        }

        void EnqueueWork()
        {
            for (var i = 0; i < COUNT; i++)
            {
                _queue.Enqueue(1);
            }
        }
    }
}
