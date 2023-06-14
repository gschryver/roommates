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
            ChoreRepository choreRepo = new ChoreRepository(CONNECTION_STRING);
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

                        if (room != null) { Console.WriteLine($"{room.Id} - {room.Name} Max Occupancy({room.MaxOccupancy})"); }

                        else { Console.WriteLine("No room found with that Id."); }
                           
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
                            Console.WriteLine($"{rm.FirstName} {rm.LastName} pays {rm.RentPortion}% of the rent. They live in {rm.Room.Name}.");
                        }
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Search for roommate"):
                        Console.Write("Roommate Id: ");
                        int roommateId = int.Parse(Console.ReadLine());

                        Roommate roommate = roommateRepo.GetById(roommateId);

                        if (roommate != null)
                        {
                            Console.WriteLine($"{roommate.FirstName} {roommate.LastName} pays {roommate.RentPortion}% of the rent and lives in {roommate.Room.Name}.");
                        }
                        else
                        {
                            Console.WriteLine("No roommate found with that Id.");
                        }
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
                    case ("Show all chores"):
                        List<Chore> chores = choreRepo.GetAll();
                        foreach (Chore c in chores)
                        {
                            Console.WriteLine($"{c.Id} - {c.Name}");
                        }
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Search for chore"):
                        Console.Write("Chore Id: ");
                        
                        int choreId = int.Parse(Console.ReadLine());
                        
                        Chore chore = choreRepo.GetById(choreId);

                        if (chore != null) { Console.WriteLine($"{chore.Id} - {chore.Name}"); }
                        else { Console.WriteLine("No chore found with that Id."); }
                        
               
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Add a chore"): 
                        Console.Write("Chore name: ");
                        string choreName = Console.ReadLine();

                        Chore choreToAdd = new Chore()
                        {
                            Name = choreName,
                        };

                        choreRepo.Insert(choreToAdd);

                        Console.WriteLine($"{choreToAdd.Name} has been added and assigned an Id of {choreToAdd.Id}");
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Delete a chore"): 
                        Console.Write("Chore Id: ");
                        int choreIdToDelete = int.Parse(Console.ReadLine());

                        choreRepo.Delete(choreIdToDelete);

                        Console.WriteLine($"Chore with Id {choreIdToDelete} has been deleted.");
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Assign a chore to a roommate"):
                        // Display all chores
                        List<Chore> choresList = choreRepo.GetAll();
                        foreach (Chore c in choresList)
                        {
                            Console.WriteLine($"{c.Id} - {c.Name}");
                        }

                        
                        Console.Write("Enter the Id of the chore you want to assign: ");
                        int choreIdToAssign = int.Parse(Console.ReadLine());

                        // Display all roommates
                        List<Roommate> roommatesList = roommateRepo.GetAll();
                        foreach (Roommate rm in roommatesList)
                        {
                            Console.WriteLine($"{rm.Id} - {rm.FirstName} {rm.LastName}");
                        }

                        // Prompt user to select a roommate
                        Console.Write("Enter the Id of the roommate you want to assign the chore to: ");
                        int roommateIdToAssign = int.Parse(Console.ReadLine());

                        // Assign the chore to the roommate
                        choreRepo.Assign(choreIdToAssign, roommateIdToAssign);

                        Console.WriteLine("The chore has been successfully assigned.");
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Chore Counts"):
                        Dictionary<string, int> choreCounts = choreRepo.GetChoreCountsByRoommate();

                        foreach (KeyValuePair<string, int> cc in choreCounts)
                        {
                            Console.WriteLine($"{cc.Key} - {cc.Value}");
                        }
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
                "Search for roommate",
                "Add a roommate",
                "Delete a roommate",
                "Show all chores",
                "Search for chore",
                "Add a chore",
                "Delete a chore",
                "Assign a chore to a roommate",
                "Chore Counts",
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