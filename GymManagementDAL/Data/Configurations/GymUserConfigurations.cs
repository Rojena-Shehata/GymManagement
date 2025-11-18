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
    public class GymUserConfigurations<T> : IEntityTypeConfiguration<T> where T : GymUser
    {
        public void Configure(EntityTypeBuilder<T> builder)
        {
            builder.Property(x => x.Name)
                .HasColumnType("varchar")
                .HasMaxLength(50);

            builder.Property(x => x.Email)
                    .HasColumnType("varchar")
                    .HasMaxLength(100);

            builder.Property(x => x.Phone)
                    .HasColumnType("varchar")
                    .HasMaxLength(11);

            //by default will add prefix Address_ before property name
            builder.OwnsOne(x => x.Address, address =>
            {
                address.Property(a => a.BuildingNumber)
                        .HasColumnName("BuildingNumber");

                address.Property(a => a.Street)
                        .HasColumnType("varchar")
                        .HasMaxLength(30)
                        .HasColumnName("Street");

                address.Property(a => a.City)
                        .HasColumnType("varchar")
                        .HasMaxLength(30)
                        .HasColumnName("City");
            });
            builder.HasIndex(x => x.Email)
                .IsUnique();
            //add constraint to database
            builder.ToTable(x =>
            {
                x.HasCheckConstraint("GymUser_EmailCheck",
                                "Email LIKE '_%@_%._%'");

                x.HasCheckConstraint("GymUser_PhoneCheck",
                                "phone LIKE '01%' and phone Not Like'%[^0-9]%'");
            });            

        }
    }
}
