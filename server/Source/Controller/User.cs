using Microsoft.AspNetCore.Mvc;
using Server.Attributes;
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
        [RequireAuth]
        public IActionResult GetAll() => Ok(_service.GetAll());

        [HttpGet("{id}")]
        [RequireAuth]
        public IActionResult Get(int id)
        {
            User? user = _service.GetById(id);
            return user is not null ? Ok(user) : NotFound();
        }

        [HttpPost]
        public IActionResult Create([FromBody] CreateUserRequestDto dto)
        {
            User created = _service.Create(dto.Name);

            CreateUserResponseDto response = new()
            {
                Id = created.Id,
                Name = created.Name,
                AuthToken = created.AuthToken,
            };

            return CreatedAtAction(nameof(Get), new { id = created.Id }, response);
        }
    }
}
