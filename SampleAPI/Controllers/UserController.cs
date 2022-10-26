using Microsoft.AspNetCore.Mvc;
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

           



        [HttpGet("{username}")]
        public ActionResult<UserItem> Get(string username)
        {
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
            SqlDataAdapter tododa = new SqlDataAdapter($"SELECT * From ToDoItems WHERE userName = '{user.UserName}'", _connection);
            DataTable tododt = new DataTable();
            tododa.Fill(tododt);
            for (int j = 0; j < tododt.Rows.Count; j++)
            {
                ToDoItem item = new ToDoItem();
                item.Id = Convert.ToInt32(tododt.Rows[j]["ListID"]);
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
            _connection = new SqlConnection(_configuration.GetConnectionString("UserAppCon").ToString());
            SqlDataAdapter userda = new SqlDataAdapter($"SELECT * From Users WHERE UserName = '{user.UserName}'", _connection);
            DataTable userdt = new DataTable();
            userda.Fill(userdt);
            if (userdt.Rows.Count != 0)
                return BadRequest();
            _connection.Open();
            SqlCommand cmd = new SqlCommand($"Insert into Users values ('{user.UserName}', {user.ToDo.GetAll().Count+1})",_connection);
            int i = cmd.ExecuteNonQuery();
            if (i > 0)
            {
                //Get UserID
                user.ToDo.Add("Make a To Do List", "misc");
                var userToDo = user.ToDo.GetAll();
                int id = 1;
                for (int j = 0; j < userToDo.Count;j++)
                {
                    SqlCommand todoCommand = new SqlCommand($"Insert into ToDoItems values ({id++}, " +
                        $"'{userToDo[j].Name}', '{userToDo[j].type}', '{DateTime.Now.ToString("MM/dd/yyyy")}', " +
                        $"0, '{user.UserName}')", _connection);
                    int k = todoCommand.ExecuteNonQuery();
                    if (k == 0)
                        return BadRequest();
                }
                return CreatedAtAction(nameof(Create), new { name = user.UserName }, user);
            }
            return BadRequest();
            
        }   

        [HttpPost("{username}")]
        public IActionResult Create(string username)
        {
            _connection = new SqlConnection(_configuration.GetConnectionString("UserAppCon").ToString());
            SqlDataAdapter userda = new SqlDataAdapter($"SELECT * From Users WHERE UserName = '{username}'", _connection);
            DataTable userdt = new DataTable();
            userda.Fill(userdt);
            if (userdt.Rows.Count != 0)
                return BadRequest();
            _connection.Open();
            SqlCommand cmd = new SqlCommand($"Insert into Users values ('{username}', 1)", _connection);
            int i = cmd.ExecuteNonQuery();
            if (i > 0)
            {
                //Get UserID
                UserItem user = new UserItem();
                user.UserName = username;
                user.ToDo.Add("Make To Do List", "misc");
                var userToDo = user.ToDo.GetAll();
                foreach (ToDoItem item in userToDo)
                {
                    SqlCommand todoCommand = new SqlCommand($"Insert into ToDoItems values (1, " +
                        $"'{item.Name}', '{item.type}', '{item.Created}', 0, '{user.UserName}')", _connection);
                    int j = todoCommand.ExecuteNonQuery();
                    if (j == 0)
                        return BadRequest();
                }
                return CreatedAtAction(nameof(Create), new { name = user.UserName }, user);
            }
            return BadRequest();
        }


        [HttpPut("{username}/{toDoName}/{category}")]
        public IActionResult Update(string username, string toDoName, string category)
        {
            _connection = new SqlConnection(_configuration.GetConnectionString("UserAppCon").ToString());
            SqlDataAdapter userda = new SqlDataAdapter($"SELECT * From Users WHERE UserName = '{username}'", _connection);
            DataTable userdt = new DataTable();
            userda.Fill(userdt);
            if (userdt.Rows.Count == 0)
                return NotFound();
            //Get List number
            int listNum = Convert.ToInt32(userdt.Rows[0]["TotalListCount"]);
            SqlDataAdapter toDoAdapter = new SqlDataAdapter($"SELECT * From ToDoItems WHERE userName = '{username}'", _connection);
            DataTable toDo = new DataTable();
            toDoAdapter.Fill(toDo);
            _connection.Open();
            SqlCommand cmd = new SqlCommand($"Insert into ToDoItems values ({++listNum}, " +
                $"'{toDoName}', '{category}', '{DateTime.Now.ToString("MM/dd/yyyy")}', 0, '{username}')", _connection);
            int i = cmd.ExecuteNonQuery();
            if(i == 0)
                return BadRequest();
            //Update number of list count
            SqlCommand updateNumCmd = new SqlCommand($"update Users set TotalListCount = {listNum} where UserName = '{username}'", _connection);
            int j = updateNumCmd.ExecuteNonQuery();
            if (j == 0)
                return BadRequest();
            return NoContent();
        }

        [HttpPut("{username}")]
        public IActionResult Update(string username, ToDoItem item)
        {
 
            _connection = new SqlConnection(_configuration.GetConnectionString("UserAppCon").ToString());
            SqlDataAdapter userda = new SqlDataAdapter($"SELECT * From Users WHERE UserName = '{username}'", _connection);
            DataTable userdt = new DataTable();
            userda.Fill(userdt);
            if (userdt.Rows.Count == 0)
                return NotFound();
            //Get List number
            int listNum = Convert.ToInt32(userdt.Rows[0]["TotalListCount"]);
            
            _connection.Open();
            SqlCommand cmd = new SqlCommand($"Insert into ToDoItems values ({++listNum}, " +
                $"'{item.Name}', '{item.type}', '{DateTime.Now.ToString("MM/dd/yyyy")}', 0, '{username}')", _connection);
            int i = cmd.ExecuteNonQuery();
            if (i == 0)
                return BadRequest();
            //UPdate list num
            SqlCommand listcommand = new SqlCommand($"update Users set TotalListCount = {listNum} where UserName = '{username}'",_connection);
            int j = listcommand.ExecuteNonQuery();
            if (j == 0)
                return BadRequest();
            return NoContent();

        }
        [HttpPatch("{username}/{Id}/{isComplete}")]
        public IActionResult Update(string username, int Id, bool isComplete)
        {
            _connection = new SqlConnection(_configuration.GetConnectionString("UserAppCon").ToString());
            int complete = isComplete? 1 : 0;
            SqlCommand cmd = new SqlCommand($"update ToDoItems set isComplete = {complete} " +
                $"where userName = '{username}' and ListID = '{Id}'", _connection);
            _connection.Open();
            int i = cmd.ExecuteNonQuery();
            if(i==0) return NotFound();
            return NoContent();
        }
        

        [HttpDelete("{name}")]
        public IActionResult Remove(string name)
        {
            _connection = new SqlConnection(_configuration.GetConnectionString("UserAppCon").ToString());
 
            //Remove to do items from todoitem table
            SqlCommand todoCmd = new SqlCommand($"delete from ToDoItems where userName = '{name}'",_connection);
            _connection.Open();
            int i = todoCmd.ExecuteNonQuery();


            //Remove user from user table
            SqlCommand userCmd = new SqlCommand($"delete from Users where UserName = '{name}'", _connection);
            int j = userCmd.ExecuteNonQuery();
            if(j==0) return NotFound();
            return NoContent();
            
        }

        [HttpDelete("{username}/{toDoId}")]
        public IActionResult Remove(string username, int toDoId)
        {
            _connection = new SqlConnection(_configuration.GetConnectionString("UserAppCon").ToString());
            SqlCommand cmd = new SqlCommand($"delete from ToDoItems where userName = '{username}' and ListID = {toDoId}", _connection);
            _connection.Open();
            int i = cmd.ExecuteNonQuery();
            if(i == 0) return NotFound();
            return NoContent();
        }



    }
}
