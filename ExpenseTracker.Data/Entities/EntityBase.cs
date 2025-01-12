using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Data.Entities
{
    public class EntityBase
    {
        public int Status { get; set; }
        [StringLength(20)]
        public string? UpdatedBy { get; set; }
        [StringLength(20)]
        public string CreatedBy { get; set; }
        public DateTime CreatedDateTimeUTC { get; set; }
        public DateTime? UpdatedDateTimeUTC { get; set; }
    }
}
