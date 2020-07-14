using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace Exceptions
{
    public static class ExtensionMethods
    {
        // Deep clone
        public static T DeepClone<T>(this T a) where T : class
        {
            using (MemoryStream stream = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, a);
                stream.Position = 0;
                return (T)formatter.Deserialize(stream);
            }
        }   
        public static IEnumerable<T> DeepClone<T>(this IEnumerable<T> data) where T : class
        {
            List<T> clone = new List<T>();
            
            foreach (T value in data)
            {
                clone.Add(value.DeepClone<T>());
            }

            return clone;
        }
    }
}
