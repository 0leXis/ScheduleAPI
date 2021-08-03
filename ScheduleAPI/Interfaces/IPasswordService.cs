using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScheduleAPI.Interfaces
{
    public interface IPasswordService
    {
        string GetHash(string password, string salt, string globalSalt);
        string GenerateSalt();
    }
}
