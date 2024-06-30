using System;
using System.Collections.Generic;
using Moq;
using NewsAPI.Constants;
using NewsAPI.Models;
using Proyecto_Hospital.Services;
using Xunit;

namespace Proyecto_Hospital.Tests.Services
{
    public class NewsServiceTests
    {
        [Fact]
        public void GetTopNews_ReturnsCorrectNumberOfArticles()
        {
            // Arrange
            var mockNewsApiClient = new Mock<INewsServiceApiClient>();
            var newsService = new NewsService(mockNewsApiClient.Object);

            // Configurar el comportamiento simulado para el método GetTopNews
            mockNewsApiClient.Setup(apiClient => apiClient.GetTopNews(It.IsAny<TopHeadlinesRequest>()))
                .Returns(new List<Article>
                {
            new Article { Title = "Article 1" },
            new Article { Title = "Article 2" },
            new Article { Title = "Article 3" }
                });

            // Act
            var result = newsService.GetTopNews("Health", 3);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<Article>>(result);
            Assert.Equal(3, result.Count);
        }

        [Fact]
        public void GetTopNews_ReturnsEmptyList_WhenApiClientFails()
        {
            // Arrange
            var mockNewsApiClient = new Mock<INewsServiceApiClient>();
            var newsService = new NewsService(mockNewsApiClient.Object);

            // Configurar el comportamiento simulado para el método GetTopNews
            mockNewsApiClient.Setup(apiClient => apiClient.GetTopNews(It.IsAny<TopHeadlinesRequest>()))
                .Returns(new List<Article>());

            // Act
            var result = newsService.GetTopNews("Health", 3);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<Article>>(result);
            Assert.Empty(result);
        }

    }
}