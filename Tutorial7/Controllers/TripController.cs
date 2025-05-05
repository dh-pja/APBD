using Microsoft.AspNetCore.Mvc;
using Tutorial7.Contracts.Responses;
using Tutorial7.Mappers;
using Tutorial7.Repositories.Interfaces;

namespace Tutorial7.Controllers;

[ApiController]
[Route("api/trips")]
public class TripController(ITripRepository tripRepository) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(ICollection<GetAllTripsResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAllTripsAsync(CancellationToken token)
    {
        var trips = await tripRepository.GetAllTripsAsync(token);

        return Ok(TripMapper.MapToGetAllTripsResponse(trips));
    }
}