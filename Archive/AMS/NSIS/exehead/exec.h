#ifndef _EXEC_H_
#define _EXEC_H_

#define EXEC_ERROR 0x10000000
int ExecuteEntry(entry *entries, int pos); // returns advance on advance, EXEC_ERROR on err.

int ExecuteCodeSegment(entry *entries, int range[2], HWND hwndProgress); // returns 0 on success, EXEC_ERROR on error.


#endif//_EXEC_H_