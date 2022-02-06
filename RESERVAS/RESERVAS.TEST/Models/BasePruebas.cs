using Microsoft.EntityFrameworkCore;
using RESERVAS.API.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RESERVAS.TEST.Models
{
    public class BasePruebas
    {
        protected DataContext ConstruirContext(string nombreDB)
        {
            var opciones = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: nombreDB).Options;
            var dbContext = new DataContext(opciones);
            dbContext.Database.EnsureCreated();
            return dbContext;
        }


    }
}
