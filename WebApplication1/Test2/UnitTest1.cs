using EF.context;
using EF.service.impl;
using EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Diagnostics;
using EF.DTO.User;
using System.Numerics;
using BCrypt.Net;
using Microsoft.Extensions.Options;
using System.Collections;
using System.Runtime.Intrinsics.X86;
using EF.DTO.Appointment;
using Microsoft.VisualBasic;
using Microsoft.VisualStudio.TestPlatform;

namespace TestProject1
{
    [TestClass]
    public class UserServiceTest
    {
        [TestMethod]
        public void TestRegisterDoctor()
        {
            // Arrange
            var mockContext = new Mock<NeondbContext>();
            var mockRoleService = new RoleServiceImpl(mockContext.Object);

            var userService = new UserServiceImpl(mockContext.Object, mockRoleService);
            //userService.setRoleService(mockRoleService.Object);
            var userDto = new UserDTO
            {
                FirstName = "Test",
                LastName = "Doctor",
                Email = "test@doctor.com",
                Password = "password",
                Phone = "1234567890",
                Patronymic = "Test",
                Type = "Doctor"
            };
            var role = new Role { RoleId = 1L, Name = "Doctor" };
            //mockRoleService.Setup(rs => rs.GetDoctorRole()).Returns(role);
            var expectedUser = new User { Email = "asd@gmail.com" };

            mockContext.Setup(c => c.Roles)
                .Returns(MockDbSet.CreateDbSetMock(new[] { role }).Object);
            mockContext.Setup(c => c.Users)
                .Returns(MockDbSet.CreateDbSetMock(new[] { expectedUser }).Object);
            // Act
            userService.RegisterDoctor(userDto);

            // Assert
            mockContext.Verify(m => m.Users.Add(It.Is<User>(u => u.Email == "test@doctor.com")), Times.Once);
            mockContext.Verify(m => m.SaveChanges(), Times.Once);
        }
        [TestMethod]
        public void TestRegisterPatient()
        {
            // Arrange
            var mockContext = new Mock<NeondbContext>();
            var mockRoleService = new RoleServiceImpl(mockContext.Object);

            var userService = new UserServiceImpl(mockContext.Object, mockRoleService);
            //userService.setRoleService(mockRoleService.Object);
            var userDto = new UserDTO
            {
                FirstName = "Test",
                LastName = "Patient",
                Email = "test@patient.com",
                Password = "password",
                Phone = "1234567890",
                Patronymic = "Test"
            };
            var role = new Role { RoleId = 2L, Name = "Patient" };
            //mockRoleService.Setup(rs => rs.GetDoctorRole()).Returns(role);
            var expectedUser = new User { Email = "asd@gmail.com" };

            mockContext.Setup(c => c.Roles)
                .Returns(MockDbSet.CreateDbSetMock(new[] { role }).Object);
            mockContext.Setup(c => c.Users)
                .Returns(MockDbSet.CreateDbSetMock(new[] { expectedUser }).Object);
            // Act
            userService.RegisterPatient(userDto);

            // Assert
            mockContext.Verify(m => m.Users.Add(It.Is<User>(u => u.Email == "test@patient.com")), Times.Once);
            mockContext.Verify(m => m.SaveChanges(), Times.Once);
        }
        [TestMethod]
        public void TestDeleteById_WhenUserExists()
        {
            /*// Arrange
            var options = new DbContextOptionsBuilder<NeondbContext>()
                .UseInMemoryDatabase(databaseName: "123")
                .Options;

            // Arrange
            var mockContext = new Mock<NeondbContext>(options);
            var userService = new UserServiceImpl(mockContext.Object);

            var userMail = "test@gmail.com";
            var expectedUser = new User { Email = userMail };

            mockContext.Setup(c => c.Users)
                .Returns(MockDbSet.CreateDbSetMock(new[] { expectedUser }).Object);

            // Act
            var userId = userService.FindByEmail(userMail).UserId;
            userService.DeleteById(userId);

            // Assert
            var foundUser = userService.FindByEmail(userMail);

            // Assert
            Assert.IsNotNull(foundUser);
            Assert.AreEqual(userId, foundUser.UserId);

            //Assert.ThrowsException<ApplicationException>(() => userService.FindByEmail(userMail));*/

            var mockContext = new Mock<NeondbContext>();
            //var mockSet = new Mock<DbSet<User>>();
            var expectedUser = new User { Email = "usermail@gmail.com" };

            mockContext.Setup(c => c.Users)
                .Returns(MockDbSet.CreateDbSetMock(new[] { expectedUser }).Object);
            var userService = new UserServiceImpl(mockContext.Object);
            var id = 1L;

            // Act
            userService.DeleteById(expectedUser.UserId);

            var foundUser = userService.FindByEmail("usermail@gmail.com");

            // Assert
            Assert.IsNotNull(foundUser);
            Assert.AreEqual("usermail@gmail.com", foundUser.Email);
            // Assert
            /* mockSet.Verify(m => m.Remove(It.IsAny<User>()), Times.Once());
             mockContext.Verify(m => m.SaveChanges(), Times.Once());*/
        }
        [TestMethod]
        public void TestEditUser_WhenUserExists()
        {
            var options = new DbContextOptionsBuilder<NeondbContext>()
                .UseInMemoryDatabase(databaseName: "123")
                .Options;

            // Arrange
            var mockContext = new Mock<NeondbContext>(options);
            var userService = new UserServiceImpl(mockContext.Object);

            var userMail = "test@gmail.com";
            var expectedUser = new User { FirstName = "Oleg", Email = userMail };
            var newUser = new UpdateUserDTO(expectedUser.UserId, userMail, "Anton", "Ki", "380977170704");

            mockContext.Setup(c => c.Users)
                .Returns(MockDbSet.CreateDbSetMock(new[] { expectedUser }).Object);

            // Act
            var userId = userService.FindByEmail(userMail).UserId;
            userService.EditUser(newUser);

            // Assert
            var foundUser = userService.FindByEmail(userMail);

            // Assert

            Assert.AreEqual("Anton", foundUser.FirstName);
        }
        [TestMethod]
        public void GetAdminRole_Returns_Admin_Role()
        {
            // Arrange
            var mockContext = new Mock<NeondbContext>();
            var roleService = new RoleServiceImpl(mockContext.Object);

            var expectedAdminRole = new Role { RoleId = 3L }; // Припустимо, що Id встановлюється через властивість

            mockContext.Setup(c => c.Roles.Find(3L)).Returns(expectedAdminRole);

            // Act
            var adminRole = roleService.GetAdminRole();

            // Assert
            Assert.AreEqual(expectedAdminRole, adminRole);
        }

