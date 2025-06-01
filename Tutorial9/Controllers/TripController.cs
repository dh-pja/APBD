using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tutorial9.DTO;
using Tutorial9.Mappers;
using Tutorial9.Models;
using Tutorial9.Repositories.Interfaces;

namespace Tutorial9.Controllers;

[ApiController]
[Route("api/trips")]
public class TripController : ControllerBase
{
    private readonly ITripRepository _tripRepository;
    private readonly IClientRepository _clientRepository;

    public TripController(ITripRepository tripRepository, IClientRepository clientRepository)
    {
        _tripRepository = tripRepository;
        _clientRepository = clientRepository;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAllTripsAsync([FromQuery] int page = 1, [FromQuery] int pageSize = 10, CancellationToken token = default)
    {
        if (page <= 0) page = 1;
        if (pageSize <= 0) pageSize = 10;

        var totalTrips = await _tripRepository.CountTripsAsync(token);
        var allPages = (int)Math.Ceiling((double)totalTrips / pageSize);
        
        var trips = await _tripRepository.GetAllTripsAsyncPaged(page, pageSize, token);
        
        return Ok(
            new PagedTripsResponse
            {
                AllPages = allPages,
                PageNum = page,
                PageSize = pageSize,
                Trips = trips.Select(t => t.ToResponse()).ToList()
            });
    }

    [HttpPost("{idTrip}/clients")]
    public async Task<IActionResult> AddClientToTripAsync(int idTrip, [FromBody] AddClientToTripRequest request, CancellationToken cancellationToken = default)
    {
        if (idTrip <= 0) return BadRequest("Invalid trip ID.");
        
        var peselExists = await _clientRepository.ClientExistsWithPeselAsync(request.Pesel, cancellationToken);

        if (peselExists)
        {
            return BadRequest("Client with this PESEL already exists.");
        }
        
        var trip = await _tripRepository.GetTripByIdAsync(idTrip, cancellationToken);
        
        if (trip == null)
        {
            return NotFound($"Trip with ID {idTrip} not found.");
        }
        
        var clientTrip = new ClientTrip
        {
            IdTrip = trip.IdTrip,
            IdClientNavigation = new Client
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Pesel = request.Pesel,
                Email = request.Email
            },
            RegisteredAt = DateTime.Now,
            PaymentDate = request.PaymentDate,
        };
        
        var clientAdded = await _clientRepository.AddClientToTripAsync(clientTrip, cancellationToken);
        
        return !clientAdded ? StatusCode(500, "Failed to add client to trip.") : Ok("Client added to trip successfully.");
    }
}