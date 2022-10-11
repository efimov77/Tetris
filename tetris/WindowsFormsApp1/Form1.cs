using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tetris.Controllers;

namespace Tetris
{
    public partial class Form1 : Form
    {


        string playerName;

        public Form1()
        {
            InitializeComponent();//построение пользовательского интерфейса
            if (!File.Exists(RecordsController.recordPath))
            
            File.Create(RecordsController.recordPath);
            
            playerName = Microsoft.VisualBasic.Interaction.InputBox("Введите имя игрока", "Настройка игрока", "Новый игрок");
            if (playerName == "")
            {
                playerName = "Новый игрок";
            }
            
            this.KeyUp += new KeyEventHandler(keyFunc);//добавление обработчика клавишей
            Init();
        }

        public void Init()//функция для инициализации переменных 
        {
            RecordsController.ShowRecords(label3);
            this.Text = "Тетрис: Текущий игрок - " + playerName;
            MapController.size = 25;//размер клетки
            MapController.score = 0;//обновили
            MapController.linesRemoved = 0;//обновили
            MapController.currentShape = new Shape(3, 0);//иницилизируем с помощью конструктора
            MapController.Interval = 300;//скорость падения фигуры
            label1.Text = "Score: " + MapController.score;//добавление очков
            label2.Text = "Lines: " + MapController.linesRemoved;//добавление линий



            timer1.Interval = MapController.Interval;
            timer1.Tick += new EventHandler(update);
            timer1.Start();


            Invalidate();
        }

        private void keyFunc(object sender, KeyEventArgs e)//обработчик клавиатуры
        {
            switch (e.KeyCode)//проверяет какая нажата кнопка
            {
                case Keys.A:

                    if (!MapController.IsIntersects())//переворачивает фигуру
                    {
                        MapController.ResetArea();
                        MapController.currentShape.RotateShape();
                        MapController.Merge();
                        Invalidate();
                    }
                    break;
                case Keys.Space://движется вниз
                    timer1.Interval = 10;//ускоренное падение фигуры
                    break;
                case Keys.Right://вправо
                    if (!MapController.CollideHor(1))//проверка
                    {
                        MapController.ResetArea();//перед этим очистить координаты где она находится
                        MapController.currentShape.MoveRight();//вызываем метод Moveright
                        MapController.Merge();//cинхронизация фугуры с картой
                        Invalidate();// для того чтобы каждый кадр отрисовыввался на холст
                    }
                    break;
                case Keys.Left://влево
                    if (!MapController.CollideHor(-1))
                    {
                        MapController.ResetArea();
                        MapController.currentShape.MoveLeft();//вызываем метод MoveLeft
                        MapController.Merge();
                        Invalidate();
                    }
                    break;
            }
        }


        private void update(object sender, EventArgs e)//статическое падение фигуры вниз
        {
            MapController.ResetArea();
            if (!MapController.Collide())
            {
                MapController.currentShape.MoveDown();//двигаем фигуру вниз
            }
            else
            {
                MapController.Merge();
                MapController.SliceMap(label1, label2);
                timer1.Interval = MapController.Interval;//восстановление таймера
                MapController.currentShape.ResetShape(3, 0);
                if (MapController.Collide())//если проиграл,то выводится результат
                {
                    RecordsController.SaveRecords(playerName);
                    MapController.ClearMap();
                    timer1.Tick -= new EventHandler(update);
                    timer1.Stop();
                    MessageBox.Show("Ваш результат: " + MapController.score);
                    Init();
                }
            }
            MapController.Merge();
            Invalidate();// для того чтобы каждый кадр отрисовыввался на холст
        }

        private void OnPaint(object sender, PaintEventArgs e)//интерфейс игры
        {
            MapController.DrawGrid(e.Graphics);
            MapController.DrawMap(e.Graphics);
            MapController.ShowNextShape(e.Graphics);
        }

        private void OnPauseButtonClick(object sender, EventArgs e)
        {
            var pressedButton = sender as ToolStripMenuItem;
            if (timer1.Enabled)
            {
                pressedButton.Text = "Продолжить";
                timer1.Stop();
            }
            else
            {
                pressedButton.Text = "Пауза";
                timer1.Start();
            }
        }

        private void OnAgainButtonClick(object sender, EventArgs e)
        {
            timer1.Tick -= new EventHandler(update);
            timer1.Stop();
            MapController.ClearMap();
            Init();
        }


        private void OnInfoPressed(object sender, EventArgs e)
        {
            string infoString = "";
            infoString = "Для управление фигурами используйте стрелочку влево/вправо.\n";
            infoString += "Чтобы ускорить падение фигуры - нажмите 'Пробел'.\n";
            infoString += "Для поворота фигуры используйте 'A'.\n";
            MessageBox.Show(infoString, "Справка");
        }


    }
}

