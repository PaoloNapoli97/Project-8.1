using FileManager.Controller;
using UserModel;

namespace UserServices
{
    public class UserService
    {
        private const string UserDb = "UsersDb.json";
        FileManagers fileManager = new();
        private List<User> users = new();
        public void CreateUserDb()
        {
            fileManager.CreateFile<User>(UserDb);
        }
        public void WriteUsers()
        {
            fileManager.WriteItems(users, UserDb);
        }
        public List<User> ReadUsers()
        {
            users = fileManager.ReadItems<User>(UserDb);
            return users;
        }
        public User? ReadUserById(string id)
        {
            return users.Find(x => x.Id == id);
        }
        public User? ReadUserByIdStatus(string id)
        {
            return users.Find(x => x.Id == id && x.Status == !false);
        }
    }
}