// xmmd5sum.cpp : Defines the entry point for the console application.
//

#ifndef _MD5SUM_H_
#define _MD5SUM_H_

#include "windows.h"
#include "stdio.h"

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

/*
License to copy and use this software is granted provided that it
is identified as the "RSA Data Security, Inc. MD5 Message-Digest
Algorithm" in all material mentioning or referencing this software
or this function.

License is also granted to make and use derivative works provided
that such works are identified as "derived from the RSA Data
Security, Inc. MD5 Message-Digest Algorithm" in all material
mentioning or referencing the derived work.

*/

void CMD5Engine::update (uint1 *input, uint4 input_length) {

	uint4 input_index, buffer_index;
	uint4 buffer_space;                // how much space is left in buffer

	if (finalized){  // so we can't update!
		return;
	}

	// Compute number of bytes mod 64
	buffer_index = (uint4)((count[0] >> 3) & 0x3F);

	// Update number of bits
	if (  (count[0] += ((uint4) input_length << 3))<((uint4) input_length << 3) )
		count[1]++;

	count[1] += ((uint4)input_length >> 29);


	buffer_space = 64 - buffer_index;  // how much space is left in buffer

	// Transform as many times as possible.
	if (input_length >= buffer_space) { // ie. we have enough to fill the buffer
		// fill the rest of the buffer and transform
		memcpy (buffer + buffer_index, input, buffer_space);
		transform (buffer);

		// now, transform each 64-byte piece of the input, bypassing the buffer
		for (input_index = buffer_space; input_index + 63 < input_length; 
			input_index += 64)
			transform (input+input_index);

		buffer_index = 0;  // so we can buffer remaining
	}
	else
		input_index=0;     // so we can buffer the whole input


	// and here we do the buffering:
	memcpy(buffer+buffer_index, input+input_index, input_length-input_index);
}

// MD5 update for files.
// Like above, except that it works on files (and uses above as a primitive.)
void CMD5Engine::update(FILE *file){

	unsigned char buffer[1024];
	int len;

	while (len=fread(buffer, 1, 1024, file))
		update(buffer, len);
	fclose (file);

}

// MD5 finalization. Ends an MD5 message-digest operation, writing the
// the message digest and zeroizing the context.
void CMD5Engine::finalize (){

	unsigned char bits[8];
	uint4 index, padLen;
	static uint1 PADDING[64]={
		0x80, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
	};

	if (finalized){
		return;
	}

	// Save number of bits
	encode (bits, count, 8);

	// Pad out to 56 mod 64.
	index = (uint4) ((count[0] >> 3) & 0x3f);
	padLen = (index < 56) ? (56 - index) : (120 - index);
	update (PADDING, padLen);

	// Append length (before padding)
	update (bits, 8);

	// Store state in digest
	encode (digest, state, 16);

	// Zeroize sensitive information
	memset (buffer, 0, sizeof(*buffer));

	finalized=1;

}

unsigned char *CMD5Engine::raw_digest(){

	if (!finalized){
		return NULL;
	}

	return digest;
}

char *CMD5Engine::hex_digest(){

	if (!finalized){
		return NULL;
	}
	
	int i;
	for (i=0; i<16; i++)
		sprintf(mtempchar+i*2, "%02x", digest[i]);

	mtempchar[32]='\0';

	return mtempchar;
}

// PRIVATE METHODS:
void CMD5Engine::init(){
	finalized=0;  // we just started!

	// Nothing counted, so count=0
	count[0] = 0;
	count[1] = 0;

	// Load magic initialization constants.
	state[0] = 0x67452301;
	state[1] = 0xefcdab89;
	state[2] = 0x98badcfe;
	state[3] = 0x10325476;
}

// Constants for MD5Transform routine.
// Although we could use C++ style constants, defines are actually better,
// since they let us easily evade scope clashes.

#define S11 7
#define S12 12
#define S13 17
#define S14 22
#define S21 5
#define S22 9
#define S23 14
#define S24 20
#define S31 4
#define S32 11
#define S33 16
#define S34 23
#define S41 6
#define S42 10
#define S43 15
#define S44 21

