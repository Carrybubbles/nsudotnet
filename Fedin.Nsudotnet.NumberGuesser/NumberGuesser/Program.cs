﻿using System;
using System.Collections.Generic;

namespace Fedin.Nsudotnet.NumberGuesser
{
    class Program
    {

        static void Main(string[] args)
        {
            string[] humilations = {
                "asshole", "idiot", "retard"
            };
            for(;;)
            {
                Console.WriteLine("Enter your name!");
                var name = Console.ReadLine();
                Console.WriteLine("Welcome {0}! To quit enter 'q' ", name);
                Console.WriteLine("Enter your number, {0}", name);
                var startTime = DateTime.Now;
                var myNumber = new Random().Next(0, 100);
                var tries = 1;
                var history = new List<string>();
                for (;;)
                {
                    var answer = Console.ReadLine();
                    if (answer != null && answer.ToLower().Equals("q"))
                    {
                        Console.WriteLine("Sorry, my dear friend! Bye!");
                        return;
                    }

                    int number;
                    if (int.TryParse(answer, out number))
                    {
                        if (number == myNumber)
                        {
                            history.Add($"{number} == {myNumber}");
                            Console.WriteLine("Congratulations! You guessed my number! ");
                            Console.WriteLine("Your tries = {0}", tries);
                            TimeSpan interval = DateTime.Now - startTime;
                            Console.WriteLine("You lost {0} minutes", interval.Minutes);
                            Console.WriteLine("Your history: ");
                            history.ForEach(Console.WriteLine);                           
                            Console.WriteLine("Do you want to continue? y/n ");
                            for (;;)
                            {
                                var opt = Console.ReadLine();
                                if (opt != null && opt.ToLower().Equals("y"))
                                {
                                    break;
                                }
                                if (opt != null && opt.ToLower().Equals("n"))
                                {
                                    Console.WriteLine("Bye");
                                    return;
                                }
                                Console.WriteLine("What? Write again!");
                            }
                            break;
                        }
                        if (0 == tries % 4)
                        {
                            Console.WriteLine("You fucking {0}, {1}", humilations[new Random().Next() % 3], name);
                        }
                        if (number > myNumber)
                        {
                            Console.WriteLine("My number is less");
                            history.Add($"{number} > {myNumber}");

                        }
                        else if (number < myNumber)
                        {
                            Console.WriteLine("My number is greater");
                            history.Add($"{number} < {myNumber}");
                        }
                        tries++;
                    }
                    else
                    {
                        Console.WriteLine("WTF?! Enter NUMBER! Try again");
                    }
                }
            }
        }
    }
}