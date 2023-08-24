
using System.Threading;
using System.Threading.Tasks;

using iXlsxWriter.Abstractions.Writer.Operations.Results;
using iXlsxWriter.Operations.Insert;

namespace iXlsxWriter.AspNet.WebApi.Code;

/// <summary>
/// Shows the use for create a xlsx file.
/// </summary>
internal static class Sample01
{
    public static XlsxInput BuildXlsxInput()
    {
        var doc = XlsxInput.Create("Sheet1");

        doc.Insert(
            new InsertText
            {
                SheetName = "Sheet1",
                Data = "Hello world! from iXlsxWriter"
            });

        return doc;
    }

    public static OutputResult Generate() =>
        BuildXlsxInput().CreateResult();


    public static async Task<OutputResult> GenerateAsync(CancellationToken cancellationToken = default) =>
        await BuildXlsxInput().CreateResultAsync(cancellationToken: cancellationToken);
}
