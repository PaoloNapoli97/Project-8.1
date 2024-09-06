using FileManager.Controller;
using LabModel;
using Newtonsoft.Json;

namespace LabServices
{
    public class LabService
    {
        private const string LabDb = "LabDb.json";
        FileManagers fileManager = new();
        [JsonProperty]
        private List<Lab> labs = new();
        public void CreateLabDb()
        {
            fileManager.CreateFile<Lab>(LabDb);
        }
        public void WriteLabs(List<Lab> labs)
        {
            fileManager.WriteItems(labs, LabDb);
        }
        public List<Lab> ReadLabs()
        {
            labs = fileManager.ReadItems<Lab>(LabDb);
            return labs;
        }
    }
}