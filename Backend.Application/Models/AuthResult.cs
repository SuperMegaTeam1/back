using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Application.Models
{
    public sealed record AuthResult(
        string Token,
        AuthUserResult User);
}
