#include <iostream>
#include <vector>
#include <map>
#include <ostream>
#include <istream>
#include <fstream>
#include <sstream>

typedef unsigned long long int ProcUnit;

namespace Approach {
namespace Renderable {
public class JSONable : Renderable
{
    public enum JSONableFlags
	{
		ObjMember = 0x1,
		ArrayMember = 0x2,
		StringMember = 0x4,
		DecimalMember = 0x8,
		IntMember = 0x10,
		OpenRendered = 0x20,
		ContentRendered = 0x40,
		ContentOnly = 0x80,
		EnsureChildren = 0x100,
		Listening = 0x200,
		PerforationPoint = 0x400
	};

	JSONableFlags flags = 0;

	bool IsArray;
	UInt64 counter, cursor;
	typedef std::pair<uint64_t, string> JSON_Property;

	map<JSON_Property, int64_t> IntegerValues;
	map<JSON_Property, double> DecimalValues;
	map<JSON_Property, string> StringValues;
	map<JSON_Property, map<bool, JSONable>> children;

	map<uint64_t, ushort> TotalOrdering;

	JSONABLE(bool _IsArray=false)
	{
		//TODO call base constructor?

		counter = 0; cursor = 0; TotalOrdering = new std::map<ulong, ushort>;
		IsArray = _IsArray;

		IntegerValues = new map<map<UInt64, string>, Int64>;
		DecimalValues = new map<map<ulong, string>, double>;
	    StringValues = new map<map<ulong, string>, string>;
		children = new map<map<ulong, string>, map<bool, JSONable>>;
		TotalOrdering = new map<ulong, ushort>;
	}

