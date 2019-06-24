using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;
using ShopOnWheels.Models;
using System.Drawing;
using System.Runtime.InteropServices;

namespace ShopOnWheels
{
    class Program
    {
        public static HubConnection connection;
        public static List<Box> Boxes { get; set; }
        static void Main(string[] args)
        {
            MainAsync(args);
        }

        public static async void MainAsync(string[] args)
        {
            connection = new HubConnectionBuilder()
                .WithUrl("https://localhost:5001/chat")
                .Build();

            connection.StartAsync().Wait();

            InitEvents();

            connection.InvokeAsync("GetBoxes").Wait();

            connection.Closed += async (error) =>
            {
                await Task.Delay(new Random().Next(0, 5) * 1000);
                await connection.StartAsync();
            };


            Thread.Sleep(1000);

            while (true)
            {
                Console.WriteLine("What do you want: ");
                Console.WriteLine("[0]: Working with Box");
                Console.WriteLine("[1]: List of boxes");
                Console.WriteLine("[2]: Clear console");

                Console.Write("Enter action: ");
                var action = int.Parse(Console.ReadLine());

                Console.WriteLine();

                switch (action)
                {
                    case 0:
                        ChooseWorkingWithBox();
                        break;
                    case 1:
                        WriteList();
                        break;
                    case 2:
                        Console.Clear();
                        continue;
                }

                Console.WriteLine();
            }

        }

        public static void InitEvents()
        {
            connection.On<string>("GetBoxes", (res) =>
            {
                var boxes = JsonConvert.DeserializeObject<List<Box>>(res);

                Boxes = boxes;
            });

            connection.On<string>("DataSent", (param) =>
            {
                Console.WriteLine("Data was sent");
            });

            connection.On<string>("BoxUpdate", (res) =>
            {
                var boxes = JsonConvert.DeserializeObject<List<Box>>(res);

                Boxes = boxes;

                Console.WriteLine("Boxes were successesfully updated");
            });

            connection.On<string>("AddedBoxes", (res) =>
            {
                var boxes = JsonConvert.DeserializeObject<List<Box>>(res);

                Boxes = boxes;

                Console.WriteLine("Boxes were successesfully added");
            });
        }

        public static void WriteProduct(int index)
        {
            Console.WriteLine("{0}) Product: {1} with weight {2} and Id: {3}", index, Boxes[index].Product.Name, Boxes[index].Weight, Boxes[index].Order.Id);
        }

        public static void ChooseBox(int index)
        {
            
            WriteProduct(index);
        }

        public static void Eat(int index, int weight)
        {
            if (Boxes[index].Weight >= weight)
            {
                Boxes[index].Weight -= weight;
            }
            else
            {
                throw new Exception();
            }
        }

        public static void ChooseWorkingWithBox()
        {
            Console.Write("Choose box: ");
            var index = int.Parse(Console.ReadLine());
            Console.Write("Box: ");
            ChooseBox(index);
            Console.WriteLine();

            Console.Write("How much to eat: ");
            var weight = int.Parse(Console.ReadLine());

            try
            {
                Eat(index, weight);
                Console.WriteLine();
                WriteProduct(index);
               
                connection.InvokeAsync("EatFromBox", JsonConvert.SerializeObject(Boxes[index])).Wait();
                Thread.Sleep(1000);
            }
            catch
            {
                Console.WriteLine("Run of product");
            }
        }

        public static void WriteList()
        {
            for (int i = 0; i < Boxes.Count; i++)
            {
                WriteProduct(i);
            }
        }
    }
}
