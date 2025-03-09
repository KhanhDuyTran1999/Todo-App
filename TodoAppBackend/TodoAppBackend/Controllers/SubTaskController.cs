using Microsoft.AspNetCore.Mvc;
using TodoAppBackend.Models;
using TodoAppBackend.Services;

namespace TodoAppBackend.Controllers
{
    [ApiController]
    [Route("api/subtasks")]
    public class SubTaskController : Controller
    {
        private readonly ISubTaskService _subTaskService;

        public SubTaskController(ISubTaskService subTaskService)
        {
            _subTaskService = subTaskService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var subTasks = await _subTaskService.GetAllAsync();
            return Ok(subTasks);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var subTask = await _subTaskService.GetByIdAsync(id);
            if (subTask == null) return NotFound("SubTask không tồn tại.");
            return Ok(subTask);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] SubTask subTask)
        {
            if (string.IsNullOrEmpty(subTask.Title)) return BadRequest("Tiêu đề không được để trống.");

            var newSubTask = await _subTaskService.CreateAsync(subTask.TodoItemId, subTask.Title);
            return CreatedAtAction(nameof(GetById), new { id = newSubTask.Id }, newSubTask);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] SubTask subTask)
        {
            if (id != subTask.Id) return BadRequest("ID không hợp lệ.");

            var updated = await _subTaskService.UpdateAsync(subTask);
            if (!updated) return NotFound("SubTask không tồn tại.");

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _subTaskService.DeleteAsync(id);
            if (!deleted) return NotFound("SubTask không tồn tại.");

            return NoContent();
        }

        [HttpGet("parent/{parentId}")]
        public async Task<IActionResult> GetSubTasksByParent(int parentId)
        {
            var subTasks = await _subTaskService.GetSubTasksByParentIdAsync(parentId);
            return Ok(subTasks);
        }
    }
}
