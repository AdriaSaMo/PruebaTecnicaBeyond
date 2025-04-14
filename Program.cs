using PruebaTecnicaBeyond.Repositories;
using PruebaTecnicaBeyond.Services;

var repository = new TodoListRepository();
var todoService = new TodoListService(repository);

int newId = repository.GetNextId();
todoService.AddItem(newId, "Complete Project Report", "Finish the final report for the project", "Work");
todoService.RegisterProgression(newId, new DateTime(2025, 3, 18), 30);
todoService.RegisterProgression(newId, new DateTime(2025, 3, 19), 50);
todoService.RegisterProgression(newId, new DateTime(2025, 3, 20), 20);

todoService.PrintItems();

Console.WriteLine("");
Console.ReadKey();