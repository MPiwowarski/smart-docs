using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartDocs.Services
{
    public interface IEncryptService
    {
        string Encrypt(string message);
    }
}
