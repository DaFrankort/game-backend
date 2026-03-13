using Microsoft.AspNetCore.Mvc;
using Server.DTO;
using Server.Models;
using Server.Services;

namespace Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LobbyController(LobbyService service) : ControllerBase
    {
        private readonly LobbyService _service = service;

        [HttpGet]
        public IActionResult GetAll() => Ok(_service.GetAll());

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            Lobby? lobby = _service.GetById(id);
            return lobby is not null ? Ok(lobby) : NotFound();
        }

        [HttpPost]
        public IActionResult Create([FromBody] CreateLobbyRequestDto dto)
        {
            Lobby lobby = new() { Name = dto.Name };
            Lobby created = _service.Create(lobby);
            return CreatedAtAction(nameof(Get), new { id = lobby.Id }, lobby);
        }

        [HttpPost("join")]
        public IActionResult Join([FromBody] JoinLobbyRequestDto dto)
        {
            bool success = _service.AddUser(dto.LobbyId, dto.UserId);
            if (!success)
                return BadRequest("Could not join lobby.");

            Lobby lobby = _service.GetById(dto.LobbyId)!;
            return Ok(lobby);
        }

        [HttpPost("leave")]
        public IActionResult Leave([FromBody] JoinLobbyRequestDto dto)
        {
            bool success = _service.RemoveUser(dto.LobbyId, dto.UserId);
            if (!success)
                return BadRequest("Unknown lobby");

            Lobby lobby = _service.GetById(dto.LobbyId)!;
            return Ok(lobby);
        }
    }
}
