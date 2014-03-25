using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

namespace Approach
{
    public interface RenderNode
    {
        string render();
        void render(TextWriter OutputStream);
        RenderNode stream(RenderNode RenderStream);
        void stream(TextWriter OutputStream);

        void RenderHead(TextWriter OutputStream);
        void RenderCorpus(TextWriter OutputStream);
        void RenderTail(TextWriter OutputStream);
        void Add(RenderNode incoming);
    }
    public class Renderable : RenderNode
    {
        static UInt64 recent;//{ get;set;}
        static UInt64 cursor;//{ get;set;}
        readonly UInt64 accessor = recent;
        public ArrayList children;

        public Renderable() { recent++; cursor = recent; }

        /*            CLASS ACTIONS         */

        public RenderNode stream(RenderNode RenderStream) 
        {
            foreach(RenderNode child in children)
            {
                RenderStream.Add((Renderable)child);
            }
            return (Renderable)RenderStream;
        }
        public void stream(TextWriter OutputStream) 
        {
            this.RenderHead(OutputStream);
            this.RenderCorpus(OutputStream);
            this.RenderTail(OutputStream);        
        }

        public void prerender(TextWriter OutputStream)
        {
            RenderHead(OutputStream);
            RenderCorpus(OutputStream);
        }
        public string render()
        {
            StringWriter InstancedStream = new StringWriter();
            this.render(InstancedStream);
            return InstancedStream.ToString();
        }
        public void render(TextWriter OutputStream)
        {
            this.RenderHead(OutputStream);
            this.RenderCorpus(OutputStream);
            this.RenderTail(OutputStream);
        }
        public void RenderHead(TextWriter OutputStream)
        {
            OutputStream.Write(this.accessor);
            OutputStream.Write(this.GetType());
        }
        public void RenderCorpus(TextWriter OutputStream)
        {
            OutputStream.Write(children.Count);
            foreach (RenderNode child in this.children)
                ((Renderable)(child)).render(OutputStream);
        }
        public void RenderTail(TextWriter OutputStream)
        {
            //optional (null terminate for C stream or structuring)
            OutputStream.Write('\0');
        }

        public void Add(RenderNode incoming) { children.Add(incoming); }
    }
    public class RenderXML : Renderable
    {
        public string tag, id, content;
        public List<string> classes=new List<string>{};
        public Dictionary<string, string> attributes = new Dictionary<string, string> { };
        
        protected new ArrayList children = new ArrayList();
        
       [FlagsAttribute]
        public enum RenderFlags
       {
           SelfContained=0x1,
           OpenRendered=0x2,
           ContentRendered=0x4,
           ContentOff=0x8,
           ContentOnly=0x10,
           EnsureChildren=0x20,
           Listening=0x40,
           PerforationPoint=0x80 
       };

        public RenderFlags flags=0;
RenderXML
        public RenderXML(
            String _tag, String _id = null, String _content = null,
            List<String> _classes = null, Dictionary<String, String> _attributes = null,
            RenderFlags _flags = 0, Dictionary<String, Type> options = null)
            : base()
        {
            tag = _tag;
            if (_id != null) id = _id;
            if (_classes != null) classes = _classes;
            if (_attributes != null) attributes = _attributes;
            if (_content != null) content = _content;
            if (_flags != 0) flags = _flags;
        }

        public string BuildAttributes()
        {
            string attr_format = "";
            foreach (KeyValuePair<string, string> attr in attributes)
            {
                attr_format += " " + attr.Key + "=\"" + attr.Value + "\"";
            }
            return attr_format;
        }
        public string BuildClasses(string class_format = " class=\"")
        {
            if (this.classes.Count > 0)
            {
                foreach (string css_class in this.classes)
                {
                    class_format += css_class + " ";
                }
                return class_format + "\"";
            }
            else return "";
        }

