using PruebaTecnicaBeyond.Repositories;
using PruebaTecnicaBeyond.Services;

namespace PruebaTecnicaBeyond.Test.Services
{
    public class TodoListServiceTests
    {

        private readonly TodoListRepository _repository;
        private readonly TodoListService _service;

        public TodoListServiceTests()
        {
            _repository = new TodoListRepository();
            _service = new TodoListService(_repository);
        }

        [Fact]
        public void AddItem_And_RegisterProgression_Ok()
        {
            int id = _repository.GetNextId();
            _service.AddItem(id, "Reporte del Proyecto", "Completar el reporte final del proyecto", "Work");

            _service.RegisterProgression(id, DateTime.Now.AddDays(1), 30);
            _service.RegisterProgression(id, DateTime.Now.AddDays(2), 50);
            _service.RegisterProgression(id, DateTime.Now.AddDays(3), 20);

            Assert.True(true, "El TodoItem fue completado correctamente al llegar al 100% de progreso.");
        }

        [Fact]
        public void RegisterProgression_DateOutOfOrder_ThrowsException()
        {
            int id = _repository.GetNextId();
            _service.AddItem(id, "Test Orden de Fechas", "Se prueba la validación de fechas", "Work");
            DateTime firstDate = DateTime.Now.AddDays(1);
            DateTime earlierDate = DateTime.Now;  // Fecha anterior a firstDate

            _service.RegisterProgression(id, firstDate, 30);

            Assert.Throws<InvalidOperationException>(() =>
                _service.RegisterProgression(id, earlierDate, 20));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(100)]
        public void RegisterProgression_InvalidPercent_ThrowsException(decimal invalidPercent)
        {
            int id = _repository.GetNextId();
            _service.AddItem(id, "Test Porcentaje Inválido", "Probar valores de porcentaje no permitidos", "Work");

            Assert.Throws<InvalidOperationException>(() =>
                _service.RegisterProgression(id, DateTime.Now.AddDays(1), invalidPercent));
        }

        [Fact]
        public void RegisterProgression_ExceedingTotalPercentage_ThrowsException()
        {
            int id = _repository.GetNextId();
            _service.AddItem(id, "Test Suma Excesiva", "Probar que la suma no supere el 100%", "Work");

            _service.RegisterProgression(id, DateTime.Now.AddDays(1), 60);
            _service.RegisterProgression(id, DateTime.Now.AddDays(2), 30);

            Assert.Throws<InvalidOperationException>(() =>
                _service.RegisterProgression(id, DateTime.Now.AddDays(3), 20));
        }

        [Fact]
        public void UpdateItem_OverFiftyPercentProgress_ThrowsException()
        {
            int id = _repository.GetNextId();
            _service.AddItem(id, "Test Actualización", "Descripción Inicial", "Work");

            _service.RegisterProgression(id, DateTime.Now.AddDays(1), 30);
            _service.RegisterProgression(id, DateTime.Now.AddDays(2), 30);

            Assert.Throws<InvalidOperationException>(() =>
                _service.UpdateItem(id, "Nueva descripción"));
        }

        [Fact]
        public void RemoveItem_OverFiftyPercentProgress_ThrowsException()
        {
            int id = _repository.GetNextId();
            _service.AddItem(id, "Test Eliminación", "Descripción Inicial", "Work");

            _service.RegisterProgression(id, DateTime.Now.AddDays(1), 30);
            _service.RegisterProgression(id, DateTime.Now.AddDays(2), 30);

            Assert.Throws<InvalidOperationException>(() =>
                _service.RemoveItem(id));
        }
    }
}
