using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace R3MUS.Devpack.Core.Tests.Models
{
    public class InvalidTestObject
    {
        public int InvId { get; set; }
        public Guid InvGuid { get; set; }
        public string InvString { get; set; }
        public List<DateTime> InvList { get; set; }
    }
}
