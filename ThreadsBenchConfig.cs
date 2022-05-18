﻿// Maksim Burtsev https://github.com/MBurtsev
// Licensed under the MIT license.

using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Exporters;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Validators;
using System.Globalization;

namespace Benchmark
{
    // Settings for all multi-threaded tests
    public class BenchConfig : ManualConfig
    {
        public BenchConfig()
        {
            AddJob(Job.MediumRun
                .WithLaunchCount(1)
                .WithWarmupCount(3)
                .WithIterationCount(5)
                .WithInvocationCount(1)
                .WithUnrollFactor(1)
                );

            AddColumn(StatisticColumn.Min);
            AddColumn(StatisticColumn.Max);
            AddColumn(StatisticColumn.OperationsPerSecond);

            AddExporter(HtmlExporter.Default);
            AddExporter(MarkdownExporter.GitHub);
            AddExporter(PlainExporter.Default);

            AddValidator(JitOptimizationsValidator.FailOnError);
        }
    }
}
