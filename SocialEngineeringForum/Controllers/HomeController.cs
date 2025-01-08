using Microsoft.AspNetCore.Mvc; // Добавляем using для Microsoft.AspNetCore.Mvc

namespace YourProjectName.Controllers // Замените YourProjectName на имя вашего проекта.
{
    public class HomeController : Controller
    {
        public IActionResult Index() // Действие (action) для главной страницы
        {
            return View(); // Возвращаем представление Index.cshtml
        }
    }
}