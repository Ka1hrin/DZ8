namespace DopPDF
{
    internal class PDF
    {
        private static void Main()
        {
            Customer customer = new Customer("Иван Петров", Post.Customer);
            TeamLeader leader = new TeamLeader("Анна Смирнова", Post.TeamLeader);

            List<Executor> team = new List<Executor>
            {
                new Executor("Алексей"),
                new Executor("Мария"),
                new Executor("Дмитрий")
            };

            Project project = customer.CreateProject("Интернет-магазин", DateTime.Now.AddMonths(1));
            leader.TakeProject(project);
            Console.WriteLine();

            List<Task> tasks = new List<Task>
            {
                new Task(project, "Спроектировать базу данных", leader, DateTime.Now.AddDays(5), ReportFrequency.Daily),
                new Task(project, "Сделать личный кабинет", leader, DateTime.Now.AddDays(10), ReportFrequency.Weekly),
                new Task(project, "Подключить оплату", leader, DateTime.Now.AddDays(29), ReportFrequency.Monthly)
            };

            project.Tasks.AddRange(tasks);

            // Назначение исполнителей
            for (int i = 0; i < tasks.Count; i++)
                leader.GiveTask(tasks[i], team[i]);

            Console.WriteLine();

            // Запуск проекта
            leader.StartProject(project);

            foreach (Task task in project.Tasks)
            {
                if (task.Executor != null)
                    task.Executor.TakeTask(task);
            }

            // Создание отчетов
            for (int i = 0; i < tasks.Count; i++)
            {
                int reportsCount = tasks[i].ReportsCount();

                for (int j = 0; j < reportsCount; j++)
                {
                    tasks[i].Executor.CreateReport(tasks[i], $"Отчет номер {j + 1}");
                }
            }

            // Проверка отчетов
            for (int i = 0; i < tasks.Count; i++)
            {
                for (int j = 0; j < tasks[i].Reports.Count; j++)
                {
                    leader.ApproveReport(tasks[i], tasks[i].Reports[j].Id);
                }
            }
        }
    }
}
