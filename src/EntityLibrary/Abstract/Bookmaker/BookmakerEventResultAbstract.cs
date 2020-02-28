using System;
using System.Collections.Generic;
using System.Text;

namespace EntityLibrary.Abstract.Bookmaker
{
    public abstract class BookmakerEventResultAbstract
    {
        public decimal Coefficient { get; }

        protected BookmakerEventResultAbstract(decimal coefficient)
        {
            Coefficient = coefficient;
        }
    }
}
