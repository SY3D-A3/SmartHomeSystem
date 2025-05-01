using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using SmartHomeSystem.Models;

namespace SmartHomeSystem.Services
{
    /// <summary>
    /// Service for handling database operations
    /// </summary>
    public class DatabaseService
    {
        private readonly string _connectionString;

        public DatabaseService(string connectionString)
        {
            _connectionString = connectionString;
        }

        /// <summary>
        /// Initializes the database by creating necessary tables if they don't exist
        /// </summary>
        public void InitializeDatabase()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    Console.WriteLine("Connected to database successfully.");

                    // Create Users table
                    string createUsersTable = @"
                    IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Users')
                    BEGIN
                        CREATE TABLE Users (
                            UserID INT PRIMARY KEY,
                            Name NVARCHAR(100) NOT NULL,
                            Role NVARCHAR(50) NOT NULL
                        );
                        PRINT 'Users table created.';
                    END";

                    // Create Rooms table
                    string createRoomsTable = @"
                    IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Rooms')
                    BEGIN
                        CREATE TABLE Rooms (
                            RoomID INT PRIMARY KEY,
                            Name NVARCHAR(100) NOT NULL
                        );
                        PRINT 'Rooms table created.';
                    END";

                    // Create Devices table
                    string createDevicesTable = @"
                    IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Devices')
                    BEGIN
                        CREATE TABLE Devices (
                            DeviceID INT PRIMARY KEY,
                            Type NVARCHAR(50) NOT NULL,
                            Status BIT NOT NULL,
                            Location NVARCHAR(100) NOT NULL,
                            RoomID INT,
                            FOREIGN KEY (RoomID) REFERENCES Rooms(RoomID)
                        );
                        PRINT 'Devices table created.';
                    END";

                    // Create DeviceProperties table for device-specific properties
                    string createDevicePropertiesTable = @"
                    IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'DeviceProperties')
                    BEGIN
                        CREATE TABLE DeviceProperties (
                            PropertyID INT PRIMARY KEY IDENTITY(1,1),
                            DeviceID INT NOT NULL,
                            PropertyName NVARCHAR(50) NOT NULL,
                            PropertyValue NVARCHAR(MAX) NOT NULL,
                            FOREIGN KEY (DeviceID) REFERENCES Devices(DeviceID)
                        );
                        PRINT 'DeviceProperties table created.';
                    END";

                    // Create Sensors table
                    string createSensorsTable = @"
                    IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Sensors')
                    BEGIN
                        CREATE TABLE Sensors (
                            SensorID INT PRIMARY KEY,
                            Type NVARCHAR(50) NOT NULL,
                            Location NVARCHAR(100) NOT NULL,
                            IsActive BIT NOT NULL,
                            LastReadingTime DATETIME,
                            RoomID INT,
                            FOREIGN KEY (RoomID) REFERENCES Rooms(RoomID)
                        );
                        PRINT 'Sensors table created.';
                    END";

