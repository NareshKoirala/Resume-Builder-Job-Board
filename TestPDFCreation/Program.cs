using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System.Text.Json;
namespace TestPDFCreation
{
    internal class Program
    {
        public class ProjectsModel
        {
            public string ProjectName { get; set; } = string.Empty;
            public string Description { get; set; } = string.Empty;
            public string Bullet1 { get; set; } = string.Empty;
            public string Bullet2 { get; set; } = string.Empty;
            public string Bullet3 { get; set; } = string.Empty;
        }

        public class EducationModel
        {
            public string Title { get; set; } = string.Empty;
            public string Course { get; set; } = string.Empty;
            public string Details { get; set; } = string.Empty;
        }

        public class CertificationModel
        {
            public string Details { get; set; } = string.Empty;
        }

        public static Dictionary<string, string> certificationData = new Dictionary<string, string>
        {
            ["details"] = "Generative AI (LinkedIn Learning)"
        };

        public static Dictionary<string, string> keyValuePairs = new Dictionary<string, string>
        {
            ["Fullname"] = "",
            ["Titlekeyword"] = "",
            ["Details"] = "",
            ["Summarydata"] = "",
            ["PortfolioURL"] = ""
        };

        public static List<string> skillsData = new List<string> {
            "full-stack development | application security | CI/CD | DevOps | deployment | system architecture",
            "React",
            "C#, React, TypeScript, MAUI, .NET Core"
        };

