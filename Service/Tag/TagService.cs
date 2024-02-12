using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wallpaper.Context;
using Wallpaper.Entities;

namespace Wallpaper.Service.Tag
{
    public class TagService : ITagService
    {
        private readonly ApplicationDbContext _context;

        public TagService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int[]> GetOrCreateTagsAsync(string[] tags)
        {
            List<int> tagIds = new List<int>();

            foreach (var tagString in tags)
            {
                string trimmedTag = tagString.Trim();

                var existingTag = await _context.Tags.FirstOrDefaultAsync(t => t.Name == trimmedTag);

                if (existingTag == null)
                {
                    existingTag = new Entities.Tag { Name = trimmedTag };
                    _context.Tags.Add(existingTag);
                    await _context.SaveChangesAsync();
                }

                tagIds.Add(existingTag.Id);
            }

            return tagIds.ToArray();
        }
    }
}
