#ifndef _UTIL_H_
#define _UTIL_H_

// these are the standard pause-before-quit shit.
extern int g_dopause;
extern void dopause(void);

// searches hdr for srch, allowing the first startoffs bytes to differ.
int find_data_offset(char *hdr, int hdr_len, char *srch, int startoffs, int srchlen);

int replace_bitmap(char *out, char *filename); // repalces out with a 358 byte image of a 20x20x16 bitmap from filename.
int replace_icon(char *out, char *filename); // replaces out with a 766 byte image of an icon, loaded from the 32x32x16 color portion of filename.

#endif //_UTIL_H_