// using CsvHelper;
// using System.Globalization;
using Newtonsoft.Json;
// using System.Text.Json;
namespace FileManager.Controller
{
    public class FileManagers
    {

        // public string GetFolderPath()
        // {
        //     string appDataTemp = Path.GetTempPath();
        //     string fileFolder = "FilesDb";
        //     string pathToFolder = Path.Combine(appDataTemp, fileFolder);
        //     return pathToFolder;
        // }
        // public string GetPathfile(string file)
        // {
        //     string dbFilePath = Path.Combine(GetFolderPath(), file);
        //     return dbFilePath;
        // }

        // public void CreateFile<T>(string file)
        // {
        //     try
        //     {
        //         if (!Directory.Exists(GetFolderPath()))
        //         {
        //             Directory.CreateDirectory(GetFolderPath());
        //         }

        //         string dbFilePath = GetPathfile(file);
        //         if (!File.Exists(dbFilePath))
        //         {
        //             using (var writer = new StreamWriter(dbFilePath))
        //             using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
        //             {
        //                 csv.WriteHeader<T>(); // This get the header automatically
        //                 csv.NextRecord();
        //             }
        //         }
        //     }
        //     catch (Exception error)
        //     {
        //         throw new IOException("Error while creating the file: ", error);
        //     }
        // }
        // public List<T> ReadItems<T>(string file)
        // {
        //     List<T> items = new();
        //     using var input = File.OpenText(GetPathfile(file));
        //     input.ReadLine();

        //     try
        //     {
        //         using (var reader = new StreamReader(GetPathfile(file)))
        //         using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
        //         {
        //             items = new List<T>(csv.GetRecords<T>());
        //         }
        //     }
        //     catch (Exception ex)
        //     {
        //         throw new Exception("An error occurred while reading the list from the CSV file: " + ex.Message);
        //     }
        //     return items;
        // }
        // public void WriteItems<T>(List<T> list, string file)
        // {
        //     try
        //     {
        //         using (var writer = new StreamWriter(GetPathfile(file))) // Overwrite existing file
        //         using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
        //         {
        //             csv.WriteRecords(list);
        //         }
        //     }
        //     catch (Exception ex)
        //     {
        //         Console.WriteLine("An error occurred while writing the list to the CSV file: " + ex.Message);
        //     }
        // }
        public string GetFolderPath()
        {
            string appDataTemp = Path.GetTempPath();
            string fileFolder = "FilesDb";
            string pathToFolder = Path.Combine(appDataTemp, fileFolder);
            return pathToFolder;
        }
        public string GetPathfile(string file)
        {
            string dbFilePath = Path.Combine(GetFolderPath(), file);
            return dbFilePath;
        }
        public void CreateFile<T>(string file)
        {
            try
            {
                if (!Directory.Exists(GetFolderPath()))
                {
                    Directory.CreateDirectory(GetFolderPath());
                }
                string dbFilePath = GetPathfile(file);
                if (!File.Exists(dbFilePath))
                {
                    File.WriteAllText(dbFilePath, ""); // Create an empty file
                }
            }
            catch (Exception error)
            {
                throw new IOException("Error while creating the file: ", error);
            }
        }

        public List<T> ReadItems<T>(string file)
        {
            List<T> items = new List<T>();
            string filePath = GetPathfile(file);

            {
                if (File.Exists(filePath))
                {
                    string jsonContent = File.ReadAllText(filePath);

                    if (!string.IsNullOrEmpty(jsonContent))
                    {
                        try
                        {
                            items = JsonConvert.DeserializeObject<List<T>>(jsonContent);
                        }
                        catch (Exception e)
                        {
                            throw new IOException("Error accurred while deserializing the Json file", e);
                        }
                    }
                }
            }
            return items;
        }

        public void WriteItems<T>(List<T> list, string file)
        {
            try
            {
                string jsonContent = JsonConvert.SerializeObject(list);
                File.WriteAllText(GetPathfile(file), jsonContent);
            }
            catch (Exception ex)
            {
                throw new IOException("An error occurred while writing the list to the JSON file: " + ex);
            }
        }
    }
}