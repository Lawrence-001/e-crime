using e_crime.Data;
using e_crime.Models;
using e_crime.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace e_crime.Services.Implementation
{
    public class PoliceStationService : IPoliceStationService
    {
        private readonly ApplicationDbContext _dbContext;

        public PoliceStationService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<PoliceStation> CreatePoliceStation(PoliceStation policeStation)
        {
            _dbContext.PoliceStations.Add(policeStation);
            await _dbContext.SaveChangesAsync();
            return policeStation;
        }

        public async Task<PoliceStation> GetPoliceStationById(int id)
        {
            return await _dbContext.PoliceStations.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<PoliceStation>> GetPoliceStations()
        {
            return await _dbContext.PoliceStations.ToListAsync();
        }

        public async Task<PoliceStation> EditPoliceStation(PoliceStation policeStation)
        {
            var station = _dbContext.PoliceStations.FirstOrDefault(x => x.Id == policeStation.Id);

            if (station != null)
            {
                station.Name = policeStation.Name;
                station.Location = policeStation.Location;
                station.County = policeStation.County;
                station.InChargeName = policeStation.InChargeName;
                station.InchargeEmail = policeStation.InchargeEmail;

                _dbContext.Update(station);
                await _dbContext.SaveChangesAsync();
            }
            return policeStation;
        }

        public void DeletePoliceStation(int id)
        {
            var station = _dbContext.PoliceStations.FirstOrDefault(x => x.Id == id);
            if (station != null)
            {
                _dbContext.Remove(station);
                _dbContext.SaveChanges();
            }
        }
    }
}
