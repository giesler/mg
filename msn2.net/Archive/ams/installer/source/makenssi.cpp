#define NSIS_VERSION "MakeNSIS v1.44"
/* 
  Nullsoft "SuperPimp" Installation System - makensis.cpp - installer compiler code
  version 1.44 - June 11, 2001

  Copyright (C) 1999-2001 Nullsoft, Inc.

  This software is provided 'as-is', without any express or implied
  warranty.  In no event will the authors be held liable for any damages
  arising from the use of this software.

  Permission is granted to anyone to use this software for any purpose,
  including commercial applications, and to alter it and redistribute it
  freely, subject to the following restrictions:

  1. The origin of this software must not be misrepresented; you must not
     claim that you wrote the original software. If you use this software
     in a product, an acknowledgment in the product documentation would be
     appreciated but is not required.
  2. Altered source versions must be plainly marked as such, and must not be
     misrepresented as being the original software.
  3. This notice may not be removed or altered from any source distribution.

  This source distribution includes portions of zlib. see zlib/zlib.h for 
  its license and so forth. Note that this license is also borrowed from zlib.
*/

#include <windows.h>
#include <stdio.h>
#include <signal.h>

#include "build.h"
#include "util.h"
#include "exedata.h"


int g_noconfig;

static char *get_featurestring();

void quit()
{
  printf("\nNote: you may have one or two (large) stale temporary file(s)\n"
         "left in your temporary directory (Generally this only happens on Windows 9x).\n");
  exit(1);
}

static void sigint(int sig)
{
  printf("\n\nAborting on Ctrl+C...\n");
  quit();
}

