// xmmd5sum.cpp : Defines the entry point for the console application.
//

#ifndef _MD5SUM_H_
#define _MD5SUM_H_

// -------------------------------------------------------------------------- MD5

// first, some types:
typedef unsigned      long uint4; // assumes integer is 4 words long
typedef unsigned short int uint2; // assumes short integer is 2 words long
typedef unsigned      char uint1; // assumes char is 1 word long

#define XM_GUIDSTRINGLENGTH		34


//md5 util functions
inline void md5tohex(char* buf, const BYTE* md5, int count=16) {
	for (int i=0; i<count; i++) {
		sprintf(buf+i*2, "%02x", md5[i]);
	}
}

inline void hextomd5(BYTE* md5, const char* buf, int count=16) {
	for (int i=0; i<count; i++) {
		sscanf(buf+i*2, "%2x", md5+i);
	}
}

inline bool md5comp(BYTE* md5a, BYTE* md5b) {
	for (int i=0; i<16; i++) {
		if (md5a[i]!=md5b[i]) {
			return false;
		}
	}
	return true;
}

/////////////////////////////////////////////////////////////////////////////
// CMD5Engine
class CMD5Engine
{
public:
	CMD5Engine()
	{
		init();
	}

public:
// methods for controlled operation:
  //MD5              ();  // simple initializer
  void  update     (unsigned char *input, unsigned long input_length);
  void  update     (FILE *file);
  void  finalize   ();

// methods to acquire finalized result
  unsigned char    *raw_digest ();  // digest as a 16-byte binary array
  char *            hex_digest ();  // digest as a 33-byte ascii-hex string

private:

// next, the private data:
  uint4 state[4];
  uint4 count[2];     // number of *bits*, mod 2^64
  uint1 buffer[64];   // input buffer
  uint1 digest[16];
  uint1 finalized;
  char  mtempchar[33];

// last, the private methods, mostly static:
  void init             ();               // called by all constructors
  void transform        (uint1 *buffer);  // does the real update work.  Note 
                                          // that length is implied to be 64.

  static void encode    (uint1 *dest, uint4 *src, uint4 length);
  static void decode    (uint4 *dest, uint1 *src, uint4 length);

  static inline uint4  rotate_left (uint4 x, uint4 n);
  static inline uint4  F           (uint4 x, uint4 y, uint4 z);
  static inline uint4  G           (uint4 x, uint4 y, uint4 z);
  static inline uint4  H           (uint4 x, uint4 y, uint4 z);
  static inline uint4  I           (uint4 x, uint4 y, uint4 z);
  static inline void   FF  (uint4& a, uint4 b, uint4 c, uint4 d, uint4 x, 
			    uint4 s, uint4 ac);
  static inline void   GG  (uint4& a, uint4 b, uint4 c, uint4 d, uint4 x, 
			    uint4 s, uint4 ac);
  static inline void   HH  (uint4& a, uint4 b, uint4 c, uint4 d, uint4 x, 
			    uint4 s, uint4 ac);
  static inline void   II  (uint4& a, uint4 b, uint4 c, uint4 d, uint4 x, 
			    uint4 s, uint4 ac);

};

//utility classes for md5s and guids
class C16ByteValue
{
public:
	C16ByteValue() {
		mStr[0] = '\0';
	};
	C16ByteValue(const char* val) {
		SetValue(val);
	};
	C16ByteValue(BYTE* val) {
		SetValue(val);
	};
	C16ByteValue(C16ByteValue &val) {
		SetValue(val);
	};
	~C16ByteValue() {};

	//misc
	inline void Random() {
		::UuidCreate((UUID*)mData);
	}
	inline bool IsEqual(C16ByteValue &val)
	{
		return md5comp(mData, val.GetValue());
	}
	inline void Zero() {
		mData[ 0] = 0;
		mData[ 1] = 0;
		mData[ 2] = 0;
		mData[ 3] = 0;
		mData[ 4] = 0;
		mData[ 5] = 0;
		mData[ 6] = 0;
		mData[ 7] = 0;
		mData[ 8] = 0;
		mData[ 9] = 0;
		mData[10] = 0;
		mData[11] = 0;
		mData[12] = 0;
		mData[13] = 0;
		mData[14] = 0;
		mData[15] = 0;
	}

	//get
	inline char* GetString() {
		if (mStr[0]=='\0') {
			md5tohex(mStr, mData);
		}
		return mStr;
	}
	inline BYTE* GetValue() {
		return mData;
	};

	//set
	inline void SetValue(C16ByteValue &val) {
		memcpy(mData, val.GetValue(), 16);
		mStr[0] = '\0';
	};
	inline void SetValue(BYTE* val) {
		memcpy(mData, val, 16);
		mStr[0] = '\0';
	};
	inline void SetValue(const char* val) {
		strncpy(mStr, val, 32);
		hextomd5(mData, mStr);
	};
	inline C16ByteValue& operator =(C16ByteValue &val) {
		SetValue(val);
		return *this;
	};
	inline C16ByteValue& operator =(BYTE* val) {
		SetValue(val);
		return *this;
	};
	inline C16ByteValue& operator =(const char* val) {
		SetValue(val);
		return *this;
	};

	//md5engine passthroughs
	inline void FromFile(FILE* file) {
		CMD5Engine e;
		e.update(file);
		e.finalize();
		SetValue(e.raw_digest());
	};
	inline void FromFile(const char* path) {
		FILE* file = fopen(path, "rb");
		if (file) {
			FromFile(file);
		}
	};
	inline void FromBuf(BYTE* buf, DWORD length) {
		CMD5Engine e;
		e.update(buf, length);
		e.finalize();
		SetValue(e.raw_digest());
	};

protected:

	BYTE mData[16];
	char mStr[33];
};

typedef C16ByteValue CGUID;
typedef C16ByteValue CMD5;


#endif _MD5SUM_H_
