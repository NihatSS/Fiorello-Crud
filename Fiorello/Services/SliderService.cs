using ElearnApp.Data;
using Fiorello.Models;
using Fiorello.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Fiorello.Services
{
    public class SliderService : ISliderService
    {
        private readonly AppDbContext _context;

        public SliderService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<Slider> GetAsync()
        {
            return await _context.Sliders.FirstOrDefaultAsync();
        }
    }
}
