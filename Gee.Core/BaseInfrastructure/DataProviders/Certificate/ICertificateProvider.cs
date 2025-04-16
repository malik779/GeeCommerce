using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gee.Core.BaseInfrastructure.DataProviders.Certificate
{
    public interface ICertificateProvider
    {
        public CertificateDetail GetCertificateDetail();
    }
}
