using MediatR;
using MediatR.Pipeline;
using Microsoft.EntityFrameworkCore;
using StargateAPI.Business.Data;
using StargateAPI.Controllers;

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
        public UpdatePersonPreProcessor(StargateContext context)
        {
            _context = context;
        }
        public async Task Process(UpdatePerson request, CancellationToken cancellationToken)
        {
            var person = await _context.People.AsNoTracking().AnyAsync(z => z.Name == request.Name, cancellationToken);

            if (!person) throw new BadHttpRequestException($"A person with the name '{request.Name}' does not exist.");
        }
    }

    public class UpdatePersonHandler : IRequestHandler<UpdatePerson, UpdatePersonResult>
    {
        private readonly StargateContext _context;

        public UpdatePersonHandler(StargateContext context)
        {
            _context = context;
        }

        public async Task<UpdatePersonResult> Handle(UpdatePerson request, CancellationToken cancellationToken)
        {
            var person = await _context.People.FirstOrDefaultAsync(p => p.Name == request.Name, cancellationToken);
            if (person == null) throw new BadHttpRequestException($"Person with name '{request.Name}' does not exist."); // This should not happen due to preprocessor

            // check new name not taken
            var newNameTaken = await _context.People.AnyAsync(p => p.Name == request.NewName && p.Id != person.Id, cancellationToken);
            if (newNameTaken)throw new BadHttpRequestException($"A person with the name '{request.NewName}' already exists.");

            person.Name = request.NewName;

            await _context.SaveChangesAsync(cancellationToken);

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
