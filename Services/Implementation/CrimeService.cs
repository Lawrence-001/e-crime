using e_crime.mvc.Data;
using e_crime.mvc.Models;
using e_crime.mvc.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace e_crime.mvc.Services.Implementation
{
    public class CrimeService : ICrimeService
    {
        private readonly ApplicationDbContext _context;

        public CrimeService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Crime> CreateCrime(Crime crime)
        {
            _context.Crimes.Add(crime);
            await _context.SaveChangesAsync();
            return crime;
        }

        public void DeleteCrime(int id)
        {
            var crime = _context.Crimes.FirstOrDefault(c => c.Id == id);
            if (crime != null)
            {
                _context.Remove(crime);
                _context.SaveChanges();
            }
        }

        public async Task<IEnumerable<Crime>> GetAllCrimes()
        {
            return await _context.Crimes.ToListAsync();
        }

        public async Task<Crime> EditCrime(Crime crime)
        {
            var crimeToEdit = _context.Crimes.FirstOrDefault(x => x.Id == crime.Id);
            if (crimeToEdit != null)
            {
                _context.Update(crimeToEdit);
                await _context.SaveChangesAsync();

            }
            return crime;
        }

        public async Task<Crime> GetCrimeById(int id)
        {
            return await _context.Crimes.FirstOrDefaultAsync(x => x.Id == id);
        }

        /// <summary>
        /// Get crimes by officer location
        /// </summary>
        /// <param name="officerEmail"></param>
        /// <param name="officerLocation"></param>
        /// <returns></returns>
        public async Task<List<Crime>> GetCrimesByOfficerLocation(string officerEmail, string officerLocation)
        {
            bool isIncharge = await _context.PoliceStations.AnyAsync(x => x.InchargeEmail == officerEmail);

            if (isIncharge)
            {
                var crimes = _context.Crimes.Where(c => c.Location == officerLocation);
                return await crimes.ToListAsync();
            }
            return null;
        }

        public Task<List<Crime>> AssignCrimeToOfficer(string officerId, int crimeId)
        {
            throw new NotImplementedException();
        }
    }
}