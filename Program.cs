using Roommates.Models;
using Roommates.Repositories;
using System;
using System.Collections.Generic;

namespace Roommates
{
    class Program
    {
        //  This is the address of the database.
        //  We define it here as a constant since it will never change.
        private const string CONNECTION_STRING = @"server=localhost\SQLExpress;database=Roommates;integrated security=true;TrustServerCertificate=True;";

        static void Main(string[] args)
        {
            RoommateRepository roommateRepo = new RoommateRepository(CONNECTION_STRING);
            RoomRepository roomRepo = new RoomRepository(CONNECTION_STRING);
            bool runProgram = true;
            while (runProgram)
            {
                string selection = GetMenuSelection();

                switch (selection)
                {
                    case ("Show all rooms"):
                        List<Room> rooms = roomRepo.GetAll();
                        foreach (Room r in rooms)
                        {
                            Console.WriteLine($"{r.Name} has an Id of {r.Id} and a max occupancy of {r.MaxOccupancy}");
                        }
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Search for room"):
                        Console.Write("Room Id: ");
                        int id = int.Parse(Console.ReadLine());

                        Room room = roomRepo.GetById(id);

                        Console.WriteLine($"{room.Id} - {room.Name} Max Occupancy({room.MaxOccupancy})");
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Add a room"):
                        Console.Write("Room name: ");
                        string name = Console.ReadLine();

                        Console.Write("Max occupancy: ");
                        int max = int.Parse(Console.ReadLine());

                        Room roomToAdd = new Room()
                        {
                            Name = name,
                            MaxOccupancy = max
                        };

                        roomRepo.Insert(roomToAdd);

                        Console.WriteLine($"{roomToAdd.Name} has been added and assigned an Id of {roomToAdd.Id}");
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Show all roommates"):
                        List<Roommate> roommates = roommateRepo.GetAll();
                        foreach (Roommate rm in roommates)
                        {
                            Console.WriteLine($"{rm.FirstName} {rm.LastName} pays {rm.RentPortion}% of the rent.");
                        }
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Search for roommate"):
                        Console.Write("Roommate Id: ");
                        int roommateId = int.Parse(Console.ReadLine());

                        Roommate roommate = roommateRepo.GetById(roommateId);

                        Console.WriteLine($"{roommate.FirstName} {roommate.LastName} pays {roommate.RentPortion}% of the rent and lives in {roommate.Room.Name}.");
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Add a roommate"):
                        Console.Write("First name: ");
                        string firstName = Console.ReadLine();

                        Console.Write("Last name: ");
                        string lastName = Console.ReadLine();

                        Console.Write("Rent portion (in %): ");
                        int rentPortion = int.Parse(Console.ReadLine());

                        Console.Write("Room Id: ");
                        int roomId = int.Parse(Console.ReadLine());

                        Roommate roommateToAdd = new Roommate()
                        {
                            FirstName = firstName,
                            LastName = lastName,
                            RentPortion = rentPortion,
                            Room = roomRepo.GetById(roomId),
                            MoveInDate = DateTime.Now
                        };

                        roommateRepo.Insert(roommateToAdd);

                        Console.WriteLine($"{roommateToAdd.FirstName} {roommateToAdd.LastName} has been added and assigned an Id of {roommateToAdd.Id}");
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case("Delete a roommate"):
                        Console.Write("Roommate Id: ");
                        int roommateIdToDelete = int.Parse(Console.ReadLine());

                        roommateRepo.Delete(roommateIdToDelete);

                        Console.WriteLine($"Roommate with Id {roommateIdToDelete} has been deleted.");
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Exit"):
                        runProgram = false;
                        break;
                }
            }

        }

        static string GetMenuSelection()
        {
            Console.Clear();

            List<string> options = new List<string>()
            {
                "Show all rooms",
                "Search for room",
                "Add a room",
                "Show all roommates",
                "Add a roommate",
                "Search for roommate",
                "Delete a roommate",
                "Exit"
            };

            for (int i = 0; i < options.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {options[i]}");
            }

            while (true)
            {
                try
                {
                    Console.WriteLine();
                    Console.Write("Select an option > ");

                    string input = Console.ReadLine();
                    int index = int.Parse(input) - 1;
                    return options[index];
                }
                catch (Exception)
                {

                    continue;
                }
            }
        }
    }
}