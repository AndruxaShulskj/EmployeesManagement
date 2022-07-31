using EmployeesManagement.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EmployeesManagement.DataAccess.EntitiesConfigurations;

public class UserEntityConfiguration : IEntityTypeConfiguration<UserEntity>
{
    public void Configure(EntityTypeBuilder<UserEntity> builder)
    {
        builder
            .Property(x => x.Hash)
            .IsRequired();
        
        builder
            .Property(x => x.Salt)
            .IsRequired();
        
        builder
            .Property(x => x.PersonId)
            .IsRequired();
    }
}