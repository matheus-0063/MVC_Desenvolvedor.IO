using Microsoft.AspNetCore.Localization;
using System.Globalization;

namespace MFER.App.Configuration
{
    public static class GlobalizationConfig
    {
        public static IApplicationBuilder UseGlobalizationConfig(this IApplicationBuilder app)
        {
            var defaultCulture = new CultureInfo("pt-BR");
            //Criando uma variavel para definir qual a linguagem do nosso sistema
            var localizationOptions = new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture(defaultCulture),
                //Nosso request será em pt-BR
                SupportedCultures = new List<CultureInfo> { defaultCulture },
                //Aqui a gente está listando quais as culturas serão aceitas, podendo listar mais de uma
                SupportedUICultures = new List<CultureInfo> { defaultCulture }
            };

            app.UseRequestLocalization(localizationOptions);
            //Aqui eu estou passando para a aplicação a configuração de linguagem que eu quero ter na aplicação


            return app;
        }
    }
}
