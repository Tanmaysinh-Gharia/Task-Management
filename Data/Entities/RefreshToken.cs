using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManagement.Data.Entities
{
    public class RefreshToken
    {
        [Key]
        public Guid RefreshTokenId { get; set; }

        [Required]
        public string Token { get; set; }

        [Required]
        public DateTime Expires { get; set; }

        public bool IsRevoked { get; set; }

        public int UserId { get; set; }

        public string CreatedFromIp { get; set; }

        [ForeignKey(nameof(UserId))]
        public User User { get; set; }
    }

}