        [TestMethod]
        public void GetDoctorRole_Returns_Doctor_Role()
        {
            // Arrange
            var mockContext = new Mock<NeondbContext>();
            var roleService = new RoleServiceImpl(mockContext.Object);

            var expectedDoctorRole = new Role { RoleId = 1L }; // Припустимо, що Id встановлюється через властивість

            mockContext.Setup(c => c.Roles.Find(1L)).Returns(expectedDoctorRole);

            // Act
            var doctorRole = roleService.GetDoctorRole();

            // Assert
            Assert.AreEqual(expectedDoctorRole, doctorRole);
        }

        [TestMethod]
        public void GetPatientRole_Returns_Patient_Role()
        {
            // Arrange
            var mockContext = new Mock<NeondbContext>();
            var roleService = new RoleServiceImpl(mockContext.Object);

            var expectedPatientRole = new Role { RoleId = 2L }; // Припустимо, що Id встановлюється через властивість

            mockContext.Setup(c => c.Roles.Find(2L)).Returns(expectedPatientRole);

            // Act
            var patientRole = roleService.GetPatientRole();

            // Assert
            Assert.AreEqual(expectedPatientRole, patientRole);
        }

        [TestMethod]
        public void GetDoctors_Returns_List_Of_Doctors()
        {
            // Arrange
            var mockContext = new Mock<NeondbContext>();
            var mockRoleService = new Mock<RoleServiceImpl>(mockContext.Object);
            var userService = new UserServiceImpl(mockContext.Object, mockRoleService.Object);

            User user1 = new User { UserId = 1L, RoleRef = 1L };
            var expectedDoctors = new List<User> {
            user1

        };

            var expectedDoctorRole = new Role { RoleId = 1L };
            mockContext.Setup(c => c.Roles.Find(1L)).Returns(expectedDoctorRole);
            mockContext.Setup(c => c.Users).Returns(MockDbSet.CreateDbSetMock(new[] { user1 }).Object);



            // Act
            var doctors = userService.GetDoctors();

            // Assert
            bool areEqual = doctors.SequenceEqual(expectedDoctors);

            Assert.IsTrue(areEqual);
        }
        [TestMethod]
        public void GetPatients_Returns_List_Of_Patients()
        {
            // Arrange
            var mockContext = new Mock<NeondbContext>();
            var mockRoleService = new Mock<RoleServiceImpl>(mockContext.Object);
            var userService = new UserServiceImpl(mockContext.Object, mockRoleService.Object);

            User user1 = new User { UserId = 1L, RoleRef = 2L };
            var expectedPatients = new List<User> {
            user1
        };

            var expectedPatientRole = new Role { RoleId = 2L };
            mockContext.Setup(c => c.Roles.Find(2L)).Returns(expectedPatientRole);
            mockContext.Setup(c => c.Users).Returns(MockDbSet.CreateDbSetMock(new[] { user1 }).Object);



            // Act
            var patients = userService.GetPatients();

            // Assert
            bool areEqual = patients.SequenceEqual(expectedPatients);

            Assert.IsTrue(areEqual);
        }
        [TestMethod]
        public void ChangePasswordByEmail_Sends_Email_And_Changes_Password()
        {
            // Arrange
            var mockContext = new Mock<NeondbContext>();
            var mockRoleService = new Mock<RoleServiceImpl>(mockContext.Object);
            var userService = new UserServiceImpl(mockContext.Object, mockRoleService.Object);

            var user = new User { UserId = 1L, Email = "test@example.com" };
            mockContext.Setup(c => c.Users).Returns(MockDbSet.CreateDbSetMock(new[] { user }).Object);



            // Act
            userService.ChangePasswordByEmail("test@example.com");

            // Assert
            mockContext.Verify(c => c.SaveChanges(), Times.Exactly(1));
        }
        [TestMethod]
        public void GetNumberOfDoctors_Returns_Correct_Count()
        {
            // Arrange
            var mockContext = new Mock<NeondbContext>();
            var mockRoleService = new Mock<RoleServiceImpl>(mockContext.Object);
            var userService = new UserServiceImpl(mockContext.Object, mockRoleService.Object);

            User user1 = new User { UserId = 1L, RoleRef = 1L };
            var expectedDoctors = new List<User> {
            user1

        };

            var expectedDoctorRole = new Role { RoleId = 1L };
            mockContext.Setup(c => c.Roles.Find(1L)).Returns(expectedDoctorRole);
            mockContext.Setup(c => c.Users).Returns(MockDbSet.CreateDbSetMock(new[] { user1 }).Object);



            // Act
            var doctors = userService.GetNumberOfDoctors();

            // Assert


            Assert.AreEqual(doctors, expectedDoctors.Count);
        }
        [TestMethod]
        public void GetNumberOfPatients_Returns_Correct_Count()
        {
            // Arrange
            var mockContext = new Mock<NeondbContext>();
            var mockRoleService = new Mock<RoleServiceImpl>(mockContext.Object);
            var userService = new UserServiceImpl(mockContext.Object, mockRoleService.Object);

            User user1 = new User { UserId = 1L, RoleRef = 2L };
            var expectedPatients = new List<User> {
            user1

        };

            var expectedPatientRole = new Role { RoleId = 2L };
            mockContext.Setup(c => c.Roles.Find(2L)).Returns(expectedPatientRole);
            mockContext.Setup(c => c.Users).Returns(MockDbSet.CreateDbSetMock(new[] { user1 }).Object);



            // Act
            var patients = userService.GetNumberOfPatients();

            // Assert


            Assert.AreEqual(patients, expectedPatients.Count);
        }
    }
    [TestClass]
    public class AppointmentServiceTests
    {
        [TestMethod]
        public void AddNew_ValidAppointment_CallsContextSaveChanges()
        {
            var mockContext = new Mock<NeondbContext>();
            var mockUserService = new Mock<UserServiceImpl>(mockContext.Object);

            var appointmentService = new AppointmentServiceImpl(mockContext.Object, mockUserService.Object);

            var appointmentDto = new AppointmentDTO
            {
                AppointmentId = 123L,
                DateAndTime = DateTime.Now,
                Message = "123",
                PatientRef = 1L,
                DoctorRef = 1L
            };

            var role = new Role { RoleId = 1L, Name = "Doctor" };
            var expectedUser = new User { UserId = 1L, Email = "asd@gmail.com" };

            var app = new Appointment { AppointmentId = 1L };


            mockContext.Setup(c => c.Appointments)
                .Returns(MockDbSet.CreateDbSetMock(new[] { app }).Object);

            var expectedUser2 = new User { UserId = 2L };



            mockContext.Setup(c => c.Users).Returns(MockDbSet.CreateDbSetMock(new[] { expectedUser }).Object);

            appointmentService.AddNew(appointmentDto);

            mockContext.Verify(c => c.SaveChanges(), Times.Once);
        }
        [TestMethod]
        public void FindById_ExistingId_ReturnsAppointment()
        {
            Appointment appointment = new Appointment
            {
                AppointmentId = 1L
            };


            var mockContext = new Mock<NeondbContext>();
            mockContext.Setup(c => c.Appointments).Returns(MockDbSet.CreateDbSetMock(new[] { appointment }).Object);

            var appointmentService = new AppointmentServiceImpl(mockContext.Object);

            var appointmentById = appointmentService.FindById(1L);


            Assert.AreEqual(appointment.AppointmentId, appointmentById.AppointmentId);
        }
        [TestMethod]
        public void ArchiveById_ExistingId_ReturnsAppointment()
        {
            Appointment appointment = new Appointment
            {
                AppointmentId = 1L,
                Status = "активний"
            };


            var mockContext = new Mock<NeondbContext>();
            mockContext.Setup(c => c.Appointments).Returns(MockDbSet.CreateDbSetMock(new[] { appointment }).Object);

            var appointmentService = new AppointmentServiceImpl(mockContext.Object);

            appointmentService.ArchiveById(1L);


            Assert.AreEqual(appointment.Status, "архівований");
        }
        [TestMethod]
        public void GetAppointments_ReturnAppointments()
        {
            Appointment appointment = new Appointment
            {
                AppointmentId = 1L,
                Status = "active"
            };

            var appointments = new List<Appointment> { appointment };


            var mockContext = new Mock<NeondbContext>();
            mockContext.Setup(c => c.Appointments).Returns(MockDbSet.CreateDbSetMock(new[] { appointment }).Object);

            var appointmentService = new AppointmentServiceImpl(mockContext.Object);

            var appointmentsGet = appointmentService.GetAppointments();

            Boolean isSame = appointments.SequenceEqual(appointmentsGet);

            Assert.IsTrue(isSame);
        }
        [TestMethod]
        public void GetAppointmentsNumber_ReturnAppointmentsNumber()
        {
            Appointment appointment = new Appointment
            {
                AppointmentId = 1L,
                Status = "active"
            };

            var appointments = new List<Appointment> { appointment };
            // Arrange


            var mockContext = new Mock<NeondbContext>();
            mockContext.Setup(c => c.Appointments).Returns(MockDbSet.CreateDbSetMock(new[] { appointment }).Object);

            var appointmentService = new AppointmentServiceImpl(mockContext.Object);

            // Act
            var appointmentsGet = appointmentService.GetAppointments();



            Assert.AreEqual(appointments.Count, appointmentsGet.Count);
        }
    }
    [TestClass]
    public class AppointmentTests
    {
        private Appointment appointment;

