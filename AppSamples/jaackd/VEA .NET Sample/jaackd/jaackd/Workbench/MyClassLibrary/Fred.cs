using System;
using System.Collections.Generic;
using System.Text;

namespace MyClassLibrary
{
    public class CFred : CPerson
	{
		
        public CFred() : base("Fred", "hepburn", 2)
        {
			m_Double = 12345.12345;
			m_Float = 123.123F;
			m_Double = 12345;
        }
    	public CFred(string town, int age) : base("Fred",town,age)
		{
			m_Double = 12345.12345;
			m_Float = 123.123F;
			m_Double = 12345;
		}
	
	}
}
