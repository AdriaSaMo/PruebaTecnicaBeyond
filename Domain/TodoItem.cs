namespace PruebaTecnicaBeyond.Domain
{
    public class TodoItem
    {
        public int Id { get; }
        public string Title { get; }
        public string Description { get; private set; }
        public string Category { get; }
        public List<Progression> Progressions { get; }
        public bool IsCompleted => TotalPercentage == 100m;
        public decimal TotalPercentage => Progressions.Sum(p => p.Percent);

        public TodoItem(int id, string title, string description, string category)
        {
            Id = id;
            Title = title;
            Description = description;
            Category = category;
            Progressions = new List<Progression>();
        }

        public void AddProgression(Progression progression)
        {
            // Valida que la fecha sea superior a la última progresión registrada
            if (Progressions.Any() && progression.Date <= Progressions.Last().Date)
            {
                throw new InvalidOperationException("La fecha de la nueva progresión debe ser superior a las ya existentes.");
            }
            // Valida que el porcentaje esté entre 0 y 100
            if (progression.Percent <= 0 || progression.Percent >= 100)
            {
                throw new InvalidOperationException("El porcentaje debe ser mayor que 0 y menor que 100.");
            }
            // Valida que la suma acumulada no sobrepase el 100%
            if (TotalPercentage + progression.Percent > 100)
            {
                throw new InvalidOperationException("La suma de los porcentajes excede el 100%.");
            }
            Progressions.Add(progression);
        }

        public void UpdateDescription(string newDescription)
        {
            // No se permite actualizar si el progreso acumulado es mayor al 50%
            if (TotalPercentage > 50)
            {
                throw new InvalidOperationException("No se puede actualizar un TodoItem con más del 50% completado.");
            }
            Description = newDescription;
        }
    }
}
