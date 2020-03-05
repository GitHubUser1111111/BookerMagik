namespace EntityLibrary.Abstract.Bookmaker
{
    public abstract class BookmakerEventParticipantAbstract
    {
        protected BookmakerEventParticipantAbstract(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }
}