using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Threading.Tasks;

namespace IntegrationTestDemo.Api.ModelBinding
{
    public class PersonResolver : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            //TODO: Do resolving-stuff, check DB, call external API or whatever

            bindingContext.Result = ModelBindingResult.Success(new PersonId
            {
                Name = "Test Testesen",
                Email = "test@testesen.com",
                UniqueId = Guid.NewGuid()
            });

            return Task.CompletedTask;
        }
    }
}
