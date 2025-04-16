using Gee.Core.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gee.Core.BaseInfrastructure.DataProviders.Certificate
{
    public class CertificateProvider : ICertificateProvider
    {
        private readonly IConfiguration _configuration;
        private readonly IBaseDataProvider _baseDataProvider;
        private readonly IHostEnvironment _env;
        public CertificateProvider(IConfiguration configuration, IBaseDataProvider baseDataProvider,IHostEnvironment env)
        {
            _configuration = configuration;
            _baseDataProvider = baseDataProvider;
            _env = env;
        }
        public CertificateDetail GetCertificateDetail()
        {
            var certificateDetail = new CertificateDetail();
            // Fall back to appsettings.json if database value is not found
            certificateDetail.CertificatePath = Path.Combine(_env.ContentRootPath, _configuration.GetRequiredValue("Certificates:Path"));
            certificateDetail.CertificatePassword = _configuration?.GetRequiredValue("Certificates:Password");
            return certificateDetail;
        }
    }
}
