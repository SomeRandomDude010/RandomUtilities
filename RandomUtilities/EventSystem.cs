using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomUtilities
{
    public static class EventSystem
    {
        public readonly static Dictionary<string, EventHandler> events = new();

        public static void InvokeEvent(string name, object sender, EventArgs e)
        {
            events[name]?.Invoke(sender, e);
        }
    }
}
