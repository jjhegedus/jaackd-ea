using System;

namespace MyClassLibrary
{
	public class CJohn : CPerson
	{
        public CJohn() : base("John","Creswick",2)
        {
        }
    	public CJohn(string town, int age) : base("John",town,age)
		{
		}

	}


    public struct TWorker
    {
        public int x;
        public int y;

        public TWorker(int a, int b)
        {
            x = a;
            y = b;
        }
    }
}
