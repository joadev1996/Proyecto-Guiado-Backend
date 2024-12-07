using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SomeController : ControllerBase
    {
        [HttpGet("sync")]
        public IActionResult GetSync()
        {

            Stopwatch sw = Stopwatch.StartNew();

            sw.Start();

            Thread.Sleep(1000);
            Console.WriteLine("Conexión a base de datos terminada");

            Thread.Sleep(1000);
            Console.WriteLine("Envío de mail terminado");

            Console.WriteLine("Todo ha terminado");
            sw.Stop();
            return Ok(sw.Elapsed);
        }

        [HttpGet("async")]
        public async Task<IActionResult> GetAsync()
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            stopwatch.Start();
            var task = new Task<int>(() =>
            {
                Thread.Sleep(1000);
                Console.WriteLine("Conexión a base de datos terminada");
                return 1;
            });
            var task1 = new Task<int>(() =>
            {
                Thread.Sleep(1000);
                Console.WriteLine("Envío de mail terminado");
                return 2;
            });
            task.Start();
            task1.Start();

            Console.WriteLine("Hago otra cosa");

            var result = await task;

            var result1 = await task1;

            Console.WriteLine("Todo ha terminado");

            stopwatch.Stop();

            return Ok(result + " " + result1 + " " + stopwatch.Elapsed);
        }
    }
}
