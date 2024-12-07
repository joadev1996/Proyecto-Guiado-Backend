using Backend.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PeopleController : ControllerBase
    {
        private IPeopleServices _peopleServices;

        public PeopleController([FromKeyedServices("people2Services")]IPeopleServices peopleServices)
        {
            _peopleServices = peopleServices;
        }

        [HttpGet("all")]
        public List<People> GetPeople() => Repository.People;

        [HttpGet("{id}")]
        //public ActionResult<People> Get(int id) => Repository.People.First(p => p.Id == id);

        public ActionResult<People> Get(int id)
        {

            var people = Repository.People.FirstOrDefault(p => p.Id == id);
            if (people == null)
            {
                return NotFound();
            }

            return Ok(people);
        }
        [HttpGet("search/{search}")]

        public List<People> Get(string search) =>
            Repository.People.Where(p => p.Name.ToUpper().Contains(search.ToUpper())).ToList();

        [HttpPost]
        public IActionResult Add(People people)
        {
            if (!_peopleServices.Validate(people))
            {
                return BadRequest();
            }

            Repository.People.Add(people);

            return NoContent();

        }



    }

    public class Repository
    {
        public static List<People> People = new List<People>()
        {
            new People()
            {
                Id = 1, Name= "Gandalf", BirthDate= new DateTime(1956,2,1)

            },
            new People()
            {
                Id = 2, Name= "Robert", BirthDate= new DateTime(1939, 12, 3)

            },
            new People()
            {
                Id = 3, Name= "Gandalf", BirthDate= new DateTime(1923, 2, 3)

            },
            new People()
            {
                Id= 4, Name= "JCN", BirthDate = new DateTime(1996,11,1)
            }

        };
    }


    public class People
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime BirthDate { get; set; }
    }
}