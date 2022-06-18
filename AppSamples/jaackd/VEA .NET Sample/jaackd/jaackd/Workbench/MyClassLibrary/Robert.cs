using System;
using System.Collections.Generic;
using System.Text;

namespace MyClassLibrary
{
    public class CRobert : CPerson
    {
        public CRobert()
            : base("Robert", "Daylesford", 2)
        {
        }
        public CRobert(string town, int age)
            : base("John", town, age)
        {
        }


        public override bool Find(string name)
        {
        	try
        	{
        		foreach ( CPerson p in Friends)
        		{
        			if(p.Name == name)
        			{
        				System.Diagnostics.Debug.Write("found " + p.Name + " in town " + p.Town);
						return true;
        			}
        		}
        	}
			catch(Exception e)
			{
				System.Diagnostics.Debug.Write(e.Message);
			}
			return false;
        }        
    }
}

