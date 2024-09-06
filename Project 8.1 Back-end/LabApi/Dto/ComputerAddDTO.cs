using LabModel;

namespace ComputerDTO
{
    public class ComputerAddDTO
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Specs { get; set; }
        public Computer.StatusList Status { get; set; }
    }
}