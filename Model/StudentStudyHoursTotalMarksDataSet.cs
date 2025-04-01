namespace AIAcademy.Model
{
    public class StudentStudyHoursTotalMarksDataSet
    {

       
        public static List<double> Expose_StudyHoursList(string path, out List<double> marksObtained)
        {
            List<double> _studyHoursPerWeek = new List<double>();
            List<double> _marksObtained = new List<double>();

            string[] studentsData = File.ReadAllLines(path);

            foreach (var line in studentsData)
            {
                
                string[] spilttedRow = line.Split(',');
                 double _studyHours = 0;
                double _marks = 0;

                if (double.TryParse(spilttedRow[0], out _studyHours))
                {
                    _studyHoursPerWeek.Add(_studyHours);
                }
                if (double.TryParse(spilttedRow[19], out _marks))
                {
                    _marksObtained.Add(_marks);
                }

                // 16  study hours per week
                // 14 Total Score
            }
            marksObtained = _marksObtained;
            return _studyHoursPerWeek;
        }
    }
}
