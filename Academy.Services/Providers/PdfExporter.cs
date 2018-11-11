using Academy.Services.Providers.Abstract;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Academy.Services.Providers
{
    public class PdfExporter : IExporter
    {
        public void GenerateReport(IList<ViewModels.GradeViewModel> grades, string username)
        {
            try
            {
                PdfPTable footer = new PdfPTable(1);
                PdfPTable name = new PdfPTable(1);
                var boldFont = FontFactory.GetFont(FontFactory.TIMES_BOLD, 16);

                //Top of the page
                Chunk c1 = new Chunk("STUDENT REPORT", boldFont);
                footer.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
                footer.DefaultCell.Border = 0;
                footer.AddCell(new Phrase(c1));

                Chunk studentName = new Chunk(username, boldFont);
                name.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
                name.DefaultCell.Border = 0;
                name.AddCell(new Phrase(studentName));

                string currentCourse = null;
                var gradeTables = new List<PdfPTable>();
                var currentTable = 0;
                foreach (var item in grades)
                {
                    if (item.Assaingment.Course.CourseName != currentCourse)
                    {
                        currentCourse = item.Assaingment.Course.CourseName;
                        gradeTables.Add(new PdfPTable(3));
                        currentTable = gradeTables.Count - 1;
                        PdfPCell headercell = new PdfPCell(new Phrase($"Grades for the course: {currentCourse}", boldFont));
                        headercell.Colspan = 3;
                        headercell.HorizontalAlignment = Element.ALIGN_CENTER;
                        gradeTables[currentTable].AddCell(headercell);
                        gradeTables[currentTable].AddCell("Assaignment Name");
                        gradeTables[currentTable].AddCell("Your Score");
                        gradeTables[currentTable].AddCell("Max Score");
                    }

                    gradeTables[currentTable].AddCell($"{item.Assaingment.Name}");
                    gradeTables[currentTable].AddCell($"{item.Score}");
                    gradeTables[currentTable].AddCell($"{item.Assaingment.MaxPoints}");
                }

                //Set up file name and directory
                string folderPath = ".\\wwwroot\\PDF\\";
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                int fileCount = Directory.GetFiles(".\\wwwroot\\PDF\\").Length;
                string strFileName = "StudentReport" + (fileCount + 1) + ".pdf";

                using (FileStream stream = new FileStream(folderPath + strFileName, FileMode.Create))
                {
                    Rectangle pageSize = new Rectangle(PageSize.A4);
                    pageSize.BackgroundColor = new BaseColor(255, 180, 203);
                    Document pdfDoc = new Document(pageSize);

                    PdfWriter.GetInstance(pdfDoc, stream);
                    pdfDoc.Open();
                    pdfDoc.Add(footer);
                    pdfDoc.Add(name);
                    foreach (var table in gradeTables)
                    {
                        table.SpacingBefore = 20;
                        pdfDoc.Add(table);
                    }
                    pdfDoc.NewPage();

                    pdfDoc.Close();
                    stream.Close();                    
                }               
            }
            catch (Exception)
            {
                throw new Exception("Cannot export to PDF");
            }
        }
    }
}
