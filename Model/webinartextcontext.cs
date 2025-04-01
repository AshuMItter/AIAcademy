namespace AIAcademy.Model
{
    public class WebinarCSVContext
    {
        public static void WriteDataToCSV(string path, Webinar data)
        {
            string dataValue = $"{data.Topic},{data.Id}";
            File.AppendAllText(path,dataValue);
        }
        public static string ReadDataFromCSV(string path)
        {
            //string dataValue = $"{data.Topic},{data.Id}";
           string result =  File.ReadAllText(path);

            return result;
        }
    }
}
