using System;
using System.Collections.Generic;
using System.Linq;

namespace GreenVsRed
{
    public class Program
    {
        public static void Main()
        {

            GridSize gameGridSize = GetGridSize();
            
            if (gameGridSize == null) 
            {
                return;
            }

            Generation generationZero = GetGenerationZero(gameGridSize);
            
            if (generationZero == null)
            {
                return;
            }

            GameInfo gameInfo = GetGameInfo(gameGridSize);
            
            if (gameInfo == null)
            {
                return;
            }

            PlayGame (gameGridSize, generationZero, gameInfo);

        }

        private static Generation GetNextGeneration(Generation generation)
        {
            Generation nextGeneration = new Generation();

            for (int i = 0; i < generation.Points.Count; i++)
            {
                GridPoint point = generation.Points[i];
                
                GridPoint newPoint = new GridPoint();

                newPoint.X = point.X;

                newPoint.Y = point.Y;

                newPoint.Value = GetNewPointValue(generation, point);

                nextGeneration.Points.Add(newPoint);
            }
            return nextGeneration;
        }

        private static int GetNewPointValue(Generation generation, GridPoint point)
        {
            List<GridPoint> neighborPoints = generation.Points
                .Where(p => (p.X <= point.X + 1) && (p.X >= point.X - 1))
                .Where(p => (p.Y <= point.Y + 1) && (p.Y >= point.Y - 1))
                .Where(p => !(p.X == point.X && p.Y == point.Y))
                .ToList();
            
            if (point.Value == 0) //red point
            {
                int greenPointsCount = neighborPoints.Count(p => p.Value == 1);
                
                if (greenPointsCount == 3 || greenPointsCount == 6)
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
            else //green point
            {
                int greenPointsCount = neighborPoints.Count(p => p.Value == 1);

                if (greenPointsCount == 2 || greenPointsCount == 3 || greenPointsCount == 6)
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
        }

        private static void PlayGame (GridSize gridSize, Generation generationZero, GameInfo gameInfo)
        {
            List<Generation> generations = new List<Generation>();
            
            generations.Add(generationZero);
            
            Generation currentGeneration = generationZero;
            
            for (int i = 0; i < gameInfo.N; i++)
            {
                currentGeneration = GetNextGeneration(currentGeneration);
                
                generations.Add(currentGeneration);

            }

            int coutGreenPointsBetweenGenerations = generations
                .SelectMany(g => g.Points)
                .Where(p => p.X == gameInfo.Point.X && p.Y == gameInfo.Point.Y)
                .Count(p => p.Value == 1);
            Console.WriteLine("Result: " + coutGreenPointsBetweenGenerations);
        }
        private static GameInfo GetGameInfo(GridSize gridSize)
        {
            Console.WriteLine("Enter coordinates X,Y,N");
            string userInput = Console.ReadLine();
            string[] inputParts = userInput.Split(",");

            if (inputParts.Length != 3)
            {
                Console.WriteLine("Incorrect parameters");
                return null;
            }

            GameInfo gameInfo = new GameInfo();

            for (int i = 0; i < inputParts.Length; i++)
            {
                int intValue;

                if (!int.TryParse(inputParts[i], out intValue))
                {
                    Console.WriteLine("Incorrect parameters " + inputParts[i]);
                    return null;
                }
    
                if (i == 1)
                {
                    if (intValue > gridSize.Rows)
                    {
                        Console.WriteLine("Incorrect parameter " + intValue);
                        return null;
                    }
                    gameInfo.Point.X = intValue;
                }

                if (i == 0)
                {
                    if (intValue > gridSize.Columns)
                    {
                        Console.WriteLine("Incorrect parameter " + intValue);
                        return null;
                    }
                    gameInfo.Point.Y = intValue;
                }

                if (i == 2)
                {
                    gameInfo.N = intValue;
                }
                
            }

            return gameInfo;
        }
        

        private static Generation GetGenerationZero(GridSize gridSize)
        {
            Console.WriteLine("Enter Generation Zero");

            Generation generationZero = new Generation();

            for (int x = 0; x < gridSize.Rows; x++)
            {
                string userInput = Console.ReadLine();

                if (userInput.Length != gridSize.Columns )
                {
                    Console.WriteLine("Incorrect columns parts");
                    return null;
                }

                for (int y = 0; y < userInput.Length; y++)
                {
                    int intValue;

                    if (!int.TryParse(userInput[y].ToString(), out intValue))
                    {
                        Console.WriteLine("Incorrect column value " + userInput[y]);
                        return null;
                    }

                    if (intValue < 0 && intValue > 1 )
                    {
                        Console.WriteLine("Incorrect column value " + userInput[y]);
                        return null;
                    }

                    GridPoint point = new GridPoint();
                    point.X = x;
                    point.Y = y;
                    point.Value = intValue;


                    generationZero.Points.Add(point);

                }

            }
            return generationZero;
        }
     

        private static GridSize GetGridSize()
        {
            Console.WriteLine("Enter rows and columns for the grid X,Y");
            string userInput = Console.ReadLine();
            string[] inputParts = userInput.Split(",");
            
            if (inputParts.Length != 2)
            {
                Console.WriteLine("Incorrect parameters");
                return null;
            }
            GridSize gameGridSize = new GridSize();

            for (int i = 0; i < inputParts.Length; i++)
            {
                int intValue;

                if (!int.TryParse(inputParts[i], out intValue))
                {
                    Console.WriteLine("Incorrect parameters " + inputParts[i]);
                    return null;
                }

                if (intValue >= 1000)
                {
                    Console.WriteLine("Incorrect parameters " + inputParts[i]);
                    return null;
                }

                if (i == 0)
                {
                    gameGridSize.Rows = intValue;
                }

                if (i == 1)
                {
                    gameGridSize.Columns = intValue;
                }
            }

            if (gameGridSize.Rows > gameGridSize.Columns)
            {
                Console.WriteLine("Rows cannot be more than columns");
                return null;
            }

            return gameGridSize;
        }
        
    }
}