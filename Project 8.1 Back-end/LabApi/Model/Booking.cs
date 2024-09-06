namespace BookingModel
{
    public class Booking
    {
        public string? Id {get; set;}
        public string? Name {get; set;}
        public string? Resource {get;set;}
        public List<Booking> SavedBookings = new List<Booking>();
        public Booking(string Id, string Name, string Resource){
            this.Id = Id;
            this.Name = Name;
            this.Resource = Resource;
        }
        public void AddBooking(Booking booking){
            SavedBookings.Add(booking);
        }
        public void DeleteBooking(Booking booking){
            SavedBookings.Remove(booking);
        }
    }
}