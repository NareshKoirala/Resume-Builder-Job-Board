using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using resume_builder_api.Models;

namespace resume_builder_api.Services
{
    public static class ResumeDocx
    {
        private static Dictionary<string, string> keyValuePairs = new Dictionary<string, string>
        {
            ["Fullname"] = "",
            ["Titlekeyword"] = "",
            ["Details"] = "",
            ["Summarydata"] = "",
            ["PortfolioURL"] = ""
        };

        private static void UpdateMap(UserModel user, ResumeModel resume)
        {
            keyValuePairs["Fullname"] = user.FirstName + " " + user.LastName;
            keyValuePairs["Titlekeyword"] = resume.TitleKeyword;
            keyValuePairs["Details"] = $"{user.Province}, {user.Location} | {user.Email} | {user.Mobile} | {user.LinkedInUrl}";
            keyValuePairs["Summarydata"] = resume.Summary;
            keyValuePairs["PortfolioURL"] = user.PortfolioUrl;
        }

        private static void educationDataFunc(Paragraph para, List<EducationModel> education)
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

        private static void workDataFunc(Paragraph para, List<string> workData)
        {
            if (para == null) return;
            var parent = para.Parent;

            foreach (var data in workData)
            {
                var splitData = data.Split('\n');

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

        private static void skillsDataFunc(Paragraph para, List<CertificationModel> cert, List<string> skillsData)
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
            skills = skills.Replace(",", "|");

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

        private static void projectDataFunc(Paragraph para, List<ProjectsModel> projects)
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

        private static void bulletText(string bullet, ref Paragraph para, OpenXmlElement parent)
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

        public static void FixValueUpdate(Body body, UserModel user,ResumeModel jobModel)
        {
            UpdateMap(user, jobModel);

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
                            workDataFunc(para, workData:jobModel.WorkExperience);
                            text.Text = "";
                            continue;
                        }

                        if (text.Text.Contains("Educationdata"))
                        {
                            educationDataFunc(para, education:jobModel.Education);
                            text.Text = "";
                            continue;
                        }

                        if (text.Text.Contains("Projectdata"))
                        {
                            projectDataFunc(para, projects:jobModel.Projects);
                            text.Text = "";
                            continue;
                        }

                        if (text.Text.Contains("Skillsdata"))
                        {
                            List<string> skillsData = new List<string>()
                            {
                                jobModel.ProgramingLanguage,
                                jobModel.Frameworks,
                                jobModel.RelevantKeywords
                            };
                            skillsDataFunc(para, cert: jobModel.Certificates, skillsData:skillsData);
                            text.Text = "";
                            continue;
                        }

                        if (keyValuePairs.ContainsKey(text.Text))
                            text.Text = keyValuePairs[text.Text];
                    }
                }
            }

        }

    }
}
