using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    enum UserCommand
    {
        LOGIN,
        REGISTER_USER
    }
    enum CommandStatus
    {
        RUNNING,
        FAILED,
        SUCCESSFUL
    }
}
