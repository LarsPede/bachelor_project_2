using BachelorModelViewController.Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using BachelorModelViewController.Models.ViewModels.DataViewModels;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Bson.Serialization;
using System.IO;
using Newtonsoft.Json.Bson;
using MongoDB.Bson.Serialization.Serializers;

namespace BachelorModelViewController.Helpers
{
    public class DatatypeHelper
    {
        public DatatypeHelper()
        {

        }

        public DatatypeModel HandleAsObject(string s)
        {
            string temp = s;
            temp.Remove(s.IndexOf('{'));
            temp.Remove(s.LastIndexOf('}') - 1);

            ClassModel result = new ClassModel();

            List<Tuple<string, string>> tempSeperated = Seperate(temp);

            foreach (Tuple<string, string> t in tempSeperated)
            {
                if (t.Item2.Contains('{') || t.Item2.Contains('}'))
                {
                    HandleAsObject(s);
                }
                else
                {
                    if (t.Item2.Contains('[') || t.Item2.Contains(']'))
                    {
                        result.properties.Add(new PropertyModel() { property = HandleAsArray(t.Item2) });
                        result.properties.Last().property.name = t.Item1;
                    }
                    else
                    {
                        PrimitiveModel prim = new PrimitiveModel();
                        prim.name = t.Item1;
                        prim.type = parser(t.Item2).ToString();
                        result.properties.Add(new PropertyModel() { property = prim });
                    }
                }

            }

            return result;
        }

        public List<Tuple<string, string>> Seperate(string s)
        {
            List<Tuple<string, string>> result = new List<Tuple<string, string>>();
            List<int> seperationLocations = new List<int>();
            string skips = "";

            string name = "";
            string value = "";

            bool inObjectOrArray = false;

            List<string> sections = new List<string>();
            string tempSection = "";

            // split into sections
            foreach (char c in s)
            {
                if (inObjectOrArray)
                {
                    if (c == ',')
                    {
                        sections.Add(tempSection);
                        tempSection = "";
                    }
                    else
                    {
                        if (c == skips.Last())
                        {
                            tempSection += c;
                            skips.Remove(skips.LastIndexOf(c));
                            if (string.IsNullOrEmpty(skips))
                            {
                                inObjectOrArray = false;
                            }
                        }
                        tempSection += c;
                    }
                }
                else
                {
                    if (c == ',')
                    {
                        sections.Add(tempSection);
                        tempSection = "";
                    }
                    else
                    {
                        tempSection += c;
                        if (c == '{' || c == '[')
                        {
                            if (c == '{') skips += "}";
                            else skips += "]";
                            inObjectOrArray = true;
                        }
                    }
                }
            }
            sections.Add(tempSection);
            // run through sections
            foreach (string sectionF in sections)
            {
                string section = sectionF.Replace("\\\"", "");
                
                name = section.Split(':')[0];

                for (int i = section.IndexOf(':'); i < section.Length; i++)
                {
                    value += section[i];
                }

                result.Add(new Tuple<string, string>(name, value));
                name = "";
                value = "";
            }

            return result;
        }

        public DatatypeModel HandleAsArray(string s)
        {
            string sTemp = s.Remove(s.IndexOf('[')).Remove(s.LastIndexOf(']'));

            CompositeModel result = new CompositeModel();

            if (sTemp.Contains('{') || sTemp.Contains('}'))
            {
                result.datatypeModel = new ClassModel();
            }
            else
            {
                if (sTemp.Contains('[') || sTemp.Contains(']'))
                {
                    List<Tuple<string, string>> tempSeperated = Seperate(sTemp);

                    foreach (Tuple<string, string> t in tempSeperated)
                    {
                        if (t.Item2.Contains('{') || t.Item2.Contains('}'))
                        {
                            HandleAsObject(s);
                        }
                        else
                        {
                            if (t.Item2.Contains('[') || t.Item2.Contains(']'))
                            {
                                result.datatypeModel = HandleAsArray(t.Item2);
                                result.name = t.Item1;

                            }
                            else
                            {
                                PrimitiveModel prim = new PrimitiveModel();
                                prim.name = t.Item1;
                                prim.type = parser(t.Item2).ToString();
                                result.datatypeModel = prim;
                            }
                        }
                    }
                }
            }
            return result;
        }

        public Type parser(string s)
        {
            Type result;

            int intValue = 0;
            bool boolValue = false;
            DateTime datetimeValue = new DateTime();
            double doubleValue = 0;

            int.TryParse(s, out intValue);
            bool.TryParse(s, out boolValue);
            DateTime.TryParse(s, out datetimeValue);
            double.TryParse(s, out doubleValue);

            if (datetimeValue != new DateTime()) { result = typeof(DateTime); }
            else
            {
                if (doubleValue != 0)
                {
                    result = typeof(double);
                }
                else
                {
                    if (intValue != 0)
                    {
                        result = typeof(int);
                    }
                    else
                    {
                        if (boolValue != false) { result = typeof(bool); }
                        else
                        {
                            result = typeof(string);
                        }
                    }
                }
            }
            return result;
        }

        public ChannelContent TakeJson(string s)
        {
            var jsonParsedObject = JObject.Parse(s);

            ChannelContent result = new ChannelContent();
            Type t = null;

            foreach (JProperty prop in jsonParsedObject.Properties())
            {

                Console.Out.WriteLine("name : " + prop.Name + " - value = " + prop.Value);
                string typeS= prop.Value.ToString();
                if (typeS.Contains('['))
                {
                    // Array
                    t = typeof(Array);
                }
                else if (typeS.Contains('{'))
                {
                    // Class
                    t = typeof(Object);
                }
                else
                {
                    // Property
                    t = parser(typeS);
                }
                result.add(new Content(prop.Name, t, true));
            }
            

            return result;
        }

        public bool AuthenticateJSON(string jsonIn, string jsonRequired)
        {
            bool result = false;

            var jIn = JObject.Parse(jsonIn);
            var jCheck = JObject.Parse(jsonRequired);
            
            return result;
        }

        public List<BsonDocument> GetRequired(string content, string requirements)
        {
           

            if (string.IsNullOrEmpty(requirements))
            {
                return new List<BsonDocument>();
            }

            var jContent = JObject.Parse(content)["value"];
            var jReq = JObject.Parse(requirements)["requiredKeys"];
            var jRes = new JObject();
            string name = "";
            string type = "";
            List<Requirement> setup = new List<Requirement>();
            

            foreach (JValue propReq in jReq)
            {
                foreach (JProperty propCont in jContent)
                {
                    if (propCont.Name == propReq.Value.ToString())
                    {
                        name = propCont.Name;
                        type = propCont.Value["typeName"].ToString();
                        
                        setup.Add(new Requirement(name, type));
                    }
                }
            }

            BsonDocument result = new BsonDocument();

            string json = JsonConvert.SerializeObject(setup);
            
            BsonArray bsonItem = BsonSerializer.Deserialize<BsonArray>(json);

            List<BsonDocument> somehting2 = new List<BsonDocument>();

            foreach (var i in bsonItem)
            {
                somehting2.Add(i.AsBsonDocument);
            }

            return somehting2;
        }
    }

    public class Requirement
    {
        string name;
        string type;

        public Requirement()
        {

        }

        public Requirement(string n, string t)
        {
            name = n;
            type = t;
        }
    }
}
