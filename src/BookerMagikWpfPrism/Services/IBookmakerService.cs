using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using BookerMagikCore.Bookmaker;

namespace BookerMagikWpfPrism.Services
{
    public interface IBookmakerService
    {
        string LoadBookmakerConfiguration(string bookmaker);

        Task<bool> LoginBookmaker(IBookmaker bookmaker, string configuration);
    }
}
