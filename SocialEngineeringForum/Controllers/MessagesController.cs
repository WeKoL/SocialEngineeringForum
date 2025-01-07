using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SocialEngineeringForum.Models;
using System.Threading.Tasks;

namespace SocialEngineeringForum.Controllers
{
    public class MessagesController : Controller
    {
        private readonly ApplicationDbContext _context; // Объявляем поле для доступа к базе данных

        public MessagesController(ApplicationDbContext context)
        {
            _context = context; // Инициализируем контекст базы данных в конструкторе
        }

        // GET: Messages
        // Метод для отображения списка всех сообщений
        public async Task<IActionResult> Index()
        {
            // Запрашиваем все сообщения из базы данных, включая информацию об авторе и теме сообщения
            var messages = await _context.Messages
                .Include(m => m.Author) // Подключаем данные автора
                .Include(m => m.Topic)  // Подключаем данные темы
                .ToListAsync(); // Конвертируем результат в список

            return View(messages); // Передаем список сообщений в представление
        }

        // GET: Messages/Details/5
        // Метод для отображения деталей конкретного сообщения по ID
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) // Проверяем, передан ли ID сообщения
            {
                return NotFound(); // Возвращаем 404 Not Found, если ID не передан
            }

            // Ищем сообщение по ID, включая информацию об авторе и теме сообщения
            var message = await _context.Messages
                .Include(m => m.Author) // Подключаем данные автора
                .Include(m => m.Topic) // Подключаем данные темы
                .FirstOrDefaultAsync(m => m.Id == id); // Ищем первое сообщение, удовлетворяющее условию

            if (message == null) // Проверяем, найдено ли сообщение
            {
                return NotFound(); // Возвращаем 404 Not Found, если сообщение не найдено
            }

            return View(message); // Передаем сообщение в представление
        }

        // GET: Messages/Create
        // Метод для отображения формы создания нового сообщения
        public IActionResult Create()
        {
            // Передаем списки пользователей и тем для выбора в представлении
            ViewData["Users"] = _context.Users.ToList();
            ViewData["Topics"] = _context.Topics.ToList();
            return View(); // Отображаем форму создания
        }

        // POST: Messages/Create
        // Метод для обработки отправки формы создания нового сообщения
        [HttpPost]
        [ValidateAntiForgeryToken] // Защита от CSRF-атак
        public async Task<IActionResult> Create(Message message)
        {
            if (ModelState.IsValid) // Проверяем, валидны ли данные формы
            {
                _context.Add(message); // Добавляем новое сообщение в контекст
                await _context.SaveChangesAsync(); // Сохраняем изменения в базе данных
                return RedirectToAction(nameof(Index)); // Перенаправляем пользователя на страницу со списком сообщений
            }
            return View(message); // Если данные невалидны, возвращаем форму с ошибками
        }

        // GET: Messages/Edit/5
        // Метод для отображения формы редактирования сообщения по ID
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) // Проверяем, передан ли ID сообщения
            {
                return NotFound(); // Возвращаем 404 Not Found, если ID не передан
            }

            var message = await _context.Messages.FindAsync(id); // Ищем сообщение по ID
            if (message == null) // Проверяем, найдено ли сообщение
            {
                return NotFound(); // Возвращаем 404 Not Found, если сообщение не найдено
            }

            return View(message); // Передаем сообщение в представление для редактирования
        }

        // POST: Messages/Edit/5
        // Метод для обработки отправки формы редактирования сообщения по ID
        [HttpPost]
        [ValidateAntiForgeryToken] // Защита от CSRF-атак
        public async Task<IActionResult> Edit(int id, Message message)
        {
            if (id != message.Id) // Проверяем, совпадает ли ID из URL с ID сообщения
            {
                return NotFound(); // Возвращаем 404 Not Found, если ID не совпадают
            }

            if (ModelState.IsValid) // Проверяем, валидны ли данные формы
            {
                try
                {
                    _context.Update(message); // Обновляем сообщение в контексте
                    await _context.SaveChangesAsync(); // Сохраняем изменения в базе данных
                    return RedirectToAction(nameof(Index)); // Перенаправляем на страницу со списком сообщений
                }
                catch (DbUpdateConcurrencyException) // Ловим исключение, если произошла ошибка при сохранении
                {
                    if (!MessageExists(message.Id)) // Проверяем, существует ли сообщение
                    {
                        return NotFound(); // Возвращаем 404 Not Found, если сообщение не найдено
                    }
                    else
                    {
                        throw; // Перебрасываем исключение, если оно не связано с отсутствием сообщения
                    }
                }
            }
            return View(message); // Если данные невалидны, возвращаем форму с ошибками
        }

        // GET: Messages/Delete/5
        // Метод для отображения формы удаления сообщения по ID
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) // Проверяем, передан ли ID сообщения
            {
                return NotFound(); // Возвращаем 404 Not Found, если ID не передан
            }

            var message = await _context.Messages.FirstOrDefaultAsync(m => m.Id == id); // Ищем сообщение по ID
            if (message == null) // Проверяем, найдено ли сообщение
            {
                return NotFound(); // Возвращаем 404 Not Found, если сообщение не найдено
            }

            return View(message); // Передаем сообщение в представление для подтверждения удаления
        }

        // POST: Messages/Delete/5
        // Метод для обработки отправки формы подтверждения удаления сообщения по ID
        [HttpPost, ActionName("Delete")] // Используем ActionName, чтобы не было конфликта с именами методов
        [ValidateAntiForgeryToken] // Защита от CSRF-атак
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var message = await _context.Messages.FindAsync(id);  // Ищем сообщение по ID
            if (message != null) // Проверяем, найдено ли сообщение
            {
                _context.Messages.Remove(message); // Удаляем сообщение из контекста
                await _context.SaveChangesAsync(); // Сохраняем изменения в базе данных
            }

            return RedirectToAction(nameof(Index)); // Перенаправляем пользователя на страницу со списком сообщений
        }

        // Helper method to check if a message exists
        private bool MessageExists(int id)
        {
            return _context.Messages.Any(e => e.Id == id); // Проверяем, существует ли сообщение с указанным ID
        }
    }
}