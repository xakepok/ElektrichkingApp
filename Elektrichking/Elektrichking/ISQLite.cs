using System;
using System.Collections.Generic;
using System.Text;

namespace Elektrichking
{
    public interface ISQLite
    {
        string GetDatabasePath(string filename);
    }
}
