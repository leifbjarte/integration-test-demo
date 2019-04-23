using System;

namespace IntegrationTestDemo.Api.ErrorHandling
{
    public class SomeCustomError : Exception
    {
        public SomeCustomError() : base("Some custom error occured")
        {
        }
    }
}
