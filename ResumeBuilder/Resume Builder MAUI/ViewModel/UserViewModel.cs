using System.Collections.ObjectModel;
using Resume_Builder_MAUI.Model;

namespace Resume_Builder_MAUI.ViewModel
{
    public class UserViewModel
    {
        public UserModel User { get; set; }

        // Unified ObservableCollections for all entry types
        public ObservableCollection<EducationEntry> Education { get; set; }
        public ObservableCollection<WorkEntry> WorkExperience { get; set; }
        public ObservableCollection<CertificateEntry> Certificates { get; set; }
        public ObservableCollection<SkillEntry> Skills { get; set; }
        public ObservableCollection<ProjectEntry> Projects { get; set; }

        // Entry templates for adding new items
        public EducationEntry educationEntry { get; set; } = new();
        public WorkEntry workEntry { get; set; } = new();
        public CertificateEntry certificateEntry { get; set; } = new();
        public SkillEntry skillEntry { get; set; } = new();
        public ProjectEntry projectEntry { get; set; } = new();

        public UserViewModel Clone()
        {
            return new UserViewModel
            {
                User = new UserModel
                {
                    FirstName = this.User.FirstName,
                    LastName = this.User.LastName,
                    Email = this.User.Email,
                    Mobile = this.User.Mobile,
                    Location = this.User.Location,
                    Province = this.User.Province,
                    JobField = this.User.JobField,
                    PortfolioUrl = this.User.PortfolioUrl,
                    LinkedInUrl = this.User.LinkedInUrl,
                    UserSummary = this.User.UserSummary,
                    Education = [.. this.Education],
                    WorkExperience = [.. this.WorkExperience],
                    Certificates = [.. this.Certificates],
                    Skills = [.. this.Skills],
                    Projects = [.. this.Projects]
                }
            };
        }

        public UserViewModel()
        {
            User = new UserModel();

            // Initialize ObservableCollections from UserModel lists
            Education = new ObservableCollection<EducationEntry>(User.Education);
            WorkExperience = new ObservableCollection<WorkEntry>(User.WorkExperience);
            Certificates = new ObservableCollection<CertificateEntry>(User.Certificates);
            Skills = new ObservableCollection<SkillEntry>(User.Skills);
            Projects = new ObservableCollection<ProjectEntry>(User.Projects);

            // Optionally, subscribe to collection changes to keep UserModel in sync
            Education.CollectionChanged += (s, e) => SyncList(User.Education, Education);
            WorkExperience.CollectionChanged += (s, e) => SyncList(User.WorkExperience, WorkExperience);
            Certificates.CollectionChanged += (s, e) => SyncList(User.Certificates, Certificates);
            Skills.CollectionChanged += (s, e) => SyncList(User.Skills, Skills);
            Projects.CollectionChanged += (s, e) => SyncList(User.Projects, Projects);
        }

        // Helper to sync ObservableCollection to List in UserModel
        private static void SyncList<T>(List<T> list, ObservableCollection<T> collection)
        {
            list.Clear();
            list.AddRange(collection);
        }

        // Helper for error management
        private static void SetError(string key, bool hasError)
        {
            if (hasError)
            {
                if (!GlobalVarables.ErrorMsg.Contains(key))
                    GlobalVarables.ErrorMsg.Add(key);
            }
            else
            {
                GlobalVarables.ErrorMsg.Remove(key);
            }
        }

        // Properties for UserModel fields
        public string FirstName
        {
            get => User.FirstName;
            set
            {
                User.FirstName = value;
                SetError(nameof(FirstName), string.IsNullOrWhiteSpace(value));
            }
        }

        public string LastName
        {
            get => User.LastName;
            set
            {
                User.LastName = value;
                SetError(nameof(LastName), string.IsNullOrWhiteSpace(value));
            }
        }

        public string Email
        {
            get => User.Email;
            set
            {
                User.Email = value;
                bool invalid = string.IsNullOrWhiteSpace(value) || !value.Contains('@');
                SetError(nameof(Email), invalid);
            }
        }

