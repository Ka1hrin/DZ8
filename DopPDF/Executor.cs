using System;

namespace DopPDF
{
    internal class Executor : Emploee
    {
        public Executor(string name) : base(name, Post.Executor) { }
        #region
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
        #endregion
        /// <summary>
        /// Отправить отчет на проверку
        /// </summary>
       
        public void CreateReport(Task task, string description)
        {
            CheckOwner(task);
            if (task.Status != TaskStatus.Work)
                throw new ArgumentException($"Отчет можно создать только когда задача в статусе: '{Help.EnumName(TaskStatus.Assigned)}'");
            if (task.Reports.Count >= task.ReportsCount())
                throw new ArgumentException("Вы привысили количество необходимых отчетов!");

            Report report = new Report(description, this);
            task.Reports.Add(report);
            report.Status = ReportStatus.Checked;

            if (task.Reports.Count == task.ReportsCount())
                task.Status = TaskStatus.Check;

            Console.WriteLine($"{Name} отпраил отчет {task.Reports[task.Reports.Count - 1].Id} по задаче {task.Id} на проверку {task.TeamLeader.Name}");
        }

        /// <summary>
        /// Доработать отчет
        /// </summary>
        public void RewriteReport(Task task, int Id, string description)
        {
            CheckOwner(task);

            Report report = task.Reports[Id-1];

            if (report == null)
                throw new ArgumentException($"Отчет с ID {Id} не найден!");

            if (report.Status != ReportStatus.Refined)
                throw new ArgumentException("Этот отчет не отправлялся на доработку!");

            report.Description = description;
            report.CompletionDate = DateTime.Now;
            report.Status = ReportStatus.Checked;

            Console.WriteLine($"{Name} доработал отчет {Id} по задаче {task.Id} и отправил на проверку!");
        }
    }
}