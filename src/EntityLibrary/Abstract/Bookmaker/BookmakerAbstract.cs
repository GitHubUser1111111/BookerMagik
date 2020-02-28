using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using EntityLibrary.Interfaces.Bookmaker;

namespace EntityLibrary.Abstract.Bookmaker
{
    public abstract class BookmakerAbstract : IBookmaker
    {
        public abstract Task<bool> Login();
    }
}
