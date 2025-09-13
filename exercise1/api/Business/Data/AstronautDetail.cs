using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace StargateAPI.Business.Data
{
    [Table("AstronautDetail")]
    public class AstronautDetail
    {
        public int Id { get; set; }

        public int PersonId { get; set; }

        public int CurrentRankId { get; set; }
        public virtual Rank CurrentRank { get; set; }

        public int CurrentDutyTitleId { get; set; }
        public virtual DutyTitle CurrentDutyTitle { get; set; }

        public DateTime CareerStartDate { get; set; }

        public DateTime? CareerEndDate { get; set; }

        public virtual Person Person { get; set; }
    }

    public class AstronautDetailConfiguration : IEntityTypeConfiguration<AstronautDetail>
    {
        public void Configure(EntityTypeBuilder<AstronautDetail> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();

            builder.HasOne(x => x.CurrentRank)
               .WithMany()
               .HasForeignKey(x => x.CurrentRankId)
               .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.CurrentDutyTitle)
                .WithMany()
                .HasForeignKey(x => x.CurrentDutyTitleId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
