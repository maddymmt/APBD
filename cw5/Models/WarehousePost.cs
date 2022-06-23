using System;
using System.ComponentModel.DataAnnotations;

namespace cw5.Models
{
    public class WarehousePost
    {
        [Required]
        public int IdProduct { get; set; }
        [Required]
        public int IdWarehouse { get; set; }
        [Required]
        [Range(0, int.MaxValue)]
        public int Amount { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; }
    }
}