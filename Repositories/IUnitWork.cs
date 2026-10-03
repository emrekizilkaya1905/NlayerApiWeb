using System;
using System.Collections.Generic;
using System.Text;

namespace App.Repositories
{
    public interface IUnitWork
    {
        Task<int> SaveChangesAsync();
    }
}
