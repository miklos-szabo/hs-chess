using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace HSC.Api.Extensions
{
    public static class OptionsExtensions
    {
        public static TOption ConfigureOption<TOption>(this IServiceCollection services, IConfiguration configuration, IValidator<TOption> validator = null)
            where TOption : class, new()
        {
            var options = new TOption();

            // To use in current method
            configuration.GetSection(typeof(TOption).Name).Bind(options);

            // For IOption<TOption> based DI usage
            services.AddOptions<TOption>(typeof(TOption).Name);
            services.Configure<TOption>(configuration.GetSection(typeof(TOption).Name));

            if (validator != null)
            {
                var validationResult = validator.Validate(options);
                if (!validationResult.IsValid)
                {
                    throw new OptionsValidationException(
                        typeof(TOption).Name,
                        typeof(TOption),
                        validationResult.Errors.Select(e => e.ErrorMessage));
                }
            }

            return options;
        }
    }
}
