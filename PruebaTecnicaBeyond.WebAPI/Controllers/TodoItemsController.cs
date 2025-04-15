using Microsoft.AspNetCore.Mvc;
using PruebaTecnicaBeyond.Interfaces;
using PruebaTecnicaBeyond.Services;
using PruebaTecnicaBeyond.WebAPI.DTOs;

namespace PruebaTecnicaBeyond.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TodoItemsController : ControllerBase
    {
        private readonly TodoListService _todoService;
        private readonly ITodoListRepository _todoRepository;

        public TodoItemsController(TodoListService todoService, ITodoListRepository todoRepository)
        {
            _todoService = todoService;
            _todoRepository = todoRepository;
        }

        [HttpGet]
        public ActionResult<IEnumerable<TodoItemDto>> GetAll()
        {
            var items = _todoService.GetAllItems(); 
            var dtoItems = items.Select(item => new TodoItemDto
            {
                Id = item.Id,
                Title = item.Title,
                Description = item.Description,
                Category = item.Category,
                IsCompleted = item.IsCompleted,
                TotalPercentage = item.TotalPercentage,
                Progressions = item.Progressions.Select(p => new ProgressionDto
                {
                    Date = p.Date,
                    Percent = p.Percent
                }).ToList()
            });

            return Ok(dtoItems);
        }

        [HttpGet("{id}")]
        public ActionResult<TodoItemDto> GetById(int id)
        {
            var item = _todoService.GetItemById(id); 
            if (item == null)
                return NotFound();

            var dto = new TodoItemDto
            {
                Id = item.Id,
                Title = item.Title,
                Description = item.Description,
                Category = item.Category,
                IsCompleted = item.IsCompleted,
                TotalPercentage = item.TotalPercentage,
                Progressions = item.Progressions.Select(p => new ProgressionDto
                {
                    Date = p.Date,
                    Percent = p.Percent
                }).ToList()
            };

            return Ok(dto);
        }

        [HttpPost]
        public IActionResult CreateTodoItem([FromBody] CreateTodoItemDto dto)
        {
            int newId = _todoRepository.GetNextId();
            try
            {
                _todoService.AddItem(newId, dto.Title, dto.Description, dto.Category);
                return CreatedAtAction(nameof(GetById), new { id = newId }, dto);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("{id}")]
        public IActionResult UpdateTodoItem(int id, [FromBody] UpdateTodoItemDto dto)
        {
            try
            {
                _todoService.UpdateItem(id, dto.Description);
                return NoContent();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteTodoItem(int id)
        {
            try
            {
                _todoService.RemoveItem(id);
                return NoContent();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost("{id}/progressions")]
        public IActionResult RegisterProgression(int id, [FromBody] ProgressionDto dto)
        {
            try
            {
                _todoService.RegisterProgression(id, dto.Date, dto.Percent);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}   

