using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SocialEngineeringForum.Models;
using System.Threading.Tasks;

namespace SocialEngineeringForum.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ApplicationDbContext _context; // Объявление поля для доступа к контексту базы данных

        public CategoriesController(ApplicationDbContext context)
        {
            _context = context; // Инициализация контекста базы данных через конструктор
        }

        // GET: Categories
        // Метод для получения списка всех категорий
        public async Task<IActionResult> Index()
        {
            // Запрашиваем все категории из базы данных и конвертируем их в список
            var categories = await _context.Categories.ToListAsync();
            // Передаем список категорий в представление
            return View(categories);
        }

        // GET: Categories/Details/5
        // Метод для отображения деталей конкретной категории
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) // Проверка на null для ID
            {
                return NotFound(); // Возвращаем 404 Not Found, если ID null
            }

            // Ищем категорию по ID
            var category = await _context.Categories.FirstOrDefaultAsync(m => m.Id == id);
            if (category == null) // Проверка на null для категории
            {
                return NotFound(); // Возвращаем 404 Not Found, если категория не найдена
            }

            // Передаем категорию в представление
            return View(category);
        }


        // GET: Categories/Create
        // Метод для отображения формы создания новой категории
        public IActionResult Create()
        {
            return View(); // Возвращаем представление для создания
        }

        // POST: Categories/Create
        // Метод для обработки отправки формы создания новой категории
        [HttpPost]
        [ValidateAntiForgeryToken] // Защита от CSRF атак
        public async Task<IActionResult> Create(Category category)
        {
            if (ModelState.IsValid) // Проверка валидности данных модели
            {
                try
                {
                    _context.Add(category); // Добавляем новую категорию в контекст
                    await _context.SaveChangesAsync(); // Сохраняем изменения в базе данных
                    return RedirectToAction(nameof(Index)); // Перенаправляем на страницу списка категорий
                }
                catch (DbUpdateException ex) // Ловим исключение, если произошла ошибка при добавлении
                {
                    // Обработка ошибки при добавлении категории (например, дублирование имени).
                    // Очень важно проанализировать DbUpdateException! Она может содержать
                    // более подробную информацию об ошибке.

                    // Более надежная проверка:
                    var existingCategory = _context.Categories.FirstOrDefault(c => c.Name.Equals(category.Name, StringComparison.OrdinalIgnoreCase));
                    if (existingCategory != null)
                    {
                        ModelState.AddModelError(nameof(Category.Name), "Категория с таким названием уже существует.");
                        return View(category);
                    }

                    // Если проблема не в дублировании, показываем исходное сообщение об ошибке.
                    ModelState.AddModelError(string.Empty, $"Ошибка при создании категории: {ex.Message}");
                    return View(category);
                }
            }
            return View(category); // Если данные не валидны, возвращаем представление с формой и ошибками
        }

        // POST: Categories/Edit/5
        // Метод для обработки отправки формы редактирования категории
        [HttpPost]
        [ValidateAntiForgeryToken] // Защита от CSRF атак
        public async Task<IActionResult> Edit(int id, Category category)
        {
            if (id != category.Id) // Проверяем, совпадает ли ID в URL и ID в модели
            {
                return NotFound(); // Возвращаем 404 Not Found, если ID не совпадают
            }

            if (ModelState.IsValid) // Проверяем валидность данных модели
            {
                try
                {
                    _context.Update(category); // Обновляем категорию в контексте
                    await _context.SaveChangesAsync(); // Сохраняем изменения в базе данных
                    return RedirectToAction(nameof(Index)); // Перенаправляем на страницу списка категорий
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(category.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return View(category); // Если данные не валидны, возвращаем представление с формой и ошибками
        }

        // GET: Categories/Delete/5
        // Метод для отображения формы удаления категории
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) // Проверка на null для ID
            {
                return NotFound(); // Возвращаем 404 Not Found, если ID null
            }

            // Ищем категорию по ID
            var category = await _context.Categories.FirstOrDefaultAsync(m => m.Id == id);
            if (category == null) // Проверка на null для категории
            {
                return NotFound(); // Возвращаем 404 Not Found, если категория не найдена
            }

            // Передаем категорию в представление для подтверждения удаления
            return View(category);
        }

        // POST: Categories/Delete/5
        // Метод для обработки отправки формы подтверждения удаления категории
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken] // Защита от CSRF атак
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            // Ищем категорию по ID
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return RedirectToAction(nameof(Index)); // Возвращаем к списку, если запись не найдена
            }
            try
            {
                _context.Categories.Remove(category); // Удаляем категорию из контекста
                await _context.SaveChangesAsync(); // Сохраняем изменения в базе данных
                return RedirectToAction(nameof(Index)); // Перенаправляем на страницу списка категорий
            }
            catch (DbUpdateException ex)
            {
                // Возникла проблема с базой данных, например, если категория используется в других записях
                ModelState.AddModelError(string.Empty, $"Ошибка при удалении категории: {ex.Message}"); // Важно: Указываем пустое поле, для корректной отрисовки сообщений об ошибках в представлении
                return View("Delete", category); //Возвращаем представление Delete с ошибкой
            }
        }


        // Helper method to check if category exists
        private bool CategoryExists(int id)
        {
            return _context.Categories.Any(e => e.Id == id); // Проверяем, существует ли категория с указанным ID
        }
    }
}