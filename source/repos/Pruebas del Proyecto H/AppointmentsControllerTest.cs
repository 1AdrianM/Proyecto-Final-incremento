using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using Proyecto_Hospital.Controllers;
using Proyecto_Hospital.Models;
using Xunit;
using Microsoft.EntityFrameworkCore.InMemory;

namespace Proyecto_Hospital.Tests
{
    public class AppointmentsControllerTests
    {
        [Fact]
        public async Task Create_ReturnsViewResult_WhenModelStateIsInvalid()
        {
            // Arrange
            var controller = new AppointmentsController(MockDbContext.CreateMockContext());

            // Simular un estado no válido del modelo
            controller.ModelState.AddModelError("Name", "Required");

            // Act
            var result = await controller.Create(new Appointment());

            // Assert
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public async Task Create_RedirectsToIndex_WhenModelStateIsValid()
        {
            // Arrange
            var mockContext = MockDbContext.CreateMockContext();
            var controller = new AppointmentsController(mockContext);

            // Act
            var result = await controller.Create(new Appointment());

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
        }

        // Agrega más pruebas según sea necesario

        [Fact]
        public async Task Index_ReturnsViewWithNoAppointments()
        {
            var mockSet = new Mock<DbSet<Appointment>>();
            var appointments = new List<Appointment>();
            var queryableAppointments = appointments.AsQueryable();

            mockSet.As<IQueryable<Appointment>>().Setup(m => m.Provider).Returns(queryableAppointments.Provider);
            mockSet.As<IQueryable<Appointment>>().Setup(m => m.Expression).Returns(queryableAppointments.Expression);
            mockSet.As<IQueryable<Appointment>>().Setup(m => m.ElementType).Returns(queryableAppointments.ElementType);
            mockSet.As<IQueryable<Appointment>>().Setup(m => m.GetEnumerator()).Returns(() => queryableAppointments.GetEnumerator());
            mockSet.As<IAsyncEnumerable<Appointment>>()
                .Setup(m => m.GetAsyncEnumerator(It.IsAny<CancellationToken>()))
                .Returns(new TestAsyncEnumerator<Appointment>(queryableAppointments.GetEnumerator()));

            var mockContext = new Mock<HospitalDbContext>();
            mockContext.Setup(c => c.Appointments).Returns(mockSet.Object);
            var controller = new AppointmentsController(mockContext.Object);

            // Act
            var result = await controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<Appointment>>(viewResult.ViewData.Model);
            Assert.Empty(model);


        }

        [Fact]
        public async Task Details_ReturnsNotFoundResultForNullId()
        {
            // Arrange
            var mockContext = new Mock<HospitalDbContext>();
            var controller = new AppointmentsController(mockContext.Object);

            // Act
            var result = await controller.Details(null);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        // Similar tests for other controller actions

        // ...
    }

    public static class MockDbContext
    {
        public static HospitalDbContext CreateMockContext()
        {
            var options = new DbContextOptionsBuilder<HospitalDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var mockContext = new Mock<HospitalDbContext>(options);

            // Configurar cualquier configuración específica de las pruebas aquí

            return mockContext.Object;
        }
    }
}
