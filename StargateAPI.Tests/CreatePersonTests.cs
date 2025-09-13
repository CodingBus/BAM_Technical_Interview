using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using StargateAPI.Business.Commands;
using StargateAPI.Business.Data;


namespace StargateAPI.Tests
{
    public class CreatePersonTests
    {
        private StargateContext GetInMemoryContext()
        {
            var options = new DbContextOptionsBuilder<StargateContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            return new StargateContext(options);
        }

        [Fact]
        public async Task PreProcessor_ShouldPreventDuplicateNames()
        {
            var context = GetInMemoryContext();

            // Seed a person
            context.People.Add(new Person { Name = "John Doe" });
            await context.SaveChangesAsync();

            var logger = NullLogger<CreatePersonHandler>.Instance;
            var preProcessor = new CreatePersonPreProcessor(context, logger);

            var request = new CreatePerson
            {
                Name = "John Doe" // Duplicate name
            };

            await Assert.ThrowsAsync<BadHttpRequestException>(async () =>
            {
                await preProcessor.Process(request, CancellationToken.None);
            });
        }

        [Fact]
        public async Task Handle_ShouldCreatePerson()
        {
            var context = GetInMemoryContext();

            var logger = NullLogger<CreatePersonHandler>.Instance;
            var handler = new CreatePersonHandler(context, logger);

            var request = new CreatePerson
            {
                Name = "Devin Perez"
            };

            var result = await handler.Handle(request, CancellationToken.None);

            var createdPerson = await context.People.FirstOrDefaultAsync(p => p.Id == result.Id);
            Assert.NotNull(createdPerson);
            Assert.Equal("Devin Perez", createdPerson.Name);
        }

        [Fact]       
        public async Task Handle_ShouldLogCreation()
        {
            var context = GetInMemoryContext();
            var logger = NullLogger<CreatePersonHandler>.Instance;
            var handler = new CreatePersonHandler(context, logger);
            var request = new CreatePerson
            {
                Name = "John Smith"
            };
            var result = await handler.Handle(request, CancellationToken.None);
            var logEntry = await context.Logs.FirstOrDefaultAsync(log => log.Message.Contains("John Smith"));
            Assert.NotNull(logEntry);
            Assert.Contains("John Smith", logEntry.Message);
        }
    }
}
