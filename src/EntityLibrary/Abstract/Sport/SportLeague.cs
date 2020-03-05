using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace EntityLibrary.Abstract.Sport
{
    [DebuggerDisplay("{Name}")]
    public class SportLeague
    {
        public string Name { get; set; }

        public SportLeague(string name)
        {
            Name = name;
        }
    }
}