int main(int argc, char **argv)
{
  CEXEBuild build;
  int do_cd=0;
  int do_license=0;
  int argpos=1;
  FILE *fp;
  printf("%s - Copyright 1999-2001 Nullsoft, Inc.\n"
         "\n"
         "Portions Copyright (C) 1995-1998 Jean-loup Gailly and Mark Adler.\n"
         "Contributors: nnop@newmail.ru, Ryan Geiss, Andras Varga, Drew Davidson, et al.\n\n",NSIS_VERSION);

  atexit(dopause);

  signal(SIGINT,sigint);

  while (argpos < argc)
  {
    if (argv[argpos][0]=='/' && (argv[argpos][1]=='D' || argv[argpos][1]=='d'))
    {
      char *p=argv[argpos]+2;
      if (p[0])
      {
        printf("Command line defined: \"%s\"\n",p);
        build.define(p);
      }
      else
      {
        build.warning("command line /D requires argument (i.e. \"/Ddefine\"). ignored.");
      }
    }
    else if (!stricmp(argv[argpos],"/CD")) do_cd=1;
    else if (!stricmp(argv[argpos],"/NOCONFIG")) g_noconfig=1;
    else if (!stricmp(argv[argpos],"/PAUSE")) g_dopause=1;
    else if (!stricmp(argv[argpos],"/LICENSE")) do_license=1;
    else if (!stricmp(argv[argpos],"/CMDHELP"))
    {
      if (argpos < argc-1)
        build.print_help(argv[argpos+1]);
      else 
        build.print_help(NULL);
      return 1;
    }
    else break;
    argpos++;
  }

  if (do_license)
  {
    printf("This software is provided 'as-is', without any express or implied warranty.  In\n"
           "no event will the authors be held liable for any damages arising from the use\n"
           "of this software.\n\n"
           "Permission is granted to anyone to use this software for any purpose, including\n"
           "commercial applications, and to alter it and redistribute it freely, subject to\n"
           "the following restrictions:\n"
           "  1. The origin of this software must not be misrepresented; you must not claim\n"
           "     that you wrote the original software. If you use this software in a\n"
           "     product, an acknowledgment in the product documentation would be\n"
           "     appreciated but is not required.\n"
           "  2. Altered source versions must be plainly marked as such, and must not be\n"
           "     misrepresented as being the original software.\n"
           "  3. This notice may not be removed or altered from any source distribution.\n\n");

    return 1;
  }

  printf("Use the /LICENSE switch to view license information.\n\n");

  if (argpos != argc-1)
  {
    printf("  Compiled-in features (header size of %d bytes):%s\n", exeheader_size,get_featurestring());
    printf("\nUsage:\n"
           "  makensis [/LICENSE /PAUSE /NOCONFIG /CD /Ddefine ...] [script.nsi | - ]\n"
           "  makensis /CMDHELP [item]\n");
    printf("    /CMDHELP item prints out help for 'item', or lists all commands\n"
           "    /LICENSE prints the license\n"
           "    /PAUSE pauses after execution\n"
           "    /NOCONFIG disables inclusion of <path to makensis.exe>\\nsisconf.nsi\n"
           "    /CD makes makensis change the current directory to that of the .nsi file\n"
           "    /Ddefine defines the symbol \"define\" for the script\n"
           "  ...and for a script file name, you can use - to read from the standard input\n");

    return 1;
  }


  if (!strcmp(argv[argpos],"-"))
  {
    g_dopause=0;
  }

  if (!g_noconfig)
  {
    char exepath[1024];
    strncpy(exepath,argv[0],1023);
    exepath[1023]=0;
    char *p=exepath;
    while (*p) p++;
    while (p > exepath && *p != '\\') p--;
    strcpy(p,"nsisconf.nsi");
    FILE *cfg=fopen(exepath,"rt");
    if (cfg)
    {
      printf("-=-=-=-=-=-=-=-=-=-=- Processing config -=-=-=-=-=-=-=-=-=-=-\n");
      int lc=0;
      int ret=build.process_script(cfg,exepath,&lc);
      fclose(cfg);
      if (ret != PS_OK && ret != PS_EOF)
      {
        printf("Error in config on line %d -- aborting creation process\n",lc);
        return 1;
      }
    }
  }

  char sfile[1024];
  if (!strcmp(argv[argpos],"-"))
  {
    fp=stdin;
    strcpy(sfile,"stdin");
  }
  else
  {
    strcpy(sfile,argv[argpos]);
    fp=fopen(sfile,"rt");
    if (!fp)
    {
      sprintf(sfile,"%s.nsi",argv[argpos]);
      fp=fopen(sfile,"rt");
      if (!fp)
      {
        perror("Can't open script");
        return 1;
      }
    }
    if (do_cd)
    {
      char dirbuf[1024],*p;
      GetFullPathName(sfile,sizeof(dirbuf),dirbuf,&p);
      p=dirbuf;
      while (*p) p++;
      while (p > dirbuf && *p != '\\') p--;
      *p=0;
      if (dirbuf[0]) 
      {
        printf("Changing directory to: \"%s\"\n",dirbuf);
        SetCurrentDirectory(dirbuf);
      }
    }
  }


  printf("-=-=-=-=-=-=-=-=-=-=- Processing script -=-=-=-=-=-=-=-=-=-=-\n");
  printf("Script file: \"%s\"\n",sfile);
  int lc=0;
  int ret=build.process_script(fp,sfile,&lc);
  if (fp != stdin) fclose(fp);

  if (ret != PS_EOF && ret != PS_OK)
  {
    printf("Error in script \"%s\" on line %d -- aborting creation process\n",sfile,lc);
    return 1;
  }
  
  ret=build.write_output();

  if (ret)
  { 
    printf("Error - aborting creation process\n");
    return 1;
  }
  printf("\n");
  return 0; 
}





static char *get_featurestring()
{
  char *f=" "
#ifdef NSIS_CONFIG_LOG
         "NSIS_CONFIG_LOG,"
#endif
#ifdef NSIS_CONFIG_UNINSTALL_SUPPORT
         "NSIS_CONFIG_UNINSTALL_SUPPORT,"
#endif
#ifdef NSIS_SUPPORT_NETSCAPEPLUGINS
         "NSIS_SUPPORT_NETSCAPEPLUGINS,"
#endif
#ifdef NSIS_SUPPORT_ACTIVEXREG
         "NSIS_SUPPORT_ACTIVEXREG,"
#endif
#ifdef NSIS_SUPPORT_BGBG
         "NSIS_SUPPORT_BGBG,"
#endif
;
  if (!f[1]) return "none.";
  f[strlen(f)-1]='.';
  return f;
}