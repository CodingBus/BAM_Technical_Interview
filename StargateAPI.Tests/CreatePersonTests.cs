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
            // Use a unique in-memory database for each test
            var options = new DbContextOptionsBuilder<StargateContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new StargateContext(options);
        }

        // Seed a person to test duplicate name prevention

        [Fact]
        public async Task PreProcessor_ShouldPreventDuplicateNames()
        {
            var context = GetInMemoryContext();

            context.People.Add(new Person { Name = "John Doe" });
            await context.SaveChangesAsync();

            var logger = NullLogger<CreatePersonHandler>.Instance;
            var preProcessor = new CreatePersonPreProcessor(context, logger);

            var request = new CreatePerson
            {
                Name = "John Doe"
            };

            await Assert.ThrowsAsync<BadHttpRequestException>(async () =>
            {
                await preProcessor.Process(request, CancellationToken.None);
            });
        }

        // Test that a person is created successfully

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

        // Test that a log entry is created when a person is created

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
