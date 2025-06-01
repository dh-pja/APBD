namespace Tutorial9.DTO;

public class AddClientToTripRequest
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Telephone { get; set; }
    public string Pesel { get; set; }
    // IdTrip and TripName are... redundant? Obtained from api route
    public DateTime? PaymentDate { get; set; }
}