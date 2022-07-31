using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeesManagement.DataAccess.Entities;

[Table("person")]
public class PersonEntity
{
    [Key]
    [Column("id")]
    public int Id { get; set; }
    
    [Column("service")]
    public bool Service { get; set; }
    
    [Column("name")]
    public string Name { get; set; }
    
    [Column("surname")]
    public string Surname { get; set; }
    
    [Column("second_name")]
    public string SecondName { get; set; }
    
    [Column("deleted")]
    public bool Deleted { get; set; }
}