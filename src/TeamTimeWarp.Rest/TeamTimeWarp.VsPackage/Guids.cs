// Guids.cs
// MUST match guids.h
using System;

namespace TeamTimeWarp.TeamTimeWarp_VsPackage
{
    static class GuidList
    {
        public const string guidTeamTimeWarp_VsPackagePkgString = "cf5fbae6-1131-4640-a469-16513ac33d34";
        public const string guidTeamTimeWarp_VsPackageCmdSetString = "3c9a5bcc-0c1d-4924-a22b-94bae9e4efb2";

        public static readonly Guid guidTeamTimeWarp_VsPackageCmdSet = new Guid(guidTeamTimeWarp_VsPackageCmdSetString);
    };
}