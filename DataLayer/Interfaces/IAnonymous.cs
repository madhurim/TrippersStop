using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Interfaces
{
    interface IAnonymous
    {
        Guid VisitorGuid { get; set; }
        DateTime VisitedTime { get; set; }
        Guid KnownGuid { get; set; }
    }
}
