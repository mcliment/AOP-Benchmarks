using BenchmarkDotNet.Running;

namespace Benchmarking
{
    class Program
    {
        static void Main(string[] args)
        {
            var switcher = BenchmarkSwitcher.FromTypes(new[]
            {
                typeof(PropertyGetterReflectionBenchmark),
                typeof(PropertySetterReflectionBenchmark),
                typeof(DynamicProxyCallBenchmark),
                typeof(DynamicProxyCreationBenchmark)
            });

            switcher.Run(args);

            // new BenchmarkRunner().RunCompetition(new PropertyGetterReflectionBenchmark());

            // new DynamicProxyCreationBenchmark().NProxyCreation();

            Console.ReadKey();
        }
    }
}
