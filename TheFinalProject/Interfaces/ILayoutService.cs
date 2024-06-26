using System;
using TheFinalProject.ViewModels.BasketViewModels;

namespace TheFinalProject.Interfaces
{
    public interface ILayoutService
    {
        Task<IEnumerable<BasketVM>> GetBaskets();
    }
}

