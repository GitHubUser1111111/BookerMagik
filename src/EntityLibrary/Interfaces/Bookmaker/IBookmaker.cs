using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EntityLibrary.Interfaces.Bookmaker
{
    public interface IBookmaker
    {
        Task<bool> Login();

    }
}
