using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomUtilities
{
    public interface ICustomDllClass
    {
        public string GetName();
        public ICustomDllClass Build(object instance);
    }
}
