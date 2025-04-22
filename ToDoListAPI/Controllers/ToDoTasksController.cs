using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class ToDoTasksController : ControllerBase
{
  private readonly ApplicationDbContext _context;

    public ToDoTasksController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllTasks()
    {
        var tasks = await _context.ToDoTasks.ToListAsync();
        return Ok(tasks);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetTaskById(int id)
    {
        var task = await _context.ToDoTasks.FindAsync(id);
        if (task == null) return NotFound();
        return Ok(task);
    }

    [HttpPost]
    public async Task<IActionResult> CreateTask([FromBody] ToDoTaskDto taskDto)
    {
        var task = new ToDoTask
        {
            Title = taskDto.Title,
            Description = taskDto.Description,
            DueDate = taskDto.DueDate,
            IsCompleted = taskDto.IsCompleted
        };

        _context.ToDoTasks.Add(task);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetTaskById), new { id = task.Id }, task);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTask(int id, [FromBody] ToDoTaskDto taskDto)
    {
        var task = await _context.ToDoTasks.FindAsync(id);
        if (task == null) return NotFound();

        task.Title = taskDto.Title;
        task.Description = taskDto.Description;
        task.DueDate = taskDto.DueDate;
        task.IsCompleted = taskDto.IsCompleted;

        _context.ToDoTasks.Update(task);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTask(int id)
    {
        var task = await _context.ToDoTasks.FindAsync(id);
        if (task == null) return NotFound();

        _context.ToDoTasks.Remove(task);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
