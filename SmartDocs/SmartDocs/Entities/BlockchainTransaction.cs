using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SmartDocs.Entities
{
    public class BlockchainTransaction
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        public string TransactionAddress { get; set; }

        public DateTime CreationDate { get; set; } = DateTime.UtcNow;

    }
}
