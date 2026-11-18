using System;

namespace PDF8
{
    internal class Report
    {
        private string _description;
        private DateTime _completionDate;
        public Emploee Executor { get; private set; }
        private ReportStatus _status;

        public Report(string description, Emploee executor)
        {
            Description = description;
            CompletionDate = DateTime.Now;
            Executor = executor;
            Status = ReportStatus.Checked;
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

        public DateTime CompletionDate
        {
            get { return _completionDate; }
            set
            {
                if (value >= DateTime.Now)
                    throw new ArgumentException("Дата выполнения не может быть позже сегоднешнего дня!");

                _completionDate = value;
            }
        }

        public ReportStatus Status
        {
            get { return _status; }
            set
            {
                if (!Enum.IsDefined(typeof(ReportStatus), value))
                    throw new ArgumentException("Неизвестный статус!");

                _status = value;
            }
        }

        public void PrintInfo()
        {
            Console.WriteLine($"Текст отчета: {_description}, дата выполнения: {_completionDate: dd.MM.yyyy}, исполнитель: {Executor}, статус: {Help.EnumName(_status)}");
        }
    }
}