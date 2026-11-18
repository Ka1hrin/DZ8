using System;
using System.Collections.Generic;

namespace PDF8
{
    internal class PDF8
    {
        private static void Main()
        {
            Customer customer = new Customer("Иван Петров", Post.Customer);
            TeamLeader leader = new TeamLeader("Анна Смирнова", Post.TeamLeader);

            List<Executor> team = new List<Executor>
            {
                new Executor("Алексей"),
                new Executor("Мария"),
                new Executor("Дмитрий"),
                new Executor("Ольга"),
                new Executor("Сергей"),
                new Executor("Елена"),
                new Executor("Никита"),
                new Executor("Татьяна"),
                new Executor("Павел"),
                new Executor("Юлия")
            };

            Project project = customer.CreateProject("Интернет-магазин", DateTime.Now.AddMonths(3));
            leader.TakeProject(project);
            Console.WriteLine();

            List<Task> tasks = new List<Task>
            {
                new Task(project, "Спроектировать базу данных", DateTime.Now.AddMonths(2), leader),
                new Task(project, "Сверстать главную страницу", DateTime.Now.AddMonths(2), leader),
                new Task(project, "Сделать корзину", DateTime.Now.AddMonths(2), leader),
                new Task(project, "Подключить оплату", DateTime.Now.AddMonths(2), leader),
                new Task(project, "Сделать личный кабинет", DateTime.Now.AddMonths(2), leader),
                new Task(project, "Написать API каталога", DateTime.Now.AddMonths(2), leader),
                new Task(project, "Настроить сервер", DateTime.Now.AddMonths(2), leader),
                new Task(project, "Написать тесты", DateTime.Now.AddMonths(2), leader),
                new Task(project, "Сделать админ-панель", DateTime.Now.AddMonths(2), leader),
                new Task(project, "Написать документацию", DateTime.Now.AddMonths(2), leader),
            };

            project.Tasks.AddRange(tasks);

            Task dopTask1 = leader.CreateTask(project, "Сделать мобильное приложение", DateTime.Now.AddMonths(2));
            Task dopTask2 = leader.CreateTask(project, "Сделать страницу акций", DateTime.Now.AddMonths(2));
            Console.WriteLine();

            leader.StartProject(project);

            //Назначение исполнителей
            for (int i = 0; i < tasks.Count; i++)
                leader.GiveTask(tasks[i], team[i]);
            leader.GiveTask(dopTask1, team[1]);
            leader.GiveTask(dopTask2, team[0]);
            Console.WriteLine();

            team[1].DelegateTask(dopTask1, team[4]);
            team[4].RejectTask(dopTask1);
            leader.GiveTask(dopTask1, team[5]);
            team[0].RejectTask(dopTask2);
            leader.DeleteTask(dopTask2);

            foreach (Task task in project.Tasks)
            {
                if (task.Executor != null)
                    task.Executor.TakeTask(task);
            }

            foreach (Task task in project.Tasks)
            {
                if (task.Executor != null)
                    task.Executor.CreateReport(task, $"Задача '{task.Description}' выполнена");
            }

            Task badTask = tasks[3];
            leader.ReturnReport(badTask, "Не работает оплата картой");
            badTask.Executor.RewriteReport(badTask, "Оплата картой исправлена и проверена");

            foreach (Task task in project.Tasks)
            {
                    leader.ApproveReport(task);
            }
        }
    }
}