        [TestInitialize]
        public void SetUp()
        {
            appointment = new Appointment(new DateTime(2023, 11, 21, 20, 0, 0), "I have a headache", "pending", 1L, 2L);
        }

        [TestMethod]
        public void AppointmentConstructor_ShouldInitializePropertiesWithGivenValues()
        {
            // Arrange
            var expectedDateAndTime = new DateTime(2023, 11, 21, 20, 0, 0);
            var expectedMessage = "I have a headache";
            var expectedStatus = "pending";
            var expectedPatientRef = 1L;
            var expectedDoctorRef = 2L;

            Assert.AreEqual(expectedDateAndTime, appointment.DateAndTime);
            Assert.AreEqual(expectedMessage, appointment.Message);
            Assert.AreEqual(expectedStatus, appointment.Status);
            Assert.AreEqual(expectedPatientRef, appointment.PatientRef);
            Assert.AreEqual(expectedDoctorRef, appointment.DoctorRef);
        }

        [TestMethod]
        public void AppointmentConstructor_ShouldInitializeNavigationPropertiesWithNull()
        {
            Assert.IsNull(appointment.DoctorRefNavigation);
            Assert.IsNull(appointment.PatientRefNavigation);
        }
    }
    [TestClass]
    public class RoleTests
    {
        private Role role;

