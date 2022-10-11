using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    public class Shape// координаты x и y отвечают за позицию фигуры на карте
    {
        public int x;
        public int y;
        public int[,] matrix;//создается двумерный массив, где будет храниться матрица
        public int[,] nextMatrix;//показ след фигуры
        public int sizeMatrix;//отвечает за размер матрицы
        public int sizeNextMatrix;

        public int[,] tetr1 = new int[4, 4]{//цифра отвечает за отрисовку цвета
            {0,0,1,0  },
            {0,0,1,0  },
            {0,0,1,0  },
            {0,0,1,0  },
        };

        public int[,] tetr2 = new int[3, 3]{
            {0,2,0  },
            {0,2,2 },
            {0,0,2 },
        };

        public int[,] tetr3 = new int[3, 3]{
            {0,0,0  },
            {3,3,3 },
            {0,3,0 },
        };

        public int[,] tetr4 = new int[3, 3]{
            { 4,0,0  },
            {4,0,0 },
            {4,4,0 },
        };
        public int[,] tetr5 = new int[2, 2]{
            { 5,5  },
            {5,5 },
        };


        public Shape(int _x, int _y)//конструктор
        {
            x = _x;
            y = _y;
            matrix = GenerateMatrix();//вызываем метод, чтобы сгенерировалась новая фигура
            sizeMatrix = (int)Math.Sqrt(matrix.Length);//текущая матрица
            nextMatrix = GenerateMatrix();//след матрица
            sizeNextMatrix = (int)Math.Sqrt(nextMatrix.Length);//размер след матрицы
        }

        public void ResetShape(int _x, int _y)//сбрасывает все значения
        {
            x = _x;
            y = _y;
            matrix = nextMatrix;//присваивает текущей матрицы значение след матрицы
            sizeMatrix = (int)Math.Sqrt(matrix.Length);
            nextMatrix = GenerateMatrix();//присваиваем рандомное
            sizeNextMatrix = (int)Math.Sqrt(nextMatrix.Length);
        }

        public int[,] GenerateMatrix()//генерирует матрицу после того, как текущая матрица приземлилась
        {
            int[,] _matrix = tetr1;
            Random r = new Random();
            switch (r.Next(1, 6))//рандомное значение в диапазоне
            {
                case 1:
                    _matrix = tetr1;//в зависимости от цифры выбираем фигуру
                    break;
                case 2:
                    _matrix = tetr2;
                    break;
                case 3:
                    _matrix = tetr3;
                    break;
                case 4:
                    _matrix = tetr4;
                    break;
                case 5:
                    _matrix = tetr5;
                    break;
            }
            return _matrix;
        }

        public void RotateShape()//метод поворота фигуры
        {
            int[,] tempMatrix = new int[sizeMatrix, sizeMatrix];
            for (int i = 0; i < sizeMatrix; i++)
            {
                for (int j = 0; j < sizeMatrix; j++)
                {
                    tempMatrix[i, j] = matrix[j, (sizeMatrix - 1) - i];//изменяем значения на противоположные
                }
            }
            matrix = tempMatrix;
            int offset1 = (8 - (x + sizeMatrix));
            if (offset1 < 0)
            {//если фигура находится в правой части, то при повороте смещаемся влево
                for (int i = 0; i < Math.Abs(offset1); i++)
                    MoveLeft();
            }

            if (x < 0)//если фигура находится в левой части, то при повороте смещаемся вправо
            {
                for (int i = 0; i < Math.Abs(x) + 1; i++)//Abs возвращает абсолютное значение
                    MoveRight();
            }

        }

        public void MoveDown()
        {
            y++;
        }
        public void MoveRight()
        {
            x++;
        }
        public void MoveLeft()
        {
            x--;
        }
    }
}
