using System.Collections.Generic;
using EntityLibrary.Base;

namespace EntityLibrary.Infrastructure.State
{
    public class EasSystemState : EasEntityBase
    {
        public IReadOnlyList<EasStateError> Errors { get; set; }

        public IReadOnlyList<EasStateWarning> Warnings { get; set; }

        public IReadOnlyList<EasStateNotification> Notifications { get; set; }
    }
}
