using BobsCorn.Domain.Entities;
using BobsCorn.Infrastructure.Utilities;

namespace BobsCorn.Infrastructure.Data
{
    public static class InMemorySeeder
    {
        public static void Seed(BobCornDbContext context)
        {
            if (context.Products.Any())
            {
                return;
            }

            var users = new List<User>
            {
                new User
                {
                    UserId = 1,
                    Email = "test@test.com",
                    Password = EncryptionUtils.Encrypt("password123"),
                    Name = "Test",
                    Lastname = "User"
                }
            };

            context.Users.AddRange(users);
            context.SaveChanges();

            var products = new List<Product>
        {
            new Product { ProductId = 1, Name = "Corn", Quantity=100 },
        };

            context.Products.AddRange(products);
            context.SaveChanges();
        }
    }
}
