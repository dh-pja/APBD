using Microsoft.AspNetCore.Mvc;
using Tutorial9.Repositories.Interfaces;

namespace Tutorial9.Controllers;

[ApiController]
[Route("api/clients")]
public class ClientController : ControllerBase
{
    private IClientRepository _clientRepository;
    
    public ClientController(IClientRepository clientRepository)
    {
        _clientRepository = clientRepository;
    }
    
    [HttpDelete]
    [Route("{id:int}")]
    public async Task<IActionResult> DeleteClientAsync(int id, CancellationToken token = default)
    {
        if (id <= 0) return BadRequest("Invalid client ID.");
        
        var client = await _clientRepository.GetClientByIdAsync(id, token);
        if (client == null)
        {
            return NotFound($"Client with ID {id} not found.");
        }
        
        var hasTrips = await _clientRepository.HasTripsAsync(id, token);
        if (hasTrips)
        {
            return BadRequest("Client has associated trips and cannot be deleted.");
        }
        
        
        await _clientRepository.DeleteClientAsync(client, token);
        return NoContent();
    }
}