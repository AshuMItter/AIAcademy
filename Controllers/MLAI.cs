using AIAcademy.Model;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AIAcademy.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudyHoursController : ControllerBase
    {
        private readonly List<double> _studyHours = new List<double> {
            31, 16, 21, 27, 37, 30, 24, 31, 34, 27, 35, 11, 25, 18, 12, 34, 32, 24, 37, 39,
            27, 14, 36, 27, 20, 26, 38, 32, 27, 39, 14, 15, 28, 20, 37, 29, 14, 28, 13, 32,
            37, 29, 21, 23, 35, 22, 35, 16, 20, 19, 37, 21, 12, 10, 29, 22, 35, 22, 37, 35,
            39, 10, 15, 17, 26, 21, 16, 24, 32, 27, 34, 32, 24, 38, 20, 28, 12, 27, 14, 31,
            30, 16, 39, 35, 19, 35, 13, 30, 15, 15, 28, 16, 33, 25, 21, 10, 23, 21, 28, 32
            // ... (add remaining data points)
        };

        private readonly List<double> _examScores = new List<double> {
            63, 50, 55, 65, 70, 61, 61, 50, 65, 55, 57, 62, 59, 50, 50, 50, 77, 56, 58, 57,
            65, 55, 65, 57, 62, 50, 71, 58, 50, 50, 61, 61, 67, 64, 61, 64, 59, 64, 55, 56,
            56, 64, 65, 55, 59, 50, 54, 55, 62, 50, 65, 50, 53, 50, 50, 50, 63, 56, 50, 60,
            50, 52, 50, 50, 56, 50, 50, 57, 53, 58, 63, 50, 65, 64, 50, 64, 65, 50, 61, 67,
            70, 50, 64, 65, 50, 65, 50, 60, 61, 61, 64, 50, 56, 50, 50, 50, 65, 50, 50, 50
            // ... (add remaining data points)
        };

        IWebHostEnvironment _env;
        public StudyHoursController(IWebHostEnvironment env) {
            _env = env;
        }

        [HttpGet("predict-hours")]
        public ActionResult<PredictionResponse> PredictRequiredHours([FromQuery] double targetScore)
        {
            // Input validation
            if (targetScore < 0 || targetScore > 100)
                return BadRequest("Target score must be between 0 and 100");

            if (_studyHours.Count != _examScores.Count)
                return StatusCode(500, "Data mismatch between study hours and exam scores");

            if (_studyHours.Count < 2)
                return StatusCode(500, "Insufficient data for regression analysis");

            try
            {
                // New Code - Calls Expose_StudyHoursList which is a static method of the class
                //StudentStudyHoursTotalMarksDataSet
                string path = Path.Combine(_env.ContentRootPath, "Dataset", "performance.csv");
                List<double> _examScores = null;
                List<double> _studyHours = StudentStudyHoursTotalMarksDataSet.Expose_StudyHoursList(path, out _examScores);


                //_examScores.AddRange(this._examScores);

                //_studyHours.AddRange(this._studyHours);

                // If we remove the code till here from New Code the old code will work.
                // New Code
                // Calculate regression parameters (marks as X, hours as Y)
                var regression = CalculateLinearRegression(_examScores, _studyHours);

                // Predict required hours
                double requiredHours = regression.Slope * targetScore + regression.Intercept;

                // Apply reasonable bounds (0-168 hours/week)
                requiredHours = Math.Max(0, Math.Min(168, requiredHours));

                return Ok(new PredictionResponse
                {
                    TargetScore = targetScore,
                    PredictedHours = Math.Round(requiredHours, 1),
                    RSquared = Math.Round(regression.RSquared, 3),
                    RegressionEquation = $"Hours = {regression.Slope:F3} * Score + {regression.Intercept:F1}",
                    DataPointsUsed = _studyHours.Count
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Prediction error: {ex.Message}");
            }
        }

        private RegressionResult CalculateLinearRegression(List<double> xValues, List<double> yValues)
        {
            int n = xValues.Count;
            double sumX = 0, sumY = 0, sumXY = 0, sumX2 = 0;

            for (int i = 0; i < n; i++)
            {
                sumX += xValues[i];
                sumY += yValues[i];
                sumXY += xValues[i] * yValues[i];
                sumX2 += xValues[i] * xValues[i];
            }

            double xMean = sumX / n;
            double yMean = sumY / n;

            // Calculate slope (b1) and intercept (b0)
            double slope = (n * sumXY - sumX * sumY) / (n * sumX2 - sumX * sumX);
            double intercept = yMean - slope * xMean;

            // Calculate R-squared
            double ssTotal = 0;
            double ssResidual = 0;
            for (int i = 0; i < n; i++)
            {
                double prediction = slope * xValues[i] + intercept;
                ssTotal += Math.Pow(yValues[i] - yMean, 2);
                ssResidual += Math.Pow(yValues[i] - prediction, 2);
            }

            double rSquared = ssTotal == 0 ? 0 : 1 - (ssResidual / ssTotal);

            return new RegressionResult
            {
                Slope = slope,
                Intercept = intercept,
                RSquared = rSquared
            };
        }
    }

    // Supporting classes
    public class RegressionResult
    {
        public double Slope { get; set; }
        public double Intercept { get; set; }
        public double RSquared { get; set; }
    }

    public class PredictionResponse
    {
        public double TargetScore { get; set; }
        public double PredictedHours { get; set; }
        public double RSquared { get; set; }
        public string RegressionEquation { get; set; }
        public int DataPointsUsed { get; set; }
    }
}