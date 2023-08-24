
using System.Threading.Tasks;
using System.Web.Http;

using iXlsxWriter.Abstractions.Writer.Operations.Actions;

using iXlsxWriter.AspNet.WebApi.Code;

namespace iXlsxWriter.AspNet.WebApi.Controllers;

public class XlsxAsyncController : ApiController
{
    public async Task GetAsync()
    {
        var result = await Sample01.GenerateAsync();
        if (result.Success)
        {
            var safeOutputData = result.Result;
            var downloadResult = await safeOutputData.Action(new DownloadAsync());
            if (!downloadResult.Success)
            {
                // Handle error(s)
            }
        }
    }
}
