using Microsoft.AspNetCore.Mvc;
using Server.DTO;
using Server.Exceptions;
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
        public IActionResult GetAll()
        {
            var summaries = _service
                .GetAll()
                .Select(l => new LobbySummaryDto
                {
                    Id = l.Id,
                    Name = l.Name,
                    UserCount = l.Users.Count,
                });

            return Ok(summaries);
        }

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
            _service.AddUser(dto.LobbyId, dto.UserId);
            Lobby lobby = _service.GetById(dto.LobbyId)!;
            return Ok(lobby);
        }

        [HttpPost("leave")]
        public IActionResult Leave([FromBody] JoinLobbyRequestDto dto)
        {
            _service.RemoveUser(dto.LobbyId, dto.UserId);
            Lobby lobby = _service.GetById(dto.LobbyId)!;
            return Ok(lobby);
        }
    }
}
