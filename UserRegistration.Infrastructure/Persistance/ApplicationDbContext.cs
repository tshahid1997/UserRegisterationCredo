﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserRegistration.Domain;

namespace UserRegistration.Infrastructure.Persistance
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<UserEntity> UserEntities { get; set; }
        public DbSet<LoanApplicationEntity> LoanApplicationEntities { get; set; }

    }


}
