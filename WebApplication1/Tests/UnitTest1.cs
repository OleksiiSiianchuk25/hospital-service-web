using Moq;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;
using Npgsql;
using System;
using Bogus;
using BCrypt.Net;
using System.Collections.Generic;

using System.Collections;
using System.Runtime.Intrinsics.X86;

using Microsoft.VisualBasic;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using Microsoft.VisualStudio.TestPlatform.TestHost;


namespace Tests
{
    public class Tests
    {
        [TestFixture]
        public class DatabaseFillerTests
        {
            private NpgsqlConnection connection;
            private DatabaseFiller filler;

            [SetUp]
            public void Setup()
            {
                string connstring = String.Format("Server={0};Port={1};" +
            "User Id={2};Password={3};Database={4};",
            "ep-damp-wood-56616604.eu-central-1.aws.neon.tech", 5432,
            "Orik25", "fnF9SZOoqQR7", "neondb");
                connection = new NpgsqlConnection(connstring);
                connection.Open();
                filler = new DatabaseFiller(connection);
            }

            [TearDown]
            public void Cleanup()
            {
                connection.Close();
                connection.Dispose();
            }

            [Test]
            public void FillRoles_AddsRolesToDatabase()
            {
                filler.FillRoles(connection);

                int[] roleIDs = filler.GetRolesID(connection);
                Assert.IsNotNull(roleIDs);
                Assert.AreEqual(3, roleIDs.Length);
            }

            [Test]
            public void GetDoctorsID_ReturnsArrayOfDoctorIDs()
            {
                int[] doctorIDs = filler.GetDoctorsID(connection);
                Assert.IsNotNull(doctorIDs);
            }


            [Test]
            public void FillUsers_AddsUsersToDatabase()
            {
                int[] roleIDs = filler.GetRolesID(connection);
                filler.FillRoles(connection);

                filler.FillUsers(connection, roleIDs);


            }

            [Test]
            public void RecordExists_ReturnsTrueIfRecordExists()
            {
                // Arrange
                filler.FillRoles(connection); // Çàïîâíþºìî òåñòîâèìè äàíèìè äëÿ òàáëèö³ roles

                // Act
                bool recordExists = filler.RecordExists(connection, "roles", "role_id", 1);

                // Assert
                Assert.IsTrue(recordExists); // Ïåðåâ³ðêà, ùî çàïèñ ç role_id = 1 ³ñíóº
            }
            [Test]
            public void GetRolesID_ReturnsRoleIDsFromDatabase()
            {
                // Arrange
                filler.FillRoles(connection); // Çàïîâíþºìî òåñòè òåñòîâèìè äàíèìè äëÿ òàáëèö³ roles

                // Act
                int[] rolesIDs = filler.GetRolesID(connection);

                // Assert
                Assert.AreEqual(3, rolesIDs.Length); // Ïåðåâ³ðêà, ùî îòðèìàíî î÷³êóâàíó ê³ëüê³ñòü ðîëåé
                Assert.Contains(1, rolesIDs); // Ïåðåâ³ðêà ÷è º ðîëü ç ID 1, òàê ñàìî äëÿ ³íøèõ ðîëåé
            }
            [Test]
            public void FillAppointments_AddsAppointmentsToDatabase()
            {
                // Arrange
                filler.FillRoles(connection);
                filler.FillUsers(connection, filler.GetRolesID(connection));

                int[] doctorsIDs = filler.GetDoctorsID(connection);
                int[] patientsIDs = filler.GetPatientID(connection);

                int[] appointmentsCountBefore = GetAppointmentsCount();
                // Act
                filler.FillAppointments(connection, doctorsIDs, patientsIDs);

                // Assert
                int[] appointmentsCount = GetAppointmentsCount();
                Assert.AreEqual(appointmentsCountBefore.Length+1, appointmentsCount.Length);
            }

            // Ìåòîä äëÿ îòðèìàííÿ ê³ëüêîñò³ çàïèñ³â ó òàáëèö³ appointments
            private int[] GetAppointmentsCount()
            {
                List<int> IDs = new List<int>();
                using (var cmd = new NpgsqlCommand("SELECT appointment_id FROM appointments", connection))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        IDs.Add(Convert.ToInt32(reader[0]));
                    }
                }
                return IDs.ToArray();
            }
            [Test]
            public void RecordExists_ReturnsTrue_WhenRecordExists()
            {
                // Arrange
                string tableName = "roles";
                string columnName = "role_id";
                object value = 1;

                // Act
                bool recordExists = filler.RecordExists(connection, tableName, columnName, value);

                // Assert
                Assert.IsTrue(recordExists); // Ïåðåâ³ðêà, ùî çàïèñ ³ñíóº ó òàáëèö³ ç âêàçàíèìè ïàðàìåòðàìè
            }

            [Test]
            public void RecordExists_ReturnsFalse_WhenRecordDoesNotExist()
            {
                // Arrange
                string tableName = "roles";
                string columnName = "role_id";
                object value = 99; // ßêùî öå çíà÷åííÿ íå ³ñíóº ó òàáëèö³, î÷³êóºìî false

                // Act
                bool recordExists = filler.RecordExists(connection, tableName, columnName, value);

                // Assert
                Assert.IsFalse(recordExists); // Ïåðåâ³ðêà, ùî çàïèñ íå ³ñíóº ó òàáëèö³ ç âêàçàíèìè ïàðàìåòðàìè
            }
            [Test]
            public void FillUsers_InsertsUsers_IntoDatabase()
            {
                // Arrange

                int[] roleIDs = { 1, 2, 3 };

                // Act
                filler.FillUsers(connection, roleIDs);

                // Assert
                bool usersInserted = CheckIfUsersWereInserted(connection);
                Assert.IsTrue(usersInserted);
            }

            private bool CheckIfUsersWereInserted(NpgsqlConnection connection)
            {
                bool usersInserted = false;
                using (var cmd = new NpgsqlCommand("SELECT COUNT(*) FROM users", connection))
                {
                    object result = cmd.ExecuteScalar();
                    if (result != null && int.TryParse(result.ToString(), out int count))
                    {
                        usersInserted = count > 0;
                    }
                }
                return usersInserted;
            }

        }
    }
}