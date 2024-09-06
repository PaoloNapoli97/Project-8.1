using UserModel;

namespace UserPatchDto
{
    public class UserPatchDTO
    {
        public string? Id {get; set;}
        public string? Name {get; set;}
        public User.RoleList? Role {get; set;}
    }
}