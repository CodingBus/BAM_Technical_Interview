using Dapper;
using MediatR;
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
