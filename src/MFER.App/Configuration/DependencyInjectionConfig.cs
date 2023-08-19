using MFER.App.Extensions;
using MFER.Business.Interfaces;
using MFER.Data.Context;
using MFER.Data.Repository;
using Microsoft.AspNetCore.Mvc.DataAnnotations;

namespace MFER.App.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection ResolveDependencies(this IServiceCollection services) 
        {

            services.AddScoped<MeuDBContext>();
            services.AddScoped<IProdutoRepository, ProdutoRepository>();
            services.AddScoped<IFornecedorRepository, FornecedorRepository>();
            services.AddScoped<IEnderecoRepository, EnderecoRepository>();
            services.AddSingleton<IValidationAttributeAdapterProvider, MoedaValidationAttributeAdapterProvider>();

            return services;
        }

    }
}
