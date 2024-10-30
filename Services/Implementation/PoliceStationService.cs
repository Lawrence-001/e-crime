using e_crime.mvc.Data;
using e_crime.mvc.Models;
using e_crime.mvc.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace e_crime.mvc.Services.Implementation
{
    public class PoliceStationService : IPoliceStationService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<ApplicationUser> _userManager;

        public PoliceStationService(ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
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

        public Task<List<ApplicationUser>> AddOfficers(string stationId, string userId)
        {
            throw new NotImplementedException();
        }

        public async Task<List<ApplicationUser>> StationOfficers(string stationLocation)
        {
            var usersInRole = await _userManager.GetUsersInRoleAsync("Officer");
            //var users = await _dbContext.Users.Where(x => x.Address == stationLocation).ToListAsync();
            var officers = usersInRole.Where(x => x.Address == stationLocation).ToList();
            return officers;

        }
    }
}
