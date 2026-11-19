using System;
using System.Collections.Generic;

namespace DopPDF
{
    internal class Task
    {
        private static int _count = 0;
        public int Id { get; }
        public Project Project { get; }
        private string _description;
        public TeamLeader TeamLeader { get; }
        public Executor Executor { get; set; }
        public TaskStatus _status;
        public List<Report> Reports { get; set; }
        public DateTime PeriodStart { get; set; }
        private DateTime _periodEnd;
        private ReportFrequency _frequency;


        internal Task(Project project, string description, TeamLeader teamLeader, DateTime periodEnd, ReportFrequency frequency)
        {
            Id = ++_count;
            Project = project;
            Description = description;
            TeamLeader = teamLeader;
            Executor = null;
            Reports = new List<Report>();
            Status = TaskStatus.Assigned;
            PeriodStart = DateTime.Now;
            PeriodEnd = periodEnd;
            Frequency = frequency;
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
        #endregion
        public DateTime PeriodEnd
        {
            get { return _periodEnd; }
            set
            {
                if (value > Project.Deadlines)
                    throw new ArgumentException("Дедлайн задачи не может быть после дедлайна проекта!");

                _periodEnd = value;
            }
        }

        public ReportFrequency Frequency
        {
            get { return _frequency; }
            set
            {
                if (!Enum.IsDefined(typeof(ReportFrequency), value))
                    throw new ArgumentException("Неизвестная периодичность!");

                _frequency = value;
            }
        }
        /// <summary>
        /// Срок задачи 
        /// </summary>
        private int DaysCount()
        {
            return (_periodEnd - PeriodStart).Days;

        }

        /// <summary>
        /// Посчитать, сколько отчетов должен сделать сотрудник (неполная неделя или месяц считать +1)
        /// </summary>
        public int ReportsCount()
        {
            switch (_frequency)
            {
                case ReportFrequency.Daily: return DaysCount();
                case ReportFrequency.Weekly: return (int)Math.Ceiling(DaysCount() / 7.0);
                case ReportFrequency.Monthly: return (int)Math.Ceiling(DaysCount() / 30.0);
                default: throw new ArgumentException("Неизвестное количество отчетов!");
            }
        }

        /// <summary>
        /// Закрыть задачу, если по ней выполнены все отчеты
        /// </summary>
        public void TaskClose()
        {
            if (_status != TaskStatus.Check || Reports.Count == 0)
                return;

            foreach (var report in Reports)
            {
                if (report.Status != ReportStatus.Approved)
                    return;
            }

            _status = TaskStatus.Completed;
            Console.WriteLine($"Все отчеты выполнены. Задача '{_description}' переведен в статус '{Help.EnumName(_status)}'");

        }
    }
}