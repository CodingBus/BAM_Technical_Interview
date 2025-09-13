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
            
            if (!person)
            {
                var nameNotFoundMessage = $"A person with the name '{request.Name}' does not exist.";

                _logger.LogWarning(nameNotFoundMessage);

                await _context.LogToDatabaseAsync(nameof(UpdatePersonPreProcessor), LogLevel.Warning, nameNotFoundMessage);

                throw new BadHttpRequestException(nameNotFoundMessage);
            }
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
            {
                var updateMessage = $"Updating person with Name '{request.Name}' to new Name '{request.NewName}'";
                _logger.LogInformation($"Updating person with Name '{request.Name}' to new Name '{request.NewName}'");
                await _context.LogToDatabaseAsync(nameof(UpdatePersonHandler), LogLevel.Information, updateMessage);
            }

            var person = await _context.People.FirstOrDefaultAsync(p => p.Name == request.Name, cancellationToken);
           
            if (person == null)
            {
                var personNotFoundMessage = $"Person with name '{request.Name}' does not exist.";

                _logger.LogWarning(personNotFoundMessage);

                await _context.LogToDatabaseAsync(nameof(UpdatePersonHandler), LogLevel.Warning, personNotFoundMessage);

                throw new BadHttpRequestException($"Person with name '{request.Name}' does not exist.");
            }

            // check new name not taken
            var newNameTaken = await _context.People.AnyAsync(p => p.Name == request.NewName && p.Id != person.Id, cancellationToken);

            
            if (newNameTaken)
            {
                var newNameCollisionMessage = $"A person with the name '{request.NewName}' already exists.";

                _logger.LogWarning(newNameCollisionMessage);

                await _context.LogToDatabaseAsync(nameof(UpdatePersonHandler), LogLevel.Warning, newNameCollisionMessage);

                throw new BadHttpRequestException(newNameCollisionMessage);
            }

            person.Name = request.NewName;

            await _context.SaveChangesAsync(cancellationToken);

            {
                var updateSuccessMessage = $"Updated person ID {person.Id} to new Name '{person.Name}'";
                _logger.LogInformation(updateSuccessMessage);
                await _context.LogToDatabaseAsync(nameof(UpdatePersonHandler), LogLevel.Information, updateSuccessMessage);
            }

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
