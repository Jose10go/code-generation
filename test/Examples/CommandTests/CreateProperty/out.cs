using System;
using System.Collections.Generic;

namespace Tests.Examples.CreateProperty
{
    protected abstract class A
    {
        public static Int32 SomeInt {internal get { return 100; } private set; }
        String SomeString { get { return null; } set; }
    }
}