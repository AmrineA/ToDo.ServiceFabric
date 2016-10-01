using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDo.Domain
{
    public sealed class ToDoItem
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool Completed { get; set; }
        public double Effort { get; set; }
        public double RealisticEffort
        {
            get
            {
                return Effort * 3;
            }
        }
    }
}
