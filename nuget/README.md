# What is iXlsxWriter.AspNet?

**iXlsxWriter.AspNet**, extends [iXlsxWriter](https://github.com/iAJTin/iXlsxWriter) to work in **AspNet (FullFramework)** projects, contains extension methods to download **XlsxInput** instances as well as **OutputResult**, facilitating its use in this environment.

I hope it helps someone. :smirk:

# Usage

## Samples

### Sample 1 - Shows the use of synchronous download

```csharp
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
```

### Sample 2 - Shows the use of asynchronous download by DownloadAsync action

```csharp   
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
```

# Changes

For more information, please visit the next link [CHANGELOG](https://github.com/iAJTin/iXlsxWriter.AspNet/blob/master/CHANGELOG.md)