        public new void RenderHead(TextWriter OutputStream)
        {
            if (this.flags != RenderFlags.ContentOnly && this.flags != RenderFlags.OpenRendered)
            {
                OutputStream.Write(
                    "<" +
                        this.tag +
                        (this.id != null ? " " + id : "") +
                        (this.classes.Count > 0 ? " " + this.BuildClasses() : "") +
                        (this.attributes.Count > 0 ? " " + this.BuildAttributes() : "") +
                        (this.flags == RenderFlags.SelfContained ? " /" : "") +
                    ">");
                this.flags = this.flags & RenderFlags.OpenRendered; //Set OpenRendered flag on
            }
        }
        public new void RenderCorpus(TextWriter OutputStream)
        {
            if (this.flags != RenderFlags.ContentRendered)
            {
                if (this.flags != RenderFlags.ContentOff)
                {
                    OutputStream.Write(this.content);
                    this.flags = this.flags & RenderFlags.ContentRendered;
                }

                if (this.flags != RenderFlags.PerforationPoint || this.flags == RenderFlags.EnsureChildren)
                {

                    foreach (var child in this.children)
                    {
                        ((RenderXML)(child)).render(OutputStream);
                    }
                }
                this.flags = this.flags & RenderFlags.ContentRendered; //Set ContentRendered flag on
            }
        }
        public new void RenderTail(TextWriter OutputStream)
        {
            if (!(this.flags == RenderFlags.SelfContained)) OutputStream.Write("</" + this.tag + ">");
        }
        public new void prerender(TextWriter OutputStream)
        {
            RenderHead(OutputStream);
            RenderCorpus(OutputStream);
        }
        public new string render()
        {
            StringWriter InstancedStream = new StringWriter();
            this.render(InstancedStream);
            return InstancedStream.ToString();

        }
        public new void render(TextWriter OutputStream)
        {
            RenderHead(OutputStream);
            RenderCorpus(OutputStream);
            RenderTail(OutputStream);
        }

        public new RenderXML stream(RenderNode RenderStream)
        {
            foreach (RenderNode child in children)
            {
                RenderStream.Add((RenderXML)child);
            }
            return (RenderXML)RenderStream;
        }
        public new void stream(TextWriter OutputStream)
        {
            this.RenderHead(OutputStream);
            this.RenderCorpus(OutputStream);
            this.RenderTail(OutputStream);
        }

        /*            CLASS ACTIONS         */
        public void Add(RenderXML incoming) { children.Add(incoming); }
    }

    public class JSONable : Renderable
    {
        [FlagsAttribute]
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
        public JSONableFlags flags = 0;

        public bool IsArray;
        public UInt64 counter, cursor;
        public Dictionary<string, UInt64> properties;

        protected Dictionary<KeyValuePair<UInt64,string>, Int64> IntegerValues;
        protected Dictionary<KeyValuePair<UInt64, string>, double> DecimalValues;
        protected Dictionary<KeyValuePair<UInt64, string>, string> StringValues;
        protected new Dictionary<KeyValuePair<UInt64,string>,KeyValuePair<bool, JSONable>> children;
        
        private Dictionary<UInt64, ushort> TotalOrdering;

