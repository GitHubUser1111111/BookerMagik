using System.Diagnostics;

namespace EntityLibrary.Bookmaker
{
    [DebuggerDisplay("{Name}")]
    public class BookmakerEventParticipant
    {
        public string Name { get; }

        public BookmakerEventParticipant(string name)
        {
            Name = name;
        }
    }
}
