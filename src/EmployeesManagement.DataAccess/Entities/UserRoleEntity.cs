using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeesManagement.DataAccess.Entities;

[Table("user_role")]
public class UserRoleEntity
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("user_id")]
    public int UserId { get; set; }

    [ForeignKey(nameof(UserId))]
    public virtual UserEntity User { get; set; }

    [Column("role_id")]
    public int RoleId { get; set; }
    
    [ForeignKey(nameof(RoleId))]
    public virtual RoleEntity Role { get; set; }
}