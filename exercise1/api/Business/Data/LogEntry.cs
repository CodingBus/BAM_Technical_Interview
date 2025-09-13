using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StargateAPI.Business.Data
{
    [Table("LogEntry")]
    public class LogEntry
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string LogSource { get; set; } = string.Empty;

        [Required]
        public LogLevel LogLevel { get; set; } = LogLevel.None;

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public string Message { get; set; } = string.Empty;
    }
}
