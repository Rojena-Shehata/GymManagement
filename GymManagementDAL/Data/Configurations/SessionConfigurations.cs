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
    public class SessionConfigurations : IEntityTypeConfiguration<Session>
    {
        public void Configure(EntityTypeBuilder<Session> builder)
        {
            builder.ToTable(x =>
            {
                x.HasCheckConstraint("SessionCapacityCheck", "Capacity between 1 and 25");
                x.HasCheckConstraint("SessionEndDateCheck", "EndDate > StartDate");
            });

            builder.HasOne(x => x.Trainer)
                .WithMany(x => x.Sessions)
                .HasForeignKey(x => x.TrainerId);

            builder.HasOne(x => x.Category)
                .WithMany(x => x.Sessions)
                .HasForeignKey(x => x.CategoryId);
           
        }
    }
}
