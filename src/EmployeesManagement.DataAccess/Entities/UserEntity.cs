using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeesManagement.DataAccess.Entities;

[Table("user")]
public class UserEntity
{
    [Key]
    [Column("id")]
    public int Id { get; set; }
    
    [Column("login")]
    public string Login { get; set; }
    
    [Column("salt")]
    public string Salt { get; set; }
    
    [Column("hash")]
    public string Hash { get; set; }
    
    [Column("person_id")]
    public int PersonId { get; set; }
    
    [Column("disabled")]
    public bool Disabled { get; set; }
    
    [InverseProperty(nameof(UserRoleEntity.User))]
    public virtual IEnumerable<UserRoleEntity> Roles { get; set; }
    
    [ForeignKey(nameof(PersonId))]
    public virtual PersonEntity Person { get; set; }
}