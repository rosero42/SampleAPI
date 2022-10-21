using Microsoft.AspNetCore.Mvc;
using SampleAPI.Model;
using SampleAPI.Services;

namespace SampleAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ToDoController : ControllerBase
    {
        public ToDoController()
        {
        }

        // GET all
/*        [HttpGet("{user}")]
        public ActionResult<List<ToDoItem>> GetAll(UserItem user) =>
            user.ToDo.GetAll();

        // GET by id
        [HttpGet("{user}/{id}")]
        public ActionResult<ToDoItem> Get(UserItem user,int id)
        {
            var item = user.ToDo.Get(id);
            if(item == null)
                return NotFound();
            return Ok(item);
        }

        // Get all by priority

        // Get all by type

        [HttpPost("{user}")]
        public IActionResult Create(UserItem user, ToDoItem item)
        {
            user.ToDo.Add(item);
            return CreatedAtAction(nameof(Create), new { id = item.Id }, item);
        }

        [HttpPost("{user}/{name}")]
        public IActionResult Create(UserItem user, string name)
        {
            user.ToDo.Add(name);
            return CreatedAtAction(nameof(Create), new { name = name }, name);
        }

        [HttpPost("{user}/{name}/{category}")]
        public IActionResult Create(UserItem user, string name, string category)
        {
            user.ToDo.Add(name, category);
            return CreatedAtAction(nameof(Create), new { name = name }, name);
        }
        [HttpPut("{user}/{id}")]
        public IActionResult Update(UserItem user, int id, ToDoItem item)
        {
            if (id != item.Id)
                return BadRequest();
            var existingItem = user.ToDo.Get(id);
            if (existingItem == null)
                return NotFound();
            user.ToDo.Update(item);
            return NoContent();
        }

        [HttpDelete("{user}/{id}")]
        public IActionResult Remove(UserItem user, int id)
        {
            var item = user.ToDo.Get(id);
            if (item == null)
                return NotFound();
            user.ToDo.Delete(id);
            return NoContent();
        }

        [HttpDelete("{user}/{name}")]
        public IActionResult Remove(UserItem user, string name)
        {
            var item = user.ToDo.Get(name);
            if (item == null)
                return NotFound();
            user.ToDo.Delete(name);
            return NoContent();
        }*/
    }
}