        public string Mobile
        {
            get => User.Mobile;
            set
            {
                User.Mobile = value;
                bool invalid = string.IsNullOrWhiteSpace(value) || value.Length < 10;
                SetError(nameof(Mobile), invalid);
            }
        }

        public string Location
        {
            get => User.Location;
            set
            {
                User.Location = value;
                SetError(nameof(Location), string.IsNullOrWhiteSpace(value));
            }
        }

        public string Province
        {
            get => User.Province;
            set
            {
                User.Province = value;
                SetError(nameof(Province), string.IsNullOrWhiteSpace(value));
            }
        }

        public string JobField
        {
            get => User.JobField;
            set
            {
                User.JobField = value;
                SetError(nameof(JobField), string.IsNullOrWhiteSpace(value));
            }
        }

        public string UserSummary
        {
            get => User.UserSummary;
            set
            {
                User.UserSummary = value;
                SetError(nameof(UserSummary), string.IsNullOrWhiteSpace(value));
            }
        }

        public string PortfolioUrl
        {
            get => User.PortfolioUrl;
            set
            {
                User.PortfolioUrl = value;
                bool invalid = string.IsNullOrWhiteSpace(value);
                SetError(nameof(PortfolioUrl), invalid);
            }
        }

        public string LinkedInUrl
        {
            get => User.LinkedInUrl;
            set
            {
                User.LinkedInUrl = value;
                bool invalid = string.IsNullOrWhiteSpace(value);
                SetError(nameof(LinkedInUrl), invalid);
            }
        }

        public ObservableCollection<string> LocationOptions { get; set; } = new()
        {
            "ca", "us", "mx"
        };

        public Dictionary<string, ObservableCollection<string>> CountryProvinces { get; set; } = new()
        {
            { "ca", new ObservableCollection<string>
                {
                    "Alberta", "British Columbia", "Manitoba", "New Brunswick", "Newfoundland and Labrador",
                    "Nova Scotia", "Ontario", "Prince Edward Island", "Quebec", "Saskatchewan"
                }
            },
            { "us", new ObservableCollection<string>
                {
                    "Alabama", "Alaska", "Arizona", "Arkansas", "California", "Colorado", "Connecticut", "Delaware",
                    "Florida", "Georgia", "Hawaii", "Idaho", "Illinois", "Indiana", "Iowa", "Kansas", "Kentucky",
                    "Louisiana", "Maine", "Maryland", "Massachusetts", "Michigan", "Minnesota", "Mississippi",
                    "Missouri", "Montana", "Nebraska", "Nevada", "New Hampshire", "New Jersey", "New Mexico",
                    "New York", "North Carolina", "North Dakota", "Ohio", "Oklahoma", "Oregon", "Pennsylvania",
                    "Rhode Island", "South Carolina", "South Dakota", "Tennessee", "Texas", "Utah", "Vermont",
                    "Virginia", "Washington", "West Virginia", "Wisconsin", "Wyoming"
                }
            },
            { "mx", new ObservableCollection<string>
                {
                    "Aguascalientes", "Baja California", "Baja California Sur", "Campeche", "Chiapas", "Chihuahua",
                    "Coahuila", "Colima", "Durango", "Guanajuato", "Guerrero", "Hidalgo", "Jalisco", "Mexico City",
                    "Mexico State", "Michoacán", "Morelos", "Nayarit", "Nuevo León", "Oaxaca", "Puebla", "Querétaro",
                    "Quintana Roo", "San Luis Potosí", "Sinaloa", "Sonora", "Tabasco", "Tamaulipas", "Tlaxcala",
                    "Veracruz", "Yucatán", "Zacatecas"
                }
            }
        };

        public ObservableCollection<string> JobFieldOptions { get; set; } = new()
        {
            "software-dev", "design", "data", "devops"
        };
    }
}
