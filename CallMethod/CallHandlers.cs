using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace UnityUtils
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class ExcludeFromCall : Attribute { }

    public class CallOptions
    {
        private const BindingFlags DefaultFlags = BindingFlags.Public
                | BindingFlags.NonPublic
                | BindingFlags.Instance
                | BindingFlags.Static;

        public readonly bool AllowMultiple;
        public readonly bool AllowChildren;
        public readonly BindingFlags flags;

        public CallOptions(bool allowMultiple, bool allowChildren, BindingFlags Flags)
        {
            AllowMultiple = allowMultiple;
            AllowChildren = allowChildren;
            flags = Flags;
        }

        public CallOptions(bool allowMultiple, bool allowChildren) : this(allowMultiple, allowChildren, DefaultFlags) { }

        public CallOptions() : this(false, false, DefaultFlags) { }

        public static implicit operator CallOptions(BindingFlags Flags) => new CallOptions(false, false, Flags);
    }

    public class AllowMultiple : CallOptions
    {
        public AllowMultiple() : base(true, false) { }
    }

    public class AllowChildren : CallOptions
    {
        public AllowChildren() : base(false, true) { }
    }

    public sealed class ResponseNode<T>
    {
        public readonly Component component;
        public readonly T Value;

        public ResponseNode(Component c, T v)
        {
            component = c;
            Value = v;
        }
    }

    public sealed class CallResponse<T> : IEnumerable<ResponseNode<T>>
    {
        public readonly List<ResponseNode<T>> Responses = new List<ResponseNode<T>>();

        public bool Success { get; private set; } = false;
        public T Value { get; private set; }

        public void Add(Component comp, T value)
        {
            Success = true;
            Responses.Add(new ResponseNode<T>(comp, value));
            Value = value;
        }

        public IEnumerator<ResponseNode<T>> GetEnumerator() => Responses.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)Responses).GetEnumerator();
    }
}