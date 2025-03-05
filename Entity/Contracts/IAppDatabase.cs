using AWSS3Zip.Entity.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AWSS3Zip.Entity.Contracts
{
    public interface IAppDatabase
    {
        public DbSet<IISLogEvent> IISLogEvents { get; set; }

        public string ConnectionString { get; set; }
    }
}
