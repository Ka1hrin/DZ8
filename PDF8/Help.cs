using System;

namespace PDF8
{
    internal static class Help
    {
        internal static string EnumName(Post post)
        {
            return post switch
            {
                Post.Customer => "Заказчик",
                Post.Executor => "Исполнитель",
                Post.TeamLeader => "Лидер команды",
                _ => "Неизвестная должность"
            };
        }

        internal static string EnumName(ProjectStatus projectStatuc)
        {
            return projectStatuc switch
            {
                ProjectStatus.Project => "Проект",
                ProjectStatus.Execution => "Исполнение",
                ProjectStatus.Closed => "Закрыт",
                _ => "Неизвестный статус"
            };
        }

        internal static string EnumName(ReportStatus reportStatus)
        {
            return reportStatus switch
            {
                ReportStatus.Checked => "На проверке",
                ReportStatus.Approved => "Утвержден",
                ReportStatus.Refined => "На доработке",
                _ => "Неизвестный статус"
            };
        }

        internal static string EnumName(TaskStatus taskStatus)
        {
            return taskStatus switch
            {
                TaskStatus.Assigned => "Назначена",
                TaskStatus.Work => "В работе",
                TaskStatus.Check => "На проверке",
                TaskStatus.Completed => "Выполнена",
                _ => "Неизвестный статус"
            };
        }
    }
}