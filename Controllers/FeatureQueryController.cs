using System;
using System.Linq;
using WebApp.Models;
using WebApp.Services;
using WebApp.Common;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers
{
    [Route("api/[controller]")]
    public class FeatureQueryController : ControllerBase
    {
        private readonly FeatureQueryService _featureQueryService;

        public FeatureQueryController(FeatureQueryService featureQueryService)
        {
            _featureQueryService = featureQueryService;
        }

        /// <summary>
        /// Поиск по клику на карте.
        /// </summary>
        /// <param name="query"> Информация по объекту. </param>
        [HttpPost]
        [Route("geo")]
        //[IsActionAllowed(Constants.AreaName, ActionsStrings.INFORMATION_TOOL)]
        public ActionResult PerformGeospatialQuery([FromBody] DtoFeaturesGeoQuery query)
        {
            try
            {
                var foundFeatures = _featureQueryService.SearchFeaturesByPoint(query);
                var dateChangeFeatures = _featureQueryService.ConvertDateTimeAttribute(foundFeatures).ToList();

                return Ok(Protocol.Response.CreatePayload(new DtoSearchResult(dateChangeFeatures)));
            }
            catch (Exception ex)
            {
                Logger.Error("Ошибка получения информации об объекте.", ex);
                return Ok(Protocol.Response.CreateError(
                    "Во время операции произошла ошибка. Обратитесь к администратору системы"));
            }
        }
    }
}
