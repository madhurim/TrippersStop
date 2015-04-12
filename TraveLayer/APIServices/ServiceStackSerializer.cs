using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceStack;
using TraveLayer.CustomTypes;


namespace TraveLayer.APIServices
{
    public static class ServiceStackSerializer
    {
    /*    public static string Serialize(ICustomType customType)
        {
            ServiceStack.Text.JsonSerializer<ICustomType> seri = new ServiceStack.Text.JsonSerializer<ICustomType>();
            String json = seri.SerializeToString(customType);

            return json;
        
        }*/
        public static string Serialize<T>(T value)
        {
            ServiceStack.Text.JsonSerializer<T> seri = new ServiceStack.Text.JsonSerializer<T>();
            String json = seri.SerializeToString(value);

            return json;

        }
        public static T DeSerialize<T>(string json){            

            ServiceStack.Text.JsonSerializer<T> seri = new ServiceStack.Text.JsonSerializer<T>();            
            return seri.DeserializeFromString(json);           
        
        }
    }
}
