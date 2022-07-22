using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DealershipManagement.Entities.Account;

namespace DealershipManagement.DataAccess.EntityFrameworkConfigurations
{
    class SystemClaimConfiguration : IEntityTypeConfiguration<SystemClaim>
    {
        public void Configure(EntityTypeBuilder<SystemClaim> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasMany(x => x.RoleClaims)
                    .WithOne(x => x.Claim)
                    .HasForeignKey(x => x.ClaimId);
        }
    }
}
