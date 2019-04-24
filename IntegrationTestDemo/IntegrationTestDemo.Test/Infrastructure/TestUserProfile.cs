using System;
using System.Collections.Generic;
using System.Text;

namespace IntegrationTestDemo.Test.Infrastructure
{
    public class TestUserProfile
    {
        public Guid UniqueId { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public static TestUserProfile TestTestesen => new TestUserProfile
        {
            UniqueId = Guid.NewGuid(),
            Name = "Test Testesen",
            Email = "test@testesen.com"
        };
    }
}
