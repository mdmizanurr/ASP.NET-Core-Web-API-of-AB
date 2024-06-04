using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EAPI.Entities
{
    public class CityEntityTypeConfiguration : IEntityTypeConfiguration<City>
    {
        public void Configure(EntityTypeBuilder<City> builder)
        {
            builder.ToTable("Cities");
            builder.HasKey(x => x.Id);
            builder.Property(i => i.Id).IsRequired();
            builder
                .HasOne(c => c.Country)
                .WithMany(x => x.Cities)
                .HasForeignKey(i => i.CountryId);
            builder.Property(x => x.Lat).HasColumnType("decimal(7,4)");
            builder.Property(x => x.Lon).HasColumnType("decimal(7,4)");
        }
    }
}
