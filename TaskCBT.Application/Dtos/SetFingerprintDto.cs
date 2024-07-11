using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskCBT.Application.Dtos
{
    public class SetFingerprintDto
    {
        public string ICNumber { get; set; }
        public bool IsFingerprintEnabled { get; set; }
    }
}
