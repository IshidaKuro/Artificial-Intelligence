using System;
using System.CodeDom;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;


namespace Dijkstra_Algorithm
{
    class Program
    {

        static void Main(string[] args)
        {
            int numberOfPoints;
            List<int> input = new List<int>();
            string solution = " ";
            List<Point> points = new List<Point>();
            List<Point> visitedPoints = new List<Point>();
            List<Point>unvisitedPoints = new List<Point>();
            string connectedPoints = "";
            string file;
            string inputString;
            int inputInt;
            double lowestTentative = double.MaxValue;
            string output;
            string[] outputStrings = {"", ""};
            

            //seperate the command from the output of the program
            Console.WriteLine(">");

            //get the .cav file
            {
                //set the working directory to where the program is
                string directory = System.IO.Directory.GetCurrentDirectory() + @"\";


                //get the name of the input file
                if (args.Length != 0)
                {
                    if (args[0].Contains(".cav"))
                    {
                        file = args[0];
                      
                    }
                    else
                    {
                        file = args[0] + ".cav";
                       
                    }
                }
                else
                {
                    Console.WriteLine("please designate a file to run");
                    return;
                }

                

                //check to see that it exists
                if (System.IO.File.Exists(directory + file))
                {
                    //if it does, read the file, otherwise, return an error
                    Console.WriteLine("File : " + directory + file + " has been found.");
                    inputString = System.IO.File.ReadAllText(directory + file);

                }
                else
                {
                    Console.WriteLine("the file " + directory + file + " does not exist");
                    return;
                }

                output = directory + args[0] + ".csn";
                
            }

            //gather information about the points in the .cav file
            {
                String[] numbers = inputString.Split(',');

                for (int i = 0; i < numbers.Length; i++)
                {
                    inputInt = Convert.ToInt32(numbers[i]);
                    input.Add(inputInt);
                }

                numberOfPoints = input [0];
                Console.WriteLine("The file has " + numberOfPoints + " Caves.");


                for (int i = 0; i < numberOfPoints; i++)
                {
                    Point p = new Point();
                    p.point = i + 1;
                    p.x = input[1 + (2 * i)];
                    p.y = input[2 + (2 * i)];


                    points.Add(p);
                }
                //connect the point to each point it is connected to

                for (int i = 0; i < numberOfPoints; i++)
                {
                    for (int j = 0; j < numberOfPoints; j++)
                    {
                        int connection =input[2 * numberOfPoints + 1 + j + i * numberOfPoints];

                        if (connection == 1)
                        {
                            points[j].connections.Add(points[i]);
                        }
                    }
                }
                //display the points that have been loaded and the points they are connected to
                foreach (Point p in points)
                {
                   
                    foreach (Point x in p.connections)
                    {
                        connectedPoints += x.point + ",";
                       
                    }
                    //Console.WriteLine("point " + p.point + "is connected to " + connectedPoints);
                    connectedPoints = "";
                }
            }

            //path finding logic
            {

                double distance;
                double distance2;
                Point startPoint = points[0];
                unvisitedPoints = points;
                //set the tentative distance to 0 for the initial node
                startPoint.tentativeDistance = 0;

                //set the initial node as the current node
                Point currentPoint = startPoint;
                Point nextPoint = currentPoint;
                Point previousPoint = currentPoint;
                Point endPoint = points[numberOfPoints - 1];

                while (unvisitedPoints.Contains(endPoint))
                {
                   
                
                    //for the current node, consider all of it's unvisited neighbours and calculate their tentitive distance through the current node
                    foreach (Point p in currentPoint.connections)
                    {

                       
                        

                        if (unvisitedPoints.Contains(p))
                        {

                            //find the point that is closest

                            distance = currentPoint.tentativeDistance + Math.Sqrt(Math.Pow(p.x - currentPoint.x, 2) + Math.Pow(p.y - currentPoint.y, 2)); 

                            if (p.tentativeDistance > distance)
                            {
                                //assign the smaller one
                                p.tentativeDistance = distance;
                                p.previousPoint = currentPoint;
                                

                            }
                            foreach (Point q in p.connections)
                            {
                                //Console.WriteLine("computing vertex " + p.point + "-" + q.point);
                                if (unvisitedPoints.Contains(q))
                                {
                                    distance2 = Math.Sqrt(Math.Pow(q.x - p.x, 2) + Math.Pow(q.y - p.y, 2)) + p.tentativeDistance;

                                    //compare the newly calculated tentative distance to the current assigned value
                                    if (q.tentativeDistance > distance2)
                                    {
                                        //assign the smaller one
                                        q.tentativeDistance = distance2;
                                        q.previousPoint = p;
                                        
                                    }
                                }
                              
                            }

                        }
                        
                        
                    }
                    
                    //mark the current node as visited
                    visitedPoints.Add(currentPoint);
                    unvisitedPoints.Remove(currentPoint);
                    //find the unvisited point with the lowest tentative distance
                    foreach (Point p in unvisitedPoints)
                    {
                        
                        if (p.tentativeDistance < lowestTentative)
                        {                   
                            lowestTentative = p.tentativeDistance;
                            nextPoint = p;                            
                        }
                        
                    }
                    if (lowestTentative == double.MaxValue) // if the lowesttentative distance is infinite, there are no more points connected to the start point
                    {
                        nextPoint = endPoint; // go to the end point to end the loop
                    }
                    currentPoint = nextPoint;
                    lowestTentative = double.MaxValue;
                }
                //use the previous points to reconstruct the route from start to end
                if (endPoint.previousPoint != null)
                {
                    previousPoint = endPoint;
                    while (previousPoint != null)
                    {
                        solution= solution.Insert(0,previousPoint.point + " ");
                        previousPoint = previousPoint.previousPoint;
                    }
                    
                }
                else
                {
                    solution = "0";
                }

                outputStrings[0] = solution;
                Console.WriteLine(solution);
                if (solution != "0")
                {
                    outputStrings[1]=("Distance : "+Math.Round(endPoint.tentativeDistance, 2));
                }
                

              
                    System.IO.File.WriteAllLines(output, outputStrings);
                
            }
        }
    }
}
