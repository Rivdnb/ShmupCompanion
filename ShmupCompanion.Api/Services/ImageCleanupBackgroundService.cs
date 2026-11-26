using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ShmupCompanion.Api.Interfaces;

namespace ShmupCompanion.Api.Services
{
    public class ImageCleanupBackgroundService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<ImageCleanupBackgroundService> _logger;
        public ImageCleanupBackgroundService(IServiceScopeFactory scopeFactory,
                                             ILogger<ImageCleanupBackgroundService> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        private async Task CleanupImagesAsync(CancellationToken cancellationToken)
        {
            try
            {
                await using var scope = _scopeFactory.CreateAsyncScope();

                var noteImageManagerService = scope.ServiceProvider
                    .GetRequiredService<INoteImageManagerService>();

                await noteImageManagerService.DeleteNoteImagesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while cleaning up noteImages.");
            }
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Booting Image Cleanup Background Service");

            while (!cancellationToken.IsCancellationRequested)
            {
                _logger.LogInformation("Starting Image Cleanup at: {time}", DateTimeOffset.Now);

                // Cleanup noteImages on boot
                await CleanupImagesAsync(cancellationToken);

                // Afterwards clean up noteImages every hour
                using var timer = new PeriodicTimer(TimeSpan.FromHours(1));

                while (await timer.WaitForNextTickAsync(cancellationToken))
                {
                    await CleanupImagesAsync(cancellationToken);
                }
            }

            _logger.LogInformation("Background Service is stopping.");
        }
    }
}
