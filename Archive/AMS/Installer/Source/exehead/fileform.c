#include <windows.h>
#include "fileform.h"
#include "util.h"
#include "state.h"
#include "../zlib/zlib.h"
#include "ui.h"

// returns -1 on read error
// returns -2 on header corrupt
// returns -3 on error seeking (should never occur)
// returns offset of data block in file on success
// on success, *r_head will be set to a pointer that should eventually be GlobalFree()'d.

static char *g_db_strtab;
HANDLE g_db_hFile;
int g_db_offset;

int isheader(firstheader *h)
{
  static int tab[3]=
  {
    ~FH_INT1,~FH_INT2,~FH_INT3
  };
  if ((h->flags & (~FH_FLAGS_MASK)) ||
      h->siginfo != FH_SIG ||
      ~h->nsinst[0] != tab[0] ||
      ~h->nsinst[1] != tab[1] ||
      ~h->nsinst[2] != tab[2]) return 0;
  return h->length_of_all_following_data;
}


int loadHeaders(void)
{
  DWORD r;
  void *data;
  firstheader h;
  
  if (!ReadFile(g_db_hFile,(LPVOID)&h,sizeof(h),&r,NULL) || r != sizeof(h) || !isheader(&h)) return -1;

  data=(void*)GlobalAlloc(GMEM_FIXED,h.length_of_header);

  if (GetCompressedDataFromDataBlockToMemory(h.header_ptr,data,h.length_of_header) != h.length_of_header)
  {
    GlobalFree((HGLOBAL)data);
    return -1;
  }

#ifdef NSIS_CONFIG_UNINSTALL_SUPPORT
  if (h.flags&FH_FLAGS_UNINSTALL)
  {
 //   *r_head=NULL;
 //   *r_sec=NULL;
    g_inst_uninstheader=(uninstall_header*)data;
    g_inst_entry=(entry *) ((g_inst_uninstheader) + 1);
    g_db_strtab = (char *)(g_inst_entry + g_inst_uninstheader->num_entries);
  }
  else
#endif
  {
//    *r_uhead=NULL;
    g_inst_header=(header *)data;
    g_inst_section=(section *) (g_inst_header + 1);
    g_inst_entry=(entry *) (g_inst_section + g_inst_header->num_sections);
    g_db_strtab = (char *)(g_inst_entry + g_inst_header->num_entries);
  }
  return 0;
}

char *GetStringFromStringTab(int offs)
{
  if (offs < 0) return "";
  return g_db_strtab+offs;
}

// returns -3 if compression error/eof/etc

int GetCompressedDataFromDataBlock(int offset, HANDLE hFileOut)
{
  char *inbuffer=(char *)GlobalAlloc(GMEM_FIXED,65536*2);
  char *outbuffer = inbuffer+65536;
  int retval=0;
  int input_len;
  DWORD r;

  if (offset>=0) SetFilePointer(g_db_hFile,g_db_offset+offset,NULL,FILE_BEGIN);

  if (!ReadFile(g_db_hFile,(LPVOID)&input_len,sizeof(int),&r,NULL)) { GlobalFree((HGLOBAL)inbuffer); return -3; }

  if (input_len & 0x80000000) // compressed
  {
    z_stream stream={0,};
    inflateInit(&stream);
    input_len &= 0x7fffffff; // take off top bit.

    while (input_len >= 0)
    {
      DWORD r;
      int err,haveread=0;

      if (input_len>0)
      {
        if (!ReadFile(g_db_hFile,(LPVOID)inbuffer,min(input_len,65536),&r,NULL)) { retval=-3; break;}
        haveread=r;
      }

      stream.next_in = inbuffer;
      stream.avail_in = (uInt)haveread;

      while (input_len == 0 || stream.avail_in)
      {
        stream.next_out = outbuffer;
        stream.avail_out = (uInt)65536;
        err=inflate(&stream, input_len?0:Z_FINISH);
        if (err == Z_STREAM_ERROR ||err==Z_DATA_ERROR||err==Z_NEED_DICT) { retval=-4; break; }
        if ((char*)stream.next_out == outbuffer) break;

        if (!WriteFile(hFileOut,outbuffer,(char*)stream.next_out-outbuffer,&r,NULL) || 
          (int)r != (char*)stream.next_out-outbuffer) { retval=-2; break; }          
        retval+=r;

        if (err == Z_BUF_ERROR) break;
      }
      if (input_len == 0) break;
      input_len-=haveread;
    }
    inflateEnd(&stream);
  }
  else
  {
    while (input_len > 0)
    {
      DWORD t;
      if (!ReadFile(g_db_hFile,(LPVOID)inbuffer,min(input_len,65536),&r,NULL)) { retval=-3; break;}
      if (!WriteFile(hFileOut,inbuffer,r,&t,NULL) || r!=t) { retval=-2; break; }
      retval+=r;
      input_len-=r;
    }
  }
  GlobalFree((HGLOBAL)inbuffer);
  return retval;
}

int GetCompressedDataFromDataBlockToMemory(int offset, char *out, int out_len)
{
  int retval;
  char *inbuffer=(char *)GlobalAlloc(GMEM_FIXED,65536);
  int input_len;
  DWORD r;
  if (offset>=0) SetFilePointer(g_db_hFile,offset+g_db_offset,NULL,FILE_BEGIN);

  if (!ReadFile(g_db_hFile,(LPVOID)&input_len,sizeof(int),&r,NULL)) { GlobalFree((HGLOBAL)inbuffer); return -3; }

  if (input_len & 0x80000000) // compressed
  {
    z_stream stream={0,};
    inflateInit(&stream);
    input_len &= 0x7fffffff; // take off top bit.
    stream.next_out = out;
    stream.avail_out = (uInt)out_len;

    while (input_len >= 0)
    {
      DWORD r;
      int err,haveread=0;

      if (input_len>0)
      {
        if (!ReadFile(g_db_hFile,(LPVOID)inbuffer,min(input_len,65536),&r,NULL)) { retval=-3; break;}
        haveread=r;
      }

      stream.next_in = inbuffer;
      stream.avail_in = (uInt)haveread;

      err=inflate(&stream, input_len?0:Z_FINISH);
      if (err == Z_STREAM_ERROR ||err==Z_DATA_ERROR||err==Z_NEED_DICT) { retval=-4; break; }

      retval=(char*)stream.next_out-out;
      if (input_len == 0) break;
      input_len-=haveread;
    }
    inflateEnd(&stream);
  }
  else
  {
    if (!ReadFile(g_db_hFile,(LPVOID)out,min(input_len,out_len),&r,NULL)) { GlobalFree((HGLOBAL)inbuffer); return -3; }
    retval=r;
  }
  GlobalFree((HGLOBAL)inbuffer);
  return retval;
}

