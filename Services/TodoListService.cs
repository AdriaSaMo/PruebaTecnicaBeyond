using PruebaTecnicaBeyond.Domain;
using PruebaTecnicaBeyond.Interfaces;

namespace PruebaTecnicaBeyond.Services
{
    public class TodoListService : ITodoList
    {
        private readonly ITodoListRepository _repository;
        private readonly List<TodoItem> _todoItems;

        public TodoListService(ITodoListRepository repository)
        {
            _repository = repository;
            _todoItems = new List<TodoItem>();
        }

        public void AddItem(int id, string title, string description, string category)
        {
            // Validar que la categoría se encuentre en las permitidas
            if (!_repository.GetAllCategories().Contains(category))
            {
                throw new ArgumentException("Categoría inválida.");
            }
            var newItem = new TodoItem(id, title, description, category);
            _todoItems.Add(newItem);
        }

        public void UpdateItem(int id, string description)
        {
            var item = _todoItems.FirstOrDefault(t => t.Id == id);
            if (item == null) 
            {
                throw new ArgumentException("El TodoItem no se encontró.");
            }                
            item.UpdateDescription(description);
        }

        public void RemoveItem(int id)
        {
            var item = _todoItems.FirstOrDefault(t => t.Id == id);
            if (item == null) 
            { 
                throw new ArgumentException("El TodoItem no se encontró."); 
            }
            if (item.TotalPercentage > 50)
            {
                throw new InvalidOperationException("No se puede eliminar un TodoItem con más del 50% completado.");
            }
            _todoItems.Remove(item);
        }

        public void RegisterProgression(int id, DateTime dateTime, decimal percent)
        {
            var item = _todoItems.FirstOrDefault(t => t.Id == id);
            if (item == null) 
            {
                throw new ArgumentException("El TodoItem no se encontró.");
            }
            var progression = new Progression(dateTime, percent);
            item.AddProgression(progression);
        }

        public void PrintItems()
        {
            // Ordenar los TodoItems por Id
            var orderedItems = _todoItems.OrderBy(t => t.Id);
            foreach (var item in orderedItems)
            {
                Console.WriteLine($"{item.Id}) {item.Title} - {item.Description} ({item.Category}) Completed:{item.IsCompleted}");
                decimal accumulatedPercent = 0;
                foreach (var progression in item.Progressions)
                {
                    accumulatedPercent += progression.Percent;
                    // Barra de progreso de 50 caracteres
                    int totalBlocks = 50;
                    int filledBlocks = (int)(accumulatedPercent / 100 * totalBlocks);
                    string progressBar = new string('O', filledBlocks).PadRight(totalBlocks, ' ');

                    int calculateNumbers = (accumulatedPercent.ToString().Replace(",", "").Replace(".", "").Count());
                    int whiteSpaces = 5;
                    string spaceAfterPercent = "";
                    if (calculateNumbers > 2) 
                    { 
                        int calculateWhiteSpacesToErase = whiteSpaces-(calculateNumbers - 2);
                        spaceAfterPercent = SpacesAfterPercent(calculateWhiteSpacesToErase);
                    }
                    else
                    {
                        spaceAfterPercent = SpacesAfterPercent(whiteSpaces);
                    }
                    Console.WriteLine($"{progression.Date:MM/dd/yyyy} - {accumulatedPercent}%{spaceAfterPercent}|{progressBar}|");
                }
                Console.WriteLine();
            }
        }


        private string SpacesAfterPercent(int spaces) 
        {
            string result = "";
            while (spaces > 0) 
            {
                result = result + " ";
                spaces --;
            }

            return result;
        }
    }
}