// MD5 basic transformation. Transforms state based on block.
void CMD5Engine::transform (uint1 block[64]){

	uint4 a = state[0], b = state[1], c = state[2], d = state[3], x[16];

	decode (x, block, 64);

	/* Round 1 */
	FF (a, b, c, d, x[ 0], S11, 0xd76aa478); /* 1 */
	FF (d, a, b, c, x[ 1], S12, 0xe8c7b756); /* 2 */
	FF (c, d, a, b, x[ 2], S13, 0x242070db); /* 3 */
	FF (b, c, d, a, x[ 3], S14, 0xc1bdceee); /* 4 */
	FF (a, b, c, d, x[ 4], S11, 0xf57c0faf); /* 5 */
	FF (d, a, b, c, x[ 5], S12, 0x4787c62a); /* 6 */
	FF (c, d, a, b, x[ 6], S13, 0xa8304613); /* 7 */
	FF (b, c, d, a, x[ 7], S14, 0xfd469501); /* 8 */
	FF (a, b, c, d, x[ 8], S11, 0x698098d8); /* 9 */
	FF (d, a, b, c, x[ 9], S12, 0x8b44f7af); /* 10 */
	FF (c, d, a, b, x[10], S13, 0xffff5bb1); /* 11 */
	FF (b, c, d, a, x[11], S14, 0x895cd7be); /* 12 */
	FF (a, b, c, d, x[12], S11, 0x6b901122); /* 13 */
	FF (d, a, b, c, x[13], S12, 0xfd987193); /* 14 */
	FF (c, d, a, b, x[14], S13, 0xa679438e); /* 15 */
	FF (b, c, d, a, x[15], S14, 0x49b40821); /* 16 */

	/* Round 2 */
	GG (a, b, c, d, x[ 1], S21, 0xf61e2562); /* 17 */
	GG (d, a, b, c, x[ 6], S22, 0xc040b340); /* 18 */
	GG (c, d, a, b, x[11], S23, 0x265e5a51); /* 19 */
	GG (b, c, d, a, x[ 0], S24, 0xe9b6c7aa); /* 20 */
	GG (a, b, c, d, x[ 5], S21, 0xd62f105d); /* 21 */
	GG (d, a, b, c, x[10], S22,  0x2441453); /* 22 */
	GG (c, d, a, b, x[15], S23, 0xd8a1e681); /* 23 */
	GG (b, c, d, a, x[ 4], S24, 0xe7d3fbc8); /* 24 */
	GG (a, b, c, d, x[ 9], S21, 0x21e1cde6); /* 25 */
	GG (d, a, b, c, x[14], S22, 0xc33707d6); /* 26 */
	GG (c, d, a, b, x[ 3], S23, 0xf4d50d87); /* 27 */
	GG (b, c, d, a, x[ 8], S24, 0x455a14ed); /* 28 */
	GG (a, b, c, d, x[13], S21, 0xa9e3e905); /* 29 */
	GG (d, a, b, c, x[ 2], S22, 0xfcefa3f8); /* 30 */
	GG (c, d, a, b, x[ 7], S23, 0x676f02d9); /* 31 */
	GG (b, c, d, a, x[12], S24, 0x8d2a4c8a); /* 32 */

	/* Round 3 */
	HH (a, b, c, d, x[ 5], S31, 0xfffa3942); /* 33 */
	HH (d, a, b, c, x[ 8], S32, 0x8771f681); /* 34 */
	HH (c, d, a, b, x[11], S33, 0x6d9d6122); /* 35 */
	HH (b, c, d, a, x[14], S34, 0xfde5380c); /* 36 */
	HH (a, b, c, d, x[ 1], S31, 0xa4beea44); /* 37 */
	HH (d, a, b, c, x[ 4], S32, 0x4bdecfa9); /* 38 */
	HH (c, d, a, b, x[ 7], S33, 0xf6bb4b60); /* 39 */
	HH (b, c, d, a, x[10], S34, 0xbebfbc70); /* 40 */
	HH (a, b, c, d, x[13], S31, 0x289b7ec6); /* 41 */
	HH (d, a, b, c, x[ 0], S32, 0xeaa127fa); /* 42 */
	HH (c, d, a, b, x[ 3], S33, 0xd4ef3085); /* 43 */
	HH (b, c, d, a, x[ 6], S34,  0x4881d05); /* 44 */
	HH (a, b, c, d, x[ 9], S31, 0xd9d4d039); /* 45 */
	HH (d, a, b, c, x[12], S32, 0xe6db99e5); /* 46 */
	HH (c, d, a, b, x[15], S33, 0x1fa27cf8); /* 47 */
	HH (b, c, d, a, x[ 2], S34, 0xc4ac5665); /* 48 */

	/* Round 4 */
	II (a, b, c, d, x[ 0], S41, 0xf4292244); /* 49 */
	II (d, a, b, c, x[ 7], S42, 0x432aff97); /* 50 */
	II (c, d, a, b, x[14], S43, 0xab9423a7); /* 51 */
	II (b, c, d, a, x[ 5], S44, 0xfc93a039); /* 52 */
	II (a, b, c, d, x[12], S41, 0x655b59c3); /* 53 */
	II (d, a, b, c, x[ 3], S42, 0x8f0ccc92); /* 54 */
	II (c, d, a, b, x[10], S43, 0xffeff47d); /* 55 */
	II (b, c, d, a, x[ 1], S44, 0x85845dd1); /* 56 */
	II (a, b, c, d, x[ 8], S41, 0x6fa87e4f); /* 57 */
	II (d, a, b, c, x[15], S42, 0xfe2ce6e0); /* 58 */
	II (c, d, a, b, x[ 6], S43, 0xa3014314); /* 59 */
	II (b, c, d, a, x[13], S44, 0x4e0811a1); /* 60 */
	II (a, b, c, d, x[ 4], S41, 0xf7537e82); /* 61 */
	II (d, a, b, c, x[11], S42, 0xbd3af235); /* 62 */
	II (c, d, a, b, x[ 2], S43, 0x2ad7d2bb); /* 63 */
	II (b, c, d, a, x[ 9], S44, 0xeb86d391); /* 64 */

	state[0] += a;
	state[1] += b;
	state[2] += c;
	state[3] += d;

	// Zeroize sensitive information.
	memset ( (uint1 *) x, 0, sizeof(x));

}

