using Wallpaper.DTO.Wallpepar;
using Wallpaper.Entities;

namespace Wallpaper.Service.Wallpaper.Inteface
{
    public interface IWallpaperService
    {
        Task<Entities.Wallpaper> Details(int? id);
        Task<bool> CreateWallpaperAsync(WallpaperCreate_DTO wallpaperCreate_DTO);
        Task<bool> UpdateWallpaperAsync(int? id, WallpaperEdit_DTO wallpaperEdit_DTO);
        Task<bool> DeleteWallpaperAsync(int? id);
    }
}
