//Copyright (C) Microsoft Corporation.  All rights reserved.

// compose.cs
using System;

delegate void MyDelegate(string s);

class CExample
{
    public static void Print(string s)
    {
#if DEBUG
        System.Diagnostics.Debug.WriteLine(s);
#endif
        System.Console.WriteLine(s);
    }

    public static void Hello(string s)
    {
        string a = String.Format("  Hello, {0}!", s);
        Print(a);
    }

    public static void Goodbye(string s)
    {
        string a = String.Format("  Goodbye, {0}!", s);
        Print(a);
    }

    public static void Main()
    {
        MyDelegate a, b, c, d;

        // Create the delegate object a that references 
        // the method Hello:
        a = new MyDelegate(Hello);
        // Create the delegate object b that references 
        // the method Goodbye:
        b = new MyDelegate(Goodbye);
        // The two delegates, a and b, are composed to form c, 
        // which calls both methods in order:
        c = a + b;
        // Remove a from the composed delegate, leaving d, 
        // which calls only the method Goodbye:
        d = c - a;

        a("A");
        Print("Invoking delegate b:");
        b("B");
        Print("Invoking delegate c:");
        c("C");
        Print("Invoking delegate d:");
        d("D");
    }
}

