using System;

namespace UnityUtils
{
    public class TypeError : Exception
    {
        public TypeError(string msg) : base(msg) { }
    }
}
