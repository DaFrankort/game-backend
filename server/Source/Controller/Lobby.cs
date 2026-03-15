using Microsoft.AspNetCore.Mvc;
using Server.Attributes;
using Server.DTO;
using Server.Models;
using Server.Services;

namespace Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [RequireAuth]
    public class LobbyController(LobbyService service) : ControllerBase
    {
        private readonly LobbyService _service = service;

        [HttpGet]
        public IActionResult GetAll()
        {
            var summaries = _service.GetAll().Select(l => new LobbySummaryDto(l));
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

        [HttpPost("{lobbyId}/members")]
        public IActionResult Join(int lobbyId)
        {
            User? user = (User?)HttpContext.Items["User"];
            if (user == null)
                return Unauthorized();
            Lobby lobby = _service.AddMember(lobbyId, user.Id);
            return Ok(lobby);
        }

        [HttpDelete("{lobbyId}/members/{userId}")]
        public IActionResult Leave(int lobbyId, int userId)
        {
            Lobby lobby = _service.RemoveMember(lobbyId, userId);
            return Ok(lobby);
        }
    }
}
