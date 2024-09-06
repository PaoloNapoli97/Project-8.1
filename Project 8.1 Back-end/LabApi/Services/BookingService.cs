using FileManager.Controller;
using BookingModel;
using Newtonsoft.Json;
using LabModel;
using LabServices;

namespace BookingServices
{
    public class BookingService
    {
        private const string BookingDb = "BookingDb.json";
        FileManagers fileManagers = new();
        LabService labServices = new();

        [JsonProperty]
        private List<Booking> bookings = new();

        public void CreateBookingDb()
        {
            fileManagers.CreateFile<Booking>(BookingDb);
        }

        public void WriteBookings()
        {
            fileManagers.WriteItems(bookings, BookingDb);
        }
        public List<Booking> ReadBookings()
        {
            bookings = fileManagers.ReadItems<Booking>(BookingDb);
            return bookings;
        }

        // public void book(string LabId, string ResourceName, string UserId, string Day, string Hour){
        //     var labs = labServices.ReadLabs();
        //     var lab = labs.FirstOrDefault(x => x.Id == LabId);
        //     if (lab == null)
        //     {
        //         throw new Exception("Lab not found");
        //     }
        //     else
        //     {
        //         var resource = lab.Resource.FirstOrDefault(x => x.Name == ResourceName);
        //         resource.AddBooking(UserId, Day, Hour);
        //         labServices.WriteLabs(labs);
        //     }
        // }
    }
}