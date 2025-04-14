namespace PruebaTecnicaBeyond.Interfaces
{
    public interface ITodoListRepository
    {
        int GetNextId();
        List<string> GetAllCategories();
    }
}
