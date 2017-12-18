using BachelorModelViewController.Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
    }
}
