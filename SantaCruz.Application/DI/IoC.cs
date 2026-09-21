using Microsoft.Extensions.DependencyInjection;
using SantaCruz.Application.Chain;
using SantaCruz.Application.Context;
using SantaCruz.Application.Service;
using SantaCruz.Application.UseCases;
using SantaCruz.Domain.Integration;
using SantaCruz.Domain.Repository;
using SantaCruz.Domain.Service;
using SantaCruz.Domain.UseCases;
using SantaCruz.Infrastructure.Integrations;
using SantaCruz.Infrastructure.NpgDbRepository;

namespace SantaCruz.Application.DI
{
    public static class IoC
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<IDatabaseContext, NpgDbContext>();

            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IProductService, ProductService>();

            services.AddScoped<MainProcessChain>();
            services.AddScoped<OrderProcessChain>();

            services.AddScoped<IGetPendingOrders, GetPendingOrders>();
            services.AddScoped<IProcessIntegration, ProcessIntegration>();
            services.AddScoped<IProcessingAttempts, ProcessingAttempts>();
            services.AddScoped<IUpdateToNextStatus, UpdateToNextStatus>();
            services.AddScoped<IProcessBegin, ProcessBegin>();
            services.AddScoped<ILogging, Logging>();

            services.AddScoped<IMockedIntegration, MockedIntegration>();

            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IOrderItemRepository, OrderItemRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IOrderProcessingAttemptRepository, OrderProcessingAttemptRepository>();

            return services;
        }
    }
}
