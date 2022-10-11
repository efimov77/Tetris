using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tetris.Controllers
{
    public static class MapController
    {
        public static Shape currentShape;
        public static int size;
        public static int[,] map = new int[16, 8];//размер сетки
        public static int linesRemoved;//кол-во убранных линий
        public static int score;//очки
        public static int Interval;//скорость падения фигуры
        public static void ShowNextShape(Graphics e)//метод отрисовывает след фигуру
        {
            for (int i = 0; i < currentShape.sizeNextMatrix; i++)//пробегаемся по массиву след фигуры
            {
                for (int j = 0; j < currentShape.sizeNextMatrix; j++)
                {
                    if (currentShape.nextMatrix[i, j] == 1)//если значение след матрицы ненулевое,то отрисовываем
                    {
                        e.FillRectangle(Brushes.Red, new Rectangle(300 + j * (size) + 1, 50 + i * (size) + 1, size - 1, size - 1));
                    }
                    if (currentShape.nextMatrix[i, j] == 2)
                    {
                        e.FillRectangle(Brushes.Yellow, new Rectangle(300 + j * (size) + 1, 50 + i * (size) + 1, size - 1, size - 1));
                    }
                    if (currentShape.nextMatrix[i, j] == 3)
                    {
                        e.FillRectangle(Brushes.Green, new Rectangle(300 + j * (size) + 1, 50 + i * (size) + 1, size - 1, size - 1));
                    }
                    if (currentShape.nextMatrix[i, j] == 4)
                    {
                        e.FillRectangle(Brushes.Blue, new Rectangle(300 + j * (size) + 1, 50 + i * (size) + 1, size - 1, size - 1));
                    }
                    if (currentShape.nextMatrix[i, j] == 5)
                    {
                        e.FillRectangle(Brushes.Purple, new Rectangle(300 + j * (size) + 1, 50 + i * (size) + 1, size - 1, size - 1));
                    }
                }
            }
        }

        public static void ClearMap()//очищает карту
        {
            for (int i = 0; i < 16; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    map[i, j] = 0;
                }
            }
        }

        public static void DrawMap(Graphics e)//функция отрисовывает цвета фигур на карте
        {
            for (int i = 0; i < 16; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (map[i, j] == 1)
                    {
                        e.FillRectangle(Brushes.Red, new Rectangle(50 + j * (size) + 1, 50 + i * (size) + 1, size - 1, size - 1));
                    }
                    if (map[i, j] == 2)
                    {
                        e.FillRectangle(Brushes.Yellow, new Rectangle(50 + j * (size) + 1, 50 + i * (size) + 1, size - 1, size - 1));
                    }
                    if (map[i, j] == 3)
                    {
                        e.FillRectangle(Brushes.Green, new Rectangle(50 + j * (size) + 1, 50 + i * (size) + 1, size - 1, size - 1));
                    }
                    if (map[i, j] == 4)
                    {
                        e.FillRectangle(Brushes.Blue, new Rectangle(50 + j * (size) + 1, 50 + i * (size) + 1, size - 1, size - 1));
                    }
                    if (map[i, j] == 5)
                    {
                        e.FillRectangle(Brushes.Purple, new Rectangle(50 + j * (size) + 1, 50 + i * (size) + 1, size - 1, size - 1));
                    }
                }
            }
        }

        public static void DrawGrid(Graphics g)//функция отрисовки сетки
        {
            for (int i = 0; i <= 16; i++)
            {
                g.DrawLine(Pens.Black, new Point(50, 50 + i * size), new Point(50 + 8 * size, 50 + i * size));//горизонтальные линии сетки(изменяем координату по у)
            }
            for (int i = 0; i <= 8; i++)
            {
                g.DrawLine(Pens.Black, new Point(50 + i * size, 50), new Point(50 + i * size, 50 + 16 * size));//вертикальные линии сетки(изменяем координату по х) 
            }
        }

        public static void SliceMap(Label label1, Label label2)//метод проверяет заполнена ли линия
        {
            int count = 0;
            int curRemovedLines = 0;//текущие убранные линии
            for (int i = 0; i < 16; i++)
            {
                count = 0;//кол-во непустых ячеек вначале обнуляем
                for (int j = 0; j < 8; j++)
                {
                    if (map[i, j] != 0)//если не пустое плюсуем
                        count++;//увеличиваем
                }
                if (count == 8)
                {
                    curRemovedLines++;//добавляем 1 в сожженые линии
                    for (int k = i; k >= 1; k--)//смещение карты вниз
                    {
                        for (int o = 0; o < 8; o++)
                        {
                            map[k, o] = map[k - 1, o];//сдвиг на единицу
                        }
                    }
                }
            }
            for (int i = 0; i < curRemovedLines; i++)
            {
                score += 10 * (i + 1);//увеличиваем кол-во очков 
            }
            linesRemoved += curRemovedLines;//добавляем 1 в общие сожженые линии

            if (linesRemoved % 5 == 0)//если кол-во линий кратно 5 вычитаем из интервала 10
            {
                if (Interval > 60)//предельное значение ниже которого опуститься не может
                    Interval -= 10;
            }

            label1.Text = "Score: " + score;//изменим текст очков
            label2.Text = "Lines: " + linesRemoved;//изменим кол-во сожженых линий
        }

        public static bool IsIntersects()//метод проверки на поворот
        {
            for (int i = currentShape.y; i < currentShape.y + currentShape.sizeMatrix; i++)
            {
                for (int j = currentShape.x; j < currentShape.x + currentShape.sizeMatrix; j++)
                {//предотвращает выхождение за сетку карты
                    if (j >= 0 && j <= 7)//помогает предусмотреть наложение двух фигур друг на друга
                    {
                        if (map[i, j] != 0 && currentShape.matrix[i - currentShape.y, j - currentShape.x] == 0)//проверяем есть ли фигуры
                            return true;
                    }
                }
            }
            return false;
        }

        public static void Merge()//cинхронизация фугуры с картой
        {
            for (int i = currentShape.y; i < currentShape.y + currentShape.sizeMatrix; i++)//проходим по всему двумерному массиву
            {
                for (int j = currentShape.x; j < currentShape.x + currentShape.sizeMatrix; j++)
                {
                    if (currentShape.matrix[i - currentShape.y, j - currentShape.x] != 0)//проверяем, чтобы матрица не была пустой
                        map[i, j] = currentShape.matrix[i - currentShape.y, j - currentShape.x];
                }
            }
        }

        public static bool Collide()//проверяет не заходим ли мы за границы сетки
        {
            for (int i = currentShape.y + currentShape.sizeMatrix - 1; i >= currentShape.y; i--)
            {
                for (int j = currentShape.x; j < currentShape.x + currentShape.sizeMatrix; j++)
                {
                    if (currentShape.matrix[i - currentShape.y, j - currentShape.x] != 0)//проверка на горизонтальную составляющую
                    {
                        if (i + 1 == 16)//смотрим влево
                            return true;
                        if (map[i + 1, j] != 0)//смотрим вправо
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public static bool CollideHor(int dir)//проверяет, есть ли фигуры
        {
            for (int i = currentShape.y; i < currentShape.y + currentShape.sizeMatrix; i++)
            {
                for (int j = currentShape.x; j < currentShape.x + currentShape.sizeMatrix; j++)
                {
                    if (currentShape.matrix[i - currentShape.y, j - currentShape.x] != 0)
                    {
                        if (j + 1 * dir > 7 || j + 1 * dir < 0)//если есть выход из границы возвращаем тру
                            return true;

                        if (map[i, j + 1 * dir] != 0)
                        {
                            if (j - currentShape.x + 1 * dir >= currentShape.sizeMatrix || j - currentShape.x + 1 * dir < 0)
                            {
                                return true;//есть ли какое-то значение справа или слева
                            }
                            if (currentShape.matrix[i - currentShape.y, j - currentShape.x + 1 * dir] == 0)
                                return true;
                        }
                    }//значит мы сталкиваемся с чем-то
                }
            }
            return false;
        }

        public static void ResetArea()//отчищает кусок карты на котором находится фигура
        {
            for (int i = currentShape.y; i < currentShape.y + currentShape.sizeMatrix; i++)
            {
                for (int j = currentShape.x; j < currentShape.x + currentShape.sizeMatrix; j++)
                {
                    if (i >= 0 && j >= 0 && i < 16 && j < 8)
                    {
                        if (currentShape.matrix[i - currentShape.y, j - currentShape.x] != 0)
                        {
                            map[i, j] = 0;
                        }
                    }
                }
            }
        }

    }
}
