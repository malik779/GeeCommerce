using Gee.Core.BaseInfrastructure;
using Gee.Core.Extensions;
using Gee.Core.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using TenantApi.Service.Infrastructure.Domain;
using TenantApi.Service.Infrastructure.ModelPrepareFactory;

namespace TenantApi.Service.Controllers
{
    [Route("api/tenant/[Action]")]
    [ApiController]
    [Authorize]
    public class TenantController : ControllerBase
    {
        private IBaseRepository<Tenant> _repository;

        private TenantModelFactory _modelPrepareFactory;
        public TenantController(IBaseRepository<Tenant> repository, TenantModelFactory modelPrepareFactory) { 
            _repository = repository;
            _modelPrepareFactory= modelPrepareFactory;
        }
        [HttpPost]
        public async Task<IActionResult> CreateAsync(TenantModel request)
        {
            var tenant = request.ToEntity<Tenant>();
            tenant.CreatedDate = DateTime.UtcNow;
            tenant.CreatedBy = 1037;
            await _repository.InsertAsync(tenant);
            _repository.Save();

            var response = new Response<TenantModel>(tenant.ToModel<TenantModel>(), System.Net.HttpStatusCode.OK, "success!");
            return StatusCode((int)response.StatusCode,response.Data);
        }
        [HttpGet]
        public async Task<IActionResult> GetById(int Id)
        {

            var tenant = await _repository.GetByIdAsync(Id, static cache => default);

            if (tenant == null)
            {
                return StatusCode((int)HttpStatusCode.OK, "No record found!");
            }

            var model = await _modelPrepareFactory.PrepareWithDataModelAsync(new TenantModel(), tenant);
            return StatusCode((int)HttpStatusCode.OK,model);
        }
    }
}
