using Tutorial10.DTO;
using Tutorial10.Models;

namespace Tutorial10.Mappers;

public static class PatientMapper
{
    public static Patient ToModel(this PatientRequest request)
    {
        return new Patient
        {
            IdPatient = request.IdPatient,
            FirstName = request.FirstName,
            LastName = request.LastName,
            Birthdate = request.Birthdate
        };
    }
}