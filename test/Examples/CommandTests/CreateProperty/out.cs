using System;
using System.Collections.Generic;

namespace Tests.Examples.CreateProperty
{
    protected abstract class A
    {
        public static int SomeInt {internal get { return 100; } private set; }
        string SomeString { get { return null; } set; }
    }
}