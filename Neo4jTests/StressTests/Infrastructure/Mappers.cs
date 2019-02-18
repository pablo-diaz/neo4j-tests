using StressTests.Domain;

namespace StressTests.Infrastructure
{
    public static class Mappers
    {
        public static CompanyDTO MapToDTO(this Company company) => new CompanyDTO() { id = company.Id, name = company.Name };

        public static UserDTO MapToDTO(this User user) => new UserDTO() { id = user.Id, firstname = user.Firstname, lastname = user.Lastname, login = user.Login };

        public static ChallengeDTO MapToDTO(this Challenge challenge) => new ChallengeDTO() { id = challenge.Id, title = challenge.Title };

        public static PresentationDTO MapToDTO(this Presentation presentation) => new PresentationDTO() { id = presentation.Id, title = presentation.Title };

        public static SlideDTO MapToDTO(this Slide slide) => new SlideDTO() { id = slide.Id, title = slide.Title, order = slide.Order };

        public static CourseDTO MapToDTO(this Course course) => new CourseDTO() { id = course.Id, title = course.Title };
    }

    public class CompanyDTO
    {
        public int id { get; set; }
        public string name { get; set; }
    }

    public class UserDTO
    {
        public int id { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string login { get; set; }
        public string name
        {
            get
            {
                return $"{firstname} {lastname}";
            }
        }
    }

    public class ChallengeDTO
    {
        public int id { get; set; }
        public string title { get; set; }
        public string name
        {
            get
            {
                return $"{title}";
            }
        }
    }

    public class PresentationDTO
    {
        public int id { get; set;  }
        public string title { get; set; }

        public string name
        {
            get
            {
                return $"{title}";
            }
        }
    }

    public class SlideDTO
    {
        public int id { get; set; }
        public string title { get; set; }
        public int order { get; set; }

        public string name
        {
            get
            {
                return $"{title}";
            }
        }
    }

    public class CourseDTO
    {
        public int id { get; set; }
        public string title { get; set; }

        public string name
        {
            get
            {
                return $"{title}";
            }
        }
    }
}
