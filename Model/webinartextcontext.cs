namespace AIAcademy.Model
{
    public class WebinarCSVContext
    {
        public static void WriteDataToCSV(string path, Webinar data)
        {
            string dataValue = $"{data.Topic},{data.Id}";
            File.AppendAllText(path, dataValue);
        }
        public static List<Webinar> ReadDataFromCSV(string path)
        {
            //string dataValue = $"{data.Topic},{data.Id}";
            string[] lines = File.ReadAllLines(path);
            List<Webinar> webinars = new List<Webinar>();


            Webinar webinar = null;
          

            foreach (var line in lines)
            {
                string[] splittedLines = line.Split(',');
                
                webinar = new Webinar();
                bool flag = true;
                Dictionary<string, string> map = new Dictionary<string, string>();
                foreach (var splittedLine in splittedLines)
                {
                    if (splittedLine != "")
                    {

                        string[] keyVal = splittedLine.Split('|');
                        map.Add(keyVal[0], keyVal[1]);
                    }
                    else
                    {
                        flag=false;
                    }
                }
                if (flag)
                {
                    webinar.Id = Convert.ToInt32(map["id"]);
                    webinar.Topic = map["topic"];
                    webinar.Description = map["description"];
                    webinar.Date = Convert.ToDateTime(map["date"]);
                    webinar.Time = map["time"];
                    webinar.Venue = map["venue"];
                    webinar.Speaker = map["speaker"];
                    webinar.CreatedAt = Convert.ToDateTime(map["createdAt"]);
                    webinar.WebexUrl = map["webexUrl"];
                    webinar.WebexMeetingId = map["webexMeetingId"];
                    webinar.WebexPasscode = map["webexPasscode"];

                    webinars.Add(webinar);
                }
                

            }


            return webinars;


        }
    }
}

