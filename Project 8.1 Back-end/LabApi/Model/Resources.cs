namespace LabModel
{
    public class Resources
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public Dictionary<string, string> Calendar { get; set; }
        private string _slot = "0";
        public Resources(string Name, string Description)
        {
            this.Name = Name;
            this.Description = Description;
            Calendar = getCalendar();
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
    }
}
