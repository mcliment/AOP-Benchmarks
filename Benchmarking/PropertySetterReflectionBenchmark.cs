﻿using System.Linq.Expressions;
using System.Reflection;
using BenchmarkDotNet.Attributes;

namespace Benchmarking
{
    public class PropertySetterReflectionBenchmark
    {
        private readonly PropertyInfo _cached;
        private readonly Delegate _delegate;
        private readonly Action<string> _lambda;
        private readonly Action<string> _typedDelegate;
        
        public string? Property { get; set; }

        public PropertySetterReflectionBenchmark()
        {
            _cached = GetType().GetProperties().First();
            _delegate = _cached.SetMethod!.CreateDelegate(typeof(Action<String>), this);
            _typedDelegate = (Action<String>) _cached.SetMethod.CreateDelegate(typeof(Action<String>), this);

            _lambda = CreateLambda(this, nameof(Property));
        }

        private static Action<string> CreateLambda(object value, string propertyName)
        {
            var parameterExpression = Expression.Parameter(typeof (String), "StringName");
            var body = Expression.Assign(Expression.Property(Expression.Constant(value), propertyName), parameterExpression);
            var expression = Expression.Lambda<Action<String>>(body, parameterExpression);
            return expression.Compile();
        }

        [Benchmark]
        public void UncachedPropertyInfoSet()
        {
            GetType().GetProperties().First().SetValue(this, "a");
        }

        [Benchmark]
        public void CachedPropertyInfoSet()
        {
            _cached.SetValue(this, "b");
        }

        [Benchmark]
        public void DynamicInvokeDelegateCreate()
        {
            _delegate.DynamicInvoke("c");
        }

        [Benchmark]
        public void InvokeDelegateCreate()
        {
            _typedDelegate.Invoke("d");
        }

        [Benchmark]
        public void CachedExpression()
        {
            _lambda("e");
        }

        [Benchmark(Baseline = true)]
        public void NativeAssignment()
        {
            Property = "f";
        }
    }
}