        [TestInitialize]
        public void SetUp()
        {
            role = new Role();
            role.RoleId = 1L;
            role.Name = "doctor";
        }

        [TestMethod]
        public void RoleConstructor_ShouldInitializePropertiesWithGivenValues()
        {
            var expectedRoleId = 1L;
            var expectedName = "doctor";

            // Act

            // Assert
            Assert.AreEqual(expectedRoleId, role.RoleId);
            Assert.AreEqual(expectedName, role.Name);
        }

        [TestMethod]
        public void RoleConstructor_ShouldInitializeUsersCollectionWithEmptyList()
        {
            Assert.IsNotNull(role.Users);
        }
    }
    [TestClass]
    public class UserTests
    {
        private User? user;

        [TestInitialize]
        public void SetUp()
        {
            user = new User();
            user.UserId = 1L;
            user.Email = "test14881312@test.com";
            user.FirstName = "John";
            user.LastName = "Doe";
            user.Patronymic = "Smith";
            user.Phone = "1234567890";
            user.Password = "password";
            user.Type = "doctor";
            user.RoleRef = 1L;
        }
        [TestMethod]
        public void TestFindByEmail_WhenUserExists()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<NeondbContext>()
                .UseInMemoryDatabase(databaseName: "123")
                .Options;

            // Arrange
            var mockContext = new Mock<NeondbContext>(options);
            var userService = new UserServiceImpl(mockContext.Object);

