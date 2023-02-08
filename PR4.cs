using System;
using System.Collections.Generic;
using System.Linq;


namespace ConsoleSharpProb
{

    struct Note
    {
        internal readonly string name;
        internal readonly string descrtiption;
        internal readonly DateTime completionDate;

        internal Note(string name, string descrtiption, DateTime completionDate) =>
            (this.name, this.descrtiption, this.completionDate) =
            (name, descrtiption, completionDate);

        internal void Show()
        {
            Console.Clear();
            Console.WriteLine(
                name + '\n' +
                "-------------------------------\n" +
                "Описание: " + descrtiption + '\n' +
                "Дата: " + completionDate);
        }
        
    }

    class CalendarDate
    {
        private readonly DateTime _date;
        private readonly List<Note> _notes = new List<Note>();

        internal int NotesLenght { get => _notes.Count; }

        internal CalendarDate(DateTime date) =>
            _date = date;

        internal void AddNote(string name, string descrtiption) =>
            _notes.Add(new Note(name, descrtiption, _date));

        internal void ShowNote(int index) =>
            _notes[index].Show();

        internal void Show(int arrowPos)
        {
            Console.Clear();
            Console.WriteLine($"Выбрана дата: {_date}");
            int i = 0;
            foreach (Note note in _notes)
            {
                if (i == arrowPos)
                    Console.Write("->");
                else
                    Console.Write("  ");

                Console.Write(i.ToString() + '.' + note.name + '\n');
                i++;
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Random rnd = new Random();
            List<(string, string)> notesData = new List<(string, string)>()
            {
                ("Сходить в универ","С 1-й по 5-ю"),
                ("Прибраться в гараже","1-я и 2-я полки"),
                ("Вынести муор","Тот что в моей комнате"),
                ("Проведать деда","Не забудь бутылку"),
                ("Поспать","Надеюсь я не разучился")
            };

            List<CalendarDate> dates = new List<CalendarDate>(3);
            //Заполняем рандомными датами
            for (int i = 0; i < 3; i++)
                dates.Add(new CalendarDate(
                    new DateTime(2023, 1 + rnd.Next() % 12, 1+rnd.Next()%26)));

            //Заполняем даты заметками (первые три даты первыми тремя записками, остальные 2 заметки приписываються рандомным датам)
            for (int i = 0; i < 5; i++)
                dates[i<3?i:rnd.Next() % 3].AddNote(notesData[i].Item1, notesData[i].Item2);

            //Изначальные позиции даты и стрелочки
            int arrow_pos = -1, date_pos = 1; 
            while (true)
            {
                CalendarDate curr_date = dates[date_pos];
                curr_date.Show(arrow_pos); //Отображаем дату с заметками

                //Действие пользователя
                switch(Console.ReadKey().Key)
                {
                    case ConsoleKey.LeftArrow:
                        if (date_pos - 1 < 0)
                            date_pos = dates.Count - 1;
                        else
                            date_pos--;
                        break;
                    case ConsoleKey.RightArrow:
                        if (date_pos + 1 >= dates.Count)
                            date_pos = 0;
                        else
                            date_pos++;
                        break;
                    case ConsoleKey.UpArrow:
                        if (arrow_pos - 1 < 0)
                            arrow_pos = 0;
                        else
                            arrow_pos--;
                        break;
                    case ConsoleKey.DownArrow:
                        if (arrow_pos + 1 >= curr_date.NotesLenght)
                            arrow_pos = curr_date.NotesLenght - 1;
                        else
                            arrow_pos++;
                        break;
                    case ConsoleKey.Enter:
                        curr_date.ShowNote(arrow_pos);
                        WaitForEsc();
                        break;
                    case ConsoleKey.Escape: 
                        return; 
                }
                
            }
        }

        //Метод ожидания нажатия клавиши Esc
        static void WaitForEsc()
        {
            while(true)
                if (Console.ReadKey().Key == ConsoleKey.Escape)
                    return;            
        }
    }
}
