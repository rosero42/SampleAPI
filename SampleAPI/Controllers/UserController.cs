using Microsoft.AspNetCore.Mvc;
using SampleAPI.Model;
using SampleAPI.Services;

namespace SampleAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        public UserController()
        {
        }
        // Get all
        [HttpGet]
        public ActionResult<List<UserItem>> GetAll() =>
           UserService.GetAll();
        
        
        // Get by id
        [HttpGet("{id}")]
        public ActionResult<UserItem> Get(int id)
        {
            var user = UserService.Get(id);
            if (user == null)
                return NotFound();
            return Ok(user);
        }

        [HttpGet("{username}")]
        public ActionResult<UserItem> Get(string username)
        {
            var user = UserService.Get(username);
            if (user == null)
                return NotFound();
            return Ok(user);
        }

        [HttpPost("{user}")]
        public IActionResult Create(UserItem user)
        {
            var tempUser = UserService.Get(user.UserName);
            if (tempUser != null)
                return BadRequest();
            UserService.Add(user);
            return CreatedAtAction(nameof(Create), new { name = user.UserName }, user);
        }

        [HttpPost("{username}")]
        public IActionResult Create(string username)
        {
            var tempUser = UserService.Get(username);
            if (tempUser != null)
                return BadRequest(tempUser);
            UserService.Add(username);
            return CreatedAtAction(nameof(Create), new { name = username }, username);
        }



    }
}
