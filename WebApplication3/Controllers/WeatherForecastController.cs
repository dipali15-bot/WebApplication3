using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace WebApplication3.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpPost]
        public async Task<ActionResult> Post(string test)
        {
            List<string> items = new List<string>()
            {
                "abc",
                "def",
                "xyz"
            };


            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            cancellationTokenSource.CancelAfter(100);
            var token = cancellationTokenSource.Token;

            //first approach
            Stopwatch stopwatch = new Stopwatch();

            stopwatch.Start();
            ParallelOptions po = new ParallelOptions()
            {
                CancellationToken = token
            };
            try
            {
                var strResults =
            Parallel.ForEach<string>(items, po, item => DoSomething(item).GetAwaiter());
            }
            catch (Exception ex)
            {

            }
            stopwatch.Stop();
            Console.WriteLine("Parallel execution completed in ms " + stopwatch.ElapsedMilliseconds);

            //second approach
            //Stopwatch stopwatch1 = new Stopwatch();
            //stopwatch1.Start();
            //try
            //{
            //    foreach (var item in items)
            //    {
            //        await Task.Factory.StartNew(() => DoSomething(item), token);
            //    }
            //}
            //catch (Exception ex)
            //{

            //}
            //stopwatch1.Stop();
            //Console.WriteLine("foreach execution completed in ms " + stopwatch1.ElapsedMilliseconds);

            //third approach
            //Stopwatch stopwatch2 = new Stopwatch();
            //stopwatch2.Start();
            //string[] re;
            //try
            //{
            //    var rslt =
            //    await Task.WhenAll(items.Select(s => Task.Run(() => DoSomething(s), token)));

            //    //   var rslt =
            //    //await Task.WhenAll(items.Select(s => Task.Run(() => DoSomething(s))));
            //    //await Task.WhenAll(items.Select(s => DoSomething(s), token)));

            //    foreach (var g in rslt)
            //    {

            //    }
            //}  
            //catch(Exception ex)
            //{

            //}            
            //stopwatch2.Stop();            
            //Console.WriteLine("Parallel execution completed in ms " + stopwatch2.ElapsedMilliseconds);            


            return Ok("success");
        }

        private async Task<string> DoSomething(string item)
        {
            try
            {
                await Task.Delay(200);
                Thread.Sleep(200);
                return "hi " + item;
            }
            catch(Exception ex)
            {
                return null;
            }
        }
    }
}
