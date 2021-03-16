using System.Threading.Tasks;
using GalaSoft.MvvmLight.Views;

namespace ShapeDatasetGeneratorWPF.Services
{
    public interface IFrameNavigationService : INavigationService
    {
        object Parameter { get; }
        Task NavigateToAsync(string pageKey);
    }
}
