using System;
using System.Collections.Generic;
using System.Text;
using EntityLibrary.Base;

namespace EntityLibrary.Infrastructure.State
{
    public class EasStateError : EasEntityBase
    {
        public string Description { get; set; }

        public int ErrorCode { get; set; }
    }
}
