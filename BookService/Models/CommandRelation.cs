using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookService.Models
{
    public class CommandRelation
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int UserId { get; set; }
        public int NextUserId { get; set; }
        public int GlowHubId { get; set; }
        public string Command { get; set; }
    }
}