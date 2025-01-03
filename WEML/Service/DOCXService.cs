using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using WEML.Models;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using WEML.Controllers;

namespace WEML.Service
{
    public class DOCXService
    {
        public FileContentResult GenerateHealthStatisticsDocx(List<MonthlyStatistics> SymptomStatistics, List<MonthlyStatistics> FeelingStatistics, Statistics MostFrequentSymptom, Statistics MostFrequentFeeling)
        {
           
            using var memoryStream = new MemoryStream();
            using (var wordDocument = WordprocessingDocument.Create(memoryStream, DocumentFormat.OpenXml.WordprocessingDocumentType.Document))
            {
                var mainPart = wordDocument.AddMainDocumentPart();
                mainPart.Document = new DocumentFormat.OpenXml.Wordprocessing.Document(new Body());
                var body = mainPart.Document.Body;
                
                body.Append(new Paragraph(new Run(new Text(" Health Statistics Report  ")) { RunProperties = new RunProperties(new FontSize() { Val = "36" }) }));

                if (MostFrequentSymptom != null)
                {
                    body.Append(new Paragraph(new Run(new Text($" Most Frequent Symptom:  {MostFrequentSymptom.Name}")) { RunProperties = new RunProperties(new Bold()) }));
                    body.Append(new Paragraph(new Run(new Text($" Occurrences:  {MostFrequentSymptom.Count}"))));
                    body.Append(new Paragraph(new Run(new Text($" Average Severity:  {MostFrequentSymptom.AverageSeverity:F2}"))));
                }

                if (MostFrequentFeeling != null)
                {
                    body.Append(new Paragraph(new Run(new Text($" Most Frequent Feeling:  {MostFrequentFeeling.Name}")) { RunProperties = new RunProperties(new Bold()) }));
                    body.Append(new Paragraph(new Run(new Text($" Occurrences:  {MostFrequentFeeling.Count}"))));
                    body.Append(new Paragraph(new Run(new Text($" Average Severity:  {MostFrequentFeeling.AverageSeverity:F2}"))));
                }

                var recentMonths = DateTime.Now.AddMonths(-3);
                body.Append(new Paragraph(new Run(new Text(" Last 3 Months - Symptoms ")) { RunProperties = new RunProperties(new FontSize() { Val = "28" }) }));
                foreach (var month in SymptomStatistics.Where(stat => stat.MonthYear >= recentMonths))
                {
                    body.Append(new Paragraph(new Run(new Text($" {month.MonthYear:MMMM yyyy} ")) { RunProperties = new RunProperties(new Bold()) }));
                    foreach (var entry in month.Entries)
                    {
                        body.Append(new Paragraph(new Run(new Text($"{entry.Name}: Count = {entry.Count}, Avg Severity = {entry.AverageSeverity:F2}"))));
                    }
                }

                body.Append(new Paragraph(new Run(new Text(" Last 3 Months - Feelings ")) { RunProperties = new RunProperties(new FontSize() { Val = "28" }) }));
                foreach (var month in FeelingStatistics.Where(stat => stat.MonthYear >= recentMonths))
                {
                    body.Append(new Paragraph(new Run(new Text($" {month.MonthYear:MMMM yyyy} "))));
                    foreach (var entry in month.Entries)
                    {
                        body.Append(new Paragraph(new Run(new Text($"{entry.Name}: Number Of Ocurrances = {entry.Count}, Average Severity = {entry.AverageSeverity:F2}"))));
                    }
                }
            }  

            byte[] docxBytes = memoryStream.ToArray();
            return new FileContentResult(docxBytes, "application/vnd.openxmlformats-officedocument.wordprocessingml.document")
            {
                FileDownloadName = "HealthReport"+System.DateTime.Now.Date+".docx"
            };
        }

        
    }
}
