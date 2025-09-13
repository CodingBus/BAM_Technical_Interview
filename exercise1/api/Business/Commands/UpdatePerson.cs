using MediatR;
using MediatR.Pipeline;
using Microsoft.EntityFrameworkCore;
using StargateAPI.Business.Data;
using StargateAPI.Controllers;
using Microsoft.Extensions.Logging;

namespace StargateAPI.Business.Commands
{
    public class UpdatePerson : IRequest<UpdatePersonResult>
    {
        public string Name { get; set; } = string.Empty;
        public string NewName { get; set; } = string.Empty;
    }

    public class UpdatePersonPreProcessor : IRequestPreProcessor<UpdatePerson>
    {
        private readonly StargateContext _context;
        private readonly ILogger<UpdatePersonPreProcessor> _logger;
        public UpdatePersonPreProcessor(StargateContext context, ILogger<UpdatePersonPreProcessor> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task Process(UpdatePerson request, CancellationToken cancellationToken)
        {
            var person = await _context.People.AsNoTracking().AnyAsync(z => z.Name == request.Name, cancellationToken);
            _logger.LogWarning($"A person with the name '{request.Name}' does not exist.");
            if (!person) throw new BadHttpRequestException($"A person with the name '{request.Name}' does not exist.");
        }
    }

    public class UpdatePersonHandler : IRequestHandler<UpdatePerson, UpdatePersonResult>
    {
        private readonly StargateContext _context;
        private readonly ILogger<UpdatePersonHandler> _logger;

        public UpdatePersonHandler(StargateContext context, ILogger<UpdatePersonHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<UpdatePersonResult> Handle(UpdatePerson request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Updating person with Name '{request.Name}' to new Name '{request.NewName}'");

            var person = await _context.People.FirstOrDefaultAsync(p => p.Name == request.Name, cancellationToken);
            _logger.LogWarning($"Person with name '{request.Name}' does not exist.");
            if (person == null) throw new BadHttpRequestException($"Person with name '{request.Name}' does not exist."); // This should not happen due to preprocessor

            // check new name not taken
            var newNameTaken = await _context.People.AnyAsync(p => p.Name == request.NewName && p.Id != person.Id, cancellationToken);
            _logger.LogWarning($"A person with the name '{request.NewName}' already exists.");
            if (newNameTaken)throw new BadHttpRequestException($"A person with the name '{request.NewName}' already exists.");

            person.Name = request.NewName;

            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation($"Updated person ID {person.Id} to new Name '{person.Name}'");
            return new UpdatePersonResult()
            {
                Id = person.Id
            };
        }
    }

    public class UpdatePersonResult : BaseResponse
    {
        public int Id { get; set; }
    }
}
