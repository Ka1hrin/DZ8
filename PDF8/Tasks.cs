using System;
using System.Collections.Generic;

namespace PDF8
{
    internal class Task
    {
        private static int _count = 0;
        public int Id { get; }
        public Project Project { get; }
        private string _description;
        private DateTime _deadlines;
        public TeamLeader TeamLeader { get; }
        public Executor Executor { get; set; }
        public TaskStatus _status;
        public Report Report { get; set; }

        internal Task(Project project, string description, DateTime deadlines, TeamLeader teamLeader)
        {
            Id = ++_count;
            Project = project;
            Description = description;
            Deadlines = deadlines;
            TeamLeader = teamLeader;
            Executor = null;
            Report = null;
            Status = TaskStatus.Assigned;
        }

        
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

        public TaskStatus Status
        {
            get { return _status; }
            set
            {
                if (!Enum.IsDefined(typeof(TaskStatus), value))
                    throw new ArgumentException("Неизвестный статус!");

                _status = value;
            }
        }

        public void PrintInfo()
        {
            string executor = "Отсутствует";
            string report = "Отсутствует";

            if (Executor != null)
            {
                executor = Executor.Name;
            }

            if (Report != null)
            {
                report = Help.EnumName(Report.Status);
            }

            Console.WriteLine($"Проект: {Project}, Тимлидер: {TeamLeader}, Номер задачи: {Id}, Текст задачи: {_description}, Дедлайн: {_deadlines}, Исполнитель: {executor}, Статус отчета: {report}");
        }
    }
}