        static Dictionary<string, string> project = new Dictionary<string, string>
        {
            ["projectName"] = "Resume Builder App",
            ["description"] = "I led the development of a cross-platform Resume Builder App using .NET MAUI, integrating multiple job site APIs and AI-driven customization to generate tailored resumes and cover letters automatically. This project highlights my ability to architect scalable, user-focused solutions that align with CES Corporation’s need for dynamic web applications.",
            ["bullet1"] = "Integrated multiple job site APIs to fetch and validate listings based on user preferences.",
            ["bullet2"] = "Implemented AI-powered customization to generate tailored documents matching selected job descriptions.",
            ["bullet3"] = "Enabled DOCX download functionality for personalized application materials."
        };
        static Dictionary<string, string> education = new Dictionary<string, string>
        {
            ["title"] = "NAIT, Edmonton, Alberta (2023–2025)",
            ["course"] = "Course: Computer Engineering Technology",
            ["details"] = "Where I developed software applications using C# and .NET by designing and implementing full-stack solutions and embedded system integrations, resulting in practical skills to deliver reliable, real-world software projects."
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

        public static void educationDataFunc(Paragraph para, List<EducationModel> education)
        {
            var parent = para.Parent;
            foreach (var edu in education)
            {
                // --- Education Title (blue, larger font) ---
                Paragraph titlePara = new Paragraph();
                Run titleRun = new Run(new Text(edu.Title));
                RunProperties titleProps = new RunProperties
                {
                    Bold = new Bold(),
                    FontSize = new FontSize() { Val = "22" }, // 12pt
                    RunFonts = new RunFonts() { Ascii = "Calibri", HighAnsi = "Calibri" } // set font
                };
                titleRun.PrependChild(titleProps);
                titlePara.Append(titleRun);
                parent.InsertAfter(titlePara, para);
                para = titlePara;
                // --- Course Name ---
                Paragraph coursePara = new Paragraph();
                Run courseRun = new Run(new Text(edu.Course));
                RunProperties courseProps = new RunProperties
                {
                    FontSize = new FontSize() { Val = "20" }, // 11pt
                    RunFonts = new RunFonts() { Ascii = "Calibri", HighAnsi = "Calibri" } // set font
                };
                courseRun.PrependChild(courseProps);
                coursePara.Append(courseRun);
                ParagraphProperties ppCourse = new ParagraphProperties();
                ppCourse.ParagraphStyleId = new ParagraphStyleId() { Val = "ListParagraph" };
                ppCourse.Indentation = new Indentation() { Left = "300" };
                coursePara.PrependChild(ppCourse);
                parent.InsertAfter(coursePara, para);
                para = coursePara;
                // --- Details ---
                Paragraph detailsPara = new Paragraph();
                Run detailsRun = new Run(new Text(edu.Details));
                RunProperties detailsProps = new RunProperties
                {
                    FontSize = new FontSize() { Val = "20" }, // 11pt
                    RunFonts = new RunFonts() { Ascii = "Calibri", HighAnsi = "Calibri" } // set font
                };
                detailsRun.PrependChild(detailsProps);
                detailsPara.Append(detailsRun);
                ParagraphProperties ppDetails = new ParagraphProperties();
                ppDetails.ParagraphStyleId = new ParagraphStyleId() { Val = "ListParagraph" };
                ppDetails.Indentation = new Indentation() { Left = "400" };
                detailsPara.PrependChild(ppDetails);
                parent.InsertAfter(detailsPara, para);
                para = detailsPara;
            }
        }

        public static void workDataFunc(Paragraph para, List<string> workData)
        {
            if (para == null) return;
            var parent = para.Parent;

            foreach (var data in workData)
            {
                var splitData = data.Split(':');

                if (splitData.Length < 2) continue;

                Paragraph bulletPara = new Paragraph();
                Run run = new Run(new Text(splitData[0]));
                RunProperties runProps = new RunProperties
                {
                    Bold = new Bold(),
                    FontSize = new FontSize() { Val = "22" }, // 12pt (OpenXML uses half-points)
                    RunFonts = new RunFonts() { Ascii = "Calibri", HighAnsi = "Calibri" } // set font
                };
                run.PrependChild(runProps);
                bulletPara.Append(run);
                parent.InsertAfter(bulletPara, para);
                para = bulletPara;

                Paragraph detailsPara = new Paragraph();
                Run detailsRun = new Run(new Text(splitData[1].Trim()));
                RunProperties detailsProps = new RunProperties
                {
                    FontSize = new FontSize() { Val = "20" }, // 11pt
                    RunFonts = new RunFonts() { Ascii = "Calibri", HighAnsi = "Calibri" } // set font
                };
                detailsRun.PrependChild(detailsProps);
                detailsPara.Append(detailsRun);
                ParagraphProperties ppDetails = new ParagraphProperties();
                ppDetails.ParagraphStyleId = new ParagraphStyleId() { Val = "ListParagraph" };
                ppDetails.Indentation = new Indentation() { Left = "300" };
                detailsPara.PrependChild(ppDetails);
                parent.InsertAfter(detailsPara, para);
                para = detailsPara;
            }

        }

        public static void skillsDataFunc(Paragraph para, List<CertificationModel> cert)
        {
            var parent = para.Parent;

            string certTitle = "";

            foreach (var c in cert)
            {
                certTitle += c.Details.Split(':')[0] + " | ";
            }


            Paragraph certPara = new Paragraph();
            Run certRun = new Run(new Text(certTitle));
            RunProperties certProps = new RunProperties
            {
                FontSize = new FontSize() { Val = "20" }, // 11pt
                RunFonts = new RunFonts() { Ascii = "Calibri", HighAnsi = "Calibri" } // set font
            };
            certRun.PrependChild(certProps);
            certPara.Append(certRun);
            ParagraphProperties ppCert = new ParagraphProperties();
            ppCert.ParagraphStyleId = new ParagraphStyleId() { Val = "ListParagraph" };
            certPara.PrependChild(ppCert);
            parent.InsertAfter(certPara, para);
            para = certPara;


            string skills = string.Join(" | ", skillsData);
            skills = skills.Replace(",", " |");

            Paragraph bulletPara = new Paragraph();

            // Run with text
            Run run = new Run();
            run.Append(new Text(skills));

            // Customize text formatting
            RunProperties runProps = new RunProperties();
            runProps.Color = new Color() { Val = "0070C0" }; // blue color (hex)
            runProps.FontSize = new FontSize() { Val = "20" }; // 12pt (OpenXML uses half-points)
            runProps.RunFonts = new RunFonts() { Ascii = "Calibri", HighAnsi = "Calibri" }; // set font
            run.PrependChild(runProps);

            bulletPara.Append(run);

            // Insert into document
            parent.InsertAfter(bulletPara, para);
            para = bulletPara;
        }

        public static void projectDataFunc(Paragraph para, List<ProjectsModel> projects)
        {
            var parent = para.Parent;

            foreach (var project in projects)
            {
                // --- Project Name (blue, larger font) ---
                Paragraph namePara = new Paragraph();
                Run nameRun = new Run(new Text(project.ProjectName));
                RunProperties nameProps = new RunProperties
                {
                    Bold = new Bold(),
                    FontSize = new FontSize() { Val = "22" }, // 12pt
                    RunFonts = new RunFonts() { Ascii = "Calibri", HighAnsi = "Calibri" } // set font
                };
                nameRun.PrependChild(nameProps);
                namePara.Append(nameRun);
                parent.InsertAfter(namePara, para);
                para = namePara;

                // --- Project Description ---
                Paragraph descPara = new Paragraph();
                Run descRun = new Run(new Text(project.Description));
                RunProperties descProps = new RunProperties
                {
                    FontSize = new FontSize() { Val = "20" }, // 11pt
                    RunFonts = new RunFonts() { Ascii = "Calibri", HighAnsi = "Calibri" } // set font
                };
                descRun.PrependChild(descProps);
                descPara.Append(descRun);

                ParagraphProperties pp = new ParagraphProperties();
                pp.ParagraphStyleId = new ParagraphStyleId() { Val = "ListParagraph" };
                pp.Indentation = new Indentation() { Left = "300" };
                descPara.PrependChild(pp);

                parent.InsertAfter(descPara, para);
                para = descPara;

                // --- Bullet Points ---
                bulletText(project.Bullet1, ref para, parent);
                bulletText(project.Bullet2, ref para, parent);
                bulletText(project.Bullet3, ref para, parent);
            }
        }

        public static void bulletText(string bullet, ref Paragraph para, OpenXmlElement parent)
        {
            Paragraph bulletPara = new Paragraph();
            Run run = new Run(new Text("⁍   " + bullet));

            RunProperties runProps = new RunProperties
            {
                FontSize = new FontSize() { Val = "20" }, // 11pt
                RunFonts = new RunFonts() { Ascii = "Calibri", HighAnsi = "Calibri" } // set font
            };
            run.PrependChild(runProps);

            bulletPara.Append(run);

            ParagraphProperties pp = new ParagraphProperties();
            pp.ParagraphStyleId = new ParagraphStyleId() { Val = "ListParagraph" };
            pp.Indentation = new Indentation() { Left = "600" };
            bulletPara.PrependChild(pp);

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

                        if (text.Text.Contains("Workdata"))
                        {
                            List<string> workDataList = new List<string>()
                            {
                                "Self-Employed, Edmonton, AB (2021–2024): As a Software Developer, built and maintained full-stack applications using C#, .NET Core, MAUI, and SQL, led API integrations for AI-driven features, and conducted security-focused code reviews. Collaborated cross-functionally to gather requirements, optimized performance, and ensured maintainability. Delivered user-friendly solutions including a cross-platform resume builder and robotic gameplay system.",
                                "Self-Employed, Edmonton, AB (2021–2024): As a Software Developer, built and maintained full-stack applications using C#, .NET Core, MAUI, and SQL, led API integrations for AI-driven features, and conducted security-focused code reviews. Collaborated cross-functionally to gather requirements, optimized performance, and ensured maintainability. Delivered user-friendly solutions including a cross-platform resume builder and robotic gameplay system."
                            };  
                            workDataFunc(para, workDataList);
                            text.Text = "";
                            continue;
                        }

                        if (text.Text.Contains("Educationdata"))
                        {
                            text.Text = "";
                            List<EducationModel> educationList = new List<EducationModel>()
                            {
                                new EducationModel
                                {
                                    Title = education["title"],
                                    Course = education["course"],
                                    Details = education["details"]
                                },
                                new EducationModel
                                {
                                    Title = education["title"],
                                    Course = education["course"],
                                    Details = education["details"]
                                }
                            };
                            educationDataFunc(para, educationList);
                            continue;
                        }

                        if (text.Text.Contains("Projectdata"))
                        {
                            text.Text = "";
                            List<ProjectsModel> projects = new List<ProjectsModel>() {
                                new ProjectsModel
                                {
                                    ProjectName = project["projectName"],
                                    Description = project["description"],
                                    Bullet1 = project["bullet1"],
                                    Bullet2 = project["bullet2"],
                                    Bullet3 = project["bullet3"]
                                },
                                new ProjectsModel
                                {
                                    ProjectName = project["projectName"],
                                    Description = project["description"],
                                    Bullet1 = project["bullet1"],
                                    Bullet2 = project["bullet2"],
                                    Bullet3 = project["bullet3"]
                                }
                            };
                            projectDataFunc(para, projects);
                            continue;
                        }

                        if (text.Text.Contains("Skillsdata"))
                        {
                            List<CertificationModel> certList = new List<CertificationModel>()
                            {
                                new CertificationModel
                                {
                                    Details = certificationData["details"]
                                },
                                new CertificationModel
                                {
                                    Details = certificationData["details"]
                                }
                            };
                            skillsDataFunc(para, certList);
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