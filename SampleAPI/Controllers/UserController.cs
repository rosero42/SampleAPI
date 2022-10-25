﻿using Microsoft.AspNetCore.Mvc;
using SampleAPI.Model;
using SampleAPI.Services;
using System.Data;
using System.Data.SqlClient;

namespace SampleAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        public readonly IConfiguration _configuration;
        private SqlConnection _connection;
        public UserController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        // Get all
        [HttpGet]
        public ActionResult<List<UserItem>> GetAll() {
            _connection = new SqlConnection(_configuration.GetConnectionString("UserAppCon").ToString());
            SqlDataAdapter userda = new SqlDataAdapter("SELECT * From Users",_connection);
            DataTable userdt = new DataTable();
            userda.Fill(userdt);
            List<UserItem> userList = new List<UserItem>();
            if (userdt.Rows.Count > 0)
            {
                for(int i = 0; i < userdt.Rows.Count; i++)
                {
                    UserItem user = CreateUser(userdt.Rows, i);
                    userList.Add(user);
                }
                if (userList.Count > 0)
                    return userList;

            }

            return NotFound();
        }

           


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
            /*var user = UserService.Get(username);
            if (user == null)
                return NotFound();
            return Ok(user);*/
            _connection = new SqlConnection(_configuration.GetConnectionString("UserAppCon").ToString());
            SqlDataAdapter userda = new SqlDataAdapter($"SELECT * From Users WHERE UserName = '{username}'", _connection);
            DataTable userdt = new DataTable();
            userda.Fill(userdt);
            if(userdt.Rows.Count > 0)
            {
                return CreateUser(userdt.Rows, 0);
            }
            else
            {
                return NotFound();
            }
        }

        private UserItem CreateUser(DataRowCollection dt, int index)
        {
            UserItem user = new UserItem();
            user.Id = Convert.ToInt32(dt[index]["UserID"]);
            user.UserName = Convert.ToString(dt[index]["UserName"]);
            SqlDataAdapter tododa = new SqlDataAdapter($"SELECT * From ToDoItems WHERE userName = {user.UserName}", _connection);
            DataTable tododt = new DataTable();
            tododa.Fill(tododt);
            for (int j = 0; j < tododt.Rows.Count; j++)
            {
                ToDoItem item = new ToDoItem();
                item.Id = Convert.ToInt32(tododt.Rows[j]["ItemListID"]);
                item.Name = Convert.ToString(tododt.Rows[j]["ItemName"]);
                item.type = Convert.ToString(tododt.Rows[j]["ItemCategory"]);
                item.Created = Convert.ToString(tododt.Rows[j]["ItemCreated"]);
                item.isComplete = Convert.ToBoolean(tododt.Rows[j]["isComplete"]);
                user.ToDo.Add(item);
            }
            return user;
        }

        [HttpPost]
        public IActionResult Create(UserItem user)
        {
            /*var tempUser = UserService.Get(user.UserName);
            if (tempUser != null)
                return BadRequest();
            UserService.Add(user);
            return CreatedAtAction(nameof(Create), new { name = user.UserName }, user);*/
            _connection = new SqlConnection(_configuration.GetConnectionString("UserAppCon").ToString());
            SqlDataAdapter userda = new SqlDataAdapter($"SELECT * From Users WHERE UserName = '{user.UserName}'", _connection);
            DataTable userdt = new DataTable();
            userda.Fill(userdt);
            if (userdt.Rows.Count != 0)
                return BadRequest();
            _connection.Open();
            SqlCommand cmd = new SqlCommand($"Insert into Users values ('{user.UserName}')",_connection);
            int i = cmd.ExecuteNonQuery();
            if (i > 0)
            {
                //Get UserID
                user.ToDo.Add("Make a To Do List", "misc");
                var userToDo = user.ToDo.GetAll();
                foreach (ToDoItem item in userToDo)
                {
                    SqlCommand todoCommand = new SqlCommand($"Insert into ToDoItems values ({item.Id}, '{item.Name}', '{item.type}', '{item.Created}', 0, {user.UserName})", _connection);
                    int j = todoCommand.ExecuteNonQuery();
                    if (j == 0)
                        return BadRequest();
                }
                return CreatedAtAction(nameof(Create), new { name = user.UserName }, user);
            }
            return BadRequest();
            
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

        [HttpPut("{username}/{toDoName}/{category}")]
        public IActionResult Update(string username, string toDoName, string category)
        {
            var existingUser = UserService.Get(username);
            if (existingUser == null)
                return NotFound();
            existingUser.ToDo.Add(toDoName,category);
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

        [HttpDelete("{username}/{toDoId}")]
        public IActionResult Remove(string username, int toDoId)
        {
            var user = UserService.Get(username);
            if (user == null)
                return NotFound();
            user.ToDo.Delete(toDoId);
            return NoContent();
        }



    }
}
