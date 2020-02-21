using System;
using System.Collections.Generic;
using System.Text;
using EntityLibrary.Base;

namespace EntityLibrary.Infrastructure.State
{
    public class EasStateNotification : EasEntityBase
    {
        public string Notification { get; set; }

        public string Email { get; set; }
    }
}
