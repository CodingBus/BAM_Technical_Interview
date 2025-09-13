using Dapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using StargateAPI.Business.Data;
using StargateAPI.Business.Dtos;
using StargateAPI.Controllers;

namespace StargateAPI.Business.Queries
{
    public class GetPeople : IRequest<GetPeopleResult>
    {

    }

    public class GetPeopleHandler : IRequestHandler<GetPeople, GetPeopleResult>
    {
        public readonly StargateContext _context;
        public GetPeopleHandler(StargateContext context)
        {
            _context = context;
        }
        public async Task<GetPeopleResult> Handle(GetPeople request, CancellationToken cancellationToken)
        {
            var result = new GetPeopleResult();

            var people = await _context.People
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
                    CareerStartDate = p.AstronautDetail != null ? p.AstronautDetail.CareerStartDate : null,
                    CareerEndDate = p.AstronautDetail != null ? p.AstronautDetail.CareerEndDate : null,
                    AstronautDuties = p.AstronautDuties.OrderByDescending(d => d.DutyStartDate).ToList()
                })
                .ToListAsync(cancellationToken);

            result.People = people;
            return result;
        }

    }

    public class GetPeopleResult : BaseResponse
    {
        public List<PersonAstronaut> People { get; set; } = new List<PersonAstronaut> { };

    }
}
