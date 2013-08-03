#include <iostream>
#include <math.h>
using namespace std;
class Dr 
{ 
	int c;
	int ch;
	int z;
public:
	Dr(int x=0, int y=0, int z=0)
	{
		c=x;
		ch=y;
		this->z=z;
	}
	void izm ()
	{ 
		if ((ch)%(z)>=z) 
		{  
			ch=ch-((ch)%(z));
			c=c+(ch)%(z);
		}
		for (int i=2; i<ch; i++)
		{  
			int k=0;
			do
			{ 
				if ((ch%i==0)&&(z%i==0))
					{
						ch=ch/i;
						z=z/i;
						k++;
					}
				else k=0;
			}
			while (k!=0);
		}
	}
	void show()
	{
		izm();
		
		cout<<c<<" "<<ch<<"/"<<z<<endl;
	}
	Dr operator + (Dr & obj)
	{
		Dr t;
		int x;
		int z1=this->z;
		int z2=obj.z;
		if (this->z=obj.z) t.ch=this->ch+obj.ch;
		else
		{
			int k=this->z;
			if (obj.z>z) k=obj.z;
			for (int i=0; i<=k; i++)
			{
			int k=0;
			do
			{
				if ((z1%i==0)||(z2%i==0))
					{
						ch=ch/i;
						z=z/i;
						k++;
						x=x*i;
					}
				else k=0;
			}
			while (k!=0);
			}
			cout<<x;
			
			k=x/this->z;
			ch=ch*k;
			this->z=this->z*k;
			k=x/obj.z;
			obj.ch=obj.ch*k;
			obj.z=obj.z*k;
			t.c=this->c+obj.c;
			t.ch=this->ch+obj.ch;
			t.z=x;
			t.izm();
			izm();
			obj.izm();
			return t;
			
		}
	}
};
void main()
{
	Dr a(1, 4,35);
	Dr b(1, 3, 15);
	a.izm();
	a.show();
	Dr m=a+b;
	m.show();
	system("pause");
}