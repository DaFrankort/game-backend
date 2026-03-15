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
        public IActionResult GetAll([FromQuery] int page = 1, [FromQuery] int limit = 100)
        {
            var summaries = _service
                .GetPaged(page, limit)
                .Select(lobby => new LobbySummaryDto(lobby));
            return Ok(summaries);
        }

        [HttpGet("{id}")]
        public IActionResult Get(string id)
        {
            Lobby? lobby = _service.GetById(id);
            return lobby is not null ? Ok(lobby) : NotFound();
        }

        [HttpPost]
        public IActionResult Create([FromBody] CreateLobbyRequestDto dto)
        {
            User? host = (User?)HttpContext.Items["User"];
            if (host == null)
                return Unauthorized();
            Lobby lobby = new(dto.Name, host);
            Lobby created = _service.Create(lobby);
            return CreatedAtAction(nameof(Get), new { id = lobby.Id }, lobby);
        }

        [HttpPost("{lobbyId}/members")]
        public IActionResult Join(string lobbyId)
        {
            User? user = (User?)HttpContext.Items["User"];
            if (user == null)
                return Unauthorized();
            Lobby lobby = _service.AddMember(lobbyId, user.Id);
            return Ok(lobby);
        }

        [HttpDelete("{lobbyId}/members")]
        public IActionResult Leave(string lobbyId)
        {
            User? user = (User?)HttpContext.Items["User"];
            if (user == null)
                return Unauthorized();
            Lobby lobby = _service.RemoveMember(lobbyId, user.Id, user);
            return Ok(lobby);
        }

        [HttpDelete("{lobbyId}/members/{userId}")]
        public IActionResult KickMember(string lobbyId, string userId)
        {
            User? user = (User?)HttpContext.Items["User"];
            if (user == null)
                return Unauthorized();
            Lobby lobby = _service.RemoveMember(lobbyId, userId, user);
            return Ok(lobby);
        }
    }
}
