using System;
using System.Collections.Generic;
using System.Text;

namespace BookerMagikCore.Bookmaker
{
    public interface IGlobalBookmaker
    {
        int RegisterBookmaker(IBookmaker bookmaker);
    }
}
