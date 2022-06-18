using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;

namespace MyClassLibrary
{
    public class CPerson
    {
		private String occupation;

        public string Name;
        public string Town;
        public CPerson[] Friends;
        public int Age;
		public int FriendCount = 0;
		public float AverageAge = 0;
		
 		public double m_Double;
		public float m_Float;
		public int m_Int;

		public CPerson()
        {
			Init();
        }
        public CPerson(CPerson f)
        {
          	Init();
            Town = f.Town;
            Name = f.Name;
            Age = f.Age;
            for(int x = 0; x < f.FriendCount; x++)
            {
            	Friends[x] = f.Friends[x];
            }
        }
        public CPerson(string name, string town, int age)
        {
        	Init();
            Age = age;
            Name = name;
            Town = town;
        }

        public int SetAge(int age)
        {
            Age = age;
            return age;
        }
        public void SetName(string name)
        {
            Name = name;
        }        
        public String SetTown(string town)
        {
            String old = Town;
            Town = town;
            return old;
        }

        private  void Init()
        {
			m_Double = 12345.12345;
			m_Float = 123.123F;
			m_Double = 12345;
	        Friends = new CPerson[10];
			Age = 0;
			Name = "Unknown";
			Town = "Unknown";
            occupation = "Programmer";
        }

        public int CreateFriends(int count)
        {
       		if(FriendCount + count > Friends.Length)
       			count = Friends.Length - FriendCount;

			int old = FriendCount;
			for(int a = 0; a < count; a++)
			{
				String name = "friend";
				int num = (a + old);
				name += num.ToString();
				CPerson man = new CPerson(name,"Glasgow",1);
				AddFriend(man);
			}        	
        	return FriendCount;
        }
        public float GetAverage()
        {
        	AverageAge = Age;
        	for(int a = 0; a < FriendCount; a++)
        	{
     			AverageAge += Friends[a].Age;   	
        	}
        	AverageAge /= (FriendCount+1);
        	System.Diagnostics.Debug.WriteLine(AverageAge.ToString());
        	return AverageAge;
        }
        public void SetAverage(float av)
        {
        	AverageAge = av;
        }

        public bool IsFriend( CPerson person)
        { 
         	foreach(CPerson p in Friends)
        	{
        		if(p == person)
        		{
        			return true;
        		}
        	}
        	return false;
        }
        public int AddFriends(CPerson[] people)
        {
            foreach (CPerson p in people)
            {
           		if(p != null)
	                AddFriend(p);
            }
            return FriendCount;
        } 
        
        public int AddFriend(CPerson friend)
        {
        	if(!IsFriend(friend))
        	{
        		if(FriendCount + 1 < Friends.Length)
        		{
			    	Friends[FriendCount++] = friend;
			    }
			}
            return FriendCount;
        }
        
        public bool Find(string[] names)
        {
         	foreach (string name in names)
        	{
       			if(Find(name))
        		{
        			System.Diagnostics.Debug.WriteLine( name + " is a friend");
        		}
           	}
			return false;
        }
        
        public void Set(string name, int age)
        {
        	Age = age;
        	Name = name;
        }
        
        public virtual bool Find(string name)
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
        
        public string Occupation
        {
        	get
        	{
        		return occupation;				
        	}
        	set
        	{
        		occupation = value;
        	}
        }
        
        public void Run(string[] args)
        {
        	CPerson[] List = new CPerson[args.Length];
        	for (int c = 0; c < args.Length; c++)
			{
				List[c] = new CPerson(args[c],"BabyTown",(c+1)*10);
			}
        	AddFriends(List);
        	Find(args);
        }
        
		public static void main(string[] args)
        {
			CPerson p = new CPerson();
			p.Run(args);
        }
        
        public static void Test(string name)
        {
        	CPerson p = new CPerson();
        	p.Find(name);
        }
               
        
    }
}
