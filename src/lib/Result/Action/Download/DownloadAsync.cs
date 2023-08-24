
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

using iTin.AspNet.Web;
using iTin.AspNet.Web.ComponentModel;

using iTin.Core.ComponentModel;
using iTin.Core.ComponentModel.Results;

using iXlsxWriter.Abstractions.Writer.Operations.Results;

namespace iXlsxWriter.Abstractions.Writer.Operations.Actions;

/// <summary>
/// Specialization of <see cref="IOutputAction"/> interface that downloads the file.
/// </summary>
/// <seealso cref="IOutputAction"/>
public class DownloadAsync : IOutputActionAsync
{
    #region private constants

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private const string XlsxExtension = "xlsx";

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private const string ZipExtension = "zip";

    #endregion

    #region interfaces

    #region IOutputActionAsync

    /// <summary>
    /// Execute action asynchronously for specified output result.
    /// </summary>
    /// <param name="context">Target output result data.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>
    /// <para>
    /// A <see cref="BooleanResult"/> which implements the <see cref="iTin.Core.ComponentModel.IResult{T}"/> interface reference that contains the result of the operation, to check if the operation is correct, the <b>Success</b>
    /// property will be <b>true</b> and the <b>Value</b> property will contain the value; Otherwise, the the <b>Success</b> property
    /// will be false and the <b>Errors</b> property will contain the errors associated with the operation, if they have been filled in.
    /// </para>
    /// <para>
    /// The type of the return value is <see cref="bool"/>, which contains the operation result
    /// </para>
    /// </returns>
    public async Task<IResult> ExecuteAsync(IOutputResultData context, CancellationToken cancellationToken = default) => 
        await ExecuteImplAsync(context, cancellationToken);

    #endregion

    #endregion

    #region public properties   

    /// <summary>
    /// Gets or sets the current http context.
    /// </summary>
    /// <value>
    /// The current http context.
    /// </value>
    public HttpContext Context { get; set; }

    /// <summary>
    /// Gets or sets the filename output path. The use of the <b>~</b> character is allowed to indicate relative paths, and you can also use <b>UNC</b> path.
    /// </summary>
    /// <value>
    /// The output path.
    /// </value>
    public string Filename { get; set; }

    #endregion

    #region private async methods   

    private async Task<IResult> ExecuteImplAsync(IOutputResultData data, CancellationToken cancellationToken = default)
    {
        if (data == null)
        {
            return BooleanResult.NullResult;
        }

        var safeContext = Context;
        if (Context == null)
        {
            var detector = new AspDetector();
            if (detector.AspIsRunning)
            {
                safeContext = (HttpContext)HttpContextAccessor.Current;
            }
            else
            {
                return BooleanResult.ErrorResult;
            }
        }

        try
        {
            var safeFilename = Filename;
            if (string.IsNullOrEmpty(Filename))
            {
                safeFilename = GetUniqueTempRandomFile().Segments.Last();
            }

            var downloadFilename = data.IsZipped 
                ? Path.ChangeExtension(safeFilename, ZipExtension) 
                : Path.ChangeExtension(safeFilename, XlsxExtension);

            await (await data.GetOutputStreamAsync(cancellationToken)).DownloadAsync(downloadFilename, safeContext.Response, cancellationToken);

            return BooleanResult.SuccessResult;
        }
        catch (Exception ex)
        {
            return BooleanResult.FromException(ex);
        }
    }

    #endregion

    #region private static methods   

    private static Uri GetUniqueTempRandomFile()
    {

        var tempPath = Path.GetTempPath();
        var randomFileName = Path.GetRandomFileName();
        var path = Path.Combine(tempPath, randomFileName);

        return new Uri(path);
    }

    #endregion
}
