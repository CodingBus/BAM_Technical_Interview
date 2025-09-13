using Dapper;
using MediatR;
using MediatR.Pipeline;
using Microsoft.EntityFrameworkCore;
using StargateAPI.Business.Data;
using StargateAPI.Business.Dtos;
using StargateAPI.Controllers;

namespace StargateAPI.Business.Queries
{
    public class GetPersonByName : IRequest<GetPersonByNameResult>
    {
        public required string Name { get; set; } = string.Empty;
    }

    public class GetPersonByNamePreProcessor : IRequestPreProcessor<GetPersonByName>
    {
        private readonly StargateContext _context;

        public GetPersonByNamePreProcessor(StargateContext context)
        {
            _context = context;
        }

        public async Task Process(GetPersonByName request, CancellationToken cancellationToken)
        {
            var personExists = await _context.People.AsNoTracking().AnyAsync(p => p.Name == request.Name, cancellationToken);

            if (!personExists)
            {
                throw new BadHttpRequestException($"No person found with name '{request.Name}'");
            }

            var hasDuties = await _context.AstronautDuties.AsNoTracking().AnyAsync(d => d.Person!.Name == request.Name, cancellationToken);

            if (!hasDuties)
            {
                throw new BadHttpRequestException($"'{request.Name}' has no astronaut duties");
            }
        }
    }

    public class GetPersonByNameHandler : IRequestHandler<GetPersonByName, GetPersonByNameResult>
    {
        private readonly StargateContext _context;
        public GetPersonByNameHandler(StargateContext context)
        {
            _context = context;
        }

        public async Task<GetPersonByNameResult> Handle(GetPersonByName request, CancellationToken cancellationToken)
        {
            var result = new GetPersonByNameResult();

            var person = await _context.People
                .Where(p => p.Name == request.Name)
                .Include(p => p.AstronautDetail)
                    .ThenInclude(d => d != null ? d.CurrentRank : null)
                .Include(p => p.AstronautDetail)
                    .ThenInclude(d => d != null ? d.CurrentDutyTitle : null)
                .Include(p => p.AstronautDuties)
                .Select(p => new PersonAstronaut
                {
                    PersonId = p.Id,
                    Name = p.Name,
                    CurrentRank = p.AstronautDetail != null ? p.AstronautDetail.CurrentRank.Name : string.Empty,
                    CurrentDutyTitle = p.AstronautDetail != null ? p.AstronautDetail.CurrentDutyTitle.Name : string.Empty,
                    CareerStartDate = p.AstronautDetail != null ? p.AstronautDetail.CareerStartDate : (DateTime?)null,
                    CareerEndDate = p.AstronautDetail != null ? p.AstronautDetail.CareerEndDate : (DateTime?)null,
                    AstronautDuties = p.AstronautDuties.OrderByDescending(d => d.DutyStartDate).ToList()
                })
                .FirstOrDefaultAsync(cancellationToken);

            result.Person = person;

            return result;
        }
    }

    public class GetPersonByNameResult : BaseResponse
    {
        public PersonAstronaut? Person { get; set; }
    }
}
