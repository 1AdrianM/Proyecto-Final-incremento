using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Proyecto_Hospital.Controllers;
using Proyecto_Hospital.Models;
using Xunit;

namespace Proyecto_Hospital.Tests
{
    public class LoginControllerTests
    {
        [Fact]
        public void Login_ReturnsViewResult()
        {
            // Arrange
            var controller = new LoginController();
            // Antes del Act
            controller.ControllerContext.HttpContext = new DefaultHttpContext { };

            // Act
            var result = controller.Login() as ViewResult;

            // Assert
            Assert.NotNull(result);

            // Agregar información de depuración
            Console.WriteLine($"ViewName: {result.ViewName}");
            Console.WriteLine($"ViewData: {result.ViewData}");

            // Verifica si es una vista sin especificar el nombre
            Assert.IsType<ViewResult>(result);

            Assert.Empty(result.ViewData);
        }

        [Fact]
        public void Verify_WithValidCredentials_ReturnsRedirectToActionResult()
        {
            // Arrange
            var controller = new LoginController();
            var user = new Usuario { Username = "Admin", Password = "1234password" };

            // Act
            var result = controller.Verify(user) as RedirectToActionResult;

            // Assert
            // Verifica si el resultado no es nulo antes de realizar otras aserciones
            Assert.NotNull(result);

            // Verifica el tipo del resultado solo si no es nulo
            if (result != null)
            {
                Assert.IsType<RedirectToActionResult>(result);
                Assert.Equal("Index", result.ActionName);
                Assert.Equal("Home", result.ControllerName);
                Assert.Null(result.RouteValues["area"]);
            }
        }



        [Fact]
        public void Verify_WithInvalidCredentials_ReturnsViewResult()
        {
            // Arrange
            var controller = new LoginController();
            var user = new Usuario { Username = "invalidUsername", Password = "invalidPassword" };

            // Act
            var result = controller.Verify(user) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Error", result.ViewName);
        }
    }
}
