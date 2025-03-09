using Microsoft.AspNetCore.Mvc;
using TodoAppBackend.Models;
using TodoAppBackend.Services;

namespace TodoAppBackend.Controllers
{
    [ApiController]
    [Route("api/todos")]
    public class TodoController : Controller
    {
        private readonly ITodoService _todoService;

        public TodoController(ITodoService todoService)
        {
            _todoService = todoService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var todos = await _todoService.GetAllAsync();
            return Ok(todos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var todo = await _todoService.GetByIdAsync(id);
            if (todo == null) return NotFound("TodoItem không tồn tại.");
            return Ok(todo);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] string title)
        {
            if (string.IsNullOrEmpty(title)) return BadRequest("Tiêu đề không được để trống.");

            var newTodo = await _todoService.CreateAsync(title);
            return CreatedAtAction(nameof(GetById), new { id = newTodo.Id }, newTodo);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] TodoItem todo)
        {
            if (id != todo.Id) return BadRequest("ID không hợp lệ.");

            var updated = await _todoService.UpdateAsync(todo);
            if (!updated) return NotFound("TodoItem không tồn tại.");

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _todoService.DeleteAsync(id);
            if (!deleted) return NotFound("TodoItem không tồn tại.");

            return NoContent();
        }
    }
}
