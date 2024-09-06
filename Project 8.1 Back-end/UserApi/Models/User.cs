namespace UserModel
{
    public class User
    {
        public string? Name { get; set; }
        private string? _password = "password";
        public string Id { get; set; }
        private string? _challenge = null;
        public Token Token = null;
        private bool _isEnable;
        public enum RoleList
        {
            User,
            Admin,
            LabAdministrator,
        }
        // public User(string Id, string Name, string Password, bool Status)
        // // {
        // //     this.Id = Id; //This is solely to fix the csv header issue I had with HTTP when reading the files.
        // //     this.Name = Name;
        // //     this.Password = Password;
        // //     this.Status = Status;
        // // }

        public RoleList Role { get; set; }

        public User(string Id, string Name, string Password, bool Status, RoleList Role)
        {
            this.Id = Id;
            this.Name = Name;
            this.Password = Password;
            this.Status = Status;
            this.Role = Role;
        }
        private bool ValidatePassword(string? paswd)
        {
            return true;
        }
        internal string? Pass
        {
            get { return _password; }
            private set
            {
                if (ValidatePassword(value))
                {
                    _password = value;
                }
                else
                {
                    _password = value;
                }
            }
        }
        public bool Status
        {
            get { return _isEnable; }
            private set
            {
                _isEnable = value;
            }
        }
        public string? Password
        {
            get { return Pass; }
            private set
            {
                Pass = value;
            }
        }
        internal string? CreateChallenge()
        {
            _challenge = $"{Id}";
            return _challenge;
        }
        private string? CalculateChallenge()
        {
            if (_challenge == null)
            {
                return null;
            }
            return $"{_challenge}{_password}";
        }

        public bool VerifyChallenge(string challenge)
        {
            if (_challenge == null)
            {
                return false;
            }
            return challenge == CalculateChallenge();
        }

        internal Token? CreateToken()
        {
            string RoletoString= Role.ToString();
            Token = new Token(Id, RoletoString);
            return Token;
        }

        internal bool VerifyToken(Token tok)
        {
            return Token == tok;
        } 

        internal bool EditIsEnable(bool newStatus)
        {
            return _isEnable = newStatus;
        }
    }
    // public class Credentials
    // {
    //     private List<User> _users = null;

    //     public User GetUser(User user)
    //     {
    //         return _users.Find(x => x == user);
    //     }

    //     public User GetUserId(string? Id){
    //         return _users.Find(x => x.Id == Id);
    //     }

    //     internal bool GetUserByToken(string? token)
    //     {
    //         var user = _users.Find(x => x.VerifyToken(token));
    //         return user != null;
    //     }

    //     public List<User> GetAll()
    //     {
    //         return _users;
    //     }
    // }
}
