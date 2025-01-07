using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SocialEngineeringForum.Models;
using System.Threading.Tasks;

namespace SocialEngineeringForum.Controllers
{
    public class TopicsController : Controller
    {
        private readonly ApplicationDbContext _context; // Ссылка на контекст базы данных

        public TopicsController(ApplicationDbContext context) // Конструктор контроллера
        {
            _context = context; // Инициализация контекста
        }

        // GET: Topics
        public async Task<IActionResult> Index() // Получение списка всех тем
        {
            // Получаем все темы, включая информацию об авторах тем.
            var topics = await _context.Topics.Include(t => t.Author).ToListAsync(); 
            return View(topics); // Возвращаем представление со списком тем
        }

        // GET: Topics/Details/5
        public async Task<IActionResult> Details(int? id) // Получение деталей темы по ID
        {
            if (id == null)
            {
                return NotFound(); // Возвращаем ошибку 404, если ID null
            }

            // Запрос на получение темы, включающий автора и категорию.
            var topic = await _context.Topics
                .Include(t => t.Author) // Загружаем данные автора
                .Include(t => t.Category) // Загружаем данные категории
                .FirstOrDefaultAsync(m => m.Id == id); // Находим тему по ID

            if (topic == null)
            {
                return NotFound(); // Возвращаем ошибку 404, если тема не найдена
            }

            return View(topic); // Возвращаем представление с деталями темы
        }

        // GET: Topics/Create
        public IActionResult Create() // Отображение формы создания темы
        {
            // Загружаем список категорий для выбора в форме.
            ViewData["Categories"] = _context.Categories.ToList();
            return View(); // Возвращаем представление для создания темы
        }

        // POST: Topics/Create
        [HttpPost]
        [ValidateAntiForgeryToken] // Защита от CSRF-атак
        public async Task<IActionResult> Create(Topic topic) // Обработка создания темы
        {
            if (ModelState.IsValid) // Проверка валидности данных
            {
                _context.Add(topic); // Добавляем тему в контекст
                await _context.SaveChangesAsync(); // Сохраняем изменения в базе данных
                return RedirectToAction(nameof(Index)); // Переадресовываем на страницу со списком тем
            }
            return View(topic); // Возвращаем форму создания, если данные невалидны
        }

        // GET: Topics/Edit/5
        public async Task<IActionResult> Edit(int? id) // Возвращает представление для редактирования темы с заданным идентификатором
        {
            if (id == null) // Проверяем, передан ли корректный идентификатор
            {
                return NotFound(); // Возвращаем 404, если идентификатор не передан или некорректен
            }

            // Запрос к базе данных для получения темы по указанному идентификатору, включая связанный объект автора и категорию
            var topic = await _context.Topics
                .Include(t => t.Author) // Загружаем данные автора (включая всю связанную информацию)
                .Include(t => t.Category) // Загружаем данные категории
                .FirstOrDefaultAsync(m => m.Id == id); // Поиск темы по Id

            if (topic == null) // Проверка, существует ли тема с таким идентификатором
            {
                return NotFound(); // Если тема не найдена, возвращаем 404
            }
            return View(topic); // Возвращаем представление для редактирования, передавая тему в качестве модели
        }

        // POST: Topics/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken] // Защита от поддельных запросов (CSRF)
        public async Task<IActionResult> Edit(int id, Topic topic) // Обработка POST-запроса для редактирования темы
        {
            if (id != topic.Id) // Проверка, соответствует ли идентификатор темы из запроса идентификатору, полученному из модели.  Это важная проверка безопасности, предотвращающая несанкционированное изменение данных.
            {
                return NotFound(); // Возвращаем 404, если идентификаторы не совпадают
            }

            if (ModelState.IsValid) // Проверяем, что все данные в модели topic валидны
            {
                try
                {
                    _context.Update(topic); // Обновляем тему в контексте _context
                    await _context.SaveChangesAsync(); // Сохраняем изменения в базе данных
                    return RedirectToAction(nameof(Index)); // Перенаправляем пользователя на страницу со списком тем
                }
                catch (DbUpdateConcurrencyException) // Обработка исключения, которое может произойти при одновременном обновлении записи другими пользователями
                {
                    if (!TopicExists(topic.Id)) // Проверка, существует ли тема по данному Id после попытки обновления
                    {
                        return NotFound(); // Возвращаем 404, если тема не найдена
                    }
                    else
                    {
                        throw; // Если проблема не в отсутствии записи, перебрасываем исключение, чтобы оно было обработано выше
                    }
                }
            }
            return View(topic); // Возвращаем представление для редактирования, если данные невалидны, и передаем тему для отображения в форме
        }

        // GET: Topics/Delete/5
        public async Task<IActionResult> Delete(int? id) // Возвращает представление для удаления темы по заданному идентификатору
        {
            if (id == null) // Проверяет, передан ли корректный идентификатор
            {
                return NotFound(); // Возвращает ошибку 404, если идентификатор не передан или некорректен
            }

            // Запрос к базе данных для получения темы по указанному идентификатору
            var topic = await _context.Topics.FirstOrDefaultAsync(m => m.Id == id); // Поиск темы по идентификатору

            if (topic == null) // Проверка, существует ли тема с таким идентификатором
            {
                return NotFound(); // Возвращает ошибку 404, если тема не найдена
            }

            return View(topic); // Возвращает представление для отображения темы перед подтверждением удаления, передавая тему в качестве модели
        }

        // POST: Topics/Delete/5
        [HttpPost, ActionName("Delete")] // Используется ActionName для обработки POST-запроса с именем действия "Delete"
        [ValidateAntiForgeryToken] // Защита от поддельных запросов (CSRF)
        public async Task<IActionResult> DeleteConfirmed(int id) // Обработка подтверждения удаления темы
        {
            var topic = await _context.Topics.FindAsync(id); // Получение темы из базы данных по идентификатору

            if (topic == null) // Проверка, существует ли тема с указанным идентификатором
            {
                return RedirectToAction(nameof(Index)); // Перенаправляет на страницу со списком тем, если тема не найдена
            }

            try
            {
                _context.Topics.Remove(topic); // Удаляет тему из контекста базы данных
                await _context.SaveChangesAsync(); // Сохраняет изменения в базе данных
                return RedirectToAction(nameof(Index)); // Перенаправляет на страницу со списком тем после успешного удаления
            }
            catch (DbUpdateException ex)
            {
                // Обработка исключений, которые могут возникнуть при удалении темы,
                // например, если тема используется в других связанных записях.
                //  Это критически важно для надежного приложения.

                ModelState.AddModelError(string.Empty, $"Ошибка при удалении темы: {ex.Message}"); // Добавление сообщения об ошибке в ModelState
                return View("Delete", topic); // Возвращаем представление для удаления, отображая сообщение об ошибке пользователю
            }
        }


        private bool TopicExists(int id) // Проверка существования темы по ID
        {
            return _context.Topics.Any(e => e.Id == id); // Проверяем, существует ли тема с указанным ID
        }
    }
}