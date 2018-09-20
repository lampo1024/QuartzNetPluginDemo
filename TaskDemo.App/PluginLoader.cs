using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace TaskDemo.App
{
    public class PluginLoader
    {
        public static T[] GetInterfaceImplementor<T>(string directory)
        {
            if (String.IsNullOrEmpty(directory)) { return null; }

            var info = new DirectoryInfo(directory);
            if (!info.Exists) { return null; }

            var implementors = new List<T>();

            foreach (FileInfo file in info.GetFiles("*.dll"))
            {
                Assembly currentAssembly = null;

                //var name = AssemblyName.GetAssemblyName(file.FullName);
                //currentAssembly = Assembly.Load(name);
                using (Stream stream = File.OpenRead(file.FullName))
                {
                    byte[] rawAssembly = new byte[stream.Length];
                    stream.Read(rawAssembly, 0, (int)stream.Length);
                    currentAssembly = Assembly.Load(rawAssembly);
                }
                

                var types = currentAssembly.GetTypes();

                currentAssembly.GetTypes()
                    .Where(t => t != typeof(T) && typeof(T).IsAssignableFrom(t))
                    .ToList()
                    .ForEach(x => implementors.Add((T)Activator.CreateInstance(x)));
            }
            return implementors.ToArray();
        }
    }
}