                    // Create SensorReadings table
                    string createSensorReadingsTable = @"
                    IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'SensorReadings')
                    BEGIN
                        CREATE TABLE SensorReadings (
                            ReadingID INT PRIMARY KEY IDENTITY(1,1),
                            SensorID INT NOT NULL,
                            ReadingValue NVARCHAR(MAX) NOT NULL,
                            ReadingTime DATETIME NOT NULL,
                            FOREIGN KEY (SensorID) REFERENCES Sensors(SensorID)
                        );
                        PRINT 'SensorReadings table created.';
                    END";

                    // Create Schedules table
                    string createSchedulesTable = @"
                    IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Schedules')
                    BEGIN
                        CREATE TABLE Schedules (
                            ScheduleID INT PRIMARY KEY,
                            StartTime DATETIME NOT NULL,
                            EndTime DATETIME NOT NULL,
                            Action NVARCHAR(100) NOT NULL,
                            DeviceID INT NOT NULL,
                            IsRecurring BIT NOT NULL,
                            RecurrencePattern NVARCHAR(100),
                            FOREIGN KEY (DeviceID) REFERENCES Devices(DeviceID)
                        );
                        PRINT 'Schedules table created.';
                    END";

                    // Execute all create table commands
                    using (SqlCommand command = new SqlCommand())
                    {
                        command.Connection = connection;
                        
                        command.CommandText = createUsersTable;
                        command.ExecuteNonQuery();
                        
                        command.CommandText = createRoomsTable;
                        command.ExecuteNonQuery();
                        
                        command.CommandText = createDevicesTable;
                        command.ExecuteNonQuery();
                        
                        command.CommandText = createDevicePropertiesTable;
                        command.ExecuteNonQuery();
                        
                        command.CommandText = createSensorsTable;
                        command.ExecuteNonQuery();
                        
                        command.CommandText = createSensorReadingsTable;
                        command.ExecuteNonQuery();
                        
                        command.CommandText = createSchedulesTable;
                        command.ExecuteNonQuery();
                    }

                    Console.WriteLine("Database initialization completed successfully.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error initializing database: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
                }
            }
        }

        #region User Methods

        /// <summary>
        /// Adds a user to the database
        /// </summary>
        /// <param name="user">The user to add</param>
        public void AddUser(User user)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    
                    string query = "INSERT INTO Users (UserID, Name, Role) VALUES (@UserID, @Name, @Role)";
                    
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@UserID", user.UserID);
                        command.Parameters.AddWithValue("@Name", user.Name);
                        command.Parameters.AddWithValue("@Role", user.Role);
                        
                        int rowsAffected = command.ExecuteNonQuery();
                        Console.WriteLine($"User added to database. Rows affected: {rowsAffected}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding user: {ex.Message}");
            }
        }

        /// <summary>
        /// Gets a user from the database by ID
        /// </summary>
        /// <param name="userID">The ID of the user to get</param>
        /// <returns>The user if found, null otherwise</returns>
        public User GetUser(int userID)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    
                    string query = "SELECT UserID, Name, Role FROM Users WHERE UserID = @UserID";
                    
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@UserID", userID);
                        
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new User(
                                    reader.GetInt32(0),
                                    reader.GetString(1),
                                    reader.GetString(2)
                                );
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting user: {ex.Message}");
            }
            
            return null;
        }

        /// <summary>
        /// Gets all users from the database
        /// </summary>
        /// <returns>A list of all users</returns>
        public List<User> GetAllUsers()
        {
            List<User> users = new List<User>();
            
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    
                    string query = "SELECT UserID, Name, Role FROM Users";
                    
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                users.Add(new User(
                                    reader.GetInt32(0),
                                    reader.GetString(1),
                                    reader.GetString(2)
                                ));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting users: {ex.Message}");
            }
            
            return users;
        }

        #endregion

        #region Room Methods

        /// <summary>
        /// Adds a room to the database
        /// </summary>
        /// <param name="room">The room to add</param>
        public void AddRoom(Room room)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    
                    string query = "INSERT INTO Rooms (RoomID, Name) VALUES (@RoomID, @Name)";
                    
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@RoomID", room.RoomID);
                        command.Parameters.AddWithValue("@Name", room.Name);
                        
                        int rowsAffected = command.ExecuteNonQuery();
                        Console.WriteLine($"Room added to database. Rows affected: {rowsAffected}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding room: {ex.Message}");
            }
        }

        /// <summary>
        /// Gets a room from the database by ID
        /// </summary>
        /// <param name="roomID">The ID of the room to get</param>
        /// <returns>The room if found, null otherwise</returns>
        public Room GetRoom(int roomID)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    
                    string query = "SELECT RoomID, Name FROM Rooms WHERE RoomID = @RoomID";
                    
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@RoomID", roomID);
                        
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new Room(
                                    reader.GetInt32(0),
                                    reader.GetString(1)
                                );
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting room: {ex.Message}");
            }
            
            return null;
        }

        /// <summary>
        /// Gets all rooms from the database
        /// </summary>
        /// <returns>A list of all rooms</returns>
        public List<Room> GetAllRooms()
        {
            List<Room> rooms = new List<Room>();
            
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    
                    string query = "SELECT RoomID, Name FROM Rooms";
                    
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                rooms.Add(new Room(
                                    reader.GetInt32(0),
                                    reader.GetString(1)
                                ));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting rooms: {ex.Message}");
            }
            
            return rooms;
        }

        #endregion

        #region Device Methods

        /// <summary>
        /// Adds a device to the database
        /// </summary>
        /// <param name="device">The device to add</param>
        /// <param name="roomID">The ID of the room the device is in</param>
        public void AddDevice(SmartDevice device, int roomID)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    
                    // Start a transaction to ensure all operations succeed or fail together
                    using (SqlTransaction transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            // Insert into Devices table
                            string deviceQuery = @"
                                INSERT INTO Devices (DeviceID, Type, Status, Location, RoomID) 
                                VALUES (@DeviceID, @Type, @Status, @Location, @RoomID)";
                            
                            using (SqlCommand command = new SqlCommand(deviceQuery, connection, transaction))
                            {
                                command.Parameters.AddWithValue("@DeviceID", device.DeviceID);
                                command.Parameters.AddWithValue("@Type", device.GetType().Name);
                                command.Parameters.AddWithValue("@Status", device.Status);
                                command.Parameters.AddWithValue("@Location", device.Location);
                                command.Parameters.AddWithValue("@RoomID", roomID);
                                
                                command.ExecuteNonQuery();
                            }
                            
                            // Add device-specific properties
                            if (device is Light light)
                            {
                                AddDeviceProperty(connection, transaction, device.DeviceID, "Brightness", light.Brightness.ToString());
                                AddDeviceProperty(connection, transaction, device.DeviceID, "Color", light.Color);
                            }
                            else if (device is Thermostat thermostat)
                            {
                                AddDeviceProperty(connection, transaction, device.DeviceID, "CurrentTemperature", thermostat.CurrentTemperature.ToString());
                                AddDeviceProperty(connection, transaction, device.DeviceID, "TargetTemperature", thermostat.TargetTemperature.ToString());
                                AddDeviceProperty(connection, transaction, device.DeviceID, "Mode", thermostat.Mode);
                            }
                            else if (device is SecurityCamera camera)
                            {
                                AddDeviceProperty(connection, transaction, device.DeviceID, "IsRecording", camera.IsRecording.ToString());
                                AddDeviceProperty(connection, transaction, device.DeviceID, "MotionDetectionEnabled", camera.MotionDetectionEnabled.ToString());
                                AddDeviceProperty(connection, transaction, device.DeviceID, "Resolution", camera.Resolution.ToString());
                            }
                            
                            // Commit the transaction
                            transaction.Commit();
                            Console.WriteLine($"Device {device.DeviceID} added to database.");
                        }
                        catch (Exception ex)
                        {
                            // Roll back the transaction if an error occurs
                            transaction.Rollback();
                            throw new Exception($"Error in transaction: {ex.Message}", ex);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding device: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
                }
            }
        }

        /// <summary>
        /// Helper method to add a device property
        /// </summary>
        private void AddDeviceProperty(SqlConnection connection, SqlTransaction transaction, int deviceID, string propertyName, string propertyValue)
        {
            string query = @"
                INSERT INTO DeviceProperties (DeviceID, PropertyName, PropertyValue) 
                VALUES (@DeviceID, @PropertyName, @PropertyValue)";
            
            using (SqlCommand command = new SqlCommand(query, connection, transaction))
            {
                command.Parameters.AddWithValue("@DeviceID", deviceID);
                command.Parameters.AddWithValue("@PropertyName", propertyName);
                command.Parameters.AddWithValue("@PropertyValue", propertyValue);
                
                command.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Updates a device's status in the database
        /// </summary>
        /// <param name="deviceID">The ID of the device to update</param>
        /// <param name="status">The new status</param>
        public void UpdateDeviceStatus(int deviceID, bool status)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    
                    string query = "UPDATE Devices SET Status = @Status WHERE DeviceID = @DeviceID";
                    
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@DeviceID", deviceID);
                        command.Parameters.AddWithValue("@Status", status);
                        
                        int rowsAffected = command.ExecuteNonQuery();
                        Console.WriteLine($"Device status updated. Rows affected: {rowsAffected}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating device status: {ex.Message}");
            }
        }

        /// <summary>
        /// Updates a device property in the database
        /// </summary>
        /// <param name="deviceID">The ID of the device</param>
        /// <param name="propertyName">The name of the property</param>
        /// <param name="propertyValue">The new value</param>
        public void UpdateDeviceProperty(int deviceID, string propertyName, string propertyValue)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    
                    string query = @"
                        UPDATE DeviceProperties 
                        SET PropertyValue = @PropertyValue 
                        WHERE DeviceID = @DeviceID AND PropertyName = @PropertyName";
                    
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@DeviceID", deviceID);
                        command.Parameters.AddWithValue("@PropertyName", propertyName);
                        command.Parameters.AddWithValue("@PropertyValue", propertyValue);
                        
                        int rowsAffected = command.ExecuteNonQuery();
                        Console.WriteLine($"Device property updated. Rows affected: {rowsAffected}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating device property: {ex.Message}");
            }
        }

        #endregion

        #region Sensor Methods

        /// <summary>
        /// Adds a sensor to the database
        /// </summary>
        /// <param name="sensor">The sensor to add</param>
        /// <param name="roomID">The ID of the room the sensor is in</param>
        public void AddSensor(Sensor sensor, int roomID)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    
                    string query = @"
                        INSERT INTO Sensors (SensorID, Type, Location, IsActive, LastReadingTime, RoomID) 
                        VALUES (@SensorID, @Type, @Location, @IsActive, @LastReadingTime, @RoomID)";
                    
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@SensorID", sensor.SensorID);
                        command.Parameters.AddWithValue("@Type", sensor.GetType().Name);
                        command.Parameters.AddWithValue("@Location", sensor.Location);
                        command.Parameters.AddWithValue("@IsActive", sensor.IsActive);
                        command.Parameters.AddWithValue("@LastReadingTime", sensor.GetTimeSinceLastReading() == TimeSpan.MaxValue ? 
                            DBNull.Value : (object)DateTime.Now.Subtract(sensor.GetTimeSinceLastReading()));
                        command.Parameters.AddWithValue("@RoomID", roomID);
                        
                        int rowsAffected = command.ExecuteNonQuery();
                        Console.WriteLine($"Sensor added to database. Rows affected: {rowsAffected}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding sensor: {ex.Message}");
            }
        }

        /// <summary>
        /// Records a sensor reading in the database
        /// </summary>
        /// <param name="sensorID">The ID of the sensor</param>
        /// <param name="readingValue">The reading value</param>
        public void RecordSensorReading(int sensorID, string readingValue)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    
                    string query = @"
                        INSERT INTO SensorReadings (SensorID, ReadingValue, ReadingTime) 
                        VALUES (@SensorID, @ReadingValue, @ReadingTime)";
                    
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@SensorID", sensorID);
                        command.Parameters.AddWithValue("@ReadingValue", readingValue);
                        command.Parameters.AddWithValue("@ReadingTime", DateTime.Now);
                        
                        int rowsAffected = command.ExecuteNonQuery();
                        Console.WriteLine($"Sensor reading recorded. Rows affected: {rowsAffected}");
                    }
                    
                    // Update the LastReadingTime in the Sensors table
                    string updateQuery = @"
                        UPDATE Sensors 
                        SET LastReadingTime = @LastReadingTime 
                        WHERE SensorID = @SensorID";
                    
                    using (SqlCommand command = new SqlCommand(updateQuery, connection))
                    {
                        command.Parameters.AddWithValue("@SensorID", sensorID);
                        command.Parameters.AddWithValue("@LastReadingTime", DateTime.Now);
                        
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error recording sensor reading: {ex.Message}");
            }
        }

        #endregion

        #region Schedule Methods

        /// <summary>
        /// Adds a schedule to the database
        /// </summary>
        /// <param name="schedule">The schedule to add</param>
        public void AddSchedule(Schedule schedule)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    
                    string query = @"
                        INSERT INTO Schedules (ScheduleID, StartTime, EndTime, Action, DeviceID, IsRecurring, RecurrencePattern) 
                        VALUES (@ScheduleID, @StartTime, @EndTime, @Action, @DeviceID, @IsRecurring, @RecurrencePattern)";
                    
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ScheduleID", schedule.ScheduleID);
                        command.Parameters.AddWithValue("@StartTime", schedule.StartTime);
                        command.Parameters.AddWithValue("@EndTime", schedule.EndTime);
                        command.Parameters.AddWithValue("@Action", schedule.Action);
                        command.Parameters.AddWithValue("@DeviceID", schedule.DeviceID);
                        command.Parameters.AddWithValue("@IsRecurring", schedule.IsRecurring);
                        command.Parameters.AddWithValue("@RecurrencePattern", 
                            string.IsNullOrEmpty(schedule.RecurrencePattern) ? DBNull.Value : (object)schedule.RecurrencePattern);
                        
                        int rowsAffected = command.ExecuteNonQuery();
                        Console.WriteLine($"Schedule added to database. Rows affected: {rowsAffected}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding schedule: {ex.Message}");
            }
        }

        /// <summary>
        /// Gets all active schedules for the current time
        /// </summary>
        /// <returns>A list of active schedules</returns>
        public List<Schedule> GetActiveSchedules()
        {
            List<Schedule> schedules = new List<Schedule>();
            DateTime now = DateTime.Now;
            
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    
                    // Query to get schedules that are active at the current time
                    string query = @"
                        SELECT ScheduleID, StartTime, EndTime, Action, DeviceID, IsRecurring, RecurrencePattern 
                        FROM Schedules 
                        WHERE @CurrentTime BETWEEN StartTime AND EndTime";
                    
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@CurrentTime", now);
                        
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Schedule schedule = new Schedule(
                                    reader.GetInt32(0),
                                    reader.GetDateTime(1),
                                    reader.GetDateTime(2),
                                    reader.GetString(3),
                                    reader.GetInt32(4)
                                );
                                
                                bool isRecurring = reader.GetBoolean(5);
                                if (isRecurring && !reader.IsDBNull(6))
                                {
                                    schedule.SetRecurring(reader.GetString(6));
                                }
                                
                                // Check if the schedule is active based on recurrence pattern
                                if (schedule.IsActiveAt(now))
                                {
                                    schedules.Add(schedule);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting active schedules: {ex.Message}");
            }
            
            return schedules;
        }

        #endregion
    }
}
