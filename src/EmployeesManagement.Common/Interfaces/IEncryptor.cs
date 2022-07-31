namespace EmployeesManagement.Common.Interfaces;

public interface IEncryptor
{
    string GetHash(string value, string salt);
}