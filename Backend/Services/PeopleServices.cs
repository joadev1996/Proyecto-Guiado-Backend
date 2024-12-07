using Backend.Controllers;

namespace Backend.Services
{
    public class PeopleServices : IPeopleServices
    {
        public bool Validate(People people)
        {
            if (string.IsNullOrEmpty(people.Name))
            {
                return false;
            }
            return true;
        }
    }
}