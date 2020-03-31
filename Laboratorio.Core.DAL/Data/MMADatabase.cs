using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laboratorio.Core.DAL.Data
{
    public class MMADatabase : Base
    {
        private SqlCommand command;

        public MMADatabase()
        {
            SetStringConnection("Data Source=mmadatabaseserver.database.windows.net; User Id= manuelmtz_25; Password=VMorelos25-_; Initial Catalog=MMADatabase");
        }

    }
}
