﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Fedin.Nsudotnet.NumberGuesser
{
    class Program
    {

        static void Main(string[] args)
        {
            string[] humilations =
            {
                " HELLO, fucking asshole ", " You fucking idiot ", " You are retard ", " OMG, old fart ", " Pfff,You are virgin ",
                " WTF  bastard ",
            };
            var randomGenerator = new Random();
            for(;;)
            {
                Console.WriteLine("Enter your name!");
                var name = Console.ReadLine();
                Console.WriteLine("Welcome {0}! To quit enter 'q' ", name);
                Console.WriteLine("Enter your number, {0}", name);
                var startTime = DateTime.Now;
                var myNumber = randomGenerator.Next(0, 100);
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
                            TimeSpan interval = DateTime.Now.Subtract(startTime);
                            Console.WriteLine("You lost {0:c}", interval);
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
                            string phrase = humilations[randomGenerator.Next() % humilations.Length];
                            var pos = phrase.IndexOf(" ", randomGenerator.Next() % phrase.Length);
                            var builder = new StringBuilder(phrase);
                            if (pos == -1 || pos == 0)
                            {
                                pos = 0;
                                Console.WriteLine(builder.Insert(pos, $"{name},"));
                            }else if (pos == phrase.Length - 1)
                            {
                                Console.WriteLine(builder.Insert(pos, $", {name}"));
                            }
                            else
                            {
                                Console.WriteLine(builder.Insert(pos, $", {name},"));
                            }
                        }
                        if (number > myNumber)
                        {
                            Console.WriteLine("My number is less than yours");
                            history.Add($"{number} > {myNumber}");

                        }
                        else if (number < myNumber)
                        {
                            Console.WriteLine("My number is more than yours");
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
