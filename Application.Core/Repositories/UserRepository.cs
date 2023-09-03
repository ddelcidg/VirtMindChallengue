using Application.Data.Models;
using Application.Data.Context;

namespace Application.Core.Repositories
{


    public class UserRepository
    {
        private readonly CurrencyExchangeContext _context;

        public UserRepository(CurrencyExchangeContext context)
        {
            _context = context;
        }

        // Get user by ID
        public User GetUserById(string userId)
        {
            return _context.Users.FirstOrDefault(u => u.UserId == userId);
        }

        // Add a new user
        public void AddUser(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
        }

        // You can add more user-related database interactions as needed
    }

}
