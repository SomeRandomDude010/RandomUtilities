using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace RandomUtilities
{
    public static class DLLLoader
    {
        static List<RandomDll> dlls;

        public static void Init()
        {
            dlls = new();
        }

        public static void LoadDll(string name, string path)
        {
            if (dlls.Count(d => d.name == name) > 0)
                return;

            Assembly dll;

            try
            {
                dll = Assembly.LoadFile(path);
            }
            catch(Exception e)
            {
                throw e;
            }

            Dictionary<string, ILoadableRandomClass> loadableClasses = new();
            Dictionary<string, IStaticRandomClass> staticClasses = new();

            foreach (Type t in dll.GetExportedTypes())
            {
                ILoadableRandomClass loadableClass = Activator.CreateInstance(t) as ILoadableRandomClass;
                
                if(loadableClass != null)
                {
                    loadableClasses.AddOrReplace(loadableClass.GetName(), loadableClass);
                }

                IStaticRandomClass staticClass = Activator.CreateInstance(t) as IStaticRandomClass;

                if(staticClass != null)
                {
                    staticClasses.AddOrReplace(staticClass.GetName(), staticClass);
                }
            }

            dlls.Add(new RandomDll(name, loadableClasses, staticClasses));
        }

        public static void InitDllLoadableClass(string name, string className, object[] args)
        {
            if (!dlls.Exists(d => d.name == name))
                return;

            if (dlls[GetDllIndex(name)].loadableClasses.Count < 1)
                return;

            if (!dlls[GetDllIndex(name)].loadableClasses.ContainsKey(className))
                return;

            dlls[GetDllIndex(name)].loadableClasses[className].Initialize(args);
        }

        public static void InitDllStaticClass(string name, string className, object[] args)
        {
            if (!dlls.Exists(d => d.name == name))
                return;

            if (dlls[GetDllIndex(name)].staticClasses.Count < 1)
                return;

            if (!dlls[GetDllIndex(name)].staticClasses.ContainsKey(className))
                return;

            dlls[GetDllIndex(name)].staticClasses[className].Initialize(args);

            IStaticRandomClass staticClass = dlls[GetDllIndex(name)].staticClasses[className];

            dlls[GetDllIndex(name)].staticMethods.Add(className, staticClass.GetMethods());
        }

        public static object ExectuteStaticMethod(string name, string className, string methName, object[] args) 
        {
            if(!dlls.Exists(d => d.name == name))
                return null;

            if (!dlls[GetDllIndex(name)].staticMethods.ContainsKey(className))
                return null;
            if(!dlls[GetDllIndex(name)].staticMethods[className].ContainsKey(methName))
                return null;

            return dlls[GetDllIndex(name)].staticMethods[className][methName](args);
        }

        public static object ExecuteLoadableMethod(string name, string className, object[] args)
        {
            if (!dlls.Exists(d => d.name == name))
                return null;

            if (!dlls[GetDllIndex(name)].loadableClasses.ContainsKey(className))
                return null;

            return dlls[GetDllIndex(name)].loadableClasses[className].Run(args);
        }

        public static int GetDllIndex(string name)
        {
            if (!dlls.Exists(d => d.name == name))
                return -1;

            return dlls.FindIndex(d => d.name == name);
        }
    }

    public class RandomDll
    {
        public string name;

        public Dictionary<string, ILoadableRandomClass> loadableClasses;
        public Dictionary<string, IStaticRandomClass> staticClasses;

        public Dictionary<string, Dictionary<string, Func<object, object>>> staticMethods;

        public RandomDll(string name, Dictionary<string, ILoadableRandomClass> loadableClasses, Dictionary<string, IStaticRandomClass> staticClasses)
        {
            this.name = name;
            this.loadableClasses = loadableClasses;
            this.staticClasses = staticClasses;

            staticMethods = new();
        }
    }

    public interface ILoadableRandomClass
    {
        public string GetName();
        public void Initialize(object[] args);
        public object Run(object[] args);
    }

    public interface IStaticRandomClass
    {
        public string GetName();
        public void Initialize(object[] args);
        public Dictionary<string, Func<object, object>> GetMethods();
    }
}