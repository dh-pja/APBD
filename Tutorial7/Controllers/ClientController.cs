using Microsoft.AspNetCore.Mvc;
using Tutorial7.Contracts.Requests;
using Tutorial7.Contracts.Responses;
using Tutorial7.Entities;
using Tutorial7.Mappers;
using Tutorial7.Repositories.Interfaces;

namespace Tutorial7.Controllers;

[ApiController]
[Route("api/clients")]
public class ClientController(IClientRepository clientRepository, ITripRepository tripRepository) : ControllerBase
{
    [HttpGet("{clientId:int}/trips")]
    [ProducesResponseType(typeof(ICollection<GetAllClientTripsResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAllClientTripsAsync(int clientId, CancellationToken token)
    {
        if (clientId <= 0)
            return BadRequest($"{nameof(clientId)} should be greater than 0");
        
        var trips = await clientRepository.GetAllClientTripsAsync(clientId, token);
        
        if (trips is null)
            return NotFound("Client is not found");
        
        return Ok(TripMapper.MapToGetAllClientTripsResponse(trips));
    }
    
    [HttpPost]
    [ProducesResponseType(typeof(int), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> AddClientAsync([FromBody] PostClientRequest request, CancellationToken token)
    {
        if (string.IsNullOrWhiteSpace(request.FirstName) || string.IsNullOrWhiteSpace(request.LastName) || string.IsNullOrWhiteSpace(request.Email))
        {
            return BadRequest("First name, last name, and email are required.");
        }
        
        if (!string.IsNullOrWhiteSpace(request.Pesel))
        {
            if (await clientRepository.DoesPeselExistAsync(request.Pesel, token))
            {
                return BadRequest("Client with this PESEL already exists.");
            }
        }
        else
        {
             return BadRequest("PESEL is required.");
        }

        var client = new Client
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            Telephone = request.Telephone,
            Pesel = request.Pesel
        };

        try
        {
            var clientId = await clientRepository.CreateClientAsync(client, token);
            return CreatedAtAction(nameof(GetAllClientTripsAsync), new { clientId }, clientId);
        }
        catch (Exception _)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while adding the client.");
        }
    }

    [HttpPut("{clientId:int}/trips/{tripId:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> RegisterClientToTripAsync(int clientId, int tripId, CancellationToken token)
    {
        var result = await clientRepository.CreateClientTripAsync(clientId, tripId, token);

        if (!result.success)
        {
            if (result.message == "Client not found" || result.message == "Trip not found")
            {
                return NotFound(result.message);
            }
            return BadRequest(result.message);
        }

        return Ok(result.message);
    }

    [HttpDelete("{clientId:int}/trips/{tripId:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteClientTripAsync(int clientId, int tripId, CancellationToken token)
    {
        var result = await clientRepository.DeleteClientTripAsync(clientId, tripId, token);

        if (!result.success)
        {
            return NotFound(result.message);
        }

        return Ok(result.message);
    }
}