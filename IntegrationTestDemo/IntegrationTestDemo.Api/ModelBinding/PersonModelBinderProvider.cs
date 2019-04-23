using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;

namespace IntegrationTestDemo.Api.ModelBinding
{
    public class PersonModelBinderProvider : IModelBinderProvider
    {
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            if (context.Metadata.ModelType == typeof(PersonId))
            {
                return new BinderTypeModelBinder(typeof(PersonResolver));
            }

            return null;
        }
    }
}
