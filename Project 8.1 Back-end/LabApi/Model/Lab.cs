namespace LabModel
{
    public class Lab
    {
        public string? Id { get; set; }
        public List<Computer> Computers { get; set; }
        public List<Resources> Resource { get; set; }
        public Lab(string Id)
        {
            this.Id = Id;
            Computers = new();
            Resource = new();
        }
        public void addComputer(Computer computer)
        {
            Computers.Add(computer);
        }
        public void RemoveComputer(Computer computer)
        {
            Computers.Remove(computer);
        }
        public void addResource(Resources resources)
        {
            Resource.Add(resources);
        }

        public void RemoveResource(Resources resources)
        {
            Resource.Remove(resources);
        }
    }
}