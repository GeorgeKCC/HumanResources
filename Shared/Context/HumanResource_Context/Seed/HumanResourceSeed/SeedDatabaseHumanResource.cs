namespace Shared.Context.HumanResource_Context.Seed.HumanResourceSeed
{
    internal static class SeedDatabaseHumanResource
    {
        public static async Task InitDatabase(IServiceProvider services)
        {
            using var scope = services.CreateScope();

            var db = scope.ServiceProvider
                .GetRequiredService<DatabaseHumanResourceContext>();

            await db.Database.MigrateAsync();

           var existEmail = await db.Colaborators.FirstOrDefaultAsync(x => x.Email == "george@gmail.com");

            if (existEmail is null)
            {
                db.Colaborators.Add(new Colaborator()
                {
                    Name = "george",
                    LastName = "calderon",
                    Email = "george@gmail.com",
                    DocumentType = "CE",
                    DocumentNumber = "334444"
                });

                db.Securities.Add(new Security()
                {
                    Email = "george@gmail.com",
                    Password = "jezM06GGS8yord7fmrwEKbx81NqH1gCc+fr/gQaQa7w=",
                    Salt = "HGXy/IsoVhbY7/LxaoklLg==",
                    Active = true,
                    ColaboratorId = 1
                });

                await db.SaveChangesAsync();
            }
        }
    }
}