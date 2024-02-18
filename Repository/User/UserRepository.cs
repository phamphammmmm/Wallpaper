using Microsoft.EntityFrameworkCore;
using Wallpaper.Context;
using Wallpaper.Entities;

namespace Wallpaper.Repository.User
{
    public class UserRepository : IUserRepository
    {
        public readonly ApplicationDbContext _context;
        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<UserEntity>> GetAll()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<UserEntity> CreateUserAsync(UserEntity userEntity)
        {
            _context.Add(userEntity);
            await _context.SaveChangesAsync();
            return userEntity;
        }

        public async Task<UserEntity>GetUserById(int id)
        {
            return await _context.Users.FirstOrDefaultAsync(m => m.Id == id);
        }
    }
}
