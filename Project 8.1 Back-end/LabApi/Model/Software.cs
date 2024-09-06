namespace LabModel
{
    public class Software
    {
        private string? _name { get; set; }
        public Software(string Name)
        {
            this.Name = Name;
        }
        public string? Name
        {
            get { return _name; }
            private set
            {
                _name = value;
            }
        }
    }
}