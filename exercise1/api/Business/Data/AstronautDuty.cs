using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace StargateAPI.Business.Data
{
    [Table("AstronautDuty")]
    public class AstronautDuty
    {
        public int Id { get; set; }

        public int? PersonId { get; set; }

        public int RankId { get; set; }
        public virtual Rank Rank { get; set; }

        public int DutyTitleId { get; set; }
        public virtual DutyTitle DutyTitle { get; set; }

        public DateTime DutyStartDate { get; set; }

        public DateTime? DutyEndDate { get; set; }

        public virtual Person? Person { get; set; }
    }

    public class AstronautDutyConfiguration : IEntityTypeConfiguration<AstronautDuty>
    {
        public void Configure(EntityTypeBuilder<AstronautDuty> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();

            builder.HasOne(x => x.Rank)
               .WithMany() 
               .HasForeignKey(x => x.RankId)
               .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.DutyTitle)
                .WithMany() 
                .HasForeignKey(x => x.DutyTitleId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Person)
                .WithMany(p => p.AstronautDuties)
                .HasForeignKey(x => x.PersonId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired(false); // got stuck trying to coax EF into not generating an AD on CreatePerson
        }
    }
}
