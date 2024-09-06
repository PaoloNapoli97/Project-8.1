using UserModel;

public class Token
{
    public string? UserId {get; set;}
    public Guid Guid {get; set;}
    public string Role {get; set;}
    public Token(string UserId, string Role){
        this.UserId = UserId;
        this.Role = Role;
        Guid = Guid.NewGuid();
    }

}