        public JSONable(bool _IsArray=false)
            : base() 
        {
                counter = 0; cursor = 0; TotalOrdering = new Dictionary<ulong, ushort>();
                IsArray = _IsArray;

                IntegerValues = new Dictionary<KeyValuePair<UInt64, string>, Int64>();
                DecimalValues = new Dictionary<KeyValuePair<ulong, string>, double>();
                StringValues = new Dictionary<KeyValuePair<ulong, string>, string>();
                children = new Dictionary<KeyValuePair<ulong, string>, KeyValuePair<bool, JSONable>>();
                TotalOrdering = new Dictionary<ulong, ushort>();
            }
        public JSONable(ArrayList _properties,bool _IsArray=false) : base()
        {
            counter = 0; cursor = 0; TotalOrdering=new Dictionary<ulong,ushort>();
            IsArray = _IsArray;

            IntegerValues = new Dictionary<KeyValuePair<UInt64, string>, Int64>();
            DecimalValues = new Dictionary<KeyValuePair<ulong, string>, double>();
            StringValues = new Dictionary<KeyValuePair<ulong, string>, string>();
            children = new Dictionary<KeyValuePair<ulong, string>, KeyValuePair<bool, JSONable>>();
            TotalOrdering = new Dictionary<ulong, ushort>();

            foreach(var property in _properties)
            {
                System.Type TypeInUse = property.GetType();

                //Direct Values
                if (TypeInUse == typeof(Int64))
                {
                    IntegerValues.Add( new KeyValuePair<UInt64, string>(counter, null), (Int64)property);
                    TotalOrdering.Add(counter, (ushort)JSONableFlags.IntMember);

                    counter++;
                    continue;
                }
                if (TypeInUse == typeof(double))
                {
                    DecimalValues.Add( new KeyValuePair<UInt64, string>(counter, null), (double)property);
                    TotalOrdering.Add(counter, (ushort)JSONableFlags.DecimalMember);

                    counter++;
                    continue;
                }
                if (TypeInUse == typeof(string))
                {
                    StringValues.Add( new KeyValuePair<UInt64, string>(counter, null), (string)property);
                    TotalOrdering.Add(counter, (ushort)JSONableFlags.StringMember);

                    counter++;
                    continue;
                }
                if (TypeInUse == typeof(KeyValuePair<bool,JSONable>))
                {
                    children.Add( new KeyValuePair<UInt64, string>(counter, null), (KeyValuePair<bool, JSONable>)property );

                    if (((KeyValuePair<bool, JSONable>)property).Key) TotalOrdering.Add(counter, (ushort)JSONableFlags.ArrayMember);
                    else TotalOrdering.Add(counter, (ushort)JSONableFlags.ObjMember);

                    counter++;
                    continue;
                }

                //Key Value Pairs
                if (TypeInUse == typeof(KeyValuePair<string,Int64>))
                {
                    IntegerValues.Add( new KeyValuePair<UInt64, string>(counter, ((KeyValuePair<string, Int64>)property).Key),
                        ((KeyValuePair<string, Int64>)property).Value );
                    TotalOrdering.Add(counter, (ushort)JSONableFlags.IntMember);

                    counter++;
                    continue;
                }
                if (TypeInUse == typeof(KeyValuePair<string,double>))
                {
                    DecimalValues.Add( new KeyValuePair<UInt64, string>(counter, ((KeyValuePair<string, double>)property).Key),
                        ((KeyValuePair<string, double>)property).Value );
                    TotalOrdering.Add(counter, (ushort)JSONableFlags.DecimalMember);

                    counter++;
                    continue;
                }
                if (TypeInUse == typeof(KeyValuePair<string, string>)) 
                {
                    StringValues.Add(new KeyValuePair<UInt64, string>(counter, ((KeyValuePair<string, string>)property).Key),
                        ((KeyValuePair<string, string>)property).Value );
                    TotalOrdering.Add(counter, (ushort)JSONableFlags.StringMember);

                    counter++;
                    continue;
                }

                if (TypeInUse == typeof(KeyValuePair<string,KeyValuePair<bool,JSONable>>))
                {
                    children.Add( new KeyValuePair<UInt64, string>(counter, ((KeyValuePair<string, KeyValuePair<bool, JSONable>>)property).Key),
                        ((KeyValuePair<string, KeyValuePair<bool, JSONable>>)property).Value );

                    if (((KeyValuePair<string, KeyValuePair<bool, JSONable>>)property).Value.Key) TotalOrdering.Add(counter, (ushort)JSONableFlags.ArrayMember);
                    else TotalOrdering.Add(counter, (ushort)JSONableFlags.ObjMember);

                    counter++;
                    continue;
                }

                //last resort
                children.Add( new KeyValuePair<UInt64, string>(counter, null), new KeyValuePair<bool,JSONable>(false,null) );
                counter++;
            }
        }

