using Microsoft.AspNetCore.Mvc;
using Server.Attributes;
using Server.Models;
using Server.Services;
using Server.Utility;

namespace Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController(UserService service) : ControllerBase
    {
        private readonly UserService _service = service;

        [HttpGet]
        [RequireAuth]
        public IActionResult GetAll([FromQuery] int page = 1, [FromQuery] int limit = 100)
        {
            return Ok(_service.GetPaged(page, limit));
        }

        [HttpGet("{id}")]
        [RequireAuth]
        public IActionResult Get(string id)
        {
            User? user = _service.GetById(id);
            return user is not null ? Ok(user) : NotFound();
        }

        [HttpGet("me")]
        [RequireAuth]
        public IActionResult GetMe()
        {
            User user = HttpContextUtil.GetUser(HttpContext);
            return Ok(user);
        }

        [HttpPost]
        public IActionResult Create([FromBody] CreateUserRequestDto dto)
        {
            User created = _service.Create(dto.Name);

            CreateUserResponseDto response = new(created);
            return CreatedAtAction(nameof(Get), new { id = created.Id }, response);
        }
    }
}
