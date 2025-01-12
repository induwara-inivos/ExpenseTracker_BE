using System.ComponentModel.DataAnnotations;

namespace ExpenseTracker.Data.Entities
{
    public class User: EntityBase
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(100)]
        [Required]
        public string Name { get; set; }

        [MaxLength(200)]
        [Required]
        public string Email { get; set; }

        [MaxLength(200)]
        public string Password { get; set; }
        public string Provider { get; set; }
        public string ProviderId { get; set; }

    }
}
