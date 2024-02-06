using Contracts;
using Service.Contracts;

namespace Service
{
    public sealed class ServiceManager : IServiceManager
    {
        private readonly Lazy<IBTCSpyService> _btcspyService;

        public ServiceManager(ILoggerManager logger)
        {
            _btcspyService = new Lazy<IBTCSpyService>(() =>
                new BTCSpyService(logger));
        }

        public IBTCSpyService BTCSpyService => _btcspyService.Value;
    }
}
