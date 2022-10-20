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
        [HttpGet]
        public ActionResult<List<ToDoItem>> GetAll() =>
            ToDoService.GetAll();

        // GET by id
        [HttpGet("{id}")]
        public ActionResult<ToDoItem> Get(int id)
        {
            var item = ToDoService.Get(id);
            if(item == null)
                return NotFound();
            return Ok(item);
        }

        // Get all by priority

        // Get all by type

        [HttpPost]
        public IActionResult Create(ToDoItem item)
        {
            ToDoService.Add(item);
            return CreatedAtAction(nameof(Create), new { id = item.Id }, item);
        }

        [HttpPost("{name}")]
        public IActionResult Create(string name)
        {
            ToDoService.Add(name);
            return CreatedAtAction(nameof(Create), new { name = name }, name);
        }

        [HttpPost("{name}/{category}")]
        public IActionResult Create(string name, string category)
        {
            ToDoService.Add(name, category);
            return CreatedAtAction(nameof(Create), new { name = name }, name);
        }
        [HttpPut("{id}/{name}")]
        public IActionResult Update(int id, ToDoItem item)
        {
            if (id != item.Id)
                return BadRequest();
            var existingItem = ToDoService.Get(id);
            if (existingItem == null)
                return NotFound();
            ToDoService.Update(item);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Remove(int id)
        {
            var item = ToDoService.Get(id);
            if (item == null)
                return NotFound();
            ToDoService.Delete(id);
            return NoContent();
        }

        [HttpDelete("{name}")]
        public IActionResult Remove(string name)
        {
            var item = ToDoService.Get(name);
            if (item == null)
                return NotFound();
            ToDoService.Delete(name);
            return NoContent();
        }
    }
}
