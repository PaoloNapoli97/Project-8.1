using Microsoft.AspNetCore.Mvc;
using UserServices;
using UserModel;
using UserPatchDto;

namespace UserApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class AdminController : ControllerBase
    {
        UserService userService = new();

        [HttpGet]
        [Route("GetAll/Users")]
        public ActionResult<List<User>> GetUsers()
        {
            userService.CreateUserDb();
            return Ok(userService.ReadUsers());
        }

        [HttpGet]
        [Route("{Id}")]
        public ActionResult<User> GetUserById(string Id)
        {
            userService.ReadUsers();
            var user = userService.ReadUserById(Id);
            return user == null ? NotFound() : Ok(user);
        }


        [HttpPost]
        [Route("CreateUser")]
        public ActionResult PostUser(User user)
        {
            userService.ReadUsers();
            var checkIfExist = userService.ReadUserById(user.Id);
            if (checkIfExist != null)
            {
                return BadRequest("Id not unique, unable to add user");
            }
            else
            {
                // User user = new User(Id, Name, Password, Status, Role);
                userService.ReadUsers().Add(user);
                var pathToUrl = Request.Path.ToString() + '/' + user.Id;
                userService.WriteUsers();
                return Created(pathToUrl, user);
            }
        }

        [HttpPut]
        [Route("{Id}/Replace/User")]
        public ActionResult PutUser(string Id, User user)
        {
            userService.ReadUsers();
            var editUser = userService.ReadUserById(Id);
            if (editUser == null)
            {
                return BadRequest("User was not found");
            }
            else
            {
                editUser.Id = user.Id;
                editUser.Name = user.Name;
                editUser.Role = user.Role;
                userService.WriteUsers();
                return Ok(editUser);
            }
        }

        [HttpPatch]
        [Route("{Id}/Edit/User")]
        public ActionResult PatchUser(string Id, [FromBody] UserPatchDTO userPatchDTO)
        {
            userService.ReadUsers();
            var editUser = userService.ReadUserById(Id);
            if (editUser == null)
            {
                return BadRequest("User was not found");
            }
            else
            {
                if (userPatchDTO.Id != null)
                {                    
                    editUser.Id = userPatchDTO.Id;
                }
                if (userPatchDTO.Name != null)
                {
                    editUser.Name = userPatchDTO.Name;
                }
                if (userPatchDTO.Role != null)
                {                    
                    editUser.Role = userPatchDTO.Role.Value;
                }
                userService.WriteUsers();
                return Ok(editUser);
            }
        }

        [HttpPatch]
        [Route("{Id}/ChangeStatus")]
        public ActionResult PatchUserStatus(string Id, [FromBody] UserStatusDTO userStatusDTO)
        {
            userService.ReadUsers();
            var editUser = userService.ReadUserById(Id);
            if (editUser == null)
            {
                return BadRequest("User was not found");
            }
            else
            {
                editUser.EditIsEnable(userStatusDTO.Status);
                userService.WriteUsers();
                return Ok(editUser);
            }
        }

    }
}
