using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoDB
{
    class Todo
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool Done { get; set; }

        public Todo()
        {
           Id = Guid.NewGuid();
        }
    }
}
