using System.Diagnostics;

namespace EntityLibrary.Bookmaker.Sport
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
