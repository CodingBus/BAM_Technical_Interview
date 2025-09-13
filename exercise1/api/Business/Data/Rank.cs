using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace StargateAPI.Business.Data
{
    [Table("Rank")]
    public class Rank
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
    public class RankConfiguration : IEntityTypeConfiguration<Rank>
    {
        public void Configure(EntityTypeBuilder<Rank> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
        }
    }

    public enum RankEnum
    {
        Commander = 1,
        Loieutenant = 2,
        Private = 3,
        Captain = 4,
        Sergeant = 5
    }
}