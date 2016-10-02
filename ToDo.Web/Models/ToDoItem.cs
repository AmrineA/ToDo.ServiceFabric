using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ToDo.Web.Models
{
    public class ToDoItem
    {
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        public bool Completed { get; set; }
        [DisplayName("Estimate (Days)")]
        public double Effort { get; set; }
        [DisplayName("Realistic Estimate")]
        public double RealisticEffort { get; set; }
    }
}