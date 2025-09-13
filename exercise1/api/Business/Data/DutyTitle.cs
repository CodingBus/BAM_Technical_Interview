using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace StargateAPI.Business.Data
{
    [Table("DutyTitle")]
    public class DutyTitle
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
    public class DutyTitleConfiguration : IEntityTypeConfiguration<DutyTitle>
    {
        public void Configure(EntityTypeBuilder<DutyTitle> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
        }
    }

    public enum DutyTitleEnum
    {
        Janitor = 1,
        Engineer = 2,
        Pilot = 3,
        Scientist = 4,
        Retired = 5
    }
}