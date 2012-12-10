#include <windows.h>

void *mini_memcpy(void *out, const void *in, int len)
{
  char *c_out=(char*)out;
  char *c_in=(char *)in;
  while (len-- > 0)
  {
    *c_out++=*c_in++;
  }
  return out;
}

char *mini_strstr(char *a, char *b)
{
  while (*a)
  {
    char *t=a,*u=b;
    while (*t && *t == *u)
    {
      t++;
      u++;
    }
    if (!*u) return a;
    a++;
  }
  return NULL;
}

char *mini_memmove(void *out, const void *in, int len)
{
  void* data=(void*)GlobalAlloc(GMEM_FIXED,len);
  mini_memcpy(data, in, len);
  mini_memcpy(out, data, len);
  GlobalFree((HGLOBAL)data);
  return out;
}
