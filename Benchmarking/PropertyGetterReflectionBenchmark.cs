using System.Linq.Expressions;
using System.Reflection;
using BenchmarkDotNet.Attributes;

namespace Benchmarking
{
    public class PropertyGetterReflectionBenchmark
    {
        private readonly PropertyInfo _cached;
        private readonly Delegate _delegate;
        private readonly Func<object> _lambda;
        private readonly Func<object> _typedDelegate;
        
        public string Property { get; set; }
        string? _toSet;

        public PropertyGetterReflectionBenchmark()
        {
            Property = "a";

            _cached = GetType().GetProperties().First();
            _delegate = _cached.GetMethod!.CreateDelegate(typeof(Func<String>), this);
            _typedDelegate = (Func<Object>)_cached.GetMethod.CreateDelegate(typeof(Func<Object>), this);

            _lambda = CreateLambda(this, nameof(Property));
        }

        private static Func<object> CreateLambda(object value, string propertyName)
        {
            var property = Expression.Property(Expression.Constant(value), propertyName);
            var expression = Expression.Lambda<Func<Object>>(property);
            return expression.Compile();
        }

        [Benchmark]
        public void UncachedPropertyInfoGet()
        {
            _toSet = (string?) GetType().GetProperties().First().GetValue(this);
        }

        [Benchmark]
        public void CachedPropertyInfoGet()
        {
            _toSet = (string?) _cached.GetValue(this);
        }

        [Benchmark]
        public void DynamicInvokeDelegateCreate()
        {
            _toSet = (string?) _delegate.DynamicInvoke();
        }

        [Benchmark]
        public void InvokeDelegateCreate()
        {
            _toSet = (string) _typedDelegate.Invoke();
        }

        [Benchmark]
        public void CachedExpression()
        {
            _toSet = (string) _lambda();
        }

        [Benchmark(Baseline = true)]
        public void NativeAssignment()
        {
            _toSet = Property;
        }
    }
}