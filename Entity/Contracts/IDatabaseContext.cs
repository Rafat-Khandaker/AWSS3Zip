﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AWSS3Zip.Entity.Contracts
{
    public interface IDatabaseContext<X,Y> where X: IDatabaseFactory where Y: IAppDatabase 
    {
        public AppDatabase AppDatabase { get; set; }
        public DatabaseContext AddConnection(string connectionString);
        public Y Build();
    }
}