    JSONABLE(std::vector<typename someType> _properties, bool _IsArray=false)
	{
		//TODO call base constructor?

		counter = 0; cursor = 0; TotalOrdering = new std::map<ulong, ushort>;
		IsArray = _IsArray;

		IntegerValues = new map<map<UInt64, string>, Int64>;
		DecimalValues = new map<map<ulong, string>, double>;
	    StringValues = new map<map<ulong, string>, string>;
		children = new map<map<ulong, string>, map<bool, JSONable>>;
		TotalOrdering = new map<ulong, ushort>;

		std::vector<someType>::iterator iter = _properties.begin();
		while(itr != _properties.end())
		{
		    std::string TypeInUse = typeid(_properties[itr]);

		    if(TypeInUse == typeid(int64_t))
		    {
		        IntegerValues.Add( new map<uint64_t, string>(counter, null), (int64_t)property);
		        TotalOrdering.Add( counter, (ushort)JSONableFlags.IntMember);

		        counter++;
		        continue;
		    }
		    if(TypeInUse == typeid(double))
		    {
		        DecimalValues.Add( new map<uint64_t, string>(counter, null), (double)property);
		        TotalOrdering.Add( counter, (ushort)JSONableFlags.DecimalMember);

		        counter++;
		        continue;
		    }
		    if(TypeInUse == typeid(string)
		    {
		        StringValues.Add( new map<uint64_t, string>(counter, null), (string)property);
		        TotalOrdering.Add( counter, (ushort)JSONableFlags.StringMember);

		        counter++;
		        continue;
		    }
		    if(TypeInUse == typeid(map<bool, JSONable>))
		    {
		        children.Add( new map<uint64_t, string>(counter, null), (map<bool, JSONable>)property);

		        if(((map<bool, JSONable>)property).first) TotalOrdering.Add(counter, (ushort)JSONableflags.ArrayMember);
		        else TotalOrdering.Add(counter, (ushort)JSONableFlags.ObjMember);

		        counter++;
		        continue;
		    }
		    if(TypeInUse == typeid(map<string, int64_t))
		    {
		        IntegerValues.Add( new map<uint64_t, string>(counter, ((map<string, int64_t>property).first),
                                             ((map<string, int64_t>)property).second;
		        TotalOrdering.Add( counter, (ushort)JSONableFlags.IntMember);

		        counter++;
		        continue;
		    }
		    if(TypeInUse == typeid(map<string, double>))
		    {
		        DecimalValues.Add( new map<uint64, string>(counter, ((map<string, double>)property).first),
                            ((map<string, double>)property).second);
                TotalOrdering.Add(counter, (ushort)JSONableFlags.DecimalMember);

		        counter++;
		        continue;
		    }
		    if(TypeInUse == typeid(map<string, map<bool, JSONable>>))
		    {
		        children.Add( new map<uint64_t, string>(counter, ((map<string, map<bool, JSONable>>)property).first),
                       ((map<string, map<bool, JSONable)property).second);

                if(((map<string, map<bool, JSONable>>)property).second.first) TotalOrdering.Add(counter, (ushort)JSONableFlags.ArrayMember);
		        else TotalOrdering.Add(counter, (ushort)JSONableFlags.ObjMember);

		        counter++;
		        continue;
		    }

		    //last resort
		    children.Add(new map<uint64_t, string(counter, null), new map<bool, JSONable>(flase, null) )
		    counter++;
		}
	}



	inline void RenderHead(std::ostream& outputstream)
	{
		if (this->flags != JSONableFlags.ContentOnly && this->flags != JSONableFlags->OpenRendered)
		{
			if (IsArray) outputstream<<"[";
			else outputstream<<"{";

			this->flags = this->flags & JSONableFlags->OpenRendered;
		}

		this->flags = this->flags & JSONableFlags->ContentRendered //Set ContentRendered flag on
	}

	//TODO
	inline void RenderCorpus(std::ostream& outputstream)
	{
		if (this->flags != JSONableFlags.ContentRendered)
        {
            //for loop. each keyvaluepair item in totalordering
            std::map<uint64_t, ushort>::iterator iter;
            for(iter = TotalOrdering.begin(); i != TotalOrdering.end(); i++)
            {
                if (item.Key != 0) outputstream<<",";
                switch(item.second)
                {
                    case:
                        std::map<uint64_t, std::string>::iterator iter2;
                        for(iter2 = IntegerValues.begin(); iter2 != IntegerValues.end();iter2++ )
                        {
                            if(item.first == iter2->first){
                                ostream<<"\""<<((iter2.second == null) ? iter2.first.)
                            }
                        }
                        break;

                    default: break;
                }
            }
            this.flags = this.flags & JSONableFlags.ContentRendered; //Set ContentRendered flag on
        }
    }

	inline void RenderTail(std::ostream& outputstream)
	{
		ifthis->flags != JSONableFlags.Contentonly)
		{
			//outputstream<<std::endl;
			if(isArray) outputstream<<"]";
			else outputstream<<"}";
		}
	}

	inline void prerender(std::ostream& outputstream)
	{
		this->RenderHead(outputstream);
		this->RenderTail(outputstream);
	}

	inline std::ostream& render()
	{
		std::ostream outputstream;
		this->render(outputstream);
		return outputstream;
	}

	inline void render(std::ostream& outputstream)
	{
		RenderHead(outputstream);
		RenderCorpus(outputstream);
		RenderTail(outputstream);
	}

		/////////////////////////////////
		///stream functions
		/////////////////////////////////

	inline void operator>>(std::ostream& outputstream) {this->render(outputstream);}

	inline friend std::ostream& operator << (std::ostream& outputstream, JSONable &obj) {obj.render(outputstream);}

    inline void Add(JSONable incoming)
    {
        children.Add(new map<ulong, string>(counter, null), new map<bool, JSONable>(incoming.IsArray, incoming));
        TotalOrdering.Add(counter, (incoming.IsArray) ? (ushort)Jsonableflags.ArrayMember : (ushort)JSONableFlags.ObjMember);
        counter++;
    }
    public void add(string key, JSONable incoming)
    {
        children.Add(new map<ulong, string>(counter, key), new map<bool, JSONable>(incoming.IsArray, incoming));
        TotalOrdering.Add(counter, (incoming.IsArray) ? (ushort)JSONableFlags.ArrayMember : (ushort)JSONableFlags.ObjMember);
        counter++;
    }
}
}
