using e_crime.mvc.Models;

namespace e_crime.mvc.Services.Interfaces
{
    public interface ICrimeService
    {
        Task<Crime> CreateCrime(Crime crime);
        Task<Crime> GetCrimeById(int id);
        Task<Crime> EditCrime(Crime crime);
        Task<IEnumerable<Crime>> GetAllCrimes();
        void DeleteCrime(int id);
        Task<List<Crime>> GetCrimesByOfficerLocation(string officerEmail, string officerLocation);
        Task<List<Crime>> AssignCrimeToOfficer(string officerId, int crimeId);
    }
}
