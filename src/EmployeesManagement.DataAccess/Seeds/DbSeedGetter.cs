using System.Text.Json;
using EmployeesManagement.DataAccess.Entities;

namespace EmployeesManagement.DataAccess.Seeds
{
    public static class DbSeedGetter
    {
        public static IEnumerable<PersonEntity> GetPersons(string path)
            => GetItems<PersonEntity>(path, "Persons");

        public static IEnumerable<RoleEntity> GetRoles(string path)
            => GetItems<RoleEntity>(path, "Roles");

        public static IEnumerable<UserEntity> GetUsers(string path)
            => GetItems<UserEntity>(path, "Users");

        public static IEnumerable<UserRoleEntity> GetUserRoles(string path)
            => GetItems<UserRoleEntity>(path, "UserRoles");

        private static IEnumerable<T> GetItems<T>(string path, string directory) where T : class
        {
            var categories = Directory.GetFiles(Path.Combine(path, "InitialAsserts", directory));

            var results = categories
                .Select(name => GetPath(path, name))
                .Select(GetItemFromFile<IEnumerable<T>>)
                .SelectMany(x => x);

            return results;
        }

        private static string GetPath(string path, string name) => Path.Combine(path, name);

        private static T? GetItemFromFile<T>(string fileName)
        {
            var json = File.ReadAllText(fileName);

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };
            
            return JsonSerializer.Deserialize<T>(json, options);
        }
    }
}