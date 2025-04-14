using PruebaTecnicaBeyond.Interfaces;

namespace PruebaTecnicaBeyond.Repositories
{
    public class TodoListRepository : ITodoListRepository
    {
        private int _currentId = 0;
        private readonly List<string> _categories = new List<string> { "Work", "Personal", "Hobby", "Other" };

        public int GetNextId()
        {
            _currentId++;
            return _currentId;
        }

        public List<string> GetAllCategories()
        {
            return _categories;
        }
    }
}
