﻿using MediatR;
using Microsoft.EntityFrameworkCore;
using QrToPay.Api.Common.Enums; //TODO to wróci jak zmienimy baze
using QrToPay.Api.Common.Results;
using QrToPay.Api.Models;

namespace QrToPay.Api.Features.Cities
{
    public class GetCitiesHandler : IRequestHandler<GetCitiesRequestModel, Result<IEnumerable<CitiesDto>>>
    {
        private readonly QrToPayDbContext _context;

        public GetCitiesHandler(QrToPayDbContext context)
        {
            _context = context;
        }

        public async Task<Result<IEnumerable<CitiesDto>>> Handle(GetCitiesRequestModel request, CancellationToken cancellationToken)
        {
            string entityTypeString = request.EntityType.ToString();

            var cities = await _context.Entities
                .Where(e => !e.IsDeleted && e.EntityType == entityTypeString) //TODO zmiana tego na bazie na enum (0,1) a nie SkiResort/FunFair
                .Select(e => new CitiesDto
                {
                    CityName = e.CityName!,
                    EntityId = e.EntityId
                })
                .Distinct()
                .ToListAsync(cancellationToken);

            if (cities.Count == 0)
            {
                return Result<IEnumerable<CitiesDto>>.Failure($"Nie znaleziono miast dla, {request.EntityType}.");
            }

            return Result<IEnumerable<CitiesDto>>.Success(cities);
        }
    }
}