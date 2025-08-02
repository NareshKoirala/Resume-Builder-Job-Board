using Resume_Builder_MAUI.Model;
using Xceed.Document.NET;
using Xceed.Drawing;
using Xceed.Words.NET;

namespace Resume_Builder_MAUI.Services
{
    public static class DocCreationServcie
    {
        public async static Task CreateResumeAsync(GenJobsModel? model)
        {
            UserModel? user =  await GlobalVarables.service.GetUserAsync();
            List<EducationEntry> education = await GlobalVarables.service.GetEducationAsync();
            List<WorkEntry> experience = await GlobalVarables.service.GetWorkAsync();
            List<CertificateEntry> certificates = await GlobalVarables.service.GetCertificationsAsync();
            ResumeModel? resume = model?.Resume;
            List<ProjectsModel>? projects = resume?.Projects;

            string filePath = GlobalVarables.Paths($"{model?.Jobs?.companyName}_Resume_{model?.JobName}.docx");

            using (var doc = DocX.Create(filePath))
            {   
                doc.MarginBottom = 25;
                doc.MarginLeft = 25;
                doc.MarginRight = 25;
                doc.MarginTop = 25;

                // Set default font
                doc.SetDefaultFont(new Xceed.Document.NET.Font("Calibri"), 11);

                // Add a title
                AddTitles(doc, user?.FirstName + " " + user?.LastName, 30);

                // User Key Word
                AddTitles(doc, resume?.TitleKeyword, 13);

                // Add address with clickable email and LinkedIn links
                AddAddressWithLinks(
                    doc,
                    user?.Province + ", "+ user?.Location.ToUpper(),
                    user?.Mobile,
                    user?.Email,
                    user?.LinkedInUrl,
                    10
                );

                // Add summary section
                AddTitles(doc, "SUMMARY", 18);
                Paragraph(doc,
                    resume?.Summary,
                    false,
                    false,
                    11,
                    false);

                // Add skills section
                AddTitles(doc, "SKILLS", 18);
                BulletList(doc, new List<string>
                    {
                        $"Programming Languages: {resume?.ProgramingLanguage}",
                        $"Framework & Libaries: {resume?.Frameworks}",
                        $"Relevant Skills: {resume?.RelevantKeywords}"
                    },
                    1);

                if (experience.Count > 0)
                {
                    // Add experience section
                    AddTitles(doc, "EXPERIENCE", 18);
                    foreach (var job in experience)
                    {
                        Paragraph(doc, $"{job.CompanyName} ({job.Date})", true, false, 11, false);
                        Paragraph(doc,
                            $"{job.Details}",
                            false,
                            false,
                            11,
                            true
                            );
                    }
                }
                if (certificates.Count > 0)
                {
                    // Add certifications section
                    AddTitles(doc, "CERTIFICATIONS", 18);
                    foreach (var cert in certificates)
                    {
                        Paragraph(doc, $"{cert.CertificateName}", true, false, 11, false);
                        Paragraph(doc,
                            $"{cert.Details}",
                            false,
                            false,
                            11,
                            true
                            );
                    }
                }

                if (education.Count > 0)
                {
                    // Add education section
                    AddTitles(doc, "EDUCATION", 18);
                    foreach (var edu in education)
                    {
                        Paragraph(doc, $"{edu.InstitutionName} | {edu.Location} | ({edu.Date})", true, false, 11, false);
                        Paragraph(doc,
                            $"{edu.Details}",
                            false,
                            false,
                            11,
                            true
                            );
                    }
                }
                // Add portfolio link
                AddTitles(doc, "PROJECT", 18);

                AddPortfolioLink(doc, user?.PortfolioUrl);

                if (projects?.Count > 0)
                {
                    foreach (var project in projects)
                    {
                        Paragraph(doc, $"{project.ProjectName}", true, false, 11, false);
                        Paragraph(doc,
                            $"{project.Description}",
                            false,
                            false,
                            11,
                            true
                            );
                        BulletList(doc,
                            new List<string>() 
                            {
                                project.Bullet1,
                                project.Bullet2,
                                project.Bullet3
                            });
                    }
                }

                // Save the document
                doc.Save();
            }
        }

