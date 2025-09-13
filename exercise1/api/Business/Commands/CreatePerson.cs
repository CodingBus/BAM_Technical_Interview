using MediatR;
using MediatR.Pipeline;
using Microsoft.EntityFrameworkCore;
using StargateAPI.Business.Data;
using StargateAPI.Controllers;
using Microsoft.Extensions.Logging;

namespace StargateAPI.Business.Commands
{
    public class CreatePerson : IRequest<CreatePersonResult>
    {
        public required string Name { get; set; } = string.Empty;
    }

    public class CreatePersonPreProcessor : IRequestPreProcessor<CreatePerson>
    {
        private readonly StargateContext _context;
        private readonly ILogger<CreatePersonHandler> _logger;
        public CreatePersonPreProcessor(StargateContext context, ILogger<CreatePersonHandler> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task Process(CreatePerson request, CancellationToken cancellationToken)
        {
            var person = await _context.People.AsNoTracking().AnyAsync(z => z.Name == request.Name, cancellationToken);

            _logger.LogWarning($"Person with name '{request.Name}' already exists: {person}");
            if (person) throw new BadHttpRequestException($"A person with the name '{request.Name}' already exists.");
        }
    }

    public class CreatePersonHandler : IRequestHandler<CreatePerson, CreatePersonResult>
    {
        private readonly StargateContext _context;
        private readonly ILogger<CreatePersonHandler> _logger;

        public CreatePersonHandler(StargateContext context, ILogger<CreatePersonHandler> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<CreatePersonResult> Handle(CreatePerson request, CancellationToken cancellationToken)
        {

                var newPerson = new Person()
                {
                   Name = request.Name
                };

                await _context.People.AddAsync(newPerson);

                await _context.SaveChangesAsync();

                _logger.LogInformation($"Created new person with ID {newPerson.Id} and Name '{newPerson.Name}'");

                return new CreatePersonResult()
                {
                    Id = newPerson.Id
                };
          
        }
    }

    public class CreatePersonResult : BaseResponse
    {
        public int Id { get; set; }
    }
}
