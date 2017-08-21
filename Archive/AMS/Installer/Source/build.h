#ifndef _BUILD_H_
#define _BUILD_H_

#include "strlist.h"
#include "lineparse.h"

#include "exehead/fileform.h"
#include "exehead/config.h"

#define PS_OK 0
#define PS_EOF 1
#define PS_ENDIF 2
#define PS_ELSE 3
#define PS_ELSE_IF0 4
#define PS_ELSE_IF1 5
#define PS_ERROR 50
#define IS_PS_ELSE(x) (( x ) >= PS_ELSE && ( x ) <= PS_ELSE_IF1)

class CEXEBuild {
  public: 
    CEXEBuild();
    ~CEXEBuild();

    // to add a warning to the compiler's warning list.
    void warning(char *s, ...);

    // to add a defined thing.
    void define(char *p);

    // process a script (you can process as many scripts as you want,
    // it is as if they are concatenated)
    int process_script(FILE *fp, char *curfilename, int *lineptr);
    
    // you only get to call write_output once, so use it wisely.
    int write_output(void);

    void print_help(char *commandname=NULL);

  private:
    // tokens.cpp
    int get_commandtoken(char *s, int *np, int *op);

    // script.cpp
    int parseScript(FILE *fp, char *curfilename, int *lineptr, int ignore);
    int parseLine(char *str, FILE *fp, char *curfilename, int *lineptr, int ignore);
    int doCommand(int which_token, LineParser &line, FILE *fp, char *curfilename, int linecnt);
    int do_add_file(char *lgss, int recurse, int linecnt, int *total_files, char *name_override=0);
    GrowBuf m_linebuild; // used for concatenating lines

    // build.cpp functions used mostly by script.cpp
    int getcurdbsize();
    int add_section(char *secname, char *file, int line);
    int section_end();
    int add_function(char *funname);
    int function_end();
    void section_add_size_kb(int kb);
    int section_add_flags(int flags);
    int add_label(char *name);
    int add_entry(entry *ent);
    int add_data(char *data, int length); // returns offset
    int add_string(char *string); // returns offset (in string table)
    int add_string_main(char *string); // returns offset (in string table)
    int add_string_uninst(char *string); // returns offset (in string table)

    int process_jump(char *s, int *offs);
    int resolve_jump_int(int *a, int offs, int start, int end);
    int resolve_instruction(entry *w, int offs, int start, int end);

    int make_sure_not_in_secorfunc(char *str);

    // build.cpp functions used mostly within build.cpp
    int datablock_optimize(int start_offset);
    void printline(int l);
    int resolve_function(char *str, int fptr, int *ofs);
    int resolve_functions(char *str);
    void print_warnings();
    int uninstall_generate();
    void set_uninstall_mode(int un);

    // a whole bunch O data.

    int has_called_write_output;

    char build_packname[1024], build_packcmd[1024];
    int build_overwrite, build_compress, build_crcchk, 
        build_datesave, build_optimize_datablock;

    header build_header;
    int uninstall_mode;
    uninstall_header build_uninst;
    int uninstall_size,uninstall_size_full;

    char build_output_filename[1024];
    char cur_out_path[1024];
    StringList definedlist;

    StringList m_warnings;

    int db_opt_save, db_comp_save, db_full_size, db_opt_save_u, 
        db_comp_save_u, db_full_size_u;
    int build_sections_req,build_sections_div;

    StringList ns_func, ns_label; // function and label namespaces

    int build_cursection_isfunc;
    section *build_cursection;
    GrowBuf build_sections;
    GrowBuf build_entries,ubuild_entries, *cur_entries;
    GrowBuf build_functions, ubuild_functions, *cur_functions;
    GrowBuf build_labels, ubuild_labels, *cur_labels;
    StringList build_strlist,ubuild_strlist;

    MMapBuf build_datablock, ubuild_datablock, *cur_datablock; // use GrowBuf here instead of MMapBuf if you want

    unsigned char *header_data_new;
    int exeheader_size_new;
    int enabled_bitmap_offset;
    int disabled_bitmap_offset;
    int icon_offset;
    unsigned char m_unicon_data[766];
};

#endif //_BUILD_H_
