using System;

namespace DopPDF
{
    internal class Customer : Emploee
    {
        public Customer(string name, Post post) : base(name, post) { }
        #region
        /// <summary>
        /// Создать проект
        /// </summary>
        public Project CreateProject(string description, DateTime deadlines)
        {
            Project project = new Project(description, deadlines, this);
            Console.WriteLine($"Заказчик {Name} создал проект '{description}' со сроком до {deadlines:dd.MM.yyyy}");
            return project;
        }
        #endregion
    }
}