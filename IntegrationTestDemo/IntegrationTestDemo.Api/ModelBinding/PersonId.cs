using Microsoft.AspNetCore.Mvc;
using System;

namespace IntegrationTestDemo.Api.ModelBinding
{
    [ModelBinder(BinderType = typeof(PersonResolver))]
    public class PersonId
    {
        public Guid UniqueId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
    }
}
