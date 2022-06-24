using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ParkyAPI.Models;
using ParkyAPI.Repository.IRepository;

namespace ParkyAPI.Controllers
{
    [Authorize]
    [Route("api/v{version:apiVersion}/Users")]
    [ApiController]
    public class UsersController : Controller
    {
        private readonly IUserRepository _userRepo;
        public UsersController(IUserRepository userRepo)
        {
            _userRepo = userRepo;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody]Authentication model) 
        {
            var user = _userRepo.Authenticate(model.Username, model.Password);
            if (user == null)
                return BadRequest(new { message = "Username or password is invalid" });
            return Ok(user);
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public IActionResult Register([FromBody]Authentication model)
        {
            bool ifUniqueUser = _userRepo.IsUniqueUser(model.Username);
            if (!ifUniqueUser)
                return BadRequest(new { message = "Username already exists" });
            var user = _userRepo.Register(model.Username, model.Password);
            if(user == null)
                return BadRequest(new { message = " Error while registering" });
            return Ok();
        }
    }
}
