using e_crime.Models;

namespace e_crime.Services.Interfaces
{
    public interface ICrimeService
    {
        Task<Crime>CreateCrime(Crime crime);
    }
}
