using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class JsonFileSerializer<T>
    {
        public void Serialize(T obj, string path)
        {
            using (StreamWriter sw = new StreamWriter(path))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(sw, obj);
            }
        }
        public T Deserialize(string path)
        {
            using (StreamReader sr = new StreamReader(path))
            {
                JsonSerializer serializer = new JsonSerializer();
                return (T) serializer.Deserialize(sr, typeof(T));
            }
        }
    }
}
