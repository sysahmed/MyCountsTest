using Microsoft.AspNetCore.Mvc;
using MyCountsTest.Models;
using MyCountsTest.Tools;
using System.Text;
using System.Text.Json;

namespace MyCountsTest.Controllers
{

    public class HomeController : Controller
    {
        public IActionResult Index()
        {

            //Вземи контекст и ако е нула създай нов
            var numbers = HttpContext.Session.Get<List<int>>("Numbers") ?? new List<int>();

            //присвои промнеливи към модела
            var viewModel = new IndexViewModel
            {
                Numbers = numbers,
                Count = numbers.Count
            };

            return View(viewModel);
        }
        [HttpPost]
        public IActionResult AddNumber()
        {
            int randomNumber = Calculations();

            return Json(randomNumber);
        }

        private int Calculations()
        {
            //провери в сесията дали имам присвоен модел
            HttpContext.Session.TryGetValue("Numbers", out byte[] numbersData);

            //провери модела ако нул съсздай нов модел ако не десериализирай модела
            List<int> numbers = numbersData != null ? JsonSerializer.Deserialize<List<int>>(Encoding.UTF8.GetString(numbersData)) : new List<int>();

            //създай произволно число от 1 до 1000
            var randomNumber = new Random().Next(1, 1000);

            //присвои прозволното число към модела
            numbers.Add(randomNumber);

            //сериализирай модела в байт масив
            HttpContext.Session.Set("Numbers", Encoding.UTF8.GetBytes(JsonSerializer.Serialize(numbers)));
            return randomNumber;
        }

        [HttpPost]
        public IActionResult SumNumbers()
        {
            //извлечи стойностите от сесията
            var numbers = HttpContext.Session.Get<List<int>>("Numbers") ?? new List<int>();

            //сумирай стойностите
            var sum = numbers.Sum();

            //присвои във ViewBag сумата
            ViewBag.Sum = sum;

            //сумата я върни с ViewBag към _SumPartial
            return PartialView("_SumPartial");
        }

        [HttpPost]
        public IActionResult ClearNumbers()
        {
            //изтрий сессия
            HttpContext.Session.Set("Numbers", new List<int>());

            return Json("Sum: Not summed");
        }


    }
}
