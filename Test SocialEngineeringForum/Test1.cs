using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using SocialEngineeringForum.Controllers;
using SocialEngineeringForum.Models;
using Moq;
using Xunit;

namespace MyForum.Tests
{
    public class TopicsControllerTests
    {
        [Fact]
        public void Index_Returns_List_Of_Topics()
        {
            // Arrange
            var mockRepo = new Mock<IRepository<Topic>>();
            mockRepo.Setup(repo => repo.GetAll()).Returns(GetTestTopics());

            var controller = new TopicsController(mockRepo.Object);

            // Act
            var result = controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<Topic>>(viewResult.Model);
            Assert.Equal(2, model.Count());
        }

        private List<Topic> GetTestTopics()
        {
            return new List<Topic>
            {
                new Topic { Id = 1, Title = "Первая тема", CategoryId = 1 },
                new Topic { Id = 2, Title = "Вторая тема", CategoryId = 2 }
            };
        }
    }
}