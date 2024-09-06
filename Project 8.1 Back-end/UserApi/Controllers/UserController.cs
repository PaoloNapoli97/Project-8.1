using Microsoft.AspNetCore.Mvc;
using UserServices;
using UserModel;

namespace UserApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class UserController : ControllerBase
    {
        UserService userService = new();

        [HttpGet]
        [Route("{Id}")]
        public ActionResult<User> GetUserById(string Id)
        {
            userService.ReadUsers();
            var user = userService.ReadUserById(Id);
            return user == null ? NotFound() : Ok(user);
        }

        [HttpGet]
        [Route("{Id}/StartLogin")]
        public ActionResult<string> StartLogin(string Id)
        {
            userService.ReadUsers();
            var user = userService.ReadUserByIdStatus(Id);
            if (user == null)
            {
                return Unauthorized();
            }
            if (user.Status == false)
            {
                return Unauthorized("Your Account Has been Blocked");
            }

            return Ok(user.CreateChallenge());
        }

        [HttpGet]
        [Route("{Id}/{Challenge}")]
        public ActionResult<string> Login(string Id, string Challenge)
        {
            userService.ReadUsers();
            var user = userService.ReadUserByIdStatus(Id);
            if (user == null)
            {
                return Unauthorized("User not found");
            }
            if (user.Status == false)
            {
                return Unauthorized("Your Account Has been Blocked");
            }
            user.CreateChallenge();
            if (user.VerifyChallenge(Challenge))
            {
                return Ok(user.CreateToken());
            }
            else
            {
                return Unauthorized();
            }
        }
    }
}
