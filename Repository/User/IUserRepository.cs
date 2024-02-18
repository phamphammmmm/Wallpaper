using Wallpaper.DTO;
using Wallpaper.Entities;

namespace Wallpaper.Repository.User
{
    public interface IUserRepository
    {
        Task<List<Entities.UserEntity>> GetAll();
        Task<Entities.UserEntity> GetUserById(int id);
        Task<Entities.UserEntity> CreateUserAsync(UserEntity userEntity);

    }
}
