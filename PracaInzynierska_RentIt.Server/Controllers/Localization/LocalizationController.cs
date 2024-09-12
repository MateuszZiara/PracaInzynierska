using Microsoft.AspNetCore.Mvc;
using PracaInzynierska_RentIt.Server.Models.Application;
using PracaInzynierska_RentIt.Server.Models.Localization;
using PracaInzynierska_RentIt.Server.Persistence.Localization;

namespace PracaInzynierska_RentIt.Server.Controllers.Localization;
[Route("api/[controller]")]
[ApiController]
public class LocalizationController : ApplicationController<LocalizationEntity,LocalizationService,LocalizationRepository,LocalizationEntity>
{
    public LocalizationController(LocalizationService service) : base(service)
    {
        
    }
}