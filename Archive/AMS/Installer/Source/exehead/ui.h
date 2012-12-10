#ifndef _UI_H_
#define _UI_H_

int ui_doinstall(void);
void update_status_text(char *text1, char *text2);

extern char g_autoclose;
extern header *g_inst_header;
extern section *g_inst_section;
extern entry *g_inst_entry;
#ifdef NSIS_CONFIG_UNINSTALL_SUPPORT
extern uninstall_header *g_inst_uninstheader;
#endif

#endif//_UI_H_