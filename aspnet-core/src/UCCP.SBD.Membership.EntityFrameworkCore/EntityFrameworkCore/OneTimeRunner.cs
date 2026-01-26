using System;
using Volo.Abp;

namespace UCCP.SBD.Membership.EntityFrameworkCore;

public class OneTimeRunner
{
    private bool _hasRun;

    public void Run(Action action)
    {
        if (_hasRun)
        {
            return;
        }

        action();
        _hasRun = true;
    }
}