        public new void RenderHead(TextWriter OutputStream)
        {
            if (this.flags != JSONableFlags.ContentOnly && this.flags != JSONableFlags.OpenRendered)
            {
                if (IsArray) OutputStream.Write("[");
                else OutputStream.Write("{");

                this.flags = this.flags & JSONableFlags.OpenRendered; //Set OpenRendered flag on
            }
        }
        public new void RenderCorpus(TextWriter OutputStream)
        {
            if (this.flags != JSONableFlags.ContentRendered)
            {
                foreach (KeyValuePair<UInt64, ushort> item in TotalOrdering)
                {
                    if (item.Key != 0) OutputStream.Write(",");
                    switch (item.Value)
                    {
                        case (ushort)JSONableFlags.IntMember: 
                                                        foreach (KeyValuePair<UInt64, string> k in IntegerValues.Keys)
                                                        {
                                                            if (item.Key == k.Key){   OutputStream.Write("\"" + ((k.Value == null) ? k.Key.ToString() : k.Value) + "\" : " + IntegerValues[k]); }
                                                        } break;
                        case (ushort)JSONableFlags.DecimalMember: 
                                                        foreach (KeyValuePair<UInt64, string> k in DecimalValues.Keys)
                                                        {
                                                            if (item.Key == k.Key) { OutputStream.Write("\"" + ((k.Value == null) ? k.Key.ToString() : k.Value) + "\" : " + DecimalValues[k]); }
                                                        } break;
                        case (ushort)JSONableFlags.StringMember: 
                                                        foreach (KeyValuePair<UInt64, string> k in StringValues.Keys)
                                                        {
                                                            if (item.Key == k.Key) { OutputStream.Write("\"" + ((k.Value == null) ? k.Key.ToString() : k.Value) + "\" : \"" + StringValues[k] + "\""); }
                                                        } break;
                        case (ushort)JSONableFlags.ArrayMember:
                        case (ushort)JSONableFlags.ObjMember: 
                                                        foreach (KeyValuePair<UInt64, string> k in children.Keys)
                                                        {
                                                            if (item.Key == k.Key)
                                                            {
                                                                OutputStream.Write(((k.Value == null) ? ((IsArray) ? "" : "\"" + k.Key.ToString() + "\"") : "\"" + k.Value + "\"") + " : "); 
                                                                children[k].Value.render(OutputStream); 
                                                           }
                                                        } break;
                        default: break;
                    }
                }

                this.flags = this.flags & JSONableFlags.ContentRendered; //Set ContentRendered flag on
            }
        }
        public new void RenderTail(TextWriter OutputStream)
        {
            if (this.flags != JSONableFlags.ContentOnly)
            {
                if (IsArray) OutputStream.Write("]");
                else OutputStream.Write("}");
            }
        }
        public new void prerender(TextWriter OutputStream)
        {
            RenderHead(OutputStream);
            RenderCorpus(OutputStream);
        }
        public new string render()
        {
            StringWriter InstancedStream = new StringWriter();
            this.render(InstancedStream);
            return InstancedStream.ToString();
        }
        public new void render(TextWriter OutputStream)
        {
            RenderHead(OutputStream);
            RenderCorpus(OutputStream);
            RenderTail(OutputStream);
        }

        public new JSONable stream(RenderNode RenderStream)
        {
            RenderStream.Add(this);
            return (JSONable)RenderStream;
        }
        public new void stream(TextWriter OutputStream)
        {
            this.RenderHead(OutputStream);
            this.RenderCorpus(OutputStream);
            this.RenderTail(OutputStream);
        }

        /*            CLASS ACTIONS         */
        public void Add(JSONable incoming)
        {
            children.Add(new KeyValuePair<ulong, string>(counter, null), new KeyValuePair<bool, JSONable>(incoming.IsArray, incoming));
            TotalOrdering.Add(counter, (incoming.IsArray) ? (ushort)JSONableFlags.ArrayMember : (ushort)JSONableFlags.ObjMember);
            counter++;
        }
        public void Add(string key, JSONable incoming)
        {
            children.Add(new KeyValuePair<ulong, string>(counter, key), new KeyValuePair<bool, JSONable>(incoming.IsArray, incoming));
            TotalOrdering.Add(counter, (incoming.IsArray) ? (ushort)JSONableFlags.ArrayMember : (ushort)JSONableFlags.ObjMember);
            counter++;
        }
    }
}