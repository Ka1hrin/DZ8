using System;
using static System.Net.Mime.MediaTypeNames;

namespace PDF8
{
    internal class Executor : Emploee
    {
        public Executor(string name) : base(name, Post.Executor) { }

        /// <summary>
        /// Проверка, что задача дана именно этому исполнителю
        /// </summary>
        private void CheckOwner(Task task)
        {
            if (task == null)
                throw new ArgumentException("Задача отсутствует!");
            if (task.Executor != this)
                throw new ArgumentException("Вы не являетесь исполнителем задачи!");
        }

        /// <summary>
        /// Взять задачу в работу
        /// </summary>
        public void TakeTask(Task task)
        {
            CheckOwner(task);
            if (task.Project.Status != ProjectStatus.Execution)
                throw new ArgumentException($"Брать задачи в работу можно только когда проект в статусе '{Help.EnumName(ProjectStatus.Execution)}'!");
            if (task.Status != TaskStatus.Assigned)
                throw new ArgumentException($"Задачу {task.Id} нельзя взять в работу, ее статус '{Help.EnumName(task.Status)}'");

            task.Status = TaskStatus.Work;
        }

        /// <summary>
        /// Делегировать задачу
        /// </summary>
        public void DelegateTask(Task task, Executor other)
        {
            if (task == null)
                throw new ArgumentException("Задача отсутствует!");
            if (other == this)
                throw new ArgumentException("Вы не можете делегировать задачу себе!");
            if (task.Status != TaskStatus.Assigned)
                throw new ArgumentException($"Делегировать можно только задачу в статусе '{Help.EnumName(TaskStatus.Assigned)}'!");

            task.Executor = other;
            Console.WriteLine($"{Name} делегировал задачу {task.Id}: '{task.Description}' исполнителю {other.Name}");
        }

        /// <summary>
        /// Отклонить задачу
        /// </summary>
        public void RejectTask(Task task)
        {
            CheckOwner(task);
            if (task.Status != TaskStatus.Assigned)
                throw new ArgumentException($"Отклонить задачу можно только в статусе '{Help.EnumName(TaskStatus.Assigned)}'!");

            task.Executor = null;
            Console.WriteLine($"Задача {task.Id}: {task.Description} была отклонена сотрудником: {Name}");
        }

        /// <summary>
        /// Отправить отчет на проверку
        /// </summary>
        public void CreateReport(Task task, string description)
        {
            CheckOwner(task);
            if (task.Status != TaskStatus.Work)
                throw new ArgumentException($"Отчет можно создать только когда задача в статусе: '{Help.EnumName(TaskStatus.Assigned)}'");
            if (task.Report != null)
                throw new ArgumentException("Отчет уже создан!");

            task.Report = new Report(description, this);
            task.Status = TaskStatus.Check;

            Console.WriteLine($"{Name} отпраил отчет по задаче {task.Id} на проверку {task.TeamLeader.Name}");
        }

        /// <summary>
        /// Доработать отчет
        /// </summary>
        public void RewriteReport(Task task, string description)
        {
            CheckOwner(task);
            if (task.Report == null || task.Report.Status != ReportStatus.Refined)
                throw new ArgumentException($"Отчет по задаче {task.Id} не был отправлен на доработку!");

            task.Report.Description = description;
            task.Report.CompletionDate = DateTime.Now;
            task.Report.Status = ReportStatus.Checked;
            task.Status = TaskStatus.Check;
            Console.WriteLine($"{Name} доработал отчет по задаче {task.Id} и снова отправил на проверку!");
        }
    }
}