using DocumentFormat;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System.IO;
using System.Text.Json;
using System.Threading.Channels;
namespace TestPDFCreation
{
    internal class Program
    {
        public static Dictionary<string, string> keyValuePairs = new Dictionary<string, string>
        {
            ["Fullname"] = "",
            ["Titlekeyword"] = "",
            ["Details"] = "",
            ["Summarydata"] = "",
            ["PortfolioURL"] = ""
        };

        public static List<string> skillsData = new List<string> {
            "C#",
            ".NET MAUI",
            "ASP.NET Core",
            "Blazor",
            "SQL",
            "HTML5",
            "CSS3",
            "JavaScript",
            "Liquid Templating",
            "FetchXML"
        };

        static void Main(string[] args)
        {
            string json = File.ReadAllText(@"C:\\Users\\chels\\Desktop\\Resume-Builder-App\\TestPDFCreation\\data.json");
            var data = JsonSerializer.Deserialize<Dictionary<string, object>>(json);

            string tempdocx = @"C:\Users\chels\Desktop\Resume-Builder-App\resume_builder_api\Templates\Resume\Template_1.docx";
            string output = @"C:\Users\chels\Desktop\Resume-Builder-App\TestPDFCreation\out.docx";

            File.Copy(tempdocx, output, true);

            using (var doc = WordprocessingDocument.Open(output, true))
            {
                var body = doc.MainDocumentPart.Document.Body;

                UpdateMap(data);

                FixValueUpdate(body, keyValuePairs);

                doc.MainDocumentPart.Document.Save();
            }

        }

        public static void UpdateMap(Dictionary<string, object> jsonData) 
        {
            keyValuePairs["Fullname"] = jsonData["FullName"].ToString();
            keyValuePairs["Titlekeyword"] = jsonData["TitleKeyword"].ToString();
            keyValuePairs["Details"] = jsonData["Details"].ToString();
            keyValuePairs["Summarydata"] = jsonData["Summary"].ToString();
            keyValuePairs["PortfolioURL"] = jsonData["PortfolioURL"].ToString();
        }

        public static void skillsDataFunc(Paragraph para)
        {
            var parent = para.Parent;
            string skills = string.Join("                   ", skillsData);
            Paragraph bulletPara = new Paragraph();

            // Run with text
            Run run = new Run();
            run.Append(new Text(skills));

            // Customize text formatting
            RunProperties runProps = new RunProperties();
            //runProps.Bold = new Bold();                      // bold text
            runProps.Color = new Color() { Val = "0070C0" }; // blue color (hex)
            runProps.FontSize = new FontSize() { Val = "22" }; // 12pt (OpenXML uses half-points)
            run.PrependChild(runProps);

            bulletPara.Append(run);

            // Customize paragraph properties
            //ParagraphProperties pp = new ParagraphProperties();
            //pp.ParagraphStyleId = new ParagraphStyleId() { Val = "ListParagraph" }; // use bullet style from template

            // Optional: indentation for bullet
            //pp.Indentation = new Indentation() { Left = "720" }; // 720 = 0.5 inch

            //bulletPara.PrependChild(pp);

            // Insert into document
            parent.InsertAfter(bulletPara, para);
            para = bulletPara;

        }

        public static void FixValueUpdate(Body body, Dictionary<string, string> map)
        {
            foreach (var para in body.ChildElements.OfType<Paragraph>())
            {
                foreach (var run in para.ChildElements.OfType<Run>())
                {
                    foreach (var text in run.ChildElements.OfType<Text>())
                    {
                        if (string.IsNullOrEmpty(text.Text) || text.Text.All(char.IsUpper))
                            continue;

                        if (text.Text.Contains("Skillsdata"))
                        {
                            skillsDataFunc(para);
                            text.Text = "";
                            continue;
                        }

                        if (map.ContainsKey(text.Text))
                            text.Text = map[text.Text];
                    }
                }
            }

        }
    }
}