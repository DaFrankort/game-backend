using Microsoft.AspNetCore.Mvc;
using Server.Models;
using Server.Services;

namespace Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController(UserService service) : ControllerBase
    {
        private readonly UserService _service = service;

        [HttpGet]
        public IActionResult GetAll() => Ok(_service.GetAll());

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            User? lobby = _service.GetById(id);
            return lobby is not null ? Ok(lobby) : NotFound();
        }

        [HttpPost]
        public IActionResult Create([FromBody] CreateUserDto dto)
        {
            User lobby = new() { Name = dto.Name, AuthToken = "todo" };
            User created = _service.Create(lobby);
            return CreatedAtAction(nameof(Get), new { id = 1 }, lobby);
        }
    }
}
