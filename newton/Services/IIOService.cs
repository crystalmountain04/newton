using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace newton.Services
{
    public interface IIOService
    {
        string? OpenFile();

        string? SaveFile();
    }
}