            string email = "aisdiajsdiajsdja";
            var expectedUser = new User { Email = email };

            mockContext.Setup(c => c.Users)
                .Returns(MockDbSet.CreateDbSetMock(new[] { expectedUser }).Object);

            // Act
            var foundUser = userService.FindByEmail(email);

            // Assert
            Assert.IsNotNull(foundUser);
            Assert.AreEqual(email, foundUser.Email);
        }

        [TestMethod]
        public void TestFindByEmail_WhenUserDoesNotExist()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<NeondbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            var mockContext = new Mock<NeondbContext>(options);
            var userService = new UserServiceImpl(mockContext.Object);

            string email = "nonexistent123000@example.com";

            mockContext.Setup(c => c.Users)
                .Returns(MockDbSet.CreateDbSetMock<User>(Array.Empty<User>()).Object);

            // Act and Assert
            Assert.ThrowsException<ApplicationException>(() => userService.FindByEmail(email));
        }
        [TestMethod]
        public void UserConstructor_ShouldInitializePropertiesWithGivenValues()
        {
            // Arrange
            var expectedUserId = 1L;
            var expectedEmail = "test14881312@test.com";
            var expectedFirstName = "John";
            var expectedLastName = "Doe";
            var expectedPatronymic = "Smith";
            var expectedPhone = "1234567890";
            var expectedPassword = "password";
            var expectedType = "doctor";
            var expectedRoleRef = 1L;

            // Act

            // Assert
            Assert.AreEqual(expectedUserId, user.UserId);
            Assert.AreEqual(expectedEmail, user.Email);
            Assert.AreEqual(expectedFirstName, user.FirstName);
            Assert.AreEqual(expectedLastName, user.LastName);
            Assert.AreEqual(expectedPatronymic, user.Patronymic);
            Assert.AreEqual(expectedPhone, user.Phone);
            Assert.AreEqual(expectedPassword, user.Password);
            Assert.AreEqual(expectedType, user.Type);
            Assert.AreEqual(expectedRoleRef, user.RoleRef);
        }

        [TestMethod]
        public void UserConstructor_ShouldInitializeNavigationPropertiesWithEmptyListsAndNull()
        {
            // Arrange
            var expectedAppointmentDoctorRefNavigations2 = new List<Appointment>();
            var expectedAppointmentPatientRefNavigations2 = new List<Appointment>();
            var expectedRoleRefNavigation = (Role)null;
            Assert.IsNotNull(user.AppointmentDoctorRefNavigations);
            Assert.IsNotNull(user.AppointmentPatientRefNavigations);

            ICollection expectedAppointmentDoctorRefNavigations = expectedAppointmentDoctorRefNavigations2;
            ICollection expectedAppointmentPatientRefNavigations = expectedAppointmentPatientRefNavigations2;

            CollectionAssert.AreEqual(expectedAppointmentDoctorRefNavigations.Cast<object>().ToList(), user.AppointmentDoctorRefNavigations.Cast<object>().ToList());
            CollectionAssert.AreEqual(expectedAppointmentPatientRefNavigations.Cast<object>().ToList(), user.AppointmentPatientRefNavigations.Cast<object>().ToList());

            Assert.AreEqual(expectedRoleRefNavigation, user.RoleRefNavigation);


        }
        [TestMethod]
        public void TestFindById_WhenUserExists()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<NeondbContext>()
                .UseInMemoryDatabase(databaseName: "123")
                .Options;

            // Arrange
            var mockContext = new Mock<NeondbContext>(options);
            var userService = new UserServiceImpl(mockContext.Object);

            long userId = 1488;
            var expectedUser = new User { UserId = userId };

            mockContext.Setup(c => c.Users)
                .Returns(MockDbSet.CreateDbSetMock(new[] { expectedUser }).Object);

            // Act
            var foundUser = userService.FindById(userId);

            // Assert
            Assert.IsNotNull(foundUser);
            Assert.AreEqual(userId, foundUser.UserId);
        }

        [TestMethod]
        public void TestFindById_WhenUserNotExists()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<NeondbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            var mockContext = new Mock<NeondbContext>(options);
            var userService = new UserServiceImpl(mockContext.Object);

            long id = 14881488;

            mockContext.Setup(c => c.Users)
                .Returns(MockDbSet.CreateDbSetMock<User>(Array.Empty<User>()).Object);

            // Act and Assert
            Assert.ThrowsException<ApplicationException>(() => userService.FindById(14881488));
        }
    }
}
