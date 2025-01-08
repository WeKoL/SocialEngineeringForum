using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SocialEngineeringForum.Models;

namespace MyForum.Data
{
    public class TopicRepository : IRepository<Topic>
    {
        private readonly ApplicationDbContext _context;

        public TopicRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Topic> GetAll()
        {
            return _context.Topics.ToList();
        }
    }
}