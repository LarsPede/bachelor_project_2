using System;

namespace BachelorModelViewController.Models
{
    public class Atomic : Datatype
    {
        public object value { get; set; }
        public override string ToString()
        {
            return "\"" + name + "\":\"" + type;
        }

        public Atomic(string n, Type t, object val)
        {
            name = n;
            type = t;
            value = val;
        }

        public Atomic createFromString(string s)
        {
            var tempsplit = s.Split(':');
            object temp = parser(tempsplit[1]);
            return new Atomic(tempsplit[0], temp.GetType(), temp);
        }
        
        public object parser(string val)
        {
            bool Bool;
            int Int;
            double Double;
            DateTime Datetime;
            
            if(bool.TryParse(val, out Bool))
            {
                return Bool;
            }
            if(int.TryParse(val, out Int))
            {
                return Int;
            }
            if(double.TryParse(val, out Double))
            {
                return Double;
            }
            if(DateTime.TryParse(val, out Datetime))
            {

                return Datetime;
            }
            return val;
        }
    }
}
