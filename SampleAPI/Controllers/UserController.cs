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
        /*[HttpGet("{id}")]
        public ActionResult<UserItem> Get(int id)
        {
            var user = UserService.Get(id);
            if (user == null)
                return NotFound();
            return Ok(user);
        }*/


        [HttpGet("{username}")]
        public ActionResult<UserItem> Get(string username)
        {
            var user = UserService.Get(username);
            if (user == null)
                return NotFound();
            return Ok(user);
        }

        [HttpPost]
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

        [HttpPut("{username}")]
        public IActionResult Update(UserItem user, string username)
        {
            if (username != user.UserName)
                return BadRequest();
            var existingUser = UserService.Get(username);
            if (existingUser == null)
                return NotFound();
            UserService.Update(user);
            return NoContent();
        }

        [HttpPatch("{username}")]
        public IActionResult Update(string username, ToDoItem item)
        {
            var user = UserService.Get(username);
            if (user == null)
                return BadRequest();
            user.ToDo.Add(item);
            return NoContent();
        }
        [HttpPatch("{username}/{Id}/{isComplete}")]
        public IActionResult Update(string username, int Id, bool isComplete)
        {
            var user = UserService.Get(username);
            if (user == null)
                return BadRequest();
            var item = user.ToDo.Get(Id);
            if (item == null)
                return BadRequest();
            item.isComplete = isComplete;
            user.ToDo.Update(item);
            return NoContent();
        }
        

        [HttpDelete("{name}")]
        public IActionResult Remove(string name)
        {
            var user = UserService.Get(name);
            if (user == null)
                return NotFound();
            UserService.Delete(name);
            return NoContent();
        }



    }
}
