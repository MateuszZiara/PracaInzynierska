using Microsoft.AspNetCore.Mvc;
using PracaInzynierska_RentIt.Server.Models.AspNetUsersEntity.Dtos;
using PracaInzynierska_RentIt.Server.Models.Localization;

namespace PracaInzynierska_RentIt.Server.Persistence.Localization;

public class LocalizationService : ILocalizationService
{
    public LocalizationRepository Repository { get; }
    public LocalizationService(LocalizationRepository repository)
    {
        this.Repository = repository;
    }
}