// Encodes input (UINT4) into output (unsigned char). Assumes len is
// a multiple of 4.
void CMD5Engine::encode (uint1 *output, uint4 *input, uint4 len) {

	uint4 i, j;

	for (i = 0, j = 0; j < len; i++, j += 4) {
		output[j]   = (uint1)  (input[i] & 0xff);
		output[j+1] = (uint1) ((input[i] >> 8) & 0xff);
		output[j+2] = (uint1) ((input[i] >> 16) & 0xff);
		output[j+3] = (uint1) ((input[i] >> 24) & 0xff);
	}
}

// Decodes input (unsigned char) into output (UINT4). Assumes len is
// a multiple of 4.
void CMD5Engine::decode (uint4 *output, uint1 *input, uint4 len){

	uint4 i, j;

	for (i = 0, j = 0; j < len; i++, j += 4)
		output[i] = ((uint4)input[j]) | (((uint4)input[j+1]) << 8) |
		(((uint4)input[j+2]) << 16) | (((uint4)input[j+3]) << 24);
}

// ROTATE_LEFT rotates x left n bits.
inline unsigned long CMD5Engine::rotate_left  (uint4 x, uint4 n){
	return (x << n) | (x >> (32-n))  ;
}

// F, G, H and I are basic MD5 functions.
inline unsigned long CMD5Engine::F            (uint4 x, uint4 y, uint4 z){
	return (x & y) | (~x & z);
}
inline unsigned long CMD5Engine::G            (uint4 x, uint4 y, uint4 z){
	return (x & z) | (y & ~z);
}
inline unsigned long CMD5Engine::H            (uint4 x, uint4 y, uint4 z){
	return x ^ y ^ z;
}
inline unsigned long CMD5Engine::I            (uint4 x, uint4 y, uint4 z){
	return y ^ (x | ~z);
}

// FF, GG, HH, and II transformations for rounds 1, 2, 3, and 4.
// Rotation is separate from addition to prevent recomputation.

inline void CMD5Engine::FF(uint4& a, uint4 b, uint4 c, uint4 d, uint4 x, 
					 uint4  s, uint4 ac){
						 a += F(b, c, d) + x + ac;
						 a = rotate_left (a, s) +b;
					 }

inline void CMD5Engine::GG(uint4& a, uint4 b, uint4 c, uint4 d, uint4 x, 
	 uint4 s, uint4 ac){
	a += G(b, c, d) + x + ac;
	a = rotate_left (a, s) +b;
}

inline void CMD5Engine::HH(uint4& a, uint4 b, uint4 c, uint4 d, uint4 x, 
	 uint4 s, uint4 ac){
	a += H(b, c, d) + x + ac;
	a = rotate_left (a, s) +b;
}

inline void CMD5Engine::II(uint4& a, uint4 b, uint4 c, uint4 d, uint4 x, 
	 uint4 s, uint4 ac){
	a += I(b, c, d) + x + ac;
	a = rotate_left (a, s) +b;
}

// ---------------------------------------------------------------------------------- MAIN

/*
void printusage()
{
	printf("XMedia MD5SUM Utility\nUsage:\nxmmd5sum <file>");
}

int main(int argc, char* argv[])
{
	//first arg MUST be file name
	if (argc<2)
	{
		printusage();
		return -1;
	}

	//try to generate md5
	CMD5 md5;
	md5.FromFile(argv[1]);

	//print it
	printf(md5.GetString());
	return 0;
}

  */

#endif _MD5SUM_H_
