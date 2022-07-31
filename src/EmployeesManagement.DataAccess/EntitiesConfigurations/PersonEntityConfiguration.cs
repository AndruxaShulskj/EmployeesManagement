using EmployeesManagement.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EmployeesManagement.DataAccess.EntitiesConfigurations;

public class PersonEntityConfiguration : IEntityTypeConfiguration<PersonEntity>
{
    public void Configure(EntityTypeBuilder<PersonEntity> builder)
    {
        builder
            .HasIndex(p => new { p.Name, p.Surname, p.SecondName })
            .IsUnique();
    }
}
