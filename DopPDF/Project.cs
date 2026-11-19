using System;
using System.Collections.Generic;

namespace DopPDF
{
    internal class Project
    {
        private static int _count = 0;
        public int Id { get; }
        private string _description;
        private DateTime _deadlines;
        private Customer _customer;
        public TeamLeader TeamLeader { get; set; }
        public List<Task> Tasks { get; set; }
        public ProjectStatus _status;

        internal Project(string description, DateTime deadlines, Customer customer)
        {
            Id = ++_count;
            Description = description;
            Deadlines = deadlines;
            Customer = customer;
            TeamLeader = null;
            Tasks = new List<Task>();
            Status = ProjectStatus.Project;
        }
        #region
        public string Description
        {
            get { return _description; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Строка не может быть пустой!");
                _description = value;
            }
        }

        public DateTime Deadlines
        {
            get { return _deadlines; }
            set
            {
                if (value <= DateTime.Now)
                    throw new ArgumentException("Дедлайн не может быть раньше, чем дата выдачи!");

                _deadlines = value;
            }
        }

        public Customer Customer
        {
            get { return _customer; }
            set
            {
                if (value == null)
                    throw new ArgumentException("Инициатор отсутствует!");

                _customer = value;
            }
        }

        public ProjectStatus Status
        {
            get { return _status; }
            set
            {
                if (!Enum.IsDefined(typeof(ProjectStatus), value))
                    throw new ArgumentException("Неизвестный статус!");

                _status = value;
            }
        }

        /// <summary>
        /// Закрыть проект, если по нему выполнены все задачи
        /// </summary>
        public void ProjectClose()
        {
            if (_status != ProjectStatus.Execution || Tasks.Count == 0)
                return;

            foreach (Task task in Tasks)
            {
                if (task.Status != TaskStatus.Completed)
                    return;
            }

            _status = ProjectStatus.Closed;
            Console.WriteLine($"Все задачи выполнены. Проект '{_description}' переведен в статус '{Help.EnumName(_status)}'");
        }

        public void PrintInfo()
        {
            string teamLeader = "";
            if (TeamLeader != null)
                teamLeader = TeamLeader.Name;
            Console.WriteLine($"Заказчик: {Customer}, Номер проекта: {Id}, текст: {_description}, дедлайн: {_deadlines}, Тимлидер: {teamLeader}, Статус: {Help.EnumName(_status)}");
        }
        #endregion
    }
}