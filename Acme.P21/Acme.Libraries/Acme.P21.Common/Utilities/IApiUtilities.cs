using System.Collections.Generic;

namespace Acme.P21.Common.Utilities
{
    public interface IApiUtilities
    {
        #region Public Methods

        List<T> ExecP21ApiPost<T, TR>(string path, Dictionary<string, string> queryParameters, List<TR> body);

        string GetP21Token();


        #endregion
    }
}