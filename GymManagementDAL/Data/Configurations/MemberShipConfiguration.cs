using GymManagementDAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Data.Configurations
{
    public class MemberShipConfiguration : IEntityTypeConfiguration<MemberShip>
    {
        public void Configure(EntityTypeBuilder<MemberShip> builder)
        {

            builder.Ignore(x => x.Id);

            builder.Property(x => x.CreatedAt)
                .HasColumnName("BookingDate")
                .HasDefaultValue("GetDate()");

            builder.HasOne(x => x.Plan)
                .WithMany(x => x.PlanMembers)
                .HasForeignKey(x => x.PlanId);

            builder.HasOne(x => x.Member)
                .WithMany(x => x.MemberPlans)
                .HasForeignKey(x => x.MemberId);

            builder.HasKey(x => new { x.PlanId, x.MemberId });
        }
    }
}
