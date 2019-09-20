// PkgCmdID.cs
// MUST match PkgCmdID.h
using System;

namespace TeamTimeWarp.TeamTimeWarp_VsPackage
{
    static class PkgCmdIDList
    {
        public const uint cmTeamTimeWarpPackageCommand =        0x100;
        public const uint cmTeamTimeWarpEndWorkCommand = 0x101;
        public const uint cmTeamTimeWarpEnableAutoTriggeringCommand = 0x102;
        public const uint cmTeamTimeWarpSetupLoginDetailsCommand = 0x103;
        public const uint cmTeamTimeWarpSetupLogoutDetailsCommand = 0x104;


    };
}