using e_crime.Models;

namespace e_crime.Services.Interfaces
{
    public interface IPoliceStationService
    {
        Task<PoliceStation> CreatePoliceStation(PoliceStation policeStation);
        Task<PoliceStation> GetPoliceStationById(int id);
        Task<PoliceStation> EditPoliceStation(PoliceStation policeStation);
        void DeletePoliceStation(int id);
    }
}
