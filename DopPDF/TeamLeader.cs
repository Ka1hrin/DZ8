using System;
using System.Collections.Generic;

namespace DopPDF
{
    internal class TeamLeader : Emploee
    {
        public TeamLeader(string name, Post post) : base(name, post) {}
        #region
        /// <summary>
        /// Проверка, что человек является тимлидером
        /// </summary>
        private void CheckLeader(Project project)
        {
            if (project == null)
                throw new ArgumentException("Проект отсутствует!");
            if (project.TeamLeader != this)
                throw new ArgumentException("Вы не являетесь руководителем проекта!");
        }

        /// <summary>
        /// Взять проект 
        /// </summary>
        public void TakeProject(Project project)
        {
            if (project == null)
                throw new ArgumentException("Проект отсутствует!");
            if (project.TeamLeader != null)
                throw new ArgumentException("У проекта уже есть тимлидер!");

            project.TeamLeader = this;
            Console.WriteLine($"Тимлид {Name} взял в работу проект. Описание проекта: '{project.Description}'");
        }

        /// <summary>
        /// Создать задачу в проект, который взял
        /// </summary>
        public Task CreateTask(Project project, string description, DateTime periodEnd, ReportFrequency frequency)
        {
            CheckLeader(project);

            if (project.Status != ProjectStatus.Project)
                throw new ArgumentException($"Нельзя создать задачу, т.к проект находится не в статусе '{Help.EnumName(project.Status)}'");
            if (periodEnd > project.Deadlines)
                throw new ArgumentException("Дедлайн задачи не может быть позже дедлайна проекта!");

            Task task = new Task(project, description, this, periodEnd, frequency);
            project.Tasks.Add(task);
            Console.WriteLine($"Создана задача {task.Id}: {description}");
            return task;
        }

        /// <summary>
        /// Назначить исполнителя на задачу
        /// </summary>
        public void GiveTask(Task task, Executor executor)
        {
            if (task == null)
                throw new ArgumentException("Задача отсутствует!");

            CheckLeader(task.Project);
            if (task.Project.Status == ProjectStatus.Closed)
                throw new ArgumentException("Проект закрыт!");
            if (task.Executor != null)
                throw new ArgumentException($"У задачи {task.Id} уже есть исполнитель {task.Executor.Name}!");

            task.Executor = executor;
            Console.WriteLine($"Задача {task.Id}: {task.Description} назначена исполнителю {executor.Name}");
        }

        /// <summary>
        /// Перевести проект в статус "Исполнение" (когда все задачи назначены)
        /// </summary>
        public void StartProject(Project project)
        {
            CheckLeader(project);

            if (project.Status != ProjectStatus.Project)
                throw new ArgumentException($"Начать можно только когда проект в статусе '{Help.EnumName(ProjectStatus.Project)}'!");
            if (project.Tasks.Count == 0)
                throw new ArgumentException("В проекте нет задач!");

            foreach (Task task in project.Tasks)
            {
                if (task.Executor == null)
                    throw new ArgumentException($"Нельзя начать работу: у задачи {task.Id} нет исполнителя!");
            }

            project.Status = ProjectStatus.Execution;
            Console.WriteLine($"Проект '{project.Description}' переведен в статус '{Help.EnumName(project.Status)}'");
        }

        /// <summary>
        /// Удалить задачу
        /// </summary>
        public void DeleteTask(Task task)
        {
            if (task == null)
                throw new ArgumentException("Задача отсутствует!");

            CheckLeader(task.Project);

            if (task.Executor != null)
                throw new ArgumentException("Нельзя удалить задачу, у которой есть исполнитель!");

            task.Project.Tasks.Remove(task);
            Console.WriteLine($"Задача {task.Id}: {task.Description} удалена из проекта");
            task.Project.ProjectClose();
        }
        #endregion

        /// <summary>
        /// Проверить отчет перед действием
        /// </summary>
        private void CheckReport(Task task)
        {
            if (task == null)
                throw new ArgumentException("Задача отсутствует!");
            if (task.TeamLeader != this)
                throw new ArgumentException("Проверять отчет может только тимлидер проекта!");
            if (task.Status != TaskStatus.Check || task.Reports == null)
                throw new ArgumentException($"По задаче {task.Id} нет отчета на проверке!");
        }

        /// <summary>
        /// Утвердить отчет по задаче
        /// </summary>
        public void ApproveReport(Task task, int Id)
        {
            CheckReport(task);

            Report report = null;

            foreach (Report item in task.Reports)
            {
                if (item.Id == Id)
                {
                    report = item;
                    break;
                }
            }

            if (report == null)
                throw new ArgumentException($"Отчет с ID {Id} не найден!");

            report.Status = ReportStatus.Approved;
            task.TaskClose();

            Console.WriteLine($"Отчет по задаче {task.Id} утвержден.");
            task.Project.ProjectClose();
        }

        /// <summary>
        /// Вернуть отчет на доработку
        /// </summary>
        public void ReturnReport(Task task, int Id, string comment)
        {
            CheckReport(task);

            task.Reports[Id - 1].Status = ReportStatus.Refined;
            task.Status = TaskStatus.Work;
            Console.WriteLine($"Отчет {Id} по задаче {task.Id} возвращен на доработку. Комментарий: {comment}");
        }
    }
}