#region Usings declarations

using Microsoft.AspNetCore.Mvc;

using Reefact.Hateoas.Hal.AspNetCore;
using Reefact.Hateoas.Hal.Example.Models;

#endregion

namespace Reefact.Hateoas.Hal.Example.Controllers {

    [ServiceFilter(typeof(SupportsHalAttribute))]
    [Route("api/[controller]")]
    [ApiController]
    public class MeetingRoomsController : ControllerBase {

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetAll([FromQuery] int size = 5, [FromQuery] int page = 1) {
            return Ok(new PagedResult<MeetingRoom>(
                          MeetingRoom.FakeRooms.Skip((page - 1) * size).Take(size),
                          page,
                          size,
                          MeetingRoom.FakeRooms.Count(),
                          (int)Math.Ceiling((double)MeetingRoom.FakeRooms.Count() / size)));
        }

        [HttpGet("{identifier}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAsync(int identifier) {
            await Task.Delay(1);
            if (MeetingRoom.FakeRooms.Any(mr => mr.ID == identifier)) {
                return Ok(MeetingRoom.FakeRooms.First(mr => mr.ID == identifier));
            }

            return NotFound($"Meeting Room Id {identifier} doesn't exist.");
        }

        [GetByIdMethodImpl]
        [HttpGet("get-by-id/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetAnother(int id) {
            if (MeetingRoom.FakeRooms.Any(mr => mr.ID == id)) {
                return Ok(MeetingRoom.FakeRooms.First(mr => mr.ID == id));
            }

            return NotFound($"Meeting Room Id {id} doesn't exist.");
        }

        [HttpGet("get-by-name/{name}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetByName(string name) {
            return Ok(MeetingRoom.FakeRooms.Where(f => f.Name.Contains(name)));
        }

    }

}