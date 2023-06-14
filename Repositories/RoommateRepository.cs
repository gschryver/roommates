using Microsoft.Data.SqlClient;
using Roommates.Models;
using System.Collections.Generic;
using System;

namespace Roommates.Repositories
{
    public class RoommateRepository : BaseRepository
    {
        public RoommateRepository(string connectionString) : base(connectionString) { }

        public List<Roommate> GetAll()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                SELECT rm.Id, rm.FirstName, rm.LastName, rm.RentPortion, rm.MoveInDate, r.Id AS RoomId, r.Name AS RoomName
                FROM Roommate rm
                LEFT JOIN Room r ON rm.RoomId = r.Id";

                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Roommate> roommates = new List<Roommate>();

                    while (reader.Read())
                    {
                        Roommate roommate = new Roommate
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                            LastName = reader.GetString(reader.GetOrdinal("LastName")),
                            RentPortion = reader.GetInt32(reader.GetOrdinal("RentPortion")),
                            MoveInDate = reader.GetDateTime(reader.GetOrdinal("MoveInDate")),
                            Room = new Room
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("RoomId")),
                                Name = reader.GetString(reader.GetOrdinal("RoomName"))
                            }
                        };

                        roommates.Add(roommate);
                    }

                    reader.Close();
                    return roommates;
                }
            }
        }

        // GetById Method 
        public Roommate GetById(int id)
        {
            // 1. Open connection to database
            using (SqlConnection conn = Connection)
            {
                // 2. Create a SQL command
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    // 3. Set the command text
                    cmd.CommandText = @"
                        SELECT rm.Id, rm.FirstName, rm.LastName, rm.RentPortion, rm.MoveInDate, r.Id AS RoomId, r.Name AS RoomName, r.MaxOccupancy
                        FROM Roommate rm
                        LEFT JOIN Room r ON rm.RoomId = r.Id
                        WHERE rm.Id = @id
                    ";

                    // 4. Add parameters to command
                    cmd.Parameters.AddWithValue("@id", id);

                    // 5. Execute the command and read data
                    SqlDataReader reader = cmd.ExecuteReader();

                    // 6. Create a roommate object from
                    Roommate roommate = null;

                    if (reader.Read())
                    {
                        Room room = new Room
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("RoomId")),
                            Name = reader.GetString(reader.GetOrdinal("RoomName")),
                            MaxOccupancy = reader.GetInt32(reader.GetOrdinal("MaxOccupancy"))
                        };

                        roommate = new Roommate
                        {
                            Id = id,
                            FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                            LastName = reader.GetString(reader.GetOrdinal("LastName")),
                            RentPortion = reader.GetInt32(reader.GetOrdinal("RentPortion")),
                            MoveInDate = reader.IsDBNull(reader.GetOrdinal("MoveInDate"))
                            ? (DateTime?)null
                            : reader.GetDateTime(reader.GetOrdinal("MoveInDate")),
                            Room = room
                        };
                    }

                    reader.Close();
                    return roommate;
                }
            }
        }

        // GetRoommatesByRoomId Method
        public List<Roommate> GetRoommatesByRoomId(int roomId)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                SELECT r.Id, r.FirstName, r.LastName, r.RentPortion, rm.Id AS RoomId, rm.Name AS RoomName 
                FROM Roommate r
                JOIN Room rm ON r.RoomId = rm.Id";


                    cmd.Parameters.AddWithValue("@roomId", roomId);
               SqlDataReader reader = cmd.ExecuteReader();
               
                List<Roommate> roommates = new List<Roommate>();
                while (reader.Read())
                {
                    //Room room = new Room
                    //{
                    //    Id = reader.GetInt32(reader.GetOrdinal("RoomId")),
                    //    Name = reader.GetString(reader.GetOrdinal("RoomName")),
                    //    MaxOccupancy = reader.GetInt32(reader.GetOrdinal("MaxOccupancy"))
                    //};

                    Roommate roommate = new Roommate
                    {
                        Id = reader.GetInt32(reader.GetOrdinal("Id")),
                        FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                        LastName = reader.GetString(reader.GetOrdinal("LastName")),
                        RentPortion = reader.GetInt32(reader.GetOrdinal("RentPortion")),
                        MoveInDate = reader.GetDateTime(reader.GetOrdinal("MoveInDate")),
                        Room = new Room
                        { 
                            Id = reader.GetInt32(reader.GetOrdinal("RoomId")),
                            Name = reader.GetString(reader.GetOrdinal("RoomName"))
                         }
                    };

                    roommates.Add(roommate);
                }

                reader.Close();
                return roommates;
            }
        }
    }

        // Insert Method
        public void Insert(Roommate roommate)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                    INSERT INTO Roommate (FirstName, LastName, RentPortion, MoveInDate, RoomId)
                    OUTPUT INSERTED.Id
                    VALUES (@firstName, @lastName, @rentPortion, @moveInDate, @roomId)
                ";

                    cmd.Parameters.AddWithValue("@firstName", roommate.FirstName);
                    cmd.Parameters.AddWithValue("@lastName", roommate.LastName);
                    cmd.Parameters.AddWithValue("@rentPortion", roommate.RentPortion);
                    cmd.Parameters.AddWithValue("@moveInDate", roommate.MoveInDate);
                    cmd.Parameters.AddWithValue("@roomId", roommate.Room.Id);

                    int id = (int)cmd.ExecuteScalar();

                    roommate.Id = id;
                }
            }
        }

        // Update Method
        public void Update(Roommate roommate)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                    UPDATE Roommate
                    SET FirstName = @firstName, 
                        LastName = @lastName, 
                        RentPortion = @rentPortion, 
                        MoveInDate = @moveInDate, 
                        RoomId = @roomId
                    WHERE Id = @id
                ";

                    cmd.Parameters.AddWithValue("@firstName", roommate.FirstName);
                    cmd.Parameters.AddWithValue("@lastName", roommate.LastName);
                    cmd.Parameters.AddWithValue("@rentPortion", roommate.RentPortion);
                    cmd.Parameters.AddWithValue("@moveInDate", roommate.MoveInDate);
                    cmd.Parameters.AddWithValue("@roomId", roommate.Room.Id);
                    cmd.Parameters.AddWithValue("@id", roommate.Id);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        // Delete Method
        public void Delete(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "DELETE FROM Roommate WHERE Id = @id";

                    cmd.Parameters.AddWithValue("@id", id);

                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}