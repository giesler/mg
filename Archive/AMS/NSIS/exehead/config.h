#ifndef NSIS_CONFIG_H
#define NSIS_CONFIG_H

// NSIS_CONFIG_LOG enables the logging facility.
// turning this on (by uncommenting it) adds about
// 3kb, but can be useful in debugging your installers.
// #define NSIS_CONFIG_LOG

// NSIS_CONFIG_UNINSTALL_SUPPORT enables the uninstaller
// support. Comment it out if your installers don't need
// uninstallers 
// adds approximately 1.5kb.
#define NSIS_CONFIG_UNINSTALL_SUPPORT

// NSIS_SUPPORT_NETSCAPEPLUGINS enables netscape plug-in install
// and uninstall. Comment it out if you don't need it.
// adds approximately 1kb
/// #define NSIS_SUPPORT_NETSCAPEPLUGINS

// NSIS_SUPPORT_ACTIVEXREG enables activeX plug-in registration
// and deregistration. Comment it out if you don't need it.
// adds approximately 1kb
#define NSIS_SUPPORT_ACTIVEXREG

// NSIS_SUPPORT_BGBG enables support for the blue (well, whatever
// color you want) gradient background window. About 300 bytes.
// #define NSIS_SUPPORT_BGBG

#endif // NSIS_CONFIG_H
