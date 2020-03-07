using System.Diagnostics;

namespace EntityLibrary.Bookmaker.Sport
{
    [DebuggerDisplay("{Name}")]
    public class SportType
    {
        public string Name { get; }

        public SportType(string name)
        {
            Name = name;
        }
    }
}
