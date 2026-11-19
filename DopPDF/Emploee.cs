using System;

namespace DopPDF
{
    internal abstract class Emploee
    {
        private string _name;
        private Post _post;

        public Emploee(string name, Post post) 
        { 
            Name = name;
            Post = post;
        }
        #region
        public string Name
        {
            get { return _name; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Строка не может быть пустой!");
                _name = value;
            }
        }

        public Post Post
        {
            get { return _post; }
            set
            {
                if (!Enum.IsDefined(typeof(Post), value))
                    throw new ArgumentException("Неизвестная должность!");
                _post = value;
            }
        }

        /// <summary>
        /// Вывести информацию о сотруднике
        /// </summary>
        public void PrintInfo()
        {
            Console.WriteLine($"Имя сотрудника: {_name}, Должность: {_post}");
        }
        #endregion
    }
}