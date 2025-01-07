using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SocialEngineeringForum.Models;
using System.Threading.Tasks;

namespace SocialEngineeringForum.Controllers
{
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext _context; // Ссылка на контекст базы данных

        public UsersController(ApplicationDbContext context) // Конструктор контроллера
        {
            _context = context; // Инициализация контекста базы данных
        }

        // GET: Users
        public async Task<IActionResult> Index() // Возвращает представление со списком всех пользователей
        {
            // Запрос к базе данных для получения всех пользователей
            var users = await _context.Users.ToListAsync();
            return View(users); // Передаем список пользователей в представление
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(int? id) // Возвращает представление с деталями пользователя по его идентификатору
        {
            if (id == null)
            {
                return NotFound(); // Возвращает ошибку 404, если идентификатор не передан
            }

            var user = await _context.Users.FirstOrDefaultAsync(m => m.Id == id); // Получение пользователя по идентификатору
            if (user == null)
            {
                return NotFound(); // Возвращает ошибку 404, если пользователь не найден
            }

            return View(user); // Возвращает представление с деталями пользователя
        }

        // GET: Users/Create
        public IActionResult Create() // Возвращает представление для создания нового пользователя
        {
            return View(); // Возвращает форму для создания пользователя
        }

        // POST: Users/Create
        [HttpPost]
        [ValidateAntiForgeryToken] // Защита от поддельных запросов
        public async Task<IActionResult> Create(User user) // Обработка POST-запроса на создание пользователя
        {
            if (ModelState.IsValid) // Проверяем валидность данных
            {
                _context.Add(user); // Добавляем пользователя в контекст
                await _context.SaveChangesAsync(); // Сохраняем изменения в базе данных
                return RedirectToAction(nameof(Index)); // Перенаправляем на страницу со списком пользователей
            }
            return View(user); // Если данные невалидны, возвращаем форму с ошибками
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(int? id) // Возвращает представление для редактирования пользователя
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id); // Получение пользователя по id
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: Users/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, User user) // Обработка POST-запроса на редактирование пользователя
        {
            if (id != user.Id)
            {
                return NotFound(); // Возвращаем 404, если id не соответствует
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user); // Обновление данных пользователя в контексте
                    await _context.SaveChangesAsync(); // Сохранение изменений в базе данных
                    return RedirectToAction(nameof(Index)); // Перенаправление на страницу со списком
                }
                catch (DbUpdateConcurrencyException) // Обработка ошибок одновременного обновления
                {
                    if (!UserExists(user.Id))
                    {
                        return NotFound(); // Возвращает 404 если запись была удалена
                    }
                    else
                    {
                        throw; // Перебрасываем исключение, если проблема не в отсутствии записи
                    }
                }
            }
            return View(user); // Возвращаем представление с формой редактирования и ошибками, если модель невалидна
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(int? id) // Возвращает представление для удаления пользователя по его идентификатору
        {
            if (id == null) // Проверка на корректность переданного идентификатора
            {
                return NotFound(); // Возвращает ошибку 404, если идентификатор не передан или некорректен
            }

            var user = await _context.Users.FirstOrDefaultAsync(m => m.Id == id); // Получение пользователя из базы данных по переданному идентификатору

            if (user == null) // Проверка, существует ли пользователь с указанным идентификатором
            {
                return NotFound(); // Возвращает ошибку 404, если пользователь не найден
            }

            return View(user); // Возвращает представление для отображения данных пользователя перед подтверждением удаления, передавая объект пользователя в качестве модели
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")] //  Используется ActionName для соответствия имени метода POST запросу
        [ValidateAntiForgeryToken] // Защита от поддельных запросов (CSRF)
        public async Task<IActionResult> DeleteConfirmed(int id) //  Обработка подтверждения удаления пользователя
        {
            var user = await _context.Users.FindAsync(id); // Получение пользователя из базы данных по переданному идентификатору

            if (user == null) // Проверка, существует ли пользователь
            {
                return RedirectToAction(nameof(Index)); // Перенаправляет на страницу со списком пользователей, если пользователь не найден
            }

            try
            {
                _context.Users.Remove(user); // Удаление пользователя из контекста
                await _context.SaveChangesAsync(); // Сохранение изменений в базе данных
                return RedirectToAction(nameof(Index)); // Перенаправление на страницу со списком пользователей после успешного удаления
            }
            catch (DbUpdateException ex)
            {
                // Обработка исключений, которые могут возникнуть при удалении пользователя, 
                // например, если пользователь участвует в других зависимых отношениях (например, темы, сообщения и т.д.).
                // Это очень важно, чтобы избежать необработанных ошибок и проблем с целостностью данных.

                // Вместо простого throw, выводим сообщение об ошибке пользователю.
                // Сообщение об ошибке должно быть понятным и содержать информацию о типе проблемы.
                ModelState.AddModelError(string.Empty, $"Ошибка при удалении пользователя: {ex.Message}"); //Добавляем ошибку в ModelState
                return View("Delete", user); // Возврат представления Delete с добавленной ошибкой, чтобы пользователь мог увидеть её.
            }
        }

        private bool UserExists(int id) // Метод для проверки существования пользователя по идентификатору
        {
            return _context.Users.Any(e => e.Id == id); // Возвращает true, если пользователь с заданным идентификатором существует в базе данных, false иначе
        }
    }
}