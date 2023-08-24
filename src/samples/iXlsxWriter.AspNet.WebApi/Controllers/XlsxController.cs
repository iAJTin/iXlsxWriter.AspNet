
using System.Web.Http;

using iXlsxWriter.Abstractions.Writer.Operations.Results;

using iXlsxWriter.AspNet.WebApi.Code;

namespace iXlsxWriter.AspNet.WebApi.Controllers
{
    public class XlsxController : ApiController
    {
        public void Get()
        {
            var downloadResult = Sample01.Generate().Download();
            if (!downloadResult.Success)
            {
                // Handle error(s)
            }
        }
    }
}
