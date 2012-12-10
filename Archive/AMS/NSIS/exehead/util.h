#include "config.h"

void recursive_create_directory(char *directory);

// returns 0 if out==in
// returns 2 if invalid symbol
// returns 3 if error looking up symbol
int process_string(char *out, char *in);
int process_string_fromtab(char *out, int offs);

#ifdef NSIS_CONFIG_LOG
extern char log_text[4096];
void log_write(int close);
#define log_printf(x1) wsprintf(log_text,x1); log_write(0)
#define log_printf2(x1,x2) wsprintf(log_text,x1,x2); log_write(0)
#define log_printf3(x1,x2,x3) wsprintf(log_text,x1,x2,x3); log_write(0)
#define log_printf4(x1,x2,x3,x4) wsprintf(log_text,x1,x2,x3,x4); log_write(0)
#define log_printf5(x1,x2,x3,x4,x5) wsprintf(log_text,x1,x2,x3,x4,x5); log_write(0)
#define log_printf6(x1,x2,x3,x4,x5,x6) wsprintf(log_text,x1,x2,x3,x4,x5,x6); log_write(0)
#define log_printf8(x1,x2,x3,x4,x5,x6,x7,x8) wsprintf(log_text,x1,x2,x3,x4,x5,x6,x7,x8); log_write(0)
extern int log_dolog;
extern char g_log_file[1024];
#else
#define log_printf(x1)
#define log_printf2(x1,x2)
#define log_printf3(x1,x2,x3)
#define log_printf4(x1,x2,x3,x4)
#define log_printf5(x1,x2,x3,x4,x5)
#define log_printf6(x1,x2,x3,x4,x5,x6)
#define log_printf8(x1,x2,x3,x4,x5,x6,x7,x8)
#endif

extern HANDLE g_hInstance;
int CreateShortCut(HWND hwnd, LPCSTR pszShortcutFile, LPCSTR pszIconFile, int iconindex, LPCSTR pszExe, LPCSTR pszArg, LPCSTR workingdir, int showmode, int hotkey);
int is_valid_instpath(char *s);
BOOL MoveFileOnReboot(LPCTSTR pszExisting, LPCTSTR pszNew);
