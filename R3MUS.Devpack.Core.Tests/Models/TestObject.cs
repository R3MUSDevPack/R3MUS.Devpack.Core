using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace R3MUS.Devpack.Core.Tests.Models
{
    public class TestObject
    {
        public int Id { get; set; }
        public Guid Guid { get; set; }
        public string String { get; set; }
        public List<DateTime> List { get; set; }
    }
}
