#include <windows.h>
#include <stdio.h>
#include <stdarg.h>
#include <conio.h>
#include "exehead/fileform.h"
#include "util.h"
#include "strlist.h"

int g_dopause=0;


void dopause(void)
{
  if (g_dopause)
  {
    printf("MakeNSIS done - hit enter to close...");
    fflush(stdout);
    int a;
    while ((a=_getch()) != '\r' && a != 27/*esc*/);
  }
}

int find_data_offset(char *hdr, int hdr_len, char *srch, int startoffs, int srchlen)
{
  int offs=0;
  hdr_len-=srchlen;
  while (hdr_len>=0 && memcmp(hdr+offs+startoffs,srch+startoffs,srchlen-startoffs))
  {
    offs++;
    hdr_len--;
  }
  if (hdr_len<0)
  {
    printf("find_data_offset: error searching data -- failing!\n");
    return -1;
  }
  return offs;
}

// will update out+ICO_HDRSKIP with a 766-ICO_HDRSKIP bytes of 32x32x16 icon
int replace_icon(char *out, char *filename)
{
  char obuf[766];
  int success=0;
  FILE *fp=fopen(filename,"rb");
  if (!fp)
  {
    printf("replace_icon: error: error opening file \"%s\" -- failing!\n",filename);
    return -1;
  }

  if (!fgetc(fp) && !fgetc(fp) && fgetc(fp)==1 && !fgetc(fp))
  {
    int num_images=fgetc(fp);
    num_images|=fgetc(fp)<<8;
    int x;
    for (x = 0; x < num_images; x++)
    {
      fseek(fp,6+x*16,SEEK_SET);
      int w=fgetc(fp);
      int h=fgetc(fp);
      int colors=fgetc(fp);
      int res=fgetc(fp);
      int planes=fgetc(fp); planes|=fgetc(fp)<<8;
      int bitcnt=fgetc(fp); bitcnt|=fgetc(fp)<<8;
      int size=fgetc(fp); size|=fgetc(fp)<<8; size|=fgetc(fp)<<16; size|=fgetc(fp)<<24;
      int offs=fgetc(fp); offs|=fgetc(fp)<<8; offs|=fgetc(fp)<<16; offs|=fgetc(fp)<<24;
      if (w == 32 && h == 32 && ((planes == 1 && bitcnt == 4) || colors == 16) && size <= 766-6-16) 
      {
        fseek(fp,offs,SEEK_SET);
        if (fgetc(fp) == 40 && !fgetc(fp) && !fgetc(fp) && !fgetc(fp))
        {
          memset(obuf,0,sizeof(obuf));
          obuf[2]=1;
          obuf[4]=1;
          fseek(fp,6+x*16,SEEK_SET);
          fread(obuf+6,1,12,fp);
          obuf[6+12]=6+16;
          fseek(fp,offs,SEEK_SET);
          fread(obuf+6+16,1,size,fp);
          success=1;
          break;
        }
      }
    }
  }
  
  fclose(fp);
  if (!success)
  {
    printf("replace_icon: error: icon file \"%s\" has no 32x32x16 icon -- failing!\n",filename);
    return 1;
  }
  else
    memcpy(out+ICO_HDRSKIP,obuf+ICO_HDRSKIP,766-ICO_HDRSKIP);
  return 0;
}


int replace_bitmap(char *out, char *filename)
{

  static unsigned char header_4bpp[54] = {	
	0x42, 0x4d, 0x66, 0x01, 0x00, 0x00, 0x00, 0x00, 
	0x00, 0x00, 0x76, 0x00, 0x00, 0x00, 0x28, 0x00,
	0x00, 0x00, 0x14, 0x00, 0x00, 0x00, 0x14, 0x00,
	0x00, 0x00, 0x01, 0x00, 0x04, 0x00, 0x00, 0x00,
	0x00, 0x00, 0xf0, 0x00, 0x00, 0x00, 0xc4, 0x0e,
	0x00, 0x00, 0xc4, 0x0e, 0x00, 0x00, 0x00, 0x00,
	0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
	// note: input file must be 16 colors;
	//		 can be any width and height, though

	HANDLE			hbmp;
	unsigned char	header[54];
	unsigned char	colors[64];

	FILE *fIn = fopen(filename, "rb");
	if (!fIn) 
  {
    printf("replace_bitmap: error: bitmap file not found \"%s\" -- failing!\n",filename);
		return 1;
  }

	fread(header, 54, 1, fIn);	// read the header (ignore)
	//int w = header[18] + header[19]*256;
	//int h = header[22] + header[23]*256;
	//int bpp = header[28];
	fread(colors, 64, 1, fIn);	// read the colors
	fclose(fIn);
	
	hbmp = LoadImage(NULL, filename, IMAGE_BITMAP, 20, 20, LR_CREATEDIBSECTION | LR_LOADFROMFILE);
	if (!hbmp) 
	{
    printf("replace_bitmap: error: invalid bitmap file \"%s\" -- failing!\n",filename);
		return 1;
	}

	BITMAP bm;
	GetObject(hbmp, sizeof(bm), (LPSTR)&bm);
	int w = bm.bmWidth;
	int h = bm.bmHeight;
	int bpp = bm.bmBitsPixel;

	if (w == 20 && h == 20 && bpp == 4 && bm.bmWidthBytes*bm.bmHeight + 64 + 54 <= 358) 
	{
	  // update & write header
	  header_4bpp[18] = w % 256;
	  header_4bpp[19] = w / 256;
	  header_4bpp[22] = h % 256;
	  header_4bpp[23] = h / 256;

    char obuf[358];

    memcpy(obuf,header_4bpp,54);
    memcpy(obuf+54,colors,64);
	  
    bm.bmWidthBytes+=3;
    bm.bmWidthBytes&=~3;

    memcpy(obuf+54+64,bm.bmBits,bm.bmWidthBytes*bm.bmHeight);
    memcpy(out+BMP_HDRSKIP,obuf+BMP_HDRSKIP,358-BMP_HDRSKIP);
  }
  else
  {
    DeleteObject(hbmp);
    printf("replace_bitmap: error: bitmap error in \"%s\" -- failing!\n",filename);
    return 1;
  }
  DeleteObject(hbmp);
	
  return 0;
}