        public static void CreateCoverLetter(GenJobsModel? model)
        {
            UserModel user = GlobalVarables.userInfo.User;
            CoverLetterModel? cover = model?.CoverLetter;
            string filePath = GlobalVarables.Paths($"{model?.Jobs?.companyName}_CoverLetter_{model?.JobName}.docx");

            using (var doc = DocX.Create(filePath))
            {
                doc.MarginBottom = 50;
                doc.MarginLeft = 50;
                doc.MarginRight = 50;
                doc.MarginTop = 50;
                // Set default font
                doc.SetDefaultFont(new Xceed.Document.NET.Font("Calibri"), 11);

                // Add a title
                AddTitles(doc, "COVER LETTER", 30);

                // Add address with clickable email and LinkedIn links
                AddAddressWithLinks(
                    doc,
                    user.Province + ", " + user.Location.ToUpper(),
                    user.Mobile,
                    user.Email,
                    user.LinkedInUrl,
                    10
                );
                // Add the cover letter content
                Paragraph(doc, cover?.Title, false, false, 20, false);
                Paragraph(doc, cover?.Body1, false, false, 20, false);
                Paragraph(doc, cover?.Body2, false, false, 20, false);
                Paragraph(doc, cover?.Body3, false, false, 20, false);
                // Save the document
                doc.Save();
            }
        }

        private static void AddPortfolioLink(DocX doc, string? url)
        {
            var p = doc.InsertParagraph();
            p.Alignment = Alignment.center;
            p.FontSize(16);
            p.Bold();
            // Add the hyperlink to the portfolio
            AddHyperlinkToParagraph(doc, p, "Portfolio: " + url, url);
            Spacing(doc);
        }

        private static void AddTitles(DocX doc, string? data, int fontSize)
        {
            doc.InsertParagraph(data)
                .FontSize(fontSize)
                .Bold()
                .Alignment = Alignment.center;
            Spacing(doc);
        }

        /// <summary>
        /// Adds an address line with clickable email and LinkedIn hyperlinks.
        /// </summary>
        private static void AddAddressWithLinks(DocX doc, string location, string? phone, string? email, string? linkedinUrl, int fontSize)
        {
            var p = doc.InsertParagraph();
            p.Alignment = Alignment.center;
            p.FontSize(fontSize);

            // Add location and phone
            p.Append(location + " | " + phone + " | ");

            // Add email as hyperlink
            AddHyperlinkToParagraph(doc, p, "Email: " + email, "mailto:" + email);

            p.Append(" | ");

            // Add LinkedIn as hyperlink (display only the username part for brevity)
            AddHyperlinkToParagraph(doc, p, "LinkedIn", linkedinUrl);

            Spacing(doc);
        }

        /// <summary>
        /// Helper to add a hyperlink to a paragraph.
        /// </summary>
        private static void AddHyperlinkToParagraph(DocX doc, Paragraph p, string displayText, string? url)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                p.Append(displayText); // Just append text if URL is null or empty
                return;
            }
            var hyperlink = doc.AddHyperlink(displayText, new Uri(uriString: url));
            p.AppendHyperlink(hyperlink);
        }

        private static void Spacing(DocX doc)
        {
            doc.InsertParagraph("").FontSize(5);
        }

        private static void Paragraph(DocX doc, string? data, bool bold, bool centerAlign, int fontSize, bool italics)
        {
            var p = doc.InsertParagraph(data);

            if (bold)
                p.Bold();

            if (centerAlign)
                p.Alignment = Alignment.center;
            else
                p.Alignment = Alignment.left;

            if (italics)
                p.Italic();

            if (fontSize > 0)
                p.FontSize(fontSize);

            Spacing(doc);
        }

        private static void BulletList(DocX doc, List<string> items, int indentLevel = 1)
        {
            if (items == null || items.Count == 0)
                return;

            // Create the initial bulleted list with the desired indentation level
            var bulletedList = doc.AddList(items[0], indentLevel, ListItemType.Bulleted, 0, false, false);

            // Add the rest of the items at the same indentation level
            for (int i = 1; i < items.Count; i++)
            {
                doc.AddListItem(bulletedList, items[i], indentLevel, ListItemType.Bulleted, 0, false, false);
            }

            doc.InsertList(bulletedList);
            Spacing(doc);
        }

    }
}
