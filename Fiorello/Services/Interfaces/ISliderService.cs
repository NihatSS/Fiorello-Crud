using Fiorello.Models;

namespace Fiorello.Services.Interfaces
{
    public interface ISliderService
    {
        Task<Slider> GetAsync();
    }
}
