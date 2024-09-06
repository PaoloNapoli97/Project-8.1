using System.Globalization;

namespace LabModel
{
    public class Computer
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Specs { get; set; }
        public DateTime Datetime { get; set; }
        public enum StatusList
        {
            Avaiable,
            Maintenance,
            OutOfOrder,
            Removed,
            Reserved,
            InUsing,
        }
        public StatusList Status { get; set; }
        public List<Software> Softwares { get; set; }
        public Dictionary<string, string> Calendar { get; set; }
        private string _slot = "0";
        public Computer(string Name, string Description, string Specs, StatusList Status, DateTime Datetime)
        {
            this.Name = Name;
            this.Description = Description;
            this.Specs = Specs;
            this.Datetime = Datetime;
            if (Id == null) Id = ValidateId();
            Softwares = new();
            this.Status = Status;
            Calendar = getCalendar();
        }
        private string? CreateId()
        {
            string CharName = Name.Length >= 2 ? Name.Substring(0, 2) : Name.Substring(0, 1) + "n";
            string CharDescription = Description.Length >= 2 ? Description.Substring(0, 2) : Description.Substring(0, 1) + "d";
            string CharSpecs = Specs.Length >= 2 ? Specs.Substring(0, 2) : Specs.Substring(0, 1) + "s";
            string DateMonth = Datetime.Month.ToString();
            string DateToday = Datetime.Day.ToString();
            return CharName + CharDescription + CharSpecs + DateMonth + DateToday;
        }
        public string? ValidateId()
        {
            return CreateId().ToUpper();
        }
        public void addSoftware(Software software)
        {
            Softwares.Add(software);
        }
        private string[] days = { "Monday", "Tuesday", "Wendsday", "Thursday", "Friday" };
        private string[] hours = { "9:00", "10:00", "11:00", "12:00", "13:00", "14:00", "15:00", "16:00", "17:00", "18:00" };
        public Dictionary<string, string> getCalendar()
        {
            Dictionary<string, string> calendar = new();

            foreach (var day in days)
            {
                foreach (var hour in hours)
                {
                    calendar.Add(day + " " + hour, _slot);
                }
            }
            return calendar;
        }

        public bool CheckCalendar()
        {
            var calendar = Calendar.FirstOrDefault(x => x.Value == "0");
            if (calendar.Value != null) return true;
            else return false;
        }

        public void AddBooking(string UserId, string Day, string Hour)
        {
            string HourOfDay = $"{Day} {Hour}";

            var bookingsForTheDay = Calendar.Where(x => x.Key.Contains(Day) && x.Value == UserId);
            if (bookingsForTheDay.Count() >= 2)
            {
                throw new Exception("Error: Max two booking per day");
            }
            if (CheckCalendar())
            {
                var calendarDay = Calendar.FirstOrDefault(x => x.Key.Contains(HourOfDay));
                if (calendarDay.Key == null)
                {
                    throw new Exception("Not found");
                }
                else
                {
                    if (Calendar[HourOfDay] == "0")
                    {
                        Calendar[HourOfDay] = UserId;
                    }
                    else
                    {
                        throw new Exception("Already reserved");
                    }
                }
            }
        }

        public void DeleteBooking(string UserId, string Day, string Hour)
        {
            string HourOfDay = $"{Day} {Hour}";

            if (CheckCalendar())
            {
                var calendarDay = Calendar.FirstOrDefault(x => x.Key.Contains(HourOfDay));
                if (calendarDay.Key == null)
                {
                    throw new Exception("Not found");
                }
                else
                {
                    if (Calendar[HourOfDay] == "0")
                    {
                        throw new Exception("Error: no booking to delete");
                    }
                    if (Calendar[HourOfDay] == UserId)
                    {
                        Calendar[HourOfDay] = "0";
                    }
                    else
                    {
                        throw new Exception("Error: cannot delete other users booking");
                    }
                }
            }
        }

        public override string ToString()
        {
            return $"Name: {Name}, Description: {Description}, Specs: {Specs}, Datetime: {Datetime}";
        }
    }
}