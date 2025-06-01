using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tutorial9.Models;

namespace Tutorial9.Controllers;

[ApiController]
[Route("api/trips")]
public class TripController : ControllerBase
{
    private TutorialNineContext _db;

    public TripController(TutorialNineContext db)
    {
        _db = db;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAllTripsAsync(CancellationToken token)
    {
        var trips = await _db.Trips.ToListAsync(token);
        return Ok(trips);
    }
}