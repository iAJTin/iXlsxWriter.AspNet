
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
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
public class Download : IOutputAction
{
    #region private constants

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private const string XlsxExtension = "xlsx";

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private const string ZipExtension = "zip";

    #endregion

    #region interfaces

    #region IOutputAction

    /// <summary>
    /// Execute action for specified output result data.
    /// </summary>
    /// <param name="context">Target output result data.</param>
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
    public IResult Execute(IOutputResultData context) => ExecuteImpl(context);

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
    /// Gets or sets the filename.
    /// </summary>
    /// <value>
    /// The filename.
    /// </value>
    public string Filename { get; set; }

    #endregion

    #region private methods   

    private IResult ExecuteImpl(IOutputResultData data)
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

            data.GetOutputStream().Download(downloadFilename, safeContext.Response);

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
