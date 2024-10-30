using e_crime.mvc.Data;
using e_crime.mvc.Models;

namespace e_crime.mvc.Services.Interfaces
{
    public interface IPoliceStationService
    {
        Task<PoliceStation> CreatePoliceStation(PoliceStation policeStation);
        Task<PoliceStation> GetPoliceStationById(int id);
        Task<PoliceStation> EditPoliceStation(PoliceStation policeStation);
        Task<IEnumerable<PoliceStation>> GetPoliceStations();
        void DeletePoliceStation(int id);
        Task<List<ApplicationUser>> AddOfficers(string stationId, string userId);
        Task<List<ApplicationUser>> StationOfficers(string stationLocation);
